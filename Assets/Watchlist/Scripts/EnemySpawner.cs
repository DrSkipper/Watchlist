using UnityEngine;

public class EnemySpawner : VoBehavior
{
    public int[] SpawnPool; // If empty, we assume entire enemy pool
    public float SpawnCooldown = 0.0f; // If 0, wait until Spawn is called externally
    public Transform[] Targets;

    public GameObject GenericPrefab;
    public GameObject GenericMotionPrefab;

    void Update()
    {
        if (this.SpawnCooldown > 0.0f)
        {
            if (_cooldownTimer <= 0.0f)
                Spawn();
            else
                _cooldownTimer -= Time.deltaTime;
        }
    }

    public void Spawn()
    {
        _cooldownTimer = this.SpawnCooldown;

        int enemyId = this.SpawnPool != null && this.SpawnPool.Length > 0 ? this.SpawnPool[Random.Range(0, this.SpawnPool.Length)] : StaticData.EnemyData.EnemyTypeArray[Random.Range(0, StaticData.EnemyData.EnemyTypeArray.Length)].Id;
        EnemyType enemy = StaticData.EnemyData.EnemyTypes[enemyId];

        GameObject prefab = this.GenericPrefab;
        if (enemy.PrefabName == "generic_motion")
            prefab = this.GenericMotionPrefab;
        
        GameObject enemyObject = Instantiate(prefab, this.transform.position, Quaternion.identity) as GameObject;
        GenericEnemy enemyComponent = enemyObject.GetComponent<GenericEnemy>();
        enemyComponent.EnemyType = enemy;
        enemyComponent.Targets = this.Targets;
    }

    /**
     * Private
     */
    private float _cooldownTimer;
}
