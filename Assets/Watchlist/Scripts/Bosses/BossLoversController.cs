using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class BossLoversController : VoBehavior
{
    public GameObject SubBossPrefab;
    public GameObject SpawnVisualPrefab;
    public Transform[] SpawnLocations;
    public LayerMask SpawnCollisionLayers;
    public WinCondition WinCondition;
    
    public int TotalSubBosses = 6;
    public int InitialSubBosses = 2;
    public int SubBossSpawnsPerWave = 2;
    public float FireCooldown = 5.0f;
    public float CooldownSpeedupPerWave = 1.0f;
    public float FireDuration = 1.0f;
    public float InitialSpawnDelay = 1.0f;
    public string ReturnSceneDestination = "";
    public float ReturnSceneDelay = 2.0f;

    void Awake()
    {
        _health = this.GetComponent<BossHealth>();
    }

    void Start()
    {
        this.GetComponent<TimedCallbacks>().AddCallback(this, this.spawnSubBossWave, this.InitialSpawnDelay);
    }

    void Update()
    {
        if (_cooldown <= 0.0f)
        {
            _cooldown = this.FireCooldown;
            foreach (GameObject boss in _subBosses)
                boss.GetComponent<BossLoversBehavior>().InitiateFire(this.FireDuration);
        }
        else if (_subBosses.Count > 0)
        {
            _cooldown -= Time.deltaTime;
        }
    }

    void SubBossSpawned(GameObject subBoss)
    {
        _health.AddDamagable(subBoss.GetComponent<Damagable>());

        List<GameObject> otherBosses = new List<GameObject>(_subBosses);
        subBoss.GetComponent<BossLoversBehavior>().OtherBosses = otherBosses;

        foreach (GameObject boss in _subBosses)
        {
            boss.GetComponent<BossLoversBehavior>().OtherBosses.Add(subBoss);
        }

        _subBosses.Add(subBoss);
        subBoss.GetComponent<Damagable>().OnDeathCallbacks.Add(this.SubBossKilled);
    }

    void SubBossKilled(Damagable subBoss)
    {
        GameObject subBossObject = subBoss.gameObject;
        _subBosses.Remove(subBossObject);
        ++_killedSubBosses;

        foreach (GameObject boss in _subBosses)
        {
            boss.GetComponent<BossLoversBehavior>().OtherBosses.Remove(subBossObject);
        }

        if (_killedSubBosses < TotalSubBosses)
            spawnSubBossWave();
        else
            handleLastSubBossKilled();
    }

    /**
     * Private
     */
    private BossHealth _health;
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
        this.WinCondition.EndLevel();
        //this.GetComponent<TimedCallbacks>().AddCallback(this, this.returnToScene, this.ReturnSceneDelay);
    }

    private void returnToScene()
    {
        SceneManager.LoadScene(this.ReturnSceneDestination);
    }

    private bool spawnAvailable(Transform spawn)
    {
        IntegerCollider collider = spawn.GetComponent<IntegerCollider>();
        return collider == null || collider.CollideFirst(0, 0, this.SpawnCollisionLayers) == null;
    }
}
