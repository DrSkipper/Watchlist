using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(TimedCallbacks))]
[RequireComponent(typeof(LerpRotation))]
[RequireComponent(typeof(LerpMovement))]
public class BossMasterMainBehavior : VoBehavior
{
    public List<Transform> Targets;
    public List<BossMasterGroupBehavior> BossGroups;
    public List<BossMasterSubBehavior> BossSubs;
    public float ReturnHomeTime = 0.2f;
    public float HomeStateDuration = 4.0f;
    public float TransitionFirstPartTime = 0.2f;
    public float TransitionSecondPartTime = 0.2f;
    public float EnterAttackTime = 0.2f;
    public float AttackStateDuration = 4.0f;
    public float BufferTime = 0.1f;
    public float AttackDelay = 0.1f;

    void Awake()
    {
        _timedCallbacks = this.GetComponent<TimedCallbacks>();
        GlobalEvents.Notifier.Listen(PlayerSpawnedEvent.NAME, this, playerSpawned);
        _stateMachine = new FSMStateMachine();
        _stateMachine.AddState(HOME_STATE, updateHome, enterHome, exitHome);
        _stateMachine.AddState(TRANSITION_STATE, updateTransition, enterTransition, exitTransition);
        _stateMachine.AddState(ATTACK_STATE, updateAttack, enterAttack, exitAttack);
        _stateMachine.BeginWithInitialState(HOME_STATE);
    }

    /**
     * Private
     */
    private FSMStateMachine _stateMachine;
    private TimedCallbacks _timedCallbacks;
    private bool _switchState;

    private const string HOME_STATE = "home";
    private const string TRANSITION_STATE = "transition";
    private const string ATTACK_STATE = "attack";

    private void switchState()
    {
        _switchState = true;
    }

    private void playerSpawned(LocalEventNotifier.Event e)
    {
        Transform playerTransform = (e as PlayerSpawnedEvent).PlayerObject.transform;
        this.Targets.Add(playerTransform);
        for (int i = 0; i < this.BossGroups.Count; ++i)
        {
            this.BossGroups[i].Targets.Add(playerTransform);
        }
    }

    private void enterHome()
    {
        _switchState = false;
        for (int i = 0; i < this.BossGroups.Count; ++i)
        {
            this.BossGroups[i].GoHome(this.ReturnHomeTime);
        }

        for (int i = 0; i < this.BossSubs.Count; ++i)
        {
            this.BossSubs[i].GoHome(this.ReturnHomeTime);
        }

        _timedCallbacks.AddCallback(this, switchState, this.HomeStateDuration);
    }

    private string updateHome()
    {
        // Find center of targets
        // Lerp rotation to point at center of targets
        // Lerp movement toward target distance from center of targets
        return !_switchState ? HOME_STATE : TRANSITION_STATE;
    }

    private void exitHome()
    {

    }

    private void enterTransition()
    {
        _switchState = false;

        for (int i = 0; i < this.BossGroups.Count; ++i)
        {
            this.BossGroups[i].EnterTransitionPattern(this.TransitionFirstPartTime);
        }

        _timedCallbacks.AddCallback(this, secondStageTransition, this.TransitionFirstPartTime + this.BufferTime);
    }

    private void secondStageTransition()
    {
        for (int i = 0; i < this.BossSubs.Count; ++i)
        {
            this.BossSubs[i].GoToAttackPosition(this.TransitionSecondPartTime);
        }

        _timedCallbacks.AddCallback(this, switchState, this.TransitionSecondPartTime + this.BufferTime);
    }

    private string updateTransition()
    {
        return !_switchState ? TRANSITION_STATE : ATTACK_STATE;
    }

    private void exitTransition()
    {

    }

    private void enterAttack()
    {
        _switchState = false;

        for (int i = 0; i < this.BossGroups.Count; ++i)
        {
            this.BossGroups[i].EnterAttackPattern(this.EnterAttackTime, this.AttackDelay);
        }

        _timedCallbacks.AddCallback(this, switchState, this.AttackStateDuration);
    }

    private string updateAttack()
    {
        return !_switchState ? ATTACK_STATE : HOME_STATE;
    }

    private void exitAttack()
    {

    }
}
