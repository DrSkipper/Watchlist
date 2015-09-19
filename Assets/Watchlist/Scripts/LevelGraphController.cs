using UnityEngine;
using System;
using System.Collections.Generic;

public class LevelGraphController : VoBehavior
{
    public int Size = 7; // Should be odd number
    public Vector2[] BossTiles;
    public Vector2[] CompletedTiles;
    public GameObject BoxPrefab;
    public GameObject LinePrebab;
    public float GridSpaceDistance = 17.0f;
    public Color PlayerColor;
    public Color BossColor;
    public Color AvailableColor;
    public Color BaseColor;

    void Start()
    {
        _grid = new GameObject[this.Size, this.Size];
        _paths = new GameObject[this.Size * 2 - 1, this.Size * 2 - 1];
        _availableTiles = new List<Vector2>();
        int halfSize = this.Size / 2;

        // Setup boxes
        for (int x = -halfSize; x < _grid.GetLength(0) - halfSize; ++x)
        {
            for (int y = -halfSize; y < _grid.GetLength(1) - halfSize; ++y)
            {
                // Skip middle of all sides
                if ((x == 0 && Math.Abs(y) == halfSize) || (y == 0 && Math.Abs(x) == halfSize))
                    continue;

                GameObject go = Instantiate(BoxPrefab) as GameObject;
                go.transform.parent = this.transform;
                go.transform.localPosition = new Vector3(this.GridSpaceDistance * x, this.GridSpaceDistance * y, 0);
                _grid[x + halfSize, y + halfSize] = go;

                bool isCompletedTile = false;
                bool isAvailableTile = false;

                foreach (Vector2 tile in this.CompletedTiles)
                {
                    if (tile.IntX() == x && tile.IntY() == y)
                    {
                        isCompletedTile = true;
                        isAvailableTile = false;
                        makeCompletedTile(go);
                        break;
                    }
                    else if (neighborsTile(go, tile))
                    {
                        isAvailableTile = true;
                    }
                }

                if (isAvailableTile)
                {
                    _availableTiles.Add(gridPositionOfBox(go));
                    makeAvailableTile(go);
                }

                if (!isCompletedTile)
                {
                    foreach (Vector2 tile in this.BossTiles)
                    {
                        if (Mathf.RoundToInt(tile.x) == x && Mathf.RoundToInt(tile.y) == y)
                        {
                            makeBossTile(go);
                            break;
                        }
                    }
                }
            }
        }

        // Setup paths
        for (int x = 0; x < _grid.GetLength(0) - 1; ++x)
        {
            for (int y = 0; y < _grid.GetLength(1) - 1; ++y)
            {
                if (_grid[x, y] == null)
                    continue;

                bool completed = isCompletedTile(_grid[x, y]);
                bool available = !completed && isAvailableTile(_grid[x, y]);

                if (completed || available)
                {
                    bool northCompleted = _paths[x * 2, y * 2 + 1] == null && isCompletedTile(_grid[x, y + 1]);
                    bool northAvailable = !available && !northCompleted && _paths[x * 2, y * 2 + 1] == null && isAvailableTile(_grid[x, y + 1]);
                    bool eastCompleted = _paths[x * 2 + 1, y * 2] == null && isCompletedTile(_grid[x + 1, y]);
                    bool eastAvailable = !available && !eastCompleted && _paths[x * 2 + 1, y * 2] == null && isAvailableTile(_grid[x + 1, y]);

                    if (northCompleted || northAvailable)
                    {
                        GameObject go = Instantiate(this.LinePrebab);
                        go.transform.parent = this.transform;
                        go.transform.localPosition = new Vector3(this.GridSpaceDistance * (x - halfSize), this.GridSpaceDistance * (y - halfSize) + this.GridSpaceDistance / 2.0f, 0);
                        go.GetComponent<LevelSelectColorizer>().Color = completed && northCompleted ? this.PlayerColor : this.AvailableColor;
                        _paths[x * 2, y * 2 + 1] = go;
                    }

                    if (eastCompleted || eastAvailable)
                    {
                        GameObject go = Instantiate(this.LinePrebab);
                        go.transform.parent = this.transform;
                        go.transform.localPosition = new Vector3(this.GridSpaceDistance * (x - halfSize) + this.GridSpaceDistance / 2.0f, this.GridSpaceDistance * (y - halfSize), 0);
                        go.transform.localRotation = Quaternion.AngleAxis(90.0f, new Vector3(0, 0, 1));
                        go.GetComponent<LevelSelectColorizer>().Color = completed && eastCompleted ? this.PlayerColor : this.AvailableColor;
                        _paths[x * 2 + 1, y * 2] = go;
                    }
                }
            }
        }

        //TODO - Initialize current tile
    }

    /**
     * Private
     */
    private GameObject[,] _grid;
    private GameObject[,] _paths;
    private List<Vector2> _availableTiles;

    private Vector2 gridPositionOfBox(GameObject box)
    {
        return new Vector2(Mathf.RoundToInt(box.transform.localPosition.x / this.GridSpaceDistance), Mathf.RoundToInt(box.transform.localPosition.y / this.GridSpaceDistance));
    }

    private bool isBossTile(GameObject go)
    {
        Vector2 position = gridPositionOfBox(go);
        foreach (Vector2 tile in this.BossTiles)
        {
            if (tile.IntX() == position.IntX() && tile.IntY() == position.IntY())
                return true;
        }
        return false;
    }

    private bool isCompletedTile(GameObject go)
    {
        Vector2 position = gridPositionOfBox(go);
        foreach (Vector2 tile in this.CompletedTiles)
        {
            if (tile.IntX() == position.IntX() && tile.IntY() == position.IntY())
                return true;
        }
        return false;
    }

    private bool isAvailableTile(GameObject go)
    {
        Vector2 position = gridPositionOfBox(go);
        foreach (Vector2 tile in _availableTiles)
        {
            if (tile.IntX() == position.IntX() && tile.IntY() == position.IntY())
                return true;
        }
        return false;
    }

    private bool neighborsTile(GameObject go, Vector2 neighbor)
    {
        Vector2 tile = gridPositionOfBox(go);

        if (tile.IntX() == neighbor.IntX())
        {
            if (Math.Abs(tile.IntY() - neighbor.IntY()) == 1)
                return true;
        }
        else if (tile.IntY() == neighbor.IntY())
        {
            if (Math.Abs(tile.IntX() - neighbor.IntX()) == 1)
                return true;
        }

        return false;
    }

    private void makeBossTile(GameObject go)
    {
        go.transform.Find("Center").gameObject.GetComponent<LevelSelectColorizer>().Color = this.BossColor;
    }

    private void makeCompletedTile(GameObject go)
    {
        go.transform.Find("Center").gameObject.GetComponent<LevelSelectColorizer>().Color = this.PlayerColor;
    }

    private void makeAvailableTile(GameObject go)
    {
        go.transform.Find("Outline").gameObject.GetComponent<LevelSelectColorizer>().Color = this.AvailableColor;
    }

    private void moveCurrentTile(GameObject current, GameObject previous = null)
    {
        if (previous != null)
        {
            current.transform.Find("Outline").gameObject.GetComponent<LevelSelectColorizer>().Color = isAvailableTile(previous) ? this.AvailableColor : this.BaseColor;
        }
        current.transform.Find("Outline").gameObject.GetComponent<LevelSelectColorizer>().Color = this.PlayerColor;
    }
}
