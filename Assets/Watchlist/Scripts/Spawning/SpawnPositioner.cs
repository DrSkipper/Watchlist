using UnityEngine;
using System.Collections.Generic;

public class SpawnPositioner : VoBehavior
{
    public GameObject LevelGenerator;
    public GameObject Tiles;
    public GameObject EnemySpawnPrefab;
    public GameObject MinibossSpawnPrefab;
    public GameObject PlayerPrefab;
    public GameObject[] PickupPrefabs;
    public int NumEnemies = 10;
    public int NumPickups = 1;
    public float SpawnPlayersDelay = 1.0f;
    public float SpawnEnemiesDelay = 3.0f;
    public int MaxPlayerSpawnDistance = 10;
    public bool WaitForGameplayBeginEvent = false;
    public float PlayerInteractionDelay = 0.0f;

    public bool SpawnersPlaced { get { return _readyToSpawn; } }

    void Awake()
    {
        _winCondition = this.GetComponent<WinCondition>();
    }

    void Start()
    {
        _levelGenManager = this.LevelGenerator.GetComponent<LevelGenManager>();
        _starter = this.LevelGenerator.GetComponent<LevelGenStarter>();
        _map = this.LevelGenerator.GetComponent<LevelGenMap>();
        _tileRenderer = this.Tiles.GetComponent<TileMapOutlineRenderer>();
        _levelGenManager.AddUpdateDelegate(this.LevelGenUpdate);

        if (this.WaitForGameplayBeginEvent)
            GlobalEvents.Notifier.Listen(BeginGameplayEvent.NAME, this, okToBegin);
        else
            _okToBegin = true;

        if (_tileRenderer.OffsetTilesToCenter)
            this.transform.position = new Vector3(0, 0, this.transform.position.z);
    }

    void LevelGenUpdate()
    {
        if (_levelGenManager.Finished)
        {
            LevelGenOutput output = _levelGenManager.GetOutput();
            _targets = new List<Transform>();
            _enemySpawns = new List<EnemySpawnGroup>();
            _playerSpawns = new List<IntegerVector>();
            bool generationOk = false;

            switch (output.Input.Type)
            {
                default:
                case LevelGenInput.GenerationType.CA:
                    generationOk = findCASpawns(output);
                    break;
                case LevelGenInput.GenerationType.BSP:
                    generationOk = findBSPSpawns(output);
                    break;
                case LevelGenInput.GenerationType.CABSPCombo:
                    generationOk = findBSPCAComboSpawns(output);
                    break;
            }
            if (generationOk)
            {
                if (_okToBegin)
                {
                    _okToBegin = false;
                    begin();
                }
                else
                {
                    _readyToSpawn = true;
                }
            }
            else
            {
                _starter.BeginGeneration();
            }
        }
    }

