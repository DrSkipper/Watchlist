using UnityEngine;
using System.Collections.Generic;

public class SpawnPositioner : VoBehavior
{
    public GameObject LevelGenerator;
    public GameObject Tiles;
    public GameObject EnemySpawnPrefab;
    public GameObject PlayerPrefab;
    public int NumEnemies = 10;
    public int NumPlayers = 1;

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
            _targets = new List<Transform>();

            for (int i = 0; i < (this.NumEnemies + this.NumPlayers < openTiles.Count ? this.NumEnemies + this.NumPlayers : openTiles.Count); ++i)
            {
                IntegerVector position = _tileRenderer.PositionForTile(openTiles[i].x, openTiles[i].y);
                if (i < this.NumPlayers)
                {
                    GameObject player = Instantiate(this.PlayerPrefab, new Vector3(position.X, position.Y, this.transform.position.z), Quaternion.identity) as GameObject;
                    player.transform.parent = this.transform;
                    _targets.Add(player.transform);
                    Camera.main.GetComponent<CameraController>().CenterTarget = player.transform;
                }
                else
                {
                    GameObject spawner = Instantiate(this.EnemySpawnPrefab, new Vector3(position.X, position.Y, this.transform.position.z), Quaternion.identity) as GameObject;
                    spawner.transform.parent = this.transform;
                    spawner.GetComponent<EnemySpawner>().Targets = _targets.ToArray();
                }
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
    private List<Transform> _targets;
}
