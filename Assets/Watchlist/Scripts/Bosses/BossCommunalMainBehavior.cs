using UnityEngine;
using System.Collections.Generic;

public class BossCommunalMainBehavior : VoBehavior
{
    public int[] PossibleRotationDirections = { 1, -1 };
    public float RotationSpeed = 180.0f;
    public LayerMask LevelGeomLayerMask;
    public List<GameObject> SubBosses;
    public float TimeBetweenRealign = 4.0f;
    public float TimeToRealign = 1.0f;
    public float TimeBetweenSpawns = 0.4f;
    public float InitialDelay = 0.5f;
    public float DeathDelay = 0.5f;
    public float SubBossDeathSpacing = 0.1f;
    public float EndLevelDelay = 1.0f;
    public EnemySpawner[] MinionSpawners;
    public GameObject EndFlowObject;

    void Awake()
    {
        _timedCallbacks = this.GetComponent<TimedCallbacks>();
        _minions = new List<Damagable>();
        _stateMachine = new FSMStateMachine();
    }

    void Start()
    {
        _rotationAxis = new Vector3(0, 0, 1);
        _rotationDirection = this.PossibleRotationDirections[Random.Range(0, this.PossibleRotationDirections.Length)];

        this.GetComponent<BossHealth>().DeathCallbacks.Add(onDeath);
        for (int i = 0; i < this.MinionSpawners.Length; ++i)
        {
            this.MinionSpawners[i].Targets = PlayerTargetController.Targets;
            this.MinionSpawners[i].SpawnCallback = minionSpawned;
        }

        _timedCallbacks.AddCallback(this, begin, this.InitialDelay);
        _stateMachine.AddState(ROTATION_STATE, updateRotation, enterRotation, exitRotation);
        _stateMachine.AddState(ALIGNING_STATE, updateAligning, enterAligning, exitAligning);
    }

    private void begin()
    {
        _began = true;
        enterRotation();
        _stateMachine.BeginWithInitialState(ROTATION_STATE);
    }

    void Update()
    {
        if (_began && !_dead)
            _stateMachine.Update();
    }

    /**
     * Private
     */
    private FSMStateMachine _stateMachine;
    private TimedCallbacks _timedCallbacks;
    private List<Damagable> _minions;
    private int _rotationDirection;
    private Vector3 _rotationAxis;
    private float _currentAngle;
    private bool _began;
    private bool _dead;
    private bool _switchState;
    private int _alignmentsToFinish;
    private float _spawnCooldown;

    private const string ROTATION_STATE = "rotation";
    private const string ALIGNING_STATE = "align";

    private void switchState()
    {
        _switchState = true;
    }
    
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

    private void onDeath(int hp)
    {
        _dead = true;
        while (_minions.Count > 0)
        {
            _minions[_minions.Count - 1].Kill(0.0f);
        }
        for (int i = 0; i < this.SubBosses.Count; ++i)
        {
            this.SubBosses[i].GetComponent<LerpMovement>().HaltMovement();
        }
        for (int i = 0; i < this.MinionSpawners.Length; ++i)
        {
            Destroy(this.MinionSpawners[i].gameObject);
        }
        _timedCallbacks.AddCallback(this, triggerDeaths, this.DeathDelay);
        _timedCallbacks.AddCallback(this, this.EndFlowObject.GetComponent<WinCondition>().EndLevel, this.EndLevelDelay);
    }

    private void triggerDeaths()
    {
        for (int i = 0; i < this.SubBosses.Count; ++i)
        {
            _timedCallbacks.AddCallback(this, killSubBoss, i * this.SubBossDeathSpacing);
        }
    }

    private void killSubBoss()
    {
        if (this.SubBosses.Count > 0)
        {
            int index = Random.Range(0, this.SubBosses.Count);
            this.SubBosses[index].GetComponent<Damagable>().Kill(0.0f);
            this.SubBosses.RemoveAt(index);
        }
    }

    private void enterRotation()
    {
        _switchState = false;
        _timedCallbacks.AddCallback(this, switchState, this.TimeBetweenRealign);
    }

    private string updateRotation()
    {
        float additionalAngle = this.RotationSpeed * Time.deltaTime;
        _currentAngle = (_currentAngle + additionalAngle * _rotationDirection) % 360.0f;
        this.transform.localRotation = Quaternion.AngleAxis(_currentAngle, _rotationAxis);

        return !_switchState ? ROTATION_STATE : ALIGNING_STATE;
    }

    private void exitRotation()
    {
    }

    private void enterAligning()
    {
        _switchState = false;
        _alignmentsToFinish = this.SubBosses.Count;
        _spawnCooldown = -1.0f;

        for (int i = 0; i < this.SubBosses.Count; ++i)
        {
            this.SubBosses[i].GetComponent<Actor2D>().RemoveVelocityModifier(Damagable.VELOCITY_MODIFIER_KEY);
            this.SubBosses[i].GetComponent<Damagable>().Stationary = true;
            LerpMovement lerpMovement = this.SubBosses[i].GetComponent<LerpMovement>();
            lerpMovement.AddCallback(movementFinished);
            lerpMovement.BeginMovement(this.transform.localToWorldMatrix.MultiplyPoint(this.SubBosses[i].GetComponent<StartingPosition>().Local()), this.TimeToRealign);
        }
    }

    private void movementFinished(GameObject go)
    {
        --_alignmentsToFinish;
        if (_alignmentsToFinish <= 0)
            switchState();
    }

    private string updateAligning()
    {
        _spawnCooldown -= Time.deltaTime;
        if (_spawnCooldown <= 0.0f)
        {
            _spawnCooldown = this.TimeBetweenSpawns;
            this.MinionSpawners[Random.Range(0, this.MinionSpawners.Length)].BeginSpawn();
        }

        return !_switchState ? ALIGNING_STATE : ROTATION_STATE;
    }

    private void exitAligning()
    {
        for (int i = 0; i < this.SubBosses.Count; ++i)
        {
            this.SubBosses[i].GetComponent<Damagable>().Stationary = false;
            this.SubBosses[i].GetComponent<LerpMovement>().ClearCallbacks();
        }
    }
}
