using UnityEngine;

public class SpawnController : VoBehavior
{
    public GameObject[] Spawners;
    public float TimeBetweenWaves = 0.0f;

	void Start()
    {
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
                    GameObject enemy = spawner.Spawn();
                    enemy.GetComponent<Damagable>().OnDeath = this.EnemyDied;
                    ++enemyCount;
                }
            }
            else
            {
                _waveCooldown -= Time.deltaTime;
            }
        }
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
