using UnityEngine;

[RequireComponent(typeof(LerpMovement))]
public class BossMasterSubBehavior : VoBehavior
{
    public Transform HomeTransform;
    public Transform AttackTransform;

    void Awake()
    {
        this.HomeTransform.position = this.transform.position;
        _lerpMovement = this.GetComponent<LerpMovement>();
    }

    public void GoHome(float duration)
    {
        this.transform.parent = this.HomeTransform;
        _lerpMovement.ClearCallbacks();
        _lerpMovement.BeginMovement(this.HomeTransform.position, duration);
        _lerpMovement.AddCallback(reachedHome);
    }

    public void GoToAttackPosition(float duration)
    {
        this.transform.parent = this.AttackTransform;
        _lerpMovement.ClearCallbacks();
        _lerpMovement.BeginMovement(this.AttackTransform.position, duration);
        _lerpMovement.AddCallback(reachedAttackPosition);
    }

    /**
     * Private
     */
    private LerpMovement _lerpMovement;

    private void reachedHome(GameObject gameObject)
    {
    }

    private void reachedAttackPosition(GameObject gameObject)
    {
    }
}
