using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(TimedCallbacks))]
public class BossOtherMainBehavior : VoBehavior
{
    public GameObject[] SubBosses;
    public List<GameObject> SubBossParts;
    public float MinTimeBetweenPhases = 5.0f;
    public float MaxTimeBetweenPhases = 8.0f;
    public float TransitionTime = 3.0f;
    public float MovementTime = 5.0f;
    public int NumActions = 3;
    public GameObject EndFlowObject;
    public List<EnemySpawner> MinionSpawners;
    public float MinionSpawnCooldown = 1.0f;
    public float InitialDeathDelay = 0.5f;
    public float SubBossDeathSpacing = 0.1f;
    public float InitialDelay = 2.0f;

    void Awake()
    {
        _shooters = new List<Damagable>();
        _timedCallbacks = this.GetComponent<TimedCallbacks>();
        _stateMachine = new FSMStateMachine();
        _stateMachine.AddState(SPINNING_STATE, updateSpinning, enterSpinning, exitSpinning);
        _stateMachine.AddState(TRANSITION_STATE, updateTransition, enterTransition, exitTransition);
        _stateMachine.AddState(ACTION_STATE, updateAction, enterAction, exitAction);
        _health = this.GetComponent<BossHealth>();
        for (int i = 0; i < this.SubBossParts.Count; ++i)
        {
            _health.AddDamagable(this.SubBossParts[i].GetComponent<Damagable>());
        }
    }

    void Start()
    {
        for (int i = 0; i < this.SubBossParts.Count; ++i)
        {
            Damagable damagable = this.SubBossParts[i].GetComponent<Damagable>();
            damagable.OnDeathCallbacks.Add(this.SubBossPartDestroyed);
            _health.AddDamagable(damagable);
        }
        for (int i = 0; i < this.MinionSpawners.Count; ++i)
        {
            this.MinionSpawners[i].Targets = PlayerTargetController.Targets;
            this.MinionSpawners[i].SpawnCallback = shooterSpawned;
        }
        _health.DeathCallbacks.Add(this.OnDeath);
        _timedCallbacks.AddCallback(this, begin, this.InitialDelay);
    }

    void Update()
    {
        if (_begun && !_dead)
            _stateMachine.Update();
    }

    public void OnDeath(int hp)
    {
        _dead = true;
        while (_shooters.Count > 0)
        {
            _shooters[_shooters.Count - 1].Kill(0.0f);
        }
        for (int i = 0; i < this.SubBosses.Length; ++i)
        {
            this.SubBosses[i].GetComponent<LerpRotation>().HaltRotation();
            this.SubBosses[i].GetComponent<LerpMovement>().HaltMovement();
        }
        _timedCallbacks.AddCallback(this, triggerDeaths, this.InitialDeathDelay);
    }

    /**
     * Private
     */
    private TimedCallbacks _timedCallbacks;
    private BossHealth _health;
    private FSMStateMachine _stateMachine;
    private List<Damagable> _shooters;
    private int _action;
    private bool _stateSwitch;
    private float _minionSpawnCooldown;
    private bool _dead;
    private bool _begun;

    private const string SPINNING_STATE = "spinning";
    private const string TRANSITION_STATE = "transition";
    private const string ACTION_STATE = "action";

    private void begin()
    {
        if (!_dead)
        {
            _begun = true;
            _stateMachine.BeginWithInitialState(SPINNING_STATE);
            enterSpinning();
        }
    }

    private void triggerDeaths()
    {
        for (int i = 0; i < this.SubBossParts.Count; ++i)
        {
            _timedCallbacks.AddCallback(this, killSubBoss, i * this.SubBossDeathSpacing);
        }
    }

    private void shooterSpawned(GameObject go)
    {
        Damagable damagable = go.GetComponent<Damagable>();

        if (_dead)
            damagable.Kill(0.0f);
        else
        {
            _shooters.Add(damagable);
            damagable.OnDeathCallbacks.Add(shooterDied);
        }
    }

    private void shooterDied(Damagable died)
    {
        _shooters.Remove(died);
    }

    private void killSubBoss()
    {
        this.SubBossParts[Random.Range(0, this.SubBossParts.Count)].GetComponent<Damagable>().Kill(0.0f);
    }

    private void stateSwitch()
    {
        _stateSwitch = true;
    }

    private void SubBossPartDestroyed(Damagable destroyed)
    {
        this.SubBossParts.Remove(destroyed.gameObject);

        if (this.SubBossParts.Count == 0)
            this.EndFlowObject.GetComponent<WinCondition>().EndLevel();
    }

    private string updateSpinning()
    {
        _minionSpawnCooldown += Time.deltaTime;
        if (_minionSpawnCooldown > this.MinionSpawnCooldown)
        {
            _minionSpawnCooldown = 0.0f;
            this.MinionSpawners[Random.Range(0, this.MinionSpawners.Count)].BeginSpawn();
        }
        return !_stateSwitch ? SPINNING_STATE : TRANSITION_STATE;
    }

    private void enterSpinning()
    {
        _minionSpawnCooldown = 0.0f;
        _stateSwitch = false;
        foreach (GameObject subBoss in this.SubBosses)
        {
            subBoss.GetComponent<Rotation>().IsRotating = true;
        }

        _timedCallbacks.AddCallback(this, stateSwitch, Random.Range(this.MinTimeBetweenPhases, this.MaxTimeBetweenPhases));
    }

    private void exitSpinning()
    {
        _minionSpawnCooldown = 0.0f;
        _action = Random.Range(0, this.NumActions);
    }

    private string updateTransition()
    {
        return !_stateSwitch ? TRANSITION_STATE : ACTION_STATE;
    }

    private void enterTransition()
    {
        _stateSwitch = false;
        _timedCallbacks.AddCallback(this, stateSwitch, this.TransitionTime);

        foreach (GameObject subBoss in this.SubBosses)
        {
            subBoss.GetComponent<BossOtherSubBehavior>().BeginAction(_action);
        }
    }

    private void exitTransition()
    {
    }

    private string updateAction()
    {
        return !_stateSwitch ? ACTION_STATE : SPINNING_STATE;
    }

    private void enterAction()
    {
        _stateSwitch = false;
        _timedCallbacks.AddCallback(this, stateSwitch, this.MovementTime);

        foreach (GameObject subBoss in this.SubBosses)
        {
            subBoss.GetComponent<BossOtherSubBehavior>().BeginMovement();
        }
    }

    private void exitAction()
    {
    }
}
