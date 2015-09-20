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
                    else if (neighborsTile(position, tile))
                    {
                        state = TileState.Available;
                    }
                }

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
        moveCurrentTile(this.CurrentPosition);
    }

    void Update()
    {
        if (Input.anyKeyDown)
        {
            Vector2 newPosition = this.CurrentPosition;

            if (this.CurrentPosition.IntX() > -_halfSize && (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow)))
            {
                newPosition.x -= 1;
            }
            else if (this.CurrentPosition.IntY() > -_halfSize && (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow)))
            {
                newPosition.y -= 1;
            }
            else if (this.CurrentPosition.IntX() < _halfSize && (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow)))
            {
                newPosition.x += 1;
            }
            else if (this.CurrentPosition.IntY() < _halfSize && (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow)))
            {
                newPosition.y += 1;
            }

            if ((newPosition.IntX() != this.CurrentPosition.IntX() || newPosition.IntY() != this.CurrentPosition.IntY()) && _grid[newPosition.IntX() + _halfSize, newPosition.IntY() + _halfSize] != null)
                moveCurrentTile(newPosition);
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
    }

    /**
     * Private
     */
    private int _halfSize;
    private LevelGraphTile[,] _grid;
    private LevelGraphPath[,] _paths;
    private bool _currentBlinkingOff;
    private float _timeSinceBlink;

    private bool neighborsTile(Vector2 tile, Vector2 neighbor)
    {
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

        colorizeTile(tile);
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

    private void moveCurrentTile(Vector2 newPosition)
    {
        if (newPosition.IntX() != this.CurrentPosition.IntX() || newPosition.IntY() != this.CurrentPosition.IntY())
            removeCurrentEffect(this.CurrentPosition);

        this.CurrentPosition = newPosition;
        applyCurrentEffect(this.CurrentPosition);
        _timeSinceBlink = 0.0f;
        _currentBlinkingOff = false;
    }

    private void removeCurrentEffect(Vector2 position)
    {
        LevelGraphTile tile = _grid[position.IntX() + _halfSize, position.IntY() + _halfSize];
        colorizeTile(tile);
    }

    private void applyCurrentEffect(Vector2 position)
    {
        LevelGraphTile tile = _grid[position.IntX() + _halfSize, position.IntY() + _halfSize];
        tile.GameObject.transform.Find("Outline").gameObject.GetComponent<LevelSelectColorizer>().UpdateColor(this.PlayerColor);
    }

    private void colorizeTile(LevelGraphTile tile)
    {
        Color outline = this.BaseColor;
        Color center = Color.clear;

        if (tile.State == TileState.Complete)
        {
            center = this.PlayerColor;
        }
        else
        {
            if (tile.State == TileState.Available)
                outline = this.AvailableColor;

            if (tile.HasTrait(TileTrait.Boss))
                center = this.BossColor;
        }

        tile.GameObject.transform.Find("Outline").gameObject.GetComponent<LevelSelectColorizer>().UpdateColor(outline);
        tile.GameObject.transform.Find("Center").gameObject.GetComponent<LevelSelectColorizer>().UpdateColor(center);
    }
    
    private void blinkCurrentTileOn()
    {
        LevelGraphTile currentTile = _grid[this.CurrentPosition.IntX() + _halfSize, this.CurrentPosition.IntY() + _halfSize];
        currentTile.GameObject.transform.Find("Outline").gameObject.GetComponent<LevelSelectColorizer>().UpdateColor(this.PlayerColor);
        _timeSinceBlink = 0.0f;
        _currentBlinkingOff = false;
    }

    private void blinkCurrentTileOff()
    {
        LevelGraphTile currentTile = _grid[this.CurrentPosition.IntX() + _halfSize, this.CurrentPosition.IntY() + _halfSize];
        currentTile.GameObject.transform.Find("Outline").gameObject.GetComponent<LevelSelectColorizer>().UpdateColor(Color.clear);
        _timeSinceBlink = 0.0f;
        _currentBlinkingOff = true;
    }
}
