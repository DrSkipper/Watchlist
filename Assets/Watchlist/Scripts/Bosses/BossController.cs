using UnityEngine;
using System.Collections.Generic;

public class BossController : VoBehavior
{
    public GameObject SubBossPrefab;
    public GameObject SpawnVisualPrefab;
    public Transform[] SpawnLocations;
    public LayerMask SpawnCollisionLayers;
    
    public int TotalSubBosses = 6;
    public int InitialSubBosses = 2;
    public int SubBossSpawnsPerWave = 2;
    public int OpenSpaceForSpawn = 50;
    public float FireCooldown = 5.0f;
    public float CooldownSpeedupPerWave = 1.0f;
    public float FireDuration = 1.0f;

    void Start()
    {
        this.spawnSubBossWave();
    }

    void Update()
    {
        if (_cooldown <= 0.0f)
        {
            _cooldown = this.FireCooldown;
            foreach (GameObject boss in _subBosses)
                boss.GetComponent<BossBehavior>().InitiateFire(this.FireDuration);
        }
        else if (_subBosses.Count > 0)
        {
            _cooldown -= Time.deltaTime;
        }
    }

    void SubBossSpawned(GameObject subBoss)
    {
        List<GameObject> otherBosses = new List<GameObject>(_subBosses);
        subBoss.GetComponent<BossBehavior>().OtherBosses = otherBosses;

        foreach (GameObject boss in _subBosses)
        {
            boss.GetComponent<BossBehavior>().OtherBosses.Add(subBoss);
        }

        _subBosses.Add(subBoss);
        subBoss.GetComponent<Damagable>().OnDeath = this.SubBossKilled;
    }

    void SubBossKilled(Damagable subBoss)
    {
        GameObject subBossObject = subBoss.gameObject;
        _subBosses.Remove(subBossObject);
        ++_killedSubBosses;

        foreach (GameObject boss in _subBosses)
        {
            boss.GetComponent<BossBehavior>().OtherBosses.Remove(subBossObject);
        }

        if (_killedSubBosses < TotalSubBosses)
            spawnSubBossWave();
        else
            handleLastSubBossKilled();
    }

    /**
     * Private
     */
    private List<GameObject> _subBosses = new List<GameObject>();
    private int _killedSubBosses;
    private int _spawnedSubBosses;
    private float _cooldown;

    private void spawnSubBossWave()
    {
        List<Transform> spawns = new List<Transform>(this.SpawnLocations);
        Transform spawn;

        for (int i = 0; i < this.SubBossSpawnsPerWave; ++i)
        {
            if (_spawnedSubBosses >= this.TotalSubBosses)
                break;

            ++_spawnedSubBosses;

            if (spawns.Count >= this.SubBossSpawnsPerWave - i)
            {
                do
                {
                    spawn = spawns[Random.Range(0, spawns.Count)];
                    spawns.Remove(spawn);
                }
                while (!spawnAvailable(spawn) && spawns.Count > 0);
            }
            else
            {
                spawn = this.SpawnLocations[Random.Range(0, this.SpawnLocations.Length)];
            }

            spawn.GetComponent<BossSpawner>().InitiateSpawn(this.SubBossPrefab, this.SpawnVisualPrefab, this.SubBossSpawned);
        }
    }

    private void handleLastSubBossKilled()
    {
    }

    private bool spawnAvailable(Transform spawn)
    {
        List<IntegerCollider> colliders = this.CollisionManager.GetCollidersInRange(new IntegerRect(Mathf.RoundToInt(spawn.position.x), Mathf.RoundToInt(spawn.position.y), this.OpenSpaceForSpawn, this.OpenSpaceForSpawn), this.SpawnCollisionLayers);
        return colliders.Count == 0;
    }
}
