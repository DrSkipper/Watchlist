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
        _map = this.LevelGenerator.GetComponent<LevelGenMap>();
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
            _targets = new List<Transform>();
            _enemySpawns = new List<EnemySpawn>();
            _playerSpawns = new List<IntegerVector>();
            TimedCallbacks callbacks = this.GetComponent<TimedCallbacks>();
            int totalSpawns = this.NumEnemies + this.NumPlayers + this.NumPickups;

            switch (output.Input.Type)
            {
                default:
                case LevelGenInput.GenerationType.CA:
                    findCASpawns(output);
                    break;
                case LevelGenInput.GenerationType.BSP:
                    findBSPSpawns(output);
                    break;
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
    private struct EnemySpawn
    {
        public IntegerVector SpawnPosition;
        public int EnemyId;

        public EnemySpawn(IntegerVector spawnPosition, int enemyId)
        {
            this.SpawnPosition = spawnPosition;
            this.EnemyId = enemyId;
        }
    }

    private LevelGenManager _levelGenManager;
    private LevelGenMap _map;
    private TileMapOutlineRenderer _tileRenderer;
    private List<Transform> _targets;
    private List<EnemySpawn> _enemySpawns;
    private List<IntegerVector> _playerSpawns;

    private void findCASpawns(LevelGenOutput output)
    {
        List<LevelGenMap.Coordinate> openTiles = new List<LevelGenMap.Coordinate>(output.OpenTiles);
        openTiles.Shuffle();
        EnemySelector enemySelector = new EnemySelector();

        int difficulty = DynamicData.GetCurrentDifficulty();
        int numEnemies = Random.Range(output.Input.NumEnemiesRange.X, output.Input.NumEnemiesRange.Y);
        int numPlayers = 1;
        int[] guaranteedEnemiesPlaced = new int[output.Input.GuaranteedEnemiesByDifficulty.Length];

        if (openTiles.Count <= (numEnemies + numPlayers) * output.Input.MinDistanceBetweenSpawns + 1)
        {
            //TODO - Regenerate level
        }
        else
        {
            int enemiesSpawned = 0;

            // Guaranteed enemies
            for (int i = 0; i < output.Input.GuaranteedEnemiesByDifficulty.Length; ++i)
            {
                while (guaranteedEnemiesPlaced[i] < output.Input.GuaranteedEnemiesByDifficulty[i])
                {
                    int enemyId = enemySelector.ChooseEnemyOfDifficulty(i);
                    guaranteedEnemiesPlaced[i] = guaranteedEnemiesPlaced[i] + 1;
                    ++enemiesSpawned;
                    _enemySpawns.Add(new EnemySpawn(findGoodOpenPosition(openTiles, output.Input.MinDistanceBetweenSpawns).integerVector, enemyId));
                }
            }

            // Remaining enemies
            for (; enemiesSpawned < numEnemies; ++enemiesSpawned)
            {
                int enemyId = enemySelector.ChooseEnemy(difficulty);
                _enemySpawns.Add(new EnemySpawn(findGoodOpenPosition(openTiles, output.Input.MinDistanceBetweenSpawns).integerVector, enemyId));
            }

            //TODO - Player spawns
        }
    }

    private void findBSPSpawns(LevelGenOutput output)
    {
        //TODO
    }

    private LevelGenMap.Coordinate findGoodOpenPosition(List<LevelGenMap.Coordinate> availableTiles, int areaToClear)
    {
        LevelGenMap.Coordinate position;
        int minDistance = 18;
        int positionIndex;

        do
        {
            positionIndex = Random.Range(0, availableTiles.Count);
            position = availableTiles[positionIndex];
            minDistance -= 2;
        }
        while (isPositionFreeInArea(position, minDistance));

        foreach (LevelGenMap.Coordinate tile in _map.CoordinatesInManhattanRange(position, areaToClear))
        {
            availableTiles.Remove(tile);
        }
        return position;
    }

    private bool isPositionFreeInArea(LevelGenMap.Coordinate position, int minDistance)
    {
        foreach (EnemySpawn enemySpawn in _enemySpawns)
        {
            if (Vector2.Distance(position.integerVector, enemySpawn.SpawnPosition) < minDistance)
                return false;
        }
        return true;
    }
}
