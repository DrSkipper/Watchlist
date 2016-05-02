using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using System.Collections.Generic;

public class LevelGraphController : VoBehavior
{
    public int Size = 7; // Should be odd number
    public IntegerVector[] BossTiles;
    public IntegerVector[] CompletedTiles;
    public IntegerVector CurrentPosition;
    public GameObject BoxPrefab;
    public GameObject LinePrebab;
    public float GridSpaceDistance = 17.0f;
    public Color PlayerColor;
    public Color BossColor;
    public Color AvailableColor;
    public Color BaseColor;
    public float BlinkIntervalOn = 0.5f;
    public float BlinkIntervalOff = 0.2f;
    public string GameplaySceneName = "";
    public string BossSceneName = "";
    public bool UseDynamicData = true;
    public string FinalDialogSceneName = "";

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
        public List<LevelGraphPath> NeighborPaths; // For caching

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
        if (this.UseDynamicData)
        {
            this.CompletedTiles = ProgressData.CompletedTiles;
            this.CurrentPosition = ProgressData.MostRecentTile;
        }
        _grid = new LevelGraphTile[this.Size, this.Size];
        _paths = new LevelGraphPath[this.Size * 2 - 1, this.Size * 2 - 1];
        _halfSize = this.Size / 2;

        // Check if all bosses defeated
        _allBossesDefeated = true;
        for (int i = 0; i < this.BossTiles.Length; ++i)
        {
            bool contains = false;
            for (int j = 0; j < this.CompletedTiles.Length; ++j)
            {
                if (this.CompletedTiles[j].X == this.BossTiles[i].X && this.CompletedTiles[j].Y == this.BossTiles[i].Y)
                {
                    contains = true;
                    break;
                }
            }

            if (!contains)
            {
                _allBossesDefeated = false;
                break;
            }
        }

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

                if (_allBossesDefeated && x == 0 && y == 0)
                {
                    state = TileState.Available;
                }
                else
                {
                    foreach (IntegerVector tile in this.CompletedTiles)
                    {
                        if (tile.X == x && tile.Y == y)
                        {
                            state = TileState.Complete;
                            break;
                        }
                        else if (neighborsTile(position, tile))
                        {
                            state = TileState.Available;
                        }
                    }
                }

