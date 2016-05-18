using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(LerpRotation))]
[RequireComponent(typeof(TimedCallbacks))]
public class BossWeakMainBehavior : VoBehavior
{
    public List<GameObject> SubBosses;
    public List<Transform> AttackTargets;
    public EnemySpawner[] EnemySpawners;
    public GameObject Eye;
    public float AngleIncrement = 90.0f;
    public float AttackSpeed = 200.0f;
    public float DelayAfterRotation = 0.2f;
    public float DelayAfterAttackPhase = 0.2f;
    public float DelayBetweenAttacks = 0.1f;
    public float DelayBeforeAttack = 0.1f;
    public float DelayBeforeReturn = 0.1f;
    public float DelayBeforeEyeSpeedup = 0.5f;
    public float FasterEyeSpeed = 180.0f;
    public float TimeBetweenEnemySpawns = 0.5f;
    public float TimeBetweenSubSpawns = 0.25f;
    public float InitialDelay = 1.5f;
    public float InitialDeathDelay = 0.4f;
    public float SubBossDeathSpacing = 0.1f;
    public float EndLevelDelay = 0.5f;
    public GameObject SubBossPrefab;
    public WinCondition WinCondition;
    
    void Awake()
    {
        _dead = false;
        _rotation = this.GetComponent<Rotation>();
        _lerpRotation = this.GetComponent<LerpRotation>();
        _timedCallbacks = this.GetComponent<TimedCallbacks>();
        _eyeRotation = this.Eye.GetComponent<Rotation>();
        _eyeAutoFire = this.Eye.GetComponent<WeaponAutoFire>();
        _stateMachine = new FSMStateMachine();
        _minions = new List<Damagable>();
        _ogPositionsToSpawn = new List<Vector2>();
        _originalEyeSpeed = _eyeRotation.RotationSpeed;
    }

