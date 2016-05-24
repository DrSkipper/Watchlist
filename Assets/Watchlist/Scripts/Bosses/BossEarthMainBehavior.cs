using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(Rotation))]
[RequireComponent(typeof(TimedCallbacks))]
public class BossEarthMainBehavior : VoBehavior
{
    public List<GameObject> SubBosses;
    public Transform[] OuterLoopPathPoints;
    public Transform[] InnerLoopPathPoints;
    public Transform[] RotationPoints;
    public float TimeToRotate = 4.0f;
    public float TimeForSinglePath = 1.0f;
    public float DelayBetweenPaths = 0.2f;
    public int NumTimesToPath = 4;
    public int SpawnsPerRotation = 4;
    public float InitialDelay = 0.5f;
    public float DeathDelay = 0.5f;
    public float SubBossDeathSpacing = 0.1f;
    public float EndLevelDelay = 1.0f;
    public EnemySpawner MinionSpawner;
    public Transform LowerLeft;
    public Transform UpperRight;
    public GameObject EndFlowObject;

    void Awake()
    {
        _rotation = this.GetComponent<Rotation>();
        _timedCallbacks = this.GetComponent<TimedCallbacks>();
        _lerpMovement = this.GetComponent<LerpMovement>();
        _stateMachine = new FSMStateMachine();
        _minions = new List<Damagable>();
        _targetPos = this.transform.position;
        _homePos = this.transform.position;
        _timeBetweenSpawns = this.TimeToRotate / this.SpawnsPerRotation;
    }

    void Start()
    {
        for (int i = 0; i < this.SubBosses.Count; ++i)
        {
            GameObject subBoss = this.SubBosses[i];
            subBoss.GetComponent<Damagable>().OnDeathCallbacks.Add(this.SubBossKilled);
        }

        this.GetComponent<BossHealth>().DeathCallbacks.Add(onDeath);
        this.MinionSpawner.Targets = PlayerTargetController.Targets;

        _stateMachine.AddState(ROTATION_STATE, updateRotation, enterRotation, exitRotation);
        _stateMachine.AddState(PATHING_STATE, updatePathing, enterPathing, exitPathing);
        _stateMachine.AddState(TRANSITION_STATE, updateTransition, enterTransition, exitTransition);
        _timedCallbacks.AddCallback(this, begin, this.InitialDelay);
    }

    private void begin()
    {
        enterRotation();
        _stateMachine.BeginWithInitialState(ROTATION_STATE);
        _began = true;
    }

    void Update()
    {
        if (_began && !_dead)
            _stateMachine.Update();
    }

    /**
     * Private
     */
    private Rotation _rotation;
    private TimedCallbacks _timedCallbacks;
    private FSMStateMachine _stateMachine;
    private LerpMovement _lerpMovement;
    private List<Damagable> _minions;
    private Vector3 _targetPos;
    private Vector3 _homePos;
    private bool _switchState;
    private bool _began;
    private bool _dead;
    private int _numPaths;
    private float _timeBetweenSpawns;
    private int _spawns;

    private const string ROTATION_STATE = "rotation";
    private const string PATHING_STATE = "path";
    private const string TRANSITION_STATE = "transition";

    private void switchState()
    {
        _switchState = true;
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
        Destroy(this.MinionSpawner);
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
            this.SubBosses[Random.Range(0, this.SubBosses.Count)].GetComponent<Damagable>().Kill(0.0f);
    }
    
    private void findNewTarget()
    {
        if (_spawns >= this.SpawnsPerRotation - 1)
            _targetPos = _homePos;
        else
            _targetPos = new Vector3(Mathf.Round(Random.Range(this.LowerLeft.position.x, this.UpperRight.position.x)), Mathf.Round(Random.Range(this.LowerLeft.position.y, this.UpperRight.position.y)), this.transform.position.z);
        _lerpMovement.BeginMovement(_targetPos, _timeBetweenSpawns);
    }

