using UnityEngine;

[RequireComponent(typeof(TimedCallbacks))]
public class ScriptedSpawnController : VoBehavior
{
    public EnemySpawner[] EnemySpawners;
    public Transform[] PlayerSpawns;
    public GameObject PlayerPrefab;
    public float TimeBetweenWaves = 0.0f;
    public float InitialDelay = 1.0f; // Should be longer than SpawnPlayersDelay if spawning players
    public bool ShouldSpawnPlayers = true;
    public float SpawnPlayersDelay = 0.2f;
    public bool NoFireForPlayers = false;
    public bool WaitForGameplayBeginEvent = false;

	void Start()
    {
        _waveCooldown = this.InitialDelay;
        if (this.ShouldSpawnPlayers && this.PlayerSpawns.Length > 0)
        {
            if (this.WaitForGameplayBeginEvent)
                GlobalEvents.Notifier.Listen(BeginGameplayEvent.NAME, this, gameplayBegin);
            else
                gameplayBegin(null);
        }
	}

    private void gameplayBegin(LocalEventNotifier.Event e)
    {
        this.GetComponent<TimedCallbacks>().AddCallback(this, SpawnPlayers, this.SpawnPlayersDelay);
    }

    void Update()
    {
        if (this.EnemySpawners.Length > 0 && enemyCount <= 0)
        {
            if (_waveCooldown <= 0.0f)
            {
                foreach (EnemySpawner spawner in this.EnemySpawners)
                {
                    if (spawner != null)
                    {
                        spawner.SpawnCallback = this.EnemySpawned;
                        spawner.BeginSpawn();
                    }
                }

                _waveCooldown = this.TimeBetweenWaves;
            }
            else
            {
                _waveCooldown -= Time.deltaTime;
            }
        }
	}

    public void EnemySpawned(GameObject enemy)
    {
        enemy.GetComponent<Damagable>().OnDeathCallbacks.Add(this.EnemyDied);
        ++enemyCount;
    }

    public void EnemyDied(Damagable died)
    {
        --enemyCount;
    }

    public void SpawnPlayers()
    {
        int t = 0;
        for (int p = 0; p < DynamicData.MAX_PLAYERS; ++p)
        {
            SessionPlayer player = DynamicData.GetSessionPlayer(p);
            if (player.HasJoined)
            {
                while (t < this.PlayerSpawns.Length && this.PlayerSpawns[t] == null)
                {
                    ++t;
                }
                if (t < this.PlayerSpawns.Length)
                {
                    spawnPlayer(player, this.PlayerSpawns[t]);
                }
                else
                {
                    break;
                }
            }
        }
    }

    /**
     * Private
     */
    private int enemyCount;
    private float _waveCooldown;

    private void spawnPlayer(SessionPlayer sessionPlayer, Transform spawn)
    {
        IntegerVector position = new IntegerVector(spawn.position);
        GameObject player = Instantiate(this.PlayerPrefab, new Vector3(position.X, position.Y, this.transform.position.z), Quaternion.identity) as GameObject;

        foreach (EnemySpawner spawner in this.EnemySpawners)
        {
            if (spawner != null)
                spawner.Targets.Add(player.transform);
        }

        PlayerController playerController = player.GetComponent<PlayerController>();
        playerController.PlayerIndex = sessionPlayer.PlayerIndex;
        playerController.NoFire = this.NoFireForPlayers;

        GlobalEvents.Notifier.SendEvent(new PlayerSpawnedEvent(player, sessionPlayer.PlayerIndex));
    }
}
