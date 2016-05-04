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

    void Awake()
    {
        _lerpMovement = this.GetComponent<LerpMovement>();
    }

    public void GoHome(float duration)
    {
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

    /**
     * Private
     */
    private LerpMovement _lerpMovement;
    private float _freeMovementDelay;

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
        // Find closest target
        // Begin seek movement toward target
        // Lerp movement toward target distance from target
    }
}
