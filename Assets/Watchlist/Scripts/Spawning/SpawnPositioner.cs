using UnityEngine;
using System.Collections.Generic;

public class SpawnPositioner : VoBehavior
{
    public GameObject LevelGenerator;
    public GameObject Tiles;
    public GameObject EnemySpawnPrefab;
    public GameObject PlayerPrefab;
    public GameObject[] PickupPrefabs;
    public int NumEnemies = 10;
    public int NumPlayers = 1;
    public int NumPickups = 1;
    public float SpawnPlayersDelay = 1.0f;
    public float SpawnEnemiesDelay = 3.0f;

    void Start()
    {
        _levelGenManager = this.LevelGenerator.GetComponent<LevelGenManager>();
        _tileRenderer = this.Tiles.GetComponent<TileMapOutlineRenderer>();
        _levelGenManager.UpdateDelegate = this.LevelGenUpdate;

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
            _targets = new List<Transform>();
            _spawnPositions = new List<IntegerVector>();
            TimedCallbacks callbacks = this.GetComponent<TimedCallbacks>();
            int totalSpawns = this.NumEnemies + this.NumPlayers + this.NumPickups;

            for (int i = 0; i < (totalSpawns < openTiles.Count ? totalSpawns : openTiles.Count); ++i)
            {
                _spawnPositions.Add(_tileRenderer.PositionForTile(openTiles[i].x, openTiles[i].y));
            }

            callbacks.AddCallback(this, this.SpawnPlayers, this.SpawnPlayersDelay);
            callbacks.AddCallback(this, this.SpawnEnemies, this.SpawnEnemiesDelay);
        }
    }

    public void SpawnPlayers()
    {
        for (int i = 0; i < this.NumPlayers; ++i)
        {
            if (_spawnPositions.Count == 0)
                break;

            IntegerVector position = _spawnPositions[_spawnPositions.Count - 1];
            GameObject player = Instantiate(this.PlayerPrefab, new Vector3(position.X, position.Y, this.transform.position.z), Quaternion.identity) as GameObject;

            if (_tileRenderer.OffsetTilesToCenter)
                player.transform.position = new Vector3(player.transform.position.x - _tileRenderer.TileRenderSize * _tileRenderer.Width / 2, player.transform.position.y - _tileRenderer.TileRenderSize * _tileRenderer.Height / 2, player.transform.position.z);

            _targets.Add(player.transform);
            Camera.main.GetComponent<CameraController>().CenterTarget = player.transform;
            _spawnPositions.RemoveAt(_spawnPositions.Count - 1);
        }

        if (this.PickupPrefabs.Length > 0)
        {
            for (int i = 0; i < this.NumPickups; ++i)
            {
                IntegerVector position = _spawnPositions[_spawnPositions.Count - 1];
                GameObject prefab = this.PickupPrefabs[Random.Range(0, this.PickupPrefabs.Length)];
                GameObject pickup = Instantiate(prefab, new Vector3(position.X, position.Y, this.transform.position.z), Quaternion.identity) as GameObject;

                if (_tileRenderer.OffsetTilesToCenter)
                    pickup.transform.position = new Vector3(pickup.transform.position.x - _tileRenderer.TileRenderSize * _tileRenderer.Width / 2, pickup.transform.position.y - _tileRenderer.TileRenderSize * _tileRenderer.Height / 2, pickup.transform.position.z);
                
                _spawnPositions.RemoveAt(_spawnPositions.Count - 1);
            }
        }
    }

    public void SpawnEnemies()
    {
        WinCondition winCondition = this.GetComponent<WinCondition>();

        for (int i = 0; i < this.NumEnemies; ++i)
        {
            if (_spawnPositions.Count == 0)
                break;

            IntegerVector position = _spawnPositions[_spawnPositions.Count - 1];
            GameObject spawner = Instantiate(this.EnemySpawnPrefab, new Vector3(position.X, position.Y, this.transform.position.z), Quaternion.identity) as GameObject;

            if (_tileRenderer.OffsetTilesToCenter)
                spawner.transform.position = new Vector3(spawner.transform.position.x - _tileRenderer.TileRenderSize * _tileRenderer.Width / 2, spawner.transform.position.y - _tileRenderer.TileRenderSize * _tileRenderer.Height / 2, spawner.transform.position.z);

            EnemySpawner spawnBehavior = spawner.GetComponent<EnemySpawner>();
            if (winCondition != null)
                spawnBehavior.SpawnCallback = winCondition.EnemySpawned;
            spawnBehavior.Targets = _targets.ToArray();
            spawnBehavior.DestroyAfterSpawn = true;
            _spawnPositions.RemoveAt(_spawnPositions.Count - 1);
        }
    }

    /**
     * Private
     */
    private LevelGenManager _levelGenManager;
    private TileMapOutlineRenderer _tileRenderer;
    private List<Transform> _targets;
    private List<IntegerVector> _spawnPositions;
}
