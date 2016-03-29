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
    public GameObject EndFlowObject;

    void Awake()
    {
        _timedCallbacks = this.GetComponent<TimedCallbacks>();
        _timedCallbacks.ListenToPause = true;
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

    private const string ROTATION_STATE = "rotation";
    private const string ATTACKING_STATE = "path";
    private const string RETURNING_STATE = "return";

    private void switchState()
    {
        _switchState = true;
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
