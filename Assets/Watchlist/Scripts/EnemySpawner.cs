using UnityEngine;

public class EnemySpawner : VoBehavior
{
    public int[] SpawnPool; // If empty, we assume entire enemy pool
    public float SpawnCooldown = 0.0f; // If 0, wait until Spawn is called externally
    public float MinDistanceToSpawn = float.MaxValue;
    public Transform[] Targets;

    public GameObject GenericPrefab;
    public GameObject GenericMotionPrefab;
    public GameObject SpawnVisualPrefab;

    void Update()
    {
        if (this.SpawnCooldown > 0.0f)
        {
            if (_cooldownTimer <= 0.0f)
            {
                if (distanceCheck())
                    Spawn();
            }
            else
            {
                _cooldownTimer -= Time.deltaTime;
            }
        }
        else if (distanceCheck())
        {
            Spawn();
            Destroy(this.gameObject);
        }
    }

    public GameObject Spawn()
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

        if (this.SpawnVisualPrefab != null)
        {
            GameObject visual = Instantiate(this.SpawnVisualPrefab, Vector3.zero, Quaternion.identity) as GameObject;
            visual.transform.parent = this.transform;
            visual.transform.localPosition = Vector3.zero;
        }

        return enemyObject;
    }

    /**
     * Private
     */
    private float _cooldownTimer;

    private bool distanceCheck()
    {
        if (this.Targets.Length == 0)
            return true;

        foreach (Transform target in this.Targets)
        {
            if (Vector2.Distance(this.transform.position, target.position) <= this.MinDistanceToSpawn)
                return true;
        }

        return false;
    }
}