    public void SpawnPlayers()
    {
        int playerIndex = 0;
        foreach (IntegerVector position in _playerSpawns)
        {
            GameObject player = Instantiate(this.PlayerPrefab, new Vector3(position.X * _tileRenderer.TileRenderSize, position.Y * _tileRenderer.TileRenderSize, this.transform.position.z), Quaternion.identity) as GameObject;
            if (_tileRenderer.OffsetTilesToCenter)
                player.transform.position = new Vector3(player.transform.position.x - _tileRenderer.TileRenderSize * _tileRenderer.Width / 2, player.transform.position.y - _tileRenderer.TileRenderSize * _tileRenderer.Height / 2, player.transform.position.z);

            _targets.Add(player.transform);

            for (;  playerIndex < DynamicData.MAX_PLAYERS; ++playerIndex)
            {
                if (DynamicData.GetSessionPlayer(playerIndex).HasJoined)
                    break;
            }
            player.GetComponent<PlayerController>().PlayerIndex = playerIndex;
            if (this.PlayerInteractionDelay > 0.0f)
                player.GetComponent<PlayerController>().SetInteractionDelay(this.PlayerInteractionDelay);

            GlobalEvents.Notifier.SendEvent(new PlayerSpawnedEvent(player, playerIndex));
            ++playerIndex;
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
        foreach (EnemySpawnGroup enemySpawn in _enemySpawns)
        {
            if (!enemySpawn.IsMultiSpawn)
            {
                createEnemySpawner(enemySpawn.SingleSpawn.Value.SpawnPosition, enemySpawn.SingleSpawn.Value.EnemyId, _targets, false);
            }
            else
            {
                List<EnemySpawner> subSpawns = new List<EnemySpawner>();
                for (int i = 0; i < enemySpawn.MultiSpawns.Count; ++i)
                {
                    subSpawns.Add(createEnemySpawner(enemySpawn.MultiSpawns[i].SpawnPosition, enemySpawn.MultiSpawns[i].EnemyId, _targets, true));
                }

                EnemySpawner parent = createEnemySpawner(enemySpawn.Origin, 0, _targets, false);
                parent.ChildSpawners = subSpawns;
                parent.MinDistanceToSpawn = enemySpawn.SpawnDistance;
            }
        }

        if (_miniBossSpawn.HasValue)
        {
            createMinibossSpawner(_miniBossSpawn.Value);
        }
    }

    private EnemySpawner createEnemySpawner(IntegerVector position, int enemyId, List<Transform> targets, bool isChildSpawner)
    {
        GameObject spawner = Instantiate(this.EnemySpawnPrefab, new Vector3(position.X * _tileRenderer.TileRenderSize, position.Y * _tileRenderer.TileRenderSize, this.transform.position.z), Quaternion.identity) as GameObject;
        if (_tileRenderer.OffsetTilesToCenter)
            spawner.transform.position = new Vector3(spawner.transform.position.x - _tileRenderer.TileRenderSize * _tileRenderer.Width / 2, spawner.transform.position.y - _tileRenderer.TileRenderSize * _tileRenderer.Height / 2, spawner.transform.position.z);

        EnemySpawner spawnBehavior = spawner.GetComponent<EnemySpawner>();
        if (_winCondition != null)
            spawnBehavior.SpawnCallback = _winCondition.EnemySpawned;
        spawnBehavior.Rule = EnemySpawner.SpawnRule.LineOfSight;
        spawnBehavior.IsChildSpawner = isChildSpawner;
        spawnBehavior.Targets = new List<Transform>(_targets);
        spawnBehavior.DestroyAfterSpawn = true;
        spawnBehavior.SpawnPool = new int[] { enemyId };
        return spawnBehavior;
    }

    private GameObject createMinibossSpawner(IntegerVector position)
    {
        GameObject spawner = Instantiate(this.MinibossSpawnPrefab, new Vector3(position.X * _tileRenderer.TileRenderSize, position.Y * _tileRenderer.TileRenderSize, this.transform.position.z), Quaternion.identity) as GameObject;
        this.NumEnemies += 1;
        if (_winCondition != null)
            spawner.GetComponent<BossSpawner>().SpawnCallback = _winCondition.EnemySpawned;
        if (_tileRenderer.OffsetTilesToCenter)
            spawner.transform.position = new Vector3(spawner.transform.position.x - _tileRenderer.TileRenderSize * _tileRenderer.Width / 2, spawner.transform.position.y - _tileRenderer.TileRenderSize * _tileRenderer.Height / 2, spawner.transform.position.z);
        return spawner;
    }

    public Dictionary<int, int> GetEnemyCounts()
    {
        Dictionary<int, int> enemyCounts = new Dictionary<int, int>();
        for (int i = 0; i < _enemySpawns.Count; ++i)
        {
            if (_enemySpawns[i].IsMultiSpawn)
            {
                for (int j = 0; j < _enemySpawns[i].MultiSpawns.Count; ++j)
                {
                    int enemyId = _enemySpawns[i].MultiSpawns[j].EnemyId;
                    if (enemyCounts.ContainsKey(enemyId))
                        enemyCounts[enemyId] = enemyCounts[enemyId] + 1;
                    else
                        enemyCounts.Add(enemyId, 1);
                }
            }
            else
            {
                int enemyId = _enemySpawns[i].SingleSpawn.Value.EnemyId;
                if (enemyCounts.ContainsKey(enemyId))
                    enemyCounts[enemyId] = enemyCounts[enemyId] + 1;
                else
                    enemyCounts.Add(enemyId, 1);
            }
        }
        return enemyCounts;
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

    private struct EnemySpawnGroup
    {
        public bool IsMultiSpawn { get { return this.MultiSpawns != null; } }
        public EnemySpawn? SingleSpawn;
        public IntegerVector Origin;
        public List<EnemySpawn> MultiSpawns;
        public float SpawnDistance;

        public EnemySpawnGroup(IntegerVector spawnPosition, int enemyId)
        {
            this.SingleSpawn = new EnemySpawn(spawnPosition, enemyId);
            this.Origin = spawnPosition;
            this.MultiSpawns = null;
            this.SpawnDistance = 0.0f;
        }

        public EnemySpawnGroup(IntegerVector origin, List<EnemySpawn> spawns, float spawnDistance)
        {
            this.SingleSpawn = null;
            this.Origin = origin;
            this.MultiSpawns = spawns;
            this.SpawnDistance = spawnDistance;
        }
    }

    private LevelGenManager _levelGenManager;
    private LevelGenStarter _starter;
    private LevelGenMap _map;
    private TileMapOutlineRenderer _tileRenderer;
    private List<Transform> _targets;
    private List<EnemySpawnGroup> _enemySpawns;
    private List<IntegerVector> _playerSpawns;
    private IntegerVector? _miniBossSpawn;
    private WinCondition _winCondition;
    private bool _okToBegin;
    private bool _readyToSpawn;

    private const float CHANCE_FOR_EXTRA_TWO_IN_BSP_ROOM = 0.2f;
    private const float CHANCE_FOR_REMOVE_SPAWN_ROOM_FOR_FUTURE_BSP = 0.4f;

    private void okToBegin(LocalEventNotifier.Event e)
    {
        if (_readyToSpawn)
        {
            _readyToSpawn = false;
            begin();
        }
        else
        {
            _okToBegin = true;
        }
    }

    private void begin()
    {
        TimedCallbacks callbacks = this.GetComponent<TimedCallbacks>();
        callbacks.AddCallback(this, this.SpawnPlayers, this.SpawnPlayersDelay);
        callbacks.AddCallback(this, this.SpawnEnemies, this.SpawnEnemiesDelay);
    }

    private bool findCASpawns(LevelGenOutput output)
    {
        List<LevelGenMap.Coordinate> openTiles = new List<LevelGenMap.Coordinate>(output.OpenTiles);
        openTiles.Shuffle();
        EnemySelector enemySelector = new EnemySelector();

        int difficulty = ProgressData.GetCurrentDifficulty();
        IntegerVector enemyCountRange = output.Input.GetCurrentNumEnemiesRange();
        int[] guaranteedEnemiesPlaced = new int[output.Input.GuaranteedEnemiesByDifficulty.Length];
        this.NumEnemies = Random.Range(enemyCountRange.X, enemyCountRange.Y + 1);
        LevelGenCaveInfo caveInfo = output.MapInfo[LevelGenCaveInfo.KEY] as LevelGenCaveInfo;

        if (ProgressData.IsMiniBoss(ProgressData.MostRecentTile))
            findMinibossSpawn(openTiles, (caveInfo.Data as List<List<LevelGenMap.Coordinate>>)[0]);

        if (openTiles.Count <= (this.NumEnemies + DynamicData.MAX_PLAYERS) * output.Input.MinDistanceBetweenSpawns * 2 + 1)
        {
            Debug.Log("Regeneration necessary - CA");
            return false;
        }
        else
        {
            spawnSimple(0, output, guaranteedEnemiesPlaced, enemySelector, difficulty, openTiles, true);
        }
        return true;
    }

    private bool findBSPSpawns(LevelGenOutput output)
    {
        List<LevelGenMap.Coordinate> openTiles = new List<LevelGenMap.Coordinate>(output.OpenTiles);
        openTiles.Shuffle();

        LevelGenRoomInfo roomInfo = output.MapInfo[LevelGenRoomInfo.KEY] as LevelGenRoomInfo;
        EnemySelector enemySelector = new EnemySelector();
        IntegerVector enemyCountRange = output.Input.GetCurrentNumEnemiesRange();
        this.NumEnemies = Random.Range(enemyCountRange.X, enemyCountRange.Y + 1);

        if (ProgressData.IsMiniBoss(ProgressData.MostRecentTile))
            findMinibossSpawn(openTiles, openTiles);

        int difficulty = ProgressData.GetCurrentDifficulty();
        int[] guaranteedEnemiesPlaced = new int[output.Input.GuaranteedEnemiesByDifficulty.Length];
        int totalGuarantees = 0;
        for (int i = 0; i < guaranteedEnemiesPlaced.Length; ++i)
        {
            totalGuarantees += output.Input.GuaranteedEnemiesByDifficulty[i];
        }
        
        if (openTiles.Count <= (this.NumEnemies + DynamicData.MAX_PLAYERS) * output.Input.MinDistanceBetweenSpawns * 2 + 1 ||
        roomInfo == null || roomInfo.Data.Count < 4 + difficulty)
        {
            Debug.Log("Regeneration necessary - BSP 1");
            return false;
        }
        else
        {
            List<SimpleRect> availableRooms = new List<SimpleRect>(roomInfo.Data as List<SimpleRect>);
            availableRooms.Shuffle();

            // Player room
            SimpleRect playerRoom = availableRooms[availableRooms.Count - 1];
            availableRooms.RemoveAt(availableRooms.Count - 1);
            List<LevelGenMap.Coordinate> playerRoomCoords = coordinatesInRoom(playerRoom);
            openTiles.RemoveList(playerRoomCoords);
            playerRoomCoords.Shuffle();
            
            for (int p = 0; p < DynamicData.MAX_PLAYERS; ++p)
            {
                if (DynamicData.GetSessionPlayer(p).HasJoined)
                {
                    _playerSpawns.Add(playerRoomCoords[playerRoomCoords.Count - 1].integerVector);
                    playerRoomCoords.RemoveAt(playerRoomCoords.Count - 1);
                }
            }

            if (openTiles.Count <= this.NumEnemies * output.Input.MinDistanceBetweenSpawns * 2 + 1)
            {
                Debug.Log("Regeneration necessary - BSP 2");
                return false;
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

                    List<EnemySpawn> roomSpawns = new List<EnemySpawn>();

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
                    roomSpawns.Add(new EnemySpawn(firstPosition, favoredEnemyId));
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
                        roomSpawns.Add(new EnemySpawn(position, enemyId));
                        enemyDifficulty = StaticData.EnemyData.EnemyTypes[enemyId].Difficulty;
                        if (guaranteedEnemiesPlaced[enemyDifficulty] < output.Input.GuaranteedEnemiesByDifficulty[enemyDifficulty])
                        {
                            guaranteedEnemiesPlaced[enemyDifficulty] += 1;
                            ++guaranteesSpawned;
                        }
                    }

                    bool extraTwo = false;
                    if (this.NumEnemies - enemiesSpawned > 2 && Random.value < CHANCE_FOR_EXTRA_TWO_IN_BSP_ROOM)
                    {
                        // Let's add 2 along the walls of the longest room dimension
                        extraTwo = true;
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
                        roomSpawns.Add(new EnemySpawn(position1, favoredEnemyId));
                        enemyDifficulty = StaticData.EnemyData.EnemyTypes[favoredEnemyId].Difficulty;
                        if (guaranteedEnemiesPlaced[enemyDifficulty] < output.Input.GuaranteedEnemiesByDifficulty[enemyDifficulty])
                        {
                            guaranteedEnemiesPlaced[enemyDifficulty] += 1;
                            ++guaranteesSpawned;
                        }

                        int enemyId = enemySelector.ChooseEnemy(difficulty);
                        roomSpawns.Add(new EnemySpawn(position2, enemyId));
                        enemyDifficulty = StaticData.EnemyData.EnemyTypes[enemyId].Difficulty;
                        if (guaranteedEnemiesPlaced[enemyDifficulty] < output.Input.GuaranteedEnemiesByDifficulty[enemyDifficulty])
                        {
                            guaranteedEnemiesPlaced[enemyDifficulty] += 1;
                            ++guaranteesSpawned;
                        }
                    }

                    _enemySpawns.Add(new EnemySpawnGroup(new IntegerVector(room.X + room.Width / 2, room.Y + room.Height / 2), roomSpawns, ((Mathf.Max(room.Width, room.Height) + 2.6f) / 2.0f) * _tileRenderer.TileRenderSize));

                    if (!enemySelector.RemoveWeightSet(roomWeightSet))
                    {
                        Debug.Log("hrrmmmm");
                    }

                    if (extraTwo || Random.value < CHANCE_FOR_REMOVE_SPAWN_ROOM_FOR_FUTURE_BSP)
                        openTiles.RemoveList(coordinatesInRoom(room));
                }

                // Non united-room spawns
                spawnSimple(enemiesSpawned, output, guaranteedEnemiesPlaced, enemySelector, difficulty, openTiles, false);
            }
        }

        return true;
    }