    void Start()
    {
        this.AttackTargets = PlayerTargetController.Targets;
        foreach (GameObject subBoss in this.SubBosses)
        {
            subBoss.GetComponent<BossWeakSubBehavior>().OnAttackFinished = subBossAttackFinished;
            subBoss.GetComponent<Damagable>().OnDeathCallbacks.Add(this.SubBossKilled);
        }

        for (int i = 0; i < this.EnemySpawners.Length; ++i)
        {
            this.EnemySpawners[i].SpawnCallback = enemySpawned;
            this.EnemySpawners[i].Targets = this.AttackTargets;
        }

        this.GetComponent<BossHealth>().DeathCallbacks.Add(onDeath);
        _stateMachine.AddState(ROTATION_STATE, updateRotation, enterRotation, exitRotation);
        _stateMachine.AddState(ATTACKING_STATE, updateAttacking, enterAttacking, exitAttacking);
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
    
    public override void OnDestroy()
    {
        if (GlobalEvents.Notifier != null)
            GlobalEvents.Notifier.RemoveAllListenersForOwner(this);
        base.OnDestroy();
    }

    /**
     * Private
     */
    private Rotation _rotation;
    private LerpRotation _lerpRotation;
    private TimedCallbacks _timedCallbacks;
    private Rotation _eyeRotation;
    private WeaponAutoFire _eyeAutoFire;
    private FSMStateMachine _stateMachine;
    private List<Damagable> _minions;
    private List<Vector2> _ogPositionsToSpawn; 
    private float _originalEyeSpeed;
    private float _enemySpawnCooldown;
    private float _subSpawnCooldown;
    private bool _switchState;
    private int _attacksToFinish;
    private bool _began;
    private bool _dead;

    private const string ROTATION_STATE = "rotation";
    private const string ATTACKING_STATE = "path";

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
        for (int i = 0; i < this.EnemySpawners.Length; ++i)
        {
            Destroy(this.EnemySpawners[i]);
        }
        _timedCallbacks.AddCallback(this, triggerDeaths, this.InitialDeathDelay);
        _timedCallbacks.AddCallback(this, this.WinCondition.EndLevel, this.EndLevelDelay);
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

    private void SubBossKilled(Damagable died)
    {
        this.SubBosses.Remove(died.gameObject);
        BossWeakSubBehavior sub = died.GetComponent<BossWeakSubBehavior>();
        _ogPositionsToSpawn.Add(sub.OGPosition);

        if (!sub.AttackFinished)
        {
            --_attacksToFinish;

            if (_attacksToFinish == 0 && _stateMachine.CurrentState == ATTACKING_STATE)
                _timedCallbacks.AddCallback(this, switchState, this.DelayAfterAttackPhase);
        }
    }

    private void enemySpawned(GameObject enemy)
    {
        Damagable damagable = enemy.GetComponent<Damagable>();
        damagable.OnDeathCallbacks.Add(enemyDied);
        _minions.Add(damagable);
    }

    private void enemyDied(Damagable died)
    {
        _minions.Remove(died);
    }

    private void subBossAttackFinished(GameObject go)
    {
        --_attacksToFinish;
        if (_attacksToFinish == 0)
            _timedCallbacks.AddCallback(this, switchState, this.DelayAfterAttackPhase);
    }

    private string updateRotation()
    {
        _enemySpawnCooldown += Time.deltaTime;
        _subSpawnCooldown += Time.deltaTime;
        if (_enemySpawnCooldown >= this.TimeBetweenEnemySpawns)
        {
            _enemySpawnCooldown = 0.0f;
            this.EnemySpawners[Random.Range(0, this.EnemySpawners.Length - 1)].BeginSpawn();
        }
        if (_subSpawnCooldown >= this.TimeBetweenSubSpawns)
        {
            _subSpawnCooldown = 0.0f;
            if (_ogPositionsToSpawn.Count > 0)
            {
                int index = Random.Range(0, _ogPositionsToSpawn.Count);
                Vector2 choice = _ogPositionsToSpawn[index];
                _ogPositionsToSpawn.RemoveAt(index);
                GameObject subBoss = Instantiate(this.SubBossPrefab, new Vector3(choice.x, choice.y, this.transform.position.z), Quaternion.identity) as GameObject;
                subBoss.transform.parent = this.transform;
                subBoss.transform.localRotation = Quaternion.identity;
                this.SubBosses.Add(subBoss);
                subBoss.GetComponent<BossWeakSubBehavior>().OnAttackFinished = subBossAttackFinished;
                subBoss.GetComponent<Damagable>().OnDeathCallbacks.Add(this.SubBossKilled);
            }
        }
        return !_switchState ? ROTATION_STATE : ATTACKING_STATE;
    }

    private void enterRotation()
    {
        _switchState = false;
        _lerpRotation.AddCallback(rotationDone);
        _lerpRotation.TargetRotation = _rotation.GetAngle() + this.AngleIncrement;
        _lerpRotation.LerpToTargetRotation();
    }

    private void rotationDone(GameObject go)
    {
        _lerpRotation.ClearCallbacks();
        switchState();
    }

    private void rotationLerpFinished(GameObject gameObject)
    {
        _timedCallbacks.AddCallback(this, this.switchState, this.DelayAfterRotation);
    }

    private void exitRotation()
    {
        _enemySpawnCooldown = 0.0f;
    }

    private string updateAttacking()
    {
        return !_switchState ? ATTACKING_STATE : ROTATION_STATE;
    }

    private void enterAttacking()
    {
        _switchState = false;
        this.SubBosses.Shuffle();
        _eyeAutoFire.Paused = false;
        _timedCallbacks.AddCallback(this, eyeSpeedUp, this.DelayBeforeEyeSpeedup);
        _attacksToFinish = 0;

        if (this.AttackTargets.Count == 0)
        {
            _switchState = true;
        }
        else
        {
            for (int i = 0; i < this.SubBosses.Count; ++i)
            {
                ++_attacksToFinish;
                BossWeakSubBehavior subBoss = this.SubBosses[i].GetComponent<BossWeakSubBehavior>();
                Transform randomTarget = this.AttackTargets[Random.Range(0, this.AttackTargets.Count)];
                subBoss.ScheduleAttack(randomTarget, this.AttackSpeed, i * this.DelayBetweenAttacks, this.DelayBeforeAttack, this.DelayBeforeReturn);
            }

            if (_attacksToFinish == 0)
                _timedCallbacks.AddCallback(this, switchState, this.DelayAfterAttackPhase);
        }
    }

    private void eyeSpeedUp()
    {
        _eyeRotation.RotationSpeed = Mathf.Sign(_eyeRotation.RotationSpeed) * this.FasterEyeSpeed;
    }

    private void exitAttacking()
    {
        _eyeAutoFire.Paused = true;
        _eyeRotation.RotationSpeed = -Mathf.Sign(_eyeRotation.RotationSpeed) *_originalEyeSpeed;
    }
}
