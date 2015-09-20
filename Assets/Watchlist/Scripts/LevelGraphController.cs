using UnityEngine;
using System;
using System.Collections.Generic;

public class LevelGraphController : VoBehavior
{
    public int Size = 7; // Should be odd number
    public Vector2[] BossTiles;
    public Vector2[] CompletedTiles;
    public Vector2 CurrentPosition;
    public GameObject BoxPrefab;
    public GameObject LinePrebab;
    public float GridSpaceDistance = 17.0f;
    public Color PlayerColor;
    public Color BossColor;
    public Color AvailableColor;
    public Color BaseColor;
    public float BlinkIntervalOn = 0.5f;
    public float BlinkIntervalOff = 0.2f;

    /**
     * Data types
     */
    private enum TileState
    {
        Locked,
        Available,
        Complete
    }

    private enum TileTrait
    {
        Boss
    }

    private class LevelGraphTile
    {
        public Vector2 Position; // -halfsize to halfsize
        public GameObject GameObject;
        public TileState State;
        public TileTrait[] Traits;

        public bool HasTrait(TileTrait trait)
        {
            if (this.Traits == null)
                return false;

            foreach (TileTrait entry in this.Traits)
            {
                if (entry == trait)
                    return true;
            }
            return false;
        }
    }

    private class LevelGraphPath
    {
        public Vector2 Position; // -halfsize * 2 to halfsize * 2, odd numbers indicate between rows/columns, even numbers aligned with tiles at index / 2
        public GameObject GameObject;
        public TileState State;
    }

    /**
     * Public methods
     */
    void Start()
    {
        _grid = new LevelGraphTile[this.Size, this.Size];
        _paths = new LevelGraphPath[this.Size * 2 - 1, this.Size * 2 - 1];
        _availableTiles = new List<Vector2>();
        _halfSize = this.Size / 2;

        // Setup boxes
        for (int x = -_halfSize; x < _grid.GetLength(0) - _halfSize; ++x)
        {
            for (int y = -_halfSize; y < _grid.GetLength(1) - _halfSize; ++y)
            {
                // Skip middle of all sides
                if ((x == 0 && Math.Abs(y) == _halfSize) || (y == 0 && Math.Abs(x) == _halfSize))
                    continue;

                GameObject go = Instantiate(BoxPrefab) as GameObject;
                go.transform.parent = this.transform;
                go.transform.localPosition = new Vector3(this.GridSpaceDistance * x, this.GridSpaceDistance * y, 0);
                Vector2 position = new Vector2(x, y);
                
                TileState state = TileState.Locked;
                List<TileTrait> traits = new List<TileTrait>();

                foreach (Vector2 tile in this.CompletedTiles)
                {
                    if (tile.IntX() == x && tile.IntY() == y)
                    {
                        state = TileState.Complete;
                        break;
                    }
                    else if (neighborsTile(go, tile))
                    {
                        state = TileState.Available;
                    }
                }

                if (state == TileState.Available)
                    _availableTiles.Add(gridPositionOfBox(go));

                if (state != TileState.Complete)
                {
                    foreach (Vector2 tile in this.BossTiles)
                    {
                        if (Mathf.RoundToInt(tile.x) == x && Mathf.RoundToInt(tile.y) == y)
                        {
                            traits.Add(TileTrait.Boss);
                            break;
                        }
                    }
                }

                _grid[x + _halfSize, y + _halfSize] = createTile(position, go, state, traits.ToArray());
            }
        }

        // Setup paths
        for (int x = 0; x < _grid.GetLength(0) - 1; ++x)
        {
            for (int y = 0; y < _grid.GetLength(1) - 1; ++y)
            {
                if (_grid[x, y] == null)
                    continue;

                TileState state = _grid[x, y].State;

                if (state == TileState.Complete || state == TileState.Available)
                {
                    bool northCompleted = _paths[x * 2, y * 2 + 1] == null && _grid[x, y + 1].State == TileState.Complete;
                    bool northAvailable = state != TileState.Available && !northCompleted && _paths[x * 2, y * 2 + 1] == null && _grid[x, y + 1].State == TileState.Available;
                    bool eastCompleted = _paths[x * 2 + 1, y * 2] == null && _grid[x + 1, y].State == TileState.Complete;
                    bool eastAvailable = state != TileState.Available && !eastCompleted && _paths[x * 2 + 1, y * 2] == null && _grid[x + 1, y].State == TileState.Available;

                    if (northCompleted || northAvailable)
                    {
                        GameObject go = Instantiate(this.LinePrebab);
                        go.transform.parent = this.transform;
                        go.transform.localPosition = new Vector3(this.GridSpaceDistance * (x - _halfSize), this.GridSpaceDistance * (y - _halfSize) + this.GridSpaceDistance / 2.0f, 0);
                        TileState pathState = state == TileState.Complete && northCompleted ? TileState.Complete : TileState.Available;
                        _paths[x * 2, y * 2 + 1] = createPath(new Vector2((x - _halfSize) * 2, (y - _halfSize) * 2 + 1), go, pathState);
                    }

                    if (eastCompleted || eastAvailable)
                    {
                        GameObject go = Instantiate(this.LinePrebab);
                        go.transform.parent = this.transform;
                        go.transform.localPosition = new Vector3(this.GridSpaceDistance * (x - _halfSize) + this.GridSpaceDistance / 2.0f, this.GridSpaceDistance * (y - _halfSize), 0);
                        TileState pathState = state == TileState.Complete && eastCompleted ? TileState.Complete : TileState.Available;
                        _paths[x * 2, y * 2 + 1] = createPath(new Vector2((x - _halfSize) * 2 + 1, (y - _halfSize) * 2), go, pathState);
                    }
                }
            }
        }

        // Initialize current tile
        //moveCurrentTile(getCurrentTileObject());
    }

