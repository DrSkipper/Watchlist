using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(TimedCallbacks))]
[RequireComponent(typeof(LerpMovement))]
public class BossMasterGroupBehavior : VoBehavior
{
    public List<Transform> Targets;
    public Transform HomeTransform;
    public Transform TransitionTransform;
    public Transform AttackStartTransform;
    public Rotation SubElementRotator;
    public GenericEnemy Shooter;
    public float TargetDistance = 100.0f;
    public float MaxSpeed = 60.0f;

    void Awake()
    {
        _lerpMovement = this.GetComponent<LerpMovement>();
        _actor = this.GetComponent<Actor2D>();
    }

    public void GoHome(float duration)
    {
        _seeking = false;
        _actor.Velocity = Vector2.zero;
        this.SubElementRotator.IsRotating = false;
        this.SubElementRotator.ResetRotation();
        _lerpMovement.ClearCallbacks();
        _lerpMovement.BeginMovement(this.HomeTransform.position, duration);
        _lerpMovement.AddCallback(reachedHome);
    }

    public void EnterTransitionPattern(float duration)
    {
        _lerpMovement.ClearCallbacks();
        _lerpMovement.BeginMovement(this.TransitionTransform.position, duration);
        _lerpMovement.AddCallback(reachedTransitionPoint);
    }

    public void EnterAttackPattern(float duration, float freeMovementDelay)
    {
        _freeMovementDelay = freeMovementDelay;
        this.SubElementRotator.IsRotating = true;
        _lerpMovement.ClearCallbacks();
        _lerpMovement.BeginMovement(this.AttackStartTransform.position, duration);
        _lerpMovement.AddCallback(reachedAttackStart);
    }

    void Update()
    {
        if (_seeking)
        {
            // Find closest target
            float distance = float.MaxValue;
            Vector2 aimAxis = Vector2.zero;
            for (int i = 0; i < this.Targets.Count; ++i)
            {
                if (this.Targets[i] == null)
                    continue;

                float d = Vector2.Distance(this.Targets[i].position, this.transform.position);
                if (d < distance)
                {
                    distance = d;
                    aimAxis = this.Targets[i].position - this.transform.position;
                }
            }

            aimAxis.Normalize();

            // Lerp movement toward target distance from target
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
        else
        {
            _actor.Velocity = Vector2.zero;
        }

        if (this.Shooter != null)
        {
            this.Shooter.transform.rotation = this.Shooter.transform.localRotation;
        }
    }

    /**
     * Private
     */
    private LerpMovement _lerpMovement;
    private Actor2D _actor;
    private float _freeMovementDelay;
    private bool _seeking;

    private const float WIGGLE_ROOM = 10.0f;

    private void reachedHome(GameObject gameObject)
    {
    }

    private void reachedTransitionPoint(GameObject gameObject)
    {
    }

    private void reachedAttackStart(GameObject gameObject)
    {
        this.GetComponent<TimedCallbacks>().AddCallback(this, enterFreeMovement, _freeMovementDelay);
    }

    private void enterFreeMovement()
    {
        _seeking = true;
    }
}
