using UnityEngine;
using System.Collections.Generic;

public class EnemySpawner : VoBehavior
{
    public delegate void SpawnedObjectCallback(GameObject spawnedObject);

    public int[] SpawnPool; // If empty, we assume entire enemy pool
    public float SpawnCooldown = 0.0f; // If 0, wait until Spawn is called externally
    public float MinDistanceToSpawn = float.MaxValue;
    public float SpawnDelay = 0.5f;
    public bool DestroyAfterSpawn = false;
    public bool NoFire = false;
    public SpawnedObjectCallback SpawnCallback;
    public List<Transform> Targets;

    public GameObject GenericPrefab;
    public GameObject GenericMotionPrefab;
    public GameObject MoveAndFirePrefab;
    public GameObject SpawnVisualPrefab;

    void Start()
    {
        GlobalEvents.Notifier.Listen(PlayerDiedEvent.NAME, this, playerDied);
    }

    void Update()
    {
        if (this.SpawnCooldown > 0.0f)
        {
            if (_cooldownTimer <= 0.0f)
            {
                if (distanceCheck() && !_spawning)
                    BeginSpawn();
            }
            else
            {
                _cooldownTimer -= Time.deltaTime;
            }
        }
        else if (distanceCheck() && !_spawning)
        {
            BeginSpawn();
        }
    }

    public void BeginSpawn()
    {
        _spawning = true;

        if (this.SpawnVisualPrefab != null)
            Instantiate(this.SpawnVisualPrefab, this.transform.position, Quaternion.identity);

        TimedCallbacks callbacks = this.GetComponent<TimedCallbacks>();
        if (callbacks != null)
            callbacks.AddCallback(this, this.Spawn, this.SpawnDelay);
        else
            Spawn();
    }

    public void Spawn()
    {
        _cooldownTimer = this.SpawnCooldown;

        int enemyId = this.SpawnPool != null && this.SpawnPool.Length > 0 ? this.SpawnPool[Random.Range(0, this.SpawnPool.Length)] : StaticData.EnemyData.EnemyTypeArray[Random.Range(0, StaticData.EnemyData.EnemyTypeArray.Length)].Id;
        EnemyType enemy = StaticData.EnemyData.EnemyTypes[enemyId];

        GameObject prefab = this.GenericPrefab;
        if (enemy.PrefabName == "generic_motion")
            prefab = this.GenericMotionPrefab;
        else if (enemy.PrefabName == "move_and_fire")
            prefab = this.MoveAndFirePrefab;
        
        GameObject enemyObject = Instantiate(prefab, this.transform.position, Quaternion.identity) as GameObject;
        GenericEnemy enemyComponent = enemyObject.GetComponent<GenericEnemy>();
        enemyComponent.EnemyType = enemy;
        enemyComponent.Targets = new List<Transform>(this.Targets);
        enemyComponent.NoFire = this.NoFire;

        if (this.SpawnCallback != null)
            this.SpawnCallback(enemyObject);

        _spawning = false;
        if (this.DestroyAfterSpawn)
            Destroy(this.gameObject);
    }

    public override void OnDestroy()
    {
        if (GlobalEvents.Notifier != null)
            GlobalEvents.Notifier.RemoveAllListenersForOwner(this);
        base.OnDestroy();
    }

    /**
     * Private
     */
    private float _cooldownTimer;
    private bool _spawning;

    private bool distanceCheck()
    {
        if (this.Targets.Count == 0)
            return false;

        foreach (Transform target in this.Targets)
        {
            if (this.transform != null)
            {
                if (Vector2.Distance(this.transform.position, target.position) <= this.MinDistanceToSpawn)
                    return true;
            }
        }

        return false;
    }

    private void playerDied(LocalEventNotifier.Event playerDiedEvent)
    {
        this.Targets.Remove((playerDiedEvent as PlayerDiedEvent).PlayerObject.transform);
    }
}
