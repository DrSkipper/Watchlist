using UnityEngine;

public class SpawnController : VoBehavior
{
    public GameObject[] Spawners;
    public float TimeBetweenWaves = 0.0f;
    public float InitialDelay = 0.0f;

	void Start()
    {
        _waveCooldown = this.InitialDelay;
        _spawners = new EnemySpawner[this.Spawners.Length];
        for (int i = 0; i < _spawners.Length; ++i)
            _spawners[i] = this.Spawners[i].GetComponent<EnemySpawner>();
	}

    void Update()
    {
        if (enemyCount <= 0)
        {
            if (_waveCooldown <= 0.0f)
            {
                foreach (EnemySpawner spawner in _spawners)
                {
                    spawner.SpawnCallback = this.EnemySpawned;
                    spawner.BeginSpawn();
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

    /**
     * Private
     */
    private EnemySpawner[] _spawners;
    private int enemyCount;
    private float _waveCooldown;
}