    private bool findBSPCAComboSpawns(LevelGenOutput output)
    {
        List<LevelGenMap.Coordinate> openTiles = new List<LevelGenMap.Coordinate>(output.OpenTiles);
        openTiles.Shuffle();
        EnemySelector enemySelector = new EnemySelector();

        int difficulty = ProgressData.GetCurrentDifficulty();
        IntegerVector enemyCountRange = output.Input.GetCurrentNumEnemiesRange();
        int[] guaranteedEnemiesPlaced = new int[output.Input.GuaranteedEnemiesByDifficulty.Length];
        this.NumEnemies = Random.Range(enemyCountRange.X, enemyCountRange.Y + 1);
        LevelGenCaveInfo caveInfo = output.MapInfo[LevelGenCaveInfo.KEY] as LevelGenCaveInfo;

        if (ProgressData.IsMiniBoss(ProgressData.MostRecentTile))
            findMinibossSpawn(openTiles, (caveInfo.Data as List<List<LevelGenMap.Coordinate>>)[0]);

        LevelGenRoomInfo roomInfo = output.MapInfo[LevelGenRoomInfo.KEY] as LevelGenRoomInfo;

        List<SimpleRect> availableRooms = new List<SimpleRect>(roomInfo.Data as List<SimpleRect>);
        availableRooms.Shuffle();

        // Player room
        SimpleRect playerRoom = availableRooms[availableRooms.Count - 1];
        availableRooms.RemoveAt(availableRooms.Count - 1);
        List<LevelGenMap.Coordinate> playerRoomCoords = coordinatesInRoom(playerRoom);
        openTiles.RemoveList(playerRoomCoords);
        playerRoomCoords.Shuffle();

        for (int p = 0; p < DynamicData.MAX_PLAYERS; ++p)
        {
            if (DynamicData.GetSessionPlayer(p).HasJoined)
            {
                _playerSpawns.Add(playerRoomCoords[playerRoomCoords.Count - 1].integerVector);
                playerRoomCoords.RemoveAt(playerRoomCoords.Count - 1);
            }
        }

        if (openTiles.Count <= (this.NumEnemies + DynamicData.MAX_PLAYERS) * output.Input.MinDistanceBetweenSpawns * 2 + 1)
        {
            Debug.Log("Regeneration necessary - CA");
            return false;
        }
        else
        {
            spawnSimple(0, output, guaranteedEnemiesPlaced, enemySelector, difficulty, openTiles, false);
        }
        return true;
    }

