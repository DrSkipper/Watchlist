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
        _availableTiles = new List<Vector2>();
        int halfSize = this.Size / 2;

        for (int x = -halfSize; x < _grid.GetLength(0) - halfSize; ++x)
        {
            for (int y = -halfSize; y < _grid.GetLength(1) - halfSize; ++y)
            {
                // Skip middle of all sides
                if ((x == 0 && Math.Abs(y) == halfSize) || (y == 0 && Math.Abs(x) == halfSize))
                    continue;

                GameObject go = Instantiate(BoxPrefab, new Vector3(this.GridSpaceDistance * x, this.GridSpaceDistance * y, 0), Quaternion.identity) as GameObject;
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

                go.transform.parent = this.transform;
            }
        }
    }

    /**
     * Private
     */
    private GameObject[,] _grid;
    private List<Vector2> _availableTiles;

    private Vector2 gridPositionOfBox(GameObject box)
    {
        return new Vector2(Mathf.RoundToInt(box.transform.position.x / this.GridSpaceDistance), Mathf.RoundToInt(box.transform.position.y / this.GridSpaceDistance));
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
