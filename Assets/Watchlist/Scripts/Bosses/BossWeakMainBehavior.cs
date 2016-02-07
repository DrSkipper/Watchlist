using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(LerpRotation))]
[RequireComponent(typeof(TimedCallbacks))]
public class BossWeakMainBehavior : VoBehavior
{
    public List<GameObject> SubBosses;
    public GameObject AttackTarget;
    public float AngleIncrement = 90.0f;
    public float AttackSpeed = 200.0f;
    public float DelayAfterRotation = 0.2f;
    public float DelayAfterAttackPhase = 0.2f;
    public float DelayBetweenAttacks = 0.1f;
    public float DelayBeforeAttack = 0.1f;
    public float DelayBeforeReturn = 0.1f;
    public GameObject EndFlowObject;
    
    void Awake()
    {
        _rotation = this.GetComponent<Rotation>();
        _lerpRotation = this.GetComponent<LerpRotation>();
        _timedCallbacks = this.GetComponent<TimedCallbacks>();
        _stateMachine = new FSMStateMachine();
    }

    void Start()
    {
        foreach (GameObject subBoss in this.SubBosses)
        {
            subBoss.GetComponent<BossWeakSubBehavior>().OnAttackFinished = subBossAttackFinished;
            subBoss.GetComponent<Damagable>().OnDeathCallbacks.Add(this.SubBossKilled);
        }

        _stateMachine.AddState(ROTATION_STATE, updateRotation, enterRotation, exitRotation);
        _stateMachine.AddState(ATTACKING_STATE, updateAttacking, enterAttacking, exitAttacking);
        enterRotation();
        _stateMachine.BeginWithInitialState(ROTATION_STATE);
    }

    void Update()
    {
        _stateMachine.Update();
    }

    /**
     * Private
     */
    private Rotation _rotation;
    private LerpRotation _lerpRotation;
    private TimedCallbacks _timedCallbacks;
    private FSMStateMachine _stateMachine;
    private bool _switchState;

    private const string ROTATION_STATE = "rotation";
    private const string ATTACKING_STATE = "path";

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
        else if (_stateMachine.CurrentState == ATTACKING_STATE)
        {
            subBossAttackFinished(null);
        }
    }

    private void subBossAttackFinished(GameObject go)
    {
        bool attacksDone = true;
        foreach (GameObject subBoss in this.SubBosses)
        {
            attacksDone &= subBoss.GetComponent<BossWeakSubBehavior>().AttackFinished;
        }

        if (attacksDone)
            _timedCallbacks.AddCallback(this, switchState, this.DelayAfterAttackPhase);
    }

    private string updateRotation()
    {
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
    }

    private string updateAttacking()
    {
        return !_switchState ? ATTACKING_STATE : ROTATION_STATE;
    }

    private void enterAttacking()
    {
        _switchState = false;
        this.SubBosses.Shuffle();

        for (int i = 0; i < this.SubBosses.Count; ++i)
        {
            BossWeakSubBehavior subBoss = this.SubBosses[i].GetComponent<BossWeakSubBehavior>();
            subBoss.ScheduleAttack(this.AttackTarget.transform, this.AttackSpeed, i * this.DelayBetweenAttacks, this.DelayBeforeAttack, this.DelayBeforeReturn);
        }
    }

    private void exitAttacking()
    {
    }
}
