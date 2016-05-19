using UnityEngine;
using System.Collections.Generic;

public class BossYoungMainBehavior : VoBehavior
{
    public List<GameObject> SubBosses;
    public List<GameObject> SubBossGroups;
    public Transform AttackBoxLowerLeft;
    public Transform AttackBoxUpperRight;
    public float GroupRotationAmount = 90.0f;
    public float AttackDuration = 4.0f;
    public float MinionSpawnCooldown = 0.5f;
    public EnemySpawner[] MinionSpawners;
    public GameObject EndFlowObject;

    void Awake()
    {
        _timedCallbacks = this.GetComponent<TimedCallbacks>();
        _minions = new List<Damagable>();
    }

    void Start()
    {
        for (int i = 0; i < this.SubBosses.Count; ++i)
        {
            GameObject subBoss = this.SubBosses[i];
            subBoss.GetComponent<Damagable>().OnDeathCallbacks.Add(this.SubBossKilled);
            subBoss.GetComponent<BossYoungSubBehavior>().ReturnHomeCallback = returnFinished;
        }

        foreach (GameObject group in this.SubBossGroups)
        {
            group.GetComponent<LerpRotation>().AddCallback(this.rotationOver);
        }

        for (int i = 0; i < this.MinionSpawners.Length; ++i)
        {
            this.MinionSpawners[i].SpawnCallback = minionSpawned;
            this.MinionSpawners[i].Targets = PlayerTargetController.Targets;
        }

        _stateMachine = new FSMStateMachine();
        _stateMachine.AddState(ROTATION_STATE, updateRotation, enterRotation, exitRotation);
        _stateMachine.AddState(ATTACKING_STATE, updateAttacking, enterAttacking, exitAttacking);
        _stateMachine.AddState(RETURNING_STATE, updateReturning, enterReturning, exitReturning);
        enterRotation();
        _stateMachine.BeginWithInitialState(ROTATION_STATE);
    }

    void Update()
    {
        if (!PauseController.IsPaused())
            _stateMachine.Update();
    }

    /**
     * Private
     */
    private TimedCallbacks _timedCallbacks;
    private FSMStateMachine _stateMachine;
    private bool _switchState;
    private int _rotationsOver;
    private int _returnsComplete;
    private float _minionSpawnCooldown;
    private List<Damagable> _minions;

    private const string ROTATION_STATE = "rotation";
    private const string ATTACKING_STATE = "path";
    private const string RETURNING_STATE = "return";

    private void switchState()
    {
        _switchState = true;
    }

    private void minionSpawned(GameObject go)
    {
        Damagable damagable = go.GetComponent<Damagable>();
        _minions.Add(damagable);
        damagable.OnDeathCallbacks.Add(minionDied);
    }

    private void minionDied(Damagable died)
    {
        _minions.Remove(died);
    }

    private void SubBossKilled(Damagable died)
    {
        this.SubBosses.Remove(died.gameObject);

        if (this.SubBosses.Count == 0)
        {
            this.EndFlowObject.GetComponent<WinCondition>().EndLevel();
        }

        else if (_stateMachine.CurrentState == RETURNING_STATE)
            checkReturnsComplete();
    }

    private string updateRotation()
    {
        _minionSpawnCooldown -= Time.deltaTime;
        if (_minionSpawnCooldown <= 0.0f)
        {
            _minionSpawnCooldown = this.MinionSpawnCooldown;
            for (int i = 0; i < this.MinionSpawners.Length; ++i)
            {
                this.MinionSpawners[i].BeginSpawn();
            }
        }
        return !_switchState ? ROTATION_STATE : ATTACKING_STATE;
    }

    private void enterRotation()
    {
        _switchState = false;
        _rotationsOver = 0;

        foreach (GameObject group in this.SubBossGroups)
        {
            LerpRotation lerpRot = group.GetComponent<LerpRotation>();
            lerpRot.TargetRotation = group.GetComponent<Rotation>().GetAngle() + this.GroupRotationAmount;
            lerpRot.LerpToTargetRotation();
        }
    }

    private void rotationOver(GameObject go)
    {
        ++_rotationsOver;
        if (_rotationsOver >= this.SubBossGroups.Count)
            _switchState = true;
    }

    private void exitRotation()
    {
        _minionSpawnCooldown = 0.0f;
    }

    private string updateAttacking()
    {
        return !_switchState ? ATTACKING_STATE : RETURNING_STATE;
    }

    private void enterAttacking()
    {
        _switchState = false;

        foreach (GameObject subBoss in this.SubBosses)
        {
            subBoss.GetComponent<BossYoungSubBehavior>().BeginAttackPathing(new IntegerVector(this.AttackBoxLowerLeft.position), new IntegerVector(this.AttackBoxUpperRight.position));
        }

        _timedCallbacks.AddCallback(this, attackingDone, this.AttackDuration);
    }

    private void exitAttacking()
    {
    }

    private void attackingDone()
    {
        _switchState = true;
    }

    private string updateReturning()
    {
        return !_switchState ? RETURNING_STATE : ROTATION_STATE;
    }

    private void enterReturning()
    {
        _switchState = false;
        _returnsComplete = 0;

        foreach (GameObject subBoss in this.SubBosses)
        {
            subBoss.GetComponent<BossYoungSubBehavior>().StopAttackingAndReturnHome();
        }
    }

    private void exitReturning()
    {
    }

    private void returnFinished(GameObject go)
    {
        ++_returnsComplete;
        checkReturnsComplete();
    }

    private void checkReturnsComplete()
    {
        if (_returnsComplete >= this.SubBosses.Count)
            _switchState = true;
    }
}