    private void lerpMovementComplete(GameObject go)
    {
        this.MinionSpawner.transform.position = this.transform.position;
        this.MinionSpawner.BeginSpawn();

        ++_spawns;
        if (_spawns >= this.SpawnsPerRotation)
            switchState();
        else
            findNewTarget();
    }

    private void SubBossKilled(Damagable died)
    {
        this.SubBosses.Remove(died.gameObject);

        if (this.SubBosses.Count == 0)
        {
            this.EndFlowObject.GetComponent<WinCondition>().EndLevel();
        }
    }

    private string updateRotation()
    {
        return !_switchState ? ROTATION_STATE : PATHING_STATE;
    }

    private void enterRotation()
    {
        _spawns = 0;
        _lerpMovement.AddCallback(lerpMovementComplete);
        _switchState = false;
        _rotation.IsRotating = true;
        //_timedCallbacks.AddCallback(this, switchState, this.TimeToRotate);
        findNewTarget();
    }

    private void exitRotation()
    {
        _lerpMovement.ClearCallbacks();
        _rotation.IsRotating = false;
        _rotation.ResetRotation(true);
    }

    private string updatePathing()
    {
        return !_switchState ? PATHING_STATE : TRANSITION_STATE;
    }

    private void enterPathing()
    {
        _switchState = false;
        _numPaths = 0;
        this.SubBosses.Shuffle();
        int subBossIndex = 0;

        for (int i = 0; i < this.InnerLoopPathPoints.Length && subBossIndex < this.SubBosses.Count; ++i)
        {
            BossEarthSubBehavior subBoss = this.SubBosses[subBossIndex].GetComponent<BossEarthSubBehavior>();
            subBoss.PathPoints = this.InnerLoopPathPoints;
            subBoss.CurrentPathPoint = i;
            subBoss.TimeToReachPoint = this.TimeForSinglePath;
            subBoss.PathToNextPoint();
            ++subBossIndex;
        }

        for (int i = 0; i < this.OuterLoopPathPoints.Length && subBossIndex < this.SubBosses.Count; ++i)
        {
            BossEarthSubBehavior subBoss = this.SubBosses[subBossIndex].GetComponent<BossEarthSubBehavior>();
            subBoss.PathPoints = this.OuterLoopPathPoints;
            subBoss.CurrentPathPoint = i;
            subBoss.TimeToReachPoint = this.TimeForSinglePath;
            subBoss.PathToNextPoint();
            ++subBossIndex;
        }

        if (_numPaths >= this.NumTimesToPath)
            _timedCallbacks.AddCallback(this, switchState, this.TimeForSinglePath + this.DelayBetweenPaths);
        else
            _timedCallbacks.AddCallback(this, pathedOnce, this.TimeForSinglePath + this.DelayBetweenPaths);
    }

    private void pathedOnce()
    {
        ++_numPaths;

        foreach (GameObject subBoss in this.SubBosses)
        {
            subBoss.GetComponent<BossEarthSubBehavior>().PathToNextPoint();
        }

        if (_numPaths >= this.NumTimesToPath)
            _timedCallbacks.AddCallback(this, switchState, this.TimeForSinglePath + this.DelayBetweenPaths);
        else
            _timedCallbacks.AddCallback(this, pathedOnce, this.TimeForSinglePath + this.DelayBetweenPaths);
    }

    private void exitPathing()
    {
    }

    private string updateTransition()
    {
        return !_switchState ? TRANSITION_STATE : ROTATION_STATE;
    }

    private void enterTransition()
    {
        _switchState = false;
        int subBossIndex = 0;

        for (int i = 0; i < this.RotationPoints.Length && subBossIndex < this.SubBosses.Count; ++i)
        {
            LerpMovement subBoss = this.SubBosses[subBossIndex].GetComponent<LerpMovement>();
            subBoss.BeginMovement(this.RotationPoints[i].position, this.TimeForSinglePath);
            ++subBossIndex;
        }

        _timedCallbacks.AddCallback(this, switchState, this.TimeForSinglePath + 0.1f);
    }

    private void exitTransition()
    {
    }
}
