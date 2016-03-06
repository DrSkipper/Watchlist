using UnityEngine;
using System.Collections.Generic;

public class LevelGenStarter : LevelGenBehavior
{
    [System.Serializable]
    public struct InputTable
    {
        public List<LevelGenInput> PossibleInputs;
    }

    public GameObject Tiles;
    public List<InputTable> InputsByDifficulty;
    
    void Start()
    {
        int difficulty = findLevelDifficulty(DynamicData.MostRecentTile);
        List<LevelGenInput> possibleInputs = this.InputsByDifficulty[difficulty].PossibleInputs;
        this.Manager.InitiateGeneration(possibleInputs[Random.Range(0, possibleInputs.Count)]);
        _beganGeneration = true;
    }

    void Update()
    {
        if (_beganGeneration && this.Manager.Finished)
        {
            _beganGeneration = false;

            // Dump json of level gen output
            /*LevelGenOutput output = this.Manager.GetOutput();
            string json = JsonConvert.SerializeObject(output, Formatting.None);
            Debug.Log("json of level gen output:\n" + json);*/
            Debug.Log("level generation complete, json output not enabled");

            int[,] grid = this.tileTypeMapToSpriteIndexMap();
            this.Tiles.GetComponent<TileMapOutlineRenderer>().CreateMapWithGrid(grid);
            this.Tiles.GetComponent<TileGeometryCreator>().CreateMapWithGrid(grid);
            this.Manager.Cleanup();
        }
    }

    /**
	 * Private
	 */
    private bool _beganGeneration;

    private int findLevelDifficulty(IntegerVector level)
    {
        int radius = Mathf.Max(Mathf.Abs(level.X), Mathf.Abs(level.Y));
        if (radius <= 1)
            return 0; // Easy
        if (radius >= 3)
            return 2; // Hard
        return 1; // Medium
    }

    private int[,] tileTypeMapToSpriteIndexMap()
    {
        LevelGenMap.TileType[,] grid = this.Map.Grid;
        int[,] spriteIndices = new int[this.Map.Width, this.Map.Height];

        for (int x = 0; x < this.Map.Width; ++x)
        {
            for (int y = 0; y < this.Map.Height; ++y)
            {
                spriteIndices[x, y] = tileSetIndexForTile(grid[x, y]);
            }
        }

        return spriteIndices;
    }

    //TODO - fcole - Data-drive this
    private int tileSetIndexForTile(LevelGenMap.TileType tile)
    {
        switch (tile)
        {
            default:
            case LevelGenMap.TileType.A:
                return 1;
            case LevelGenMap.TileType.B:
                return 0;
            case LevelGenMap.TileType.C:
                return 2;
            case LevelGenMap.TileType.D:
                return 3;
            case LevelGenMap.TileType.E:
                return 4;
            case LevelGenMap.TileType.F:
                return 5;
        }
    }
}
