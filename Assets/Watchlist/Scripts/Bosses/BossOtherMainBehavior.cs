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

    void Awake()
    {
        _timedCallbacks = this.GetComponent<TimedCallbacks>();
        _stateMachine = new FSMStateMachine();
        _stateMachine.AddState(SPINNING_STATE, updateSpinning, enterSpinning, exitSpinning);
        _stateMachine.AddState(TRANSITION_STATE, updateTransition, enterTransition, exitTransition);
        _stateMachine.AddState(ACTION_STATE, updateAction, enterAction, exitAction);
    }

    void Start()
    {
        foreach (GameObject subBossPart in this.SubBossParts)
        {
            subBossPart.GetComponent<Damagable>().OnDeathCallbacks.Add(this.SubBossPartDestroyed);
        }
        _stateMachine.BeginWithInitialState(SPINNING_STATE);
        enterSpinning();
    }

    void Update()
    {
        _stateMachine.Update();
    }

    /**
     * Private
     */
    private TimedCallbacks _timedCallbacks;
    private FSMStateMachine _stateMachine;
    private int _action;
    private bool _stateSwitch;

    private const string SPINNING_STATE = "spinning";
    private const string TRANSITION_STATE = "transition";
    private const string ACTION_STATE = "action";

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
        return !_stateSwitch ? SPINNING_STATE : TRANSITION_STATE;
    }

    private void enterSpinning()
    {
        _stateSwitch = false;
        foreach (GameObject subBoss in this.SubBosses)
        {
            subBoss.GetComponent<Rotation>().IsRotating = true;
        }

        _timedCallbacks.AddCallback(this, stateSwitch, Random.Range(this.MinTimeBetweenPhases, this.MaxTimeBetweenPhases));
    }

    private void exitSpinning()
    {
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
