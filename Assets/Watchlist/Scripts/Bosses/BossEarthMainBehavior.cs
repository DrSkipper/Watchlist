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
    public GameObject EndFlowObject;

    void Awake()
    {
        _rotation = this.GetComponent<Rotation>();
        _timedCallbacks = this.GetComponent<TimedCallbacks>();
        _stateMachine = new FSMStateMachine();
    }

    void Start()
    {
        for (int i = 0; i < this.SubBosses.Count; ++i)
        {
            GameObject subBoss = this.SubBosses[i];
            subBoss.GetComponent<Damagable>().OnDeathCallbacks.Add(this.SubBossKilled);
        }

        _stateMachine.AddState(ROTATION_STATE, updateRotation, enterRotation, exitRotation);
        _stateMachine.AddState(PATHING_STATE, updatePathing, enterPathing, exitPathing);
        _stateMachine.AddState(TRANSITION_STATE, updateTransition, enterTransition, exitTransition);
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
    private TimedCallbacks _timedCallbacks;
    private FSMStateMachine _stateMachine;
    private bool _switchState;
    private int _numPaths;

    private const string ROTATION_STATE = "rotation";
    private const string PATHING_STATE = "path";
    private const string TRANSITION_STATE = "transition";

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
    }

    private string updateRotation()
    {
        return !_switchState ? ROTATION_STATE : PATHING_STATE;
    }

    private void enterRotation()
    {
        _switchState = false;
        _rotation.IsRotating = true;
        _timedCallbacks.AddCallback(this, switchState, this.TimeToRotate);
    }

    private void exitRotation()
    {
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

        _timedCallbacks.AddCallback(this, switchState, this.TimeForSinglePath);
    }

    private void exitTransition()
    {
    }
}
