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
        foreach (IntegerVector position in _playerSpawns)
        {
            //TODO - Hand control of spawned player object to appropriate player
            GameObject player = Instantiate(this.PlayerPrefab, new Vector3(position.X * _tileRenderer.TileRenderSize, position.Y * _tileRenderer.TileRenderSize, this.transform.position.z), Quaternion.identity) as GameObject;
            if (_tileRenderer.OffsetTilesToCenter)
                player.transform.position = new Vector3(player.transform.position.x - _tileRenderer.TileRenderSize * _tileRenderer.Width / 2, player.transform.position.y - _tileRenderer.TileRenderSize * _tileRenderer.Height / 2, player.transform.position.z);

            _targets.Add(player.transform);
            Camera.main.GetComponent<CameraController>().CenterTarget = player.transform;
        }

        /*if (this.PickupPrefabs.Length > 0)
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
        }*/
    }

    public void SpawnEnemies()
    {
        WinCondition winCondition = this.GetComponent<WinCondition>();

        foreach (EnemySpawn enemySpawn in _enemySpawns)
        {
            IntegerVector position = enemySpawn.SpawnPosition;
            GameObject spawner = Instantiate(this.EnemySpawnPrefab, new Vector3(position.X * _tileRenderer.TileRenderSize, position.Y * _tileRenderer.TileRenderSize, this.transform.position.z), Quaternion.identity) as GameObject;
            if (_tileRenderer.OffsetTilesToCenter)
                spawner.transform.position = new Vector3(spawner.transform.position.x - _tileRenderer.TileRenderSize * _tileRenderer.Width / 2, spawner.transform.position.y - _tileRenderer.TileRenderSize * _tileRenderer.Height / 2, spawner.transform.position.z);

            EnemySpawner spawnBehavior = spawner.GetComponent<EnemySpawner>();
            if (winCondition != null)
                spawnBehavior.SpawnCallback = winCondition.EnemySpawned;
            spawnBehavior.Targets = _targets.ToArray();
            spawnBehavior.DestroyAfterSpawn = true;
            spawnBehavior.SpawnPool = new int[] { enemySpawn.EnemyId };
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
        this.NumEnemies = Random.Range(output.Input.NumEnemiesRange.X, output.Input.NumEnemiesRange.Y);
        int numPlayers = 1;
        int[] guaranteedEnemiesPlaced = new int[output.Input.GuaranteedEnemiesByDifficulty.Length];

        if (openTiles.Count <= (this.NumEnemies + numPlayers) * output.Input.MinDistanceBetweenSpawns * 2+ 1)
        {
            //TODO - Regenerate level
            Debug.Log("Regeneration necessary - CA");
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
            for (; enemiesSpawned < this.NumEnemies; ++enemiesSpawned)
            {
                int enemyId = enemySelector.ChooseEnemy(difficulty);
                _enemySpawns.Add(new EnemySpawn(findGoodOpenPosition(openTiles, output.Input.MinDistanceBetweenSpawns).integerVector, enemyId));
            }

            // Players
            //TODO - Replace '1' with number of players
            for (int i = 0; i < 1; ++i)
            {
                _playerSpawns.Add(findGoodOpenPosition(openTiles, output.Input.MinDistanceBetweenSpawns).integerVector);
            }
        }
    }

    private void findBSPSpawns(LevelGenOutput output)
    {
        List<LevelGenMap.Coordinate> openTiles = new List<LevelGenMap.Coordinate>(output.OpenTiles);
        openTiles.Shuffle();
        LevelGenRoomInfo roomInfo = output.MapInfo[LevelGenRoomInfo.KEY] as LevelGenRoomInfo;
        EnemySelector enemySelector = new EnemySelector();

        int difficulty = DynamicData.GetCurrentDifficulty();
        this.NumEnemies = Random.Range(output.Input.NumEnemiesRange.X, output.Input.NumEnemiesRange.Y);
        int numPlayers = 1;
        int[] guaranteedEnemiesPlaced = new int[output.Input.GuaranteedEnemiesByDifficulty.Length];
        int totalGuarantees = 0;
        for (int i = 0; i < guaranteedEnemiesPlaced.Length; ++i)
        {
            totalGuarantees += output.Input.GuaranteedEnemiesByDifficulty[i];
        }
        
        if (openTiles.Count <= (this.NumEnemies + numPlayers) * output.Input.MinDistanceBetweenSpawns * 2 + 1 ||
        roomInfo == null || roomInfo.Data.Count < 3 + difficulty)
        {
            //TODO - Regenerate level
            Debug.Log("Regeneration necessary - BSP 1");
        }
        else
        {
            List<SimpleRect> availableRooms = new List<SimpleRect>(roomInfo.Data as List<SimpleRect>);
            availableRooms.Shuffle();

            // Player room
            SimpleRect playerRoom = availableRooms[availableRooms.Count - 1];
            availableRooms.RemoveAt(availableRooms.Count - 1);
            removeRoomFromCoordinates(playerRoom, openTiles);
            //TODO - add player positions from room

            if (openTiles.Count <= this.NumEnemies * output.Input.MinDistanceBetweenSpawns * 2 + 1)
            {
                //TODO - Regenerate level
                Debug.Log("Regeneration necessary - BSP 2");
            }
            else
            {
                int enemiesSpawned = 0;
                int guaranteesSpawned = 0;
                bool haveUnitedAllRoomsSoFar = true;

                // Enemy rooms
                for (int r = 0; r < availableRooms.Count; ++r)
                {
                    SimpleRect room = availableRooms[r];
                    if (this.NumEnemies - enemiesSpawned < 4 || (r == availableRooms.Count - 1 && haveUnitedAllRoomsSoFar && guaranteesSpawned < totalGuarantees))
                        break;
                    if (Random.value > 0.75f)
                    {
                        haveUnitedAllRoomsSoFar = false;
                        continue;
                    }

                    EnemySelector.WeightSet roomWeightSet = new EnemySelector.WeightSet();
                    enemySelector.AddWeightSet(roomWeightSet);

                    // United-spawn room
                    int favoredEnemyId = pickMaybeGuaranteedEnemy(guaranteesSpawned, totalGuarantees, enemiesSpawned, difficulty, guaranteedEnemiesPlaced, output, enemySelector);

                    roomWeightSet.WeightsByEnemyId[favoredEnemyId] = 100;

                    List<IntegerVector> roomCorners = new List<IntegerVector>();
                    roomCorners.Add(new IntegerVector(room.X, room.Y));
                    roomCorners.Add(new IntegerVector(room.X + room.Width, room.Y));
                    roomCorners.Add(new IntegerVector(room.X, room.Y + room.Height));
                    roomCorners.Add(new IntegerVector(room.X + room.Width, room.Y + room.Height));
                    roomCorners.Shuffle();

                    IntegerVector firstPosition = roomCorners[roomCorners.Count - 1];
                    ++enemiesSpawned;
                    //TODO - mark spawn as united spawn so enemies all spawn when enter room
                    _enemySpawns.Add(new EnemySpawn(firstPosition, favoredEnemyId));
                    roomCorners.RemoveAt(roomCorners.Count - 1);
                    int enemyDifficulty = StaticData.EnemyData.EnemyTypes[favoredEnemyId].Difficulty;
                    if (guaranteedEnemiesPlaced[enemyDifficulty] < output.Input.GuaranteedEnemiesByDifficulty[enemyDifficulty])
                    {
                        guaranteedEnemiesPlaced[enemyDifficulty] += 1;
                        ++guaranteesSpawned;
                    }

                    foreach (IntegerVector position in roomCorners)
                    {
                        ++enemiesSpawned;
                        int enemyId = enemySelector.ChooseEnemy(difficulty);
                        _enemySpawns.Add(new EnemySpawn(position, enemyId));
                        enemyDifficulty = StaticData.EnemyData.EnemyTypes[enemyId].Difficulty;
                        if (guaranteedEnemiesPlaced[enemyDifficulty] < output.Input.GuaranteedEnemiesByDifficulty[enemyDifficulty])
                        {
                            guaranteedEnemiesPlaced[enemyDifficulty] += 1;
                            ++guaranteesSpawned;
                        }
                    }

                    if (this.NumEnemies - enemiesSpawned > 2 && Random.value > 0.5f)
                    {
                        // Let's add 2 along the walls of the longest room dimension
                        favoredEnemyId = pickMaybeGuaranteedEnemy(guaranteesSpawned, totalGuarantees, enemiesSpawned, difficulty, guaranteedEnemiesPlaced, output, enemySelector);
                        roomWeightSet.WeightsByEnemyId[favoredEnemyId] = 100;

                        IntegerVector position1;
                        IntegerVector position2;
                        if (room.Width > room.Height)
                        {
                            position1 = new IntegerVector(room.X + room.Width / 2, room.Y);
                            position2 = new IntegerVector(room.X + room.Width / 2, room.Y + room.Height);
                        }
                        else
                        {
                            position1 = new IntegerVector(room.X, room.Y + room.Height / 2);
                            position2 = new IntegerVector(room.X + room.Width, room.Y + room.Height / 2);
                        }

                        enemiesSpawned += 2;
                        //TODO - mark spawn as united spawn so enemies all spawn when enter room
                        _enemySpawns.Add(new EnemySpawn(position1, favoredEnemyId));
                        enemyDifficulty = StaticData.EnemyData.EnemyTypes[favoredEnemyId].Difficulty;
                        if (guaranteedEnemiesPlaced[enemyDifficulty] < output.Input.GuaranteedEnemiesByDifficulty[enemyDifficulty])
                        {
                            guaranteedEnemiesPlaced[enemyDifficulty] += 1;
                            ++guaranteesSpawned;
                        }

                        int enemyId = enemySelector.ChooseEnemy(difficulty);
                        _enemySpawns.Add(new EnemySpawn(position2, enemyId));
                        enemyDifficulty = StaticData.EnemyData.EnemyTypes[enemyId].Difficulty;
                        if (guaranteedEnemiesPlaced[enemyDifficulty] < output.Input.GuaranteedEnemiesByDifficulty[enemyDifficulty])
                        {
                            guaranteedEnemiesPlaced[enemyDifficulty] += 1;
                            ++guaranteesSpawned;
                        }
                    }

                    if (Random.value > 0.5f)
                        removeRoomFromCoordinates(room, openTiles);
                }

                // Non united-room spawns
                //TODO
            }
        }

        /**
        If number of rooms is less than 2 + difficulty level, regenerate level. Place player in a room and remove the other open positions in that room from placement pool. If number of remaining open positions is less than number of enemies * 10, regenerate level. For each remaining room, 75% chance to set as united-spawn room (enemies placed "intelligently", spawn simulatneously as player enters room). After designating a room as united-spawn, there is a 50% chance to remove the rest of the positions in that room from valid placement pool (so it will only ever spawn the united-spawn enemies). Otherwise enemy positions and positions within 1 range of those are removed. If get to last room AND all rooms so far have been united-spawn rooms AND there are still "guaranteed picks" remaining, OR have run through all rooms and still have enemies to place OR still going through rooms but don't have 4 or more enemies still to place, halt room  iteration and place remaining enemies via CA's "good placement" approach (but only remove positions within 1 range).
	Method for "intelligently" placing united-spawn enemies in room:
	1. 1 enemy in each of the 4 corners, 1 space in from each wall. The first enemy placed gets temporary boosted weight for rest of united-spawn placement in this room.
	2. If enemies to place still > 2, and length of room >= 180, place enemies at midpoints along east and west walls, 1 space in from wall.
	3. Do same as step 2 for width/north, south walls.
    **/

        //TODO - remove
        //findCASpawns(output);
    }

    private int pickMaybeGuaranteedEnemy(int guaranteesSpawned, int totalGuarantees, int enemiesSpawned, int difficulty, int[] guaranteedEnemiesPlaced, LevelGenOutput output, EnemySelector enemySelector)
    {
        if (guaranteesSpawned < totalGuarantees && Random.Range(0, this.NumEnemies) < enemiesSpawned + (totalGuarantees - guaranteesSpawned))
        {
            // Use guarantee for united-spawn
            int guaranteedDifficulty = 0;
            for (int i = 0; i < guaranteedEnemiesPlaced.Length; ++i)
            {
                if (guaranteedEnemiesPlaced[i] < output.Input.GuaranteedEnemiesByDifficulty[i])
                {
                    guaranteedDifficulty = i;
                    if (Random.value > 0.5f)
                        break;
                }
            }
            return enemySelector.ChooseEnemyOfDifficulty(guaranteedDifficulty);
        }

        return enemySelector.ChooseEnemy(difficulty);
    }

    private void removeRoomFromCoordinates(SimpleRect room, List<LevelGenMap.Coordinate> coordinates)
    {
        for (int x = room.X; x < room.X + room.Width; ++x)
        {
            for (int y = room.Y; y < room.Y + room.Height; ++y)
            {
                LevelGenMap.Coordinate? coord = _map.ConstructValidCoordinate(x, y, false);
                if (coord.HasValue)
                    coordinates.Remove(coord.Value);
            }
        }
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
        while (!isPositionFreeInArea(position, minDistance));

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