                // If no completed tiles, the center should be available
                if (this.CompletedTiles.Length == 0 && x == 0 && y == 0)
                    state = TileState.Available;

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
                    bool northCompleted = _paths[x * 2, y * 2 + 1] == null && (_grid[x, y + 1] != null && _grid[x, y + 1].State == TileState.Complete);
                    bool northAvailable = state != TileState.Available && !northCompleted && _paths[x * 2, y * 2 + 1] == null && (_grid[x, y + 1] != null && _grid[x, y + 1].State == TileState.Available);
                    bool eastCompleted = _paths[x * 2 + 1, y * 2] == null && (_grid[x + 1, y] != null && _grid[x + 1, y].State == TileState.Complete);
                    bool eastAvailable = state != TileState.Available && !eastCompleted && _paths[x * 2 + 1, y * 2] == null && (_grid[x + 1, y] != null && _grid[x + 1, y].State == TileState.Available);

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
                        _paths[x * 2 + 1, y * 2] = createPath(new Vector2((x - _halfSize) * 2 + 1, (y - _halfSize) * 2), go, pathState);
                    }
                }
            }
        }

        // Initialize current tile
        moveCurrentTile(this.CurrentPosition);
    }

    void Update()
    {
        if (MenuInput.AnyInput())
        {
            IntegerVector newPosition = this.CurrentPosition;

            if (this.CurrentPosition.X > -_halfSize && MenuInput.NavLeft())
            {
                newPosition.X -= 1;
            }
            else if (this.CurrentPosition.Y > -_halfSize && MenuInput.NavDown())
            {
                newPosition.Y -= 1;
            }
            else if (this.CurrentPosition.X < _halfSize && MenuInput.NavRight())
            {
                newPosition.X += 1;
            }
            else if (this.CurrentPosition.Y < _halfSize && MenuInput.NavUp())
            {
                newPosition.Y += 1;
            }

            if ((newPosition.X != this.CurrentPosition.X || newPosition.Y != this.CurrentPosition.Y) && _grid[newPosition.X + _halfSize, newPosition.Y + _halfSize] != null)
            {
                moveCurrentTile(newPosition);
            }
            else if (MenuInput.SelectCurrentElement() && _grid[this.CurrentPosition.X + _halfSize, this.CurrentPosition.Y + _halfSize].State == TileState.Available)
            {
                int bossIndex = -1;
                for (int i = 0; i < this.BossTiles.Length; ++i)
                {
                    IntegerVector tile = this.BossTiles[i];
                
                    if (this.CurrentPosition.X == tile.X && this.CurrentPosition.Y == tile.Y)
                    {
                        bossIndex = i;
                        break;
                    }
                }

                //TODO - Send input to level generation
                ProgressData.SelectTile(this.CurrentPosition);
                if (_allBossesDefeated && this.CurrentPosition.X == 0 && this.CurrentPosition.Y == 0)
                {
                    SceneManager.LoadScene(this.FinalDialogSceneName);
                }
                else if (bossIndex == -1)
                {
                    SceneManager.LoadScene(this.GameplaySceneName);
                }
                else
                {
                    BossType boss = StaticData.BossData.BossTypes[PersistentData.GetCurrentBosses()[bossIndex]];
                    SceneManager.LoadScene(this.BossSceneName + boss.SceneKey);
                }
            }
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
    private bool _allBossesDefeated;

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

    private List<LevelGraphPath> neighborPaths(LevelGraphTile tile)
    {
        if (tile.NeighborPaths != null)
            return tile.NeighborPaths;

        int x = tile.Position.IntX();
        int y = tile.Position.IntY();
        int pathX = (x + _halfSize) * 2;
        int pathY = (y + _halfSize) * 2;
        tile.NeighborPaths = new List<LevelGraphPath>();
        
        // left
        if (x > -_halfSize && _paths[pathX - 1, pathY] != null)
            tile.NeighborPaths.Add(_paths[pathX - 1, pathY]);

        // right
        if (x < _halfSize && _paths[pathX + 1, pathY] != null)
            tile.NeighborPaths.Add(_paths[pathX + 1, pathY]);

        // down
        if (y > -_halfSize && _paths[pathX, pathY - 1] != null)
            tile.NeighborPaths.Add(_paths[pathX, pathY - 1]);

        // up
        if (y < _halfSize && _paths[pathX, pathY + 1] != null)
            tile.NeighborPaths.Add(_paths[pathX, pathY + 1]);
        
        return tile.NeighborPaths;
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

        colorizePath(path);

        if (Math.Abs(position.IntX()) % 2 == 1)
            go.transform.localRotation = Quaternion.AngleAxis(90.0f, new Vector3(0, 0, 1));

        return path;
    }

    private void moveCurrentTile(IntegerVector newPosition)
    {
        if (newPosition.X != this.CurrentPosition.X || newPosition.Y != this.CurrentPosition.Y)
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

        if (tile.State == TileState.Available)
        {
            foreach (LevelGraphPath path in neighborPaths(tile))
                colorizePath(path);
        }
    }

    private void applyCurrentEffect(Vector2 position)
    {
        LevelGraphTile tile = _grid[position.IntX() + _halfSize, position.IntY() + _halfSize];
        tile.GameObject.transform.Find("Outline").gameObject.GetComponent<LevelSelectColorizer>().UpdateColor(this.PlayerColor);

        if (tile.State == TileState.Available)
        {
            foreach (LevelGraphPath path in neighborPaths(tile))
                path.GameObject.GetComponent<LevelSelectColorizer>().UpdateColor(this.PlayerColor);
        }
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

    private void colorizePath(LevelGraphPath path)
    {
        path.GameObject.GetComponent<LevelSelectColorizer>().UpdateColor(path.State == TileState.Complete ? this.PlayerColor : this.AvailableColor);
    }
    
    private void blinkCurrentTileOn()
    {
        LevelGraphTile currentTile = _grid[this.CurrentPosition.X + _halfSize, this.CurrentPosition.Y + _halfSize];
        currentTile.GameObject.transform.Find("Outline").gameObject.GetComponent<LevelSelectColorizer>().UpdateColor(this.PlayerColor);

        if (currentTile.State == TileState.Available)
        {
            foreach (LevelGraphPath path in neighborPaths(currentTile))
                path.GameObject.GetComponent<LevelSelectColorizer>().UpdateColor(this.PlayerColor);
        }

        _timeSinceBlink = 0.0f;
        _currentBlinkingOff = false;
    }

    private void blinkCurrentTileOff()
    {
        LevelGraphTile currentTile = _grid[this.CurrentPosition.X + _halfSize, this.CurrentPosition.Y + _halfSize];
        currentTile.GameObject.transform.Find("Outline").gameObject.GetComponent<LevelSelectColorizer>().UpdateColor(Color.clear);

        if (currentTile.State == TileState.Available)
        {
            foreach (LevelGraphPath path in neighborPaths(currentTile))
                path.GameObject.GetComponent<LevelSelectColorizer>().UpdateColor(Color.clear);
        }

        _timeSinceBlink = 0.0f;
        _currentBlinkingOff = true;
    }
}
