using UnityEngine;
using System.Collections.Generic;

public class SpawnPositioner : VoBehavior
{
    public GameObject LevelGenerator;
    public GameObject Tiles;
    public GameObject EnemySpawnPrefab;
    public int NumEnemies = 10;

    void Start()
    {
        _levelGenManager = this.LevelGenerator.GetComponent<LevelGenManager>();
        _tileRenderer = this.Tiles.GetComponent<TileMapOutlineRenderer>();
        _levelGenManager.UpdateDelegate = this.LevelGenUpdate;
        _finished = false;

        if (_tileRenderer.OffsetTilesToCenter)
            this.transform.position = new Vector3(0, 0, this.transform.position.z);
    }

    void LevelGenUpdate()
    {
        if (_levelGenManager.Finished)
        {
            LevelGenOutput output = _levelGenManager.GetOutput();
            List<LevelGenMap.Coordinate> openTiles = output.OpenTiles;
            openTiles.Shuffle();
            _finished = true;

            for (int i = 0; i < (this.NumEnemies < openTiles.Count ? this.NumEnemies : openTiles.Count); ++i)
            {
                IntegerVector position = _tileRenderer.PositionForTile(openTiles[i].x, openTiles[i].y);
                GameObject spawner = Instantiate(this.EnemySpawnPrefab, new Vector3(position.X, position.Y, this.transform.position.z), Quaternion.identity) as GameObject;
                spawner.transform.parent = this.transform;
            }
        }
    }

    void Update()
    {
        if (_finished && _tileRenderer.OffsetTilesToCenter)
        {
            _finished = false;
            this.transform.position = new Vector3(this.transform.position.x - _tileRenderer.TileRenderSize * _tileRenderer.Width / 2, this.transform.position.y - _tileRenderer.TileRenderSize * _tileRenderer.Height / 2, this.transform.position.z);
        }
    }

    /**
     * Private
     */
    private LevelGenManager _levelGenManager;
    private TileMapOutlineRenderer _tileRenderer;
    private bool _finished;
}
