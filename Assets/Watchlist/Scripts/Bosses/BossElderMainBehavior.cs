using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(RadialDistancing))]
public class BossElderMainBehavior : VoBehavior
{
    public List<GameObject> SubBosses;
    public float MinRadius = 10.0f;
    public float MaxRadius = 300.0f;
    public float TimeBetweenSpawns = 1.0f;
    public float InitialDelay = 2.0f;
    public float DeathDelay = 2.0f;
    public GameObject EndFlowObject;
    public EnemySpawner MinionSpawner;
    public Transform LowerLeft;
    public Transform UpperRight;

    void Awake()
    {
        _radius = this.GetComponent<RadialDistancing>();
        _lerpMovement = this.GetComponent<LerpMovement>();
        _timedCallbacks = this.GetComponent<TimedCallbacks>();
        _minions = new List<Damagable>();
        _targetPos = this.transform.position;
        _radius.Running = false;
    }

    void Start()
    {
        this.MinionSpawner.Targets = PlayerTargetController.Targets;
        for (int i = 0; i < this.SubBosses.Count; ++i)
        {
            GameObject subBoss = this.SubBosses[i];
            subBoss.GetComponent<Damagable>().OnDeathCallbacks.Add(this.SubBossKilled);
        }
        this.MinionSpawner.SpawnCallback = minionSpawned;
        GlobalEvents.Notifier.Listen(BeginGameplayEvent.NAME, this, gameplayBegin);
    }

    private void gameplayBegin(LocalEventNotifier.Event e)
    {
        _timedCallbacks.AddCallback(this, begin, this.InitialDelay);
    }

    void Update()
    {
        if (_radius.Running && 
            ((_radius.Radius <= this.MinRadius && _radius.RadiusChangeSpeed < 0.0f) || 
            (_radius.Radius >= this.MaxRadius && _radius.RadiusChangeSpeed > 0.0f)))
        {
            if (_radius.RadiusChangeSpeed < 0.0f)
            {
                this.MinionSpawner.transform.position = this.transform.position;
                this.MinionSpawner.BeginSpawn();
                findNewTarget();
            }
            _radius.RadiusChangeSpeed *= -1;
        }
    }

    /**
     * Private
     */
    private RadialDistancing _radius;
    private TimedCallbacks _timedCallbacks;
    private LerpMovement _lerpMovement;
    private List<Damagable> _minions;
    private Vector3 _targetPos;
    
    private void minionSpawned(GameObject go)
    {
        Damagable damagable = go.GetComponent<Damagable>();
        damagable.OnDeathCallbacks.Add(minionDied);
        _minions.Add(damagable);
    }

    private void minionDied(Damagable died)
    {
        _minions.Remove(died);
    }

    private void onDeath()
    {
        Destroy(this.MinionSpawner.gameObject);
        _radius.Running = false;
        while (_minions.Count > 0)
        {
            _minions[_minions.Count - 1].Kill(0.0f);
        }
        _timedCallbacks.AddCallback(this, this.EndFlowObject.GetComponent<WinCondition>().EndLevel, this.DeathDelay);
    }

    private void SubBossKilled(Damagable died)
    {
        GameObject killedObject = died.gameObject;
        this.SubBosses.Remove(killedObject);

        if (this.SubBosses.Count == 0)
        {
            onDeath();
        }

        RadialDistancing.RadialElement? radialElement = null;

        foreach (RadialDistancing.RadialElement element in _radius.RadialElements)
        {
            if (element.transform.gameObject == killedObject)
            {
                radialElement = element;
                break;
            }
        }

        if (radialElement.HasValue)
            _radius.RadialElements.Remove(radialElement.Value);
    }

    private void findNewTarget()
    {
        _targetPos = new Vector3(Mathf.Round(Random.Range(this.LowerLeft.position.x, this.UpperRight.position.x)), Mathf.Round(Random.Range(this.LowerLeft.position.y, this.UpperRight.position.y)), this.transform.position.z);
        _lerpMovement.BeginMovement(_targetPos, this.TimeBetweenSpawns);
    }

    private void begin()
    {
        _radius.Running = true;
    }
}
