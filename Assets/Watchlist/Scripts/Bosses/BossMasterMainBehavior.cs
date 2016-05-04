using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(TimedCallbacks))]
[RequireComponent(typeof(Rotation))]
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
    public float AngleOffset = 0.0f;
    public float MaxSpeed = 50.0f;
    public float TargetDistance = 100.0f;

    void Awake()
    {
        _timedCallbacks = this.GetComponent<TimedCallbacks>();
        _rotation = this.GetComponent<Rotation>();
        _actor = this.GetComponent<Actor2D>();
        GlobalEvents.Notifier.Listen(PlayerSpawnedEvent.NAME, this, playerSpawned);
    }

    void Start()
    {
        _stateMachine = new FSMStateMachine();
        _stateMachine.AddState(HOME_STATE, updateHome, enterHome, exitHome);
        _stateMachine.AddState(TRANSITION_STATE, updateTransition, enterTransition, exitTransition);
        _stateMachine.AddState(ATTACK_STATE, updateAttack, enterAttack, exitAttack);
        _stateMachine.BeginWithInitialState(HOME_STATE);
        _seeking = true;
        _timedCallbacks.AddCallback(this, switchState, this.HomeStateDuration);
    }

    void Update()
    {
        _stateMachine.Update();
    }

    /**
     * Private
     */
    private FSMStateMachine _stateMachine;
    private TimedCallbacks _timedCallbacks;
    private Actor2D _actor;
    private Rotation _rotation;
    private bool _switchState;
    private bool _seeking;

    private const string HOME_STATE = "home";
    private const string TRANSITION_STATE = "transition";
    private const string ATTACK_STATE = "attack";
    private const float WIGGLE_ROOM = 10.0f;

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
        _timedCallbacks.AddCallback(this, beginSeeking, this.ReturnHomeTime + this.BufferTime);
    }

    private void beginSeeking()
    {
        _seeking = true;
    }

    private string updateHome()
    {
        if (_seeking)
        {
            // Find center of targets
            Vector2 avgPosition = Vector2.zero;
            int count = 0;
            for (int i = 0; i < this.Targets.Count; ++i)
            {
                if (this.Targets[i] != null)
                {
                    avgPosition += (Vector2)this.Targets[i].position;
                    ++count;
                }
            }

            if (count > 0)
                avgPosition /= count;
            
            // Lerp rotation to point at center of targets
            Vector2 aimAxis = avgPosition - (Vector2)this.transform.position;
            float distance = aimAxis.magnitude;
            aimAxis.Normalize();

            float targetAngle = Vector2.Angle(Vector2.up, aimAxis);
            if (aimAxis.x < 0.0f)
            {
                targetAngle = -targetAngle;
                targetAngle -= this.AngleOffset;
            }
            else
            {
                targetAngle += this.AngleOffset;
            }

            float currentAngle = _rotation.GetAngle();
            _rotation.SetAngle(Mathf.MoveTowardsAngle(currentAngle, targetAngle, _rotation.RotationSpeed * Time.deltaTime));

            // Lerp movement toward target distance from center of targets
            if (Mathf.Abs(distance - this.TargetDistance) > WIGGLE_ROOM)
            {
                if (distance < this.TargetDistance)
                    aimAxis *= -1;
                _actor.Velocity = aimAxis * this.MaxSpeed;
            }
            else
            {
                _actor.Velocity = Vector2.zero;
            }
        }

        return !_switchState ? HOME_STATE : TRANSITION_STATE;
    }

    private void exitHome()
    {
        _seeking = false;
        _actor.Velocity = Vector2.zero;
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