    private void spawnSimple(int enemiesSpawned, LevelGenOutput output, int[] guaranteedEnemiesPlaced, EnemySelector enemySelector, int difficulty, List<LevelGenMap.Coordinate> openTiles, bool spawnPlayers)
    {
        // Guaranteed enemies
        for (int i = 0; i < output.Input.GuaranteedEnemiesByDifficulty.Length; ++i)
        {
            while (guaranteedEnemiesPlaced[i] < output.Input.GuaranteedEnemiesByDifficulty[i])
            {
                int enemyId = enemySelector.ChooseEnemyOfDifficulty(i);
                guaranteedEnemiesPlaced[i] = guaranteedEnemiesPlaced[i] + 1;
                ++enemiesSpawned;
                _enemySpawns.Add(new EnemySpawnGroup(findGoodOpenPosition(openTiles, output.Input.MinDistanceBetweenSpawns).integerVector, enemyId));
            }
        }

        // Remaining enemies
        for (; enemiesSpawned < this.NumEnemies; ++enemiesSpawned)
        {
            int enemyId = enemySelector.ChooseEnemy(difficulty);
            _enemySpawns.Add(new EnemySpawnGroup(findGoodOpenPosition(openTiles, output.Input.MinDistanceBetweenSpawns).integerVector, enemyId));
        }

        // Players
        if (spawnPlayers)
        {
            bool first = true;
            List<LevelGenMap.Coordinate> tiles = openTiles;
            for (int i = 0; i < DynamicData.MAX_PLAYERS; ++i)
            {
                if (DynamicData.GetSessionPlayer(i).HasJoined)
                {
                    LevelGenMap.Coordinate playerSpawn = findGoodOpenPosition(tiles, 0);
                    _playerSpawns.Add(playerSpawn.integerVector);

                    if (first)
                    {
                        first = false;
                        List<LevelGenMap.Coordinate> nearbySpawns = new List<LevelGenMap.Coordinate>();
                        foreach (LevelGenMap.Coordinate coord in openTiles)
                        {
                            if (Mathf.Abs(coord.x - playerSpawn.x) + Mathf.Abs(coord.y - playerSpawn.y) <= this.MaxPlayerSpawnDistance)
                                nearbySpawns.Add(coord);
                        }

                        if (nearbySpawns.Count >= DynamicData.MAX_PLAYERS)
                            tiles = nearbySpawns;
                    }
                    else
                    {
                        openTiles.Remove(playerSpawn);
                    }
                }
            }
        }
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

    private List<LevelGenMap.Coordinate> coordinatesInRoom(SimpleRect room)
    {
        List<LevelGenMap.Coordinate> roomCoords = new List<LevelGenMap.Coordinate>();
        for (int x = room.X; x < room.X + room.Width; ++x)
        {
            for (int y = room.Y; y < room.Y + room.Height; ++y)
            {
                LevelGenMap.Coordinate? coord = _map.ConstructValidCoordinate(x, y, false);
                if (coord.HasValue)
                    roomCoords.Add(coord.Value);
            }
        }
        return roomCoords;
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
        foreach (EnemySpawnGroup enemySpawn in _enemySpawns)
        {
            if (Vector2.Distance(position.integerVector, enemySpawn.Origin) < minDistance)
                return false;
        }
        return true;
    }

    private void findMinibossSpawn(List<LevelGenMap.Coordinate> openTiles, List<LevelGenMap.Coordinate> placable)
    {
        List<LevelGenMap.Coordinate> neighbors;
        for (int i = 0; i < openTiles.Count; ++i)
        {
            
            neighbors = _map.CoordinatesInRect(new Rect(placable[i].x - 1, placable[i].y - 1, 3, 3));
            if (neighbors.Count < 9)
                continue;

            bool wall = false;
            for (int j = 0; j < neighbors.Count; ++j)
            {
                if (!openTiles.Contains(neighbors[j]))
                {
                    wall = true;
                    break;
                }
            }

            if (wall)
                continue;

            _miniBossSpawn = placable[i].integerVector;

            for (int j = 0; j < neighbors.Count; ++j)
            {
                openTiles.Remove(neighbors[j]);
            }
            return;
        }

        _miniBossSpawn = placable[0].integerVector;
    }
}