    void Update()
    {
        /*
        if (Input.anyKeyDown)
        {
            GameObject previous = getCurrentTileObject();
            Vector2 previousCoord = this.CurrentTile;

            if (this.CurrentTile.IntX() > -_halfSize && (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow)))
            {
                this.CurrentTile.x -= 1;
            }
            else if (this.CurrentTile.IntY() > -_halfSize && (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow)))
            {
                this.CurrentTile.y -= 1;
            }
            else if (this.CurrentTile.IntX() < _halfSize && (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow)))
            {
                this.CurrentTile.x += 1;
            }
            if (this.CurrentTile.IntY() < _halfSize && (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow)))
            {
                this.CurrentTile.y += 1;
            }

            GameObject nextTile = getCurrentTileObject();
            if (nextTile != null && nextTile != previous)
                moveCurrentTile(nextTile, previous);
            else
                this.CurrentTile = previousCoord;
        }
        else
        {
            if (!_currentBlinkingOff && _timeSinceBlink >= this.BlinkIntervalOn)
            {
                blinkCurrentTileOff();
            }
            else if (_currentBlinkingOff && _timeSinceBlink >= this.BlinkIntervalOff)
            {
                blinkCurrentTileOn();
            }
            else
            {
                _timeSinceBlink += Time.deltaTime;
            }
        }
        */
    }

    /**
     * Private
     */
    private int _halfSize;
    private LevelGraphTile[,] _grid;
    private LevelGraphPath[,] _paths;
    private List<Vector2> _availableTiles;
    private bool _currentBlinkingOff;
    private float _timeSinceBlink;

    /*
    private GameObject getCurrentTileObject()
    {
        return _grid[this.CurrentTile.IntX() + _halfSize, this.CurrentTile.IntY() + _halfSize];
    }*/

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

    private LevelGraphTile createTile(Vector2 position, GameObject go, TileState state, TileTrait[] traits)
    {
        LevelGraphTile tile = new LevelGraphTile();
        tile.Position = position;
        tile.GameObject = go;
        tile.State = state;
        tile.Traits = traits;

        Color outline = this.BaseColor;
        Color center = Color.clear;

        if (state == TileState.Complete)
        {
            center = this.PlayerColor;
        }
        else
        {
            if (state == TileState.Available)
                outline = this.AvailableColor;

            if (tile.HasTrait(TileTrait.Boss))
                center = this.BossColor;
        }

        go.transform.Find("Outline").gameObject.GetComponent<LevelSelectColorizer>().UpdateColor(outline);
        go.transform.Find("Center").gameObject.GetComponent<LevelSelectColorizer>().UpdateColor(center);
        return tile;
    }

    private LevelGraphPath createPath(Vector2 position, GameObject go, TileState state)
    {
        LevelGraphPath path = new LevelGraphPath();
        path.Position = position;
        path.GameObject = go;
        path.State = state;

        go.GetComponent<LevelSelectColorizer>().UpdateColor(state == TileState.Complete ? this.PlayerColor : this.AvailableColor);

        if (Math.Abs(position.IntX()) % 2 == 1)
            go.transform.localRotation = Quaternion.AngleAxis(90.0f, new Vector3(0, 0, 1));

        return path;
    }

    private void moveCurrentTile(GameObject current, GameObject previous = null)
    {
        if (previous != null)
        {
            LevelSelectColorizer previousColorizer = previous.transform.Find("Outline").gameObject.GetComponent<LevelSelectColorizer>();
            previousColorizer.Color = isAvailableTile(previous) ? this.AvailableColor : this.BaseColor;
            previousColorizer.UpdateColor();
        }

        LevelSelectColorizer currentColorizer = current.transform.Find("Outline").gameObject.GetComponent<LevelSelectColorizer>();
        currentColorizer.Color = this.PlayerColor;
        currentColorizer.UpdateColor();
        _timeSinceBlink = 0.0f;
        _currentBlinkingOff = false;
    }

    /*
    private void blinkCurrentTileOn()
    {
        GameObject current = getCurrentTileObject();
        LevelSelectColorizer currentColorizer = current.transform.Find("Outline").gameObject.GetComponent<LevelSelectColorizer>();
        currentColorizer.Color = this.PlayerColor;
        currentColorizer.UpdateColor();
        _timeSinceBlink = 0.0f;
        _currentBlinkingOff = false;
    }

    private void blinkCurrentTileOff()
    {
        GameObject current = getCurrentTileObject();
        LevelSelectColorizer currentColorizer = current.transform.Find("Outline").gameObject.GetComponent<LevelSelectColorizer>();
        currentColorizer.Color = Color.clear;
        currentColorizer.UpdateColor();
        _timeSinceBlink = 0.0f;
        _currentBlinkingOff = true;
    }*/
}
