using UnityEngine;

[RequireComponent(typeof(Rotation))]
[RequireComponent(typeof(LerpMovement))]
[RequireComponent(typeof(TimedCallbacks))]
public class BossWeakSubBehavior : VoBehavior
{
    public delegate void AttackFinishedCallback(GameObject go);
    public AttackFinishedCallback OnAttackFinished;

    public float MinAttackDistance = 150.0f;
    public bool AttackFinished { get { return _attackFinished; } }

    void Awake()
    {
        _rotation = this.GetComponent<Rotation>();
        _lerpMovement = this.GetComponent<LerpMovement>();
        _timedCallbacks = this.GetComponent<TimedCallbacks>();
    }

    public void ScheduleAttack(Transform target, float attackSpeed, float time, float attackDelay, float returnDelay)
    {
        _attackFinished = false;
        _target = target;
        _attackSpeed = attackSpeed;
        _attackDelay = attackDelay;
        _returnDelay = returnDelay;
        _timedCallbacks.AddCallback(this, prepareAttack, time);
    }

    /**
     * Private
     */
    private Vector2 _homePosition;
    private Rotation _rotation;
    private LerpMovement _lerpMovement;
    private TimedCallbacks _timedCallbacks;
    private Transform _target;
    private float _attackSpeed;
    private float _attackDelay;
    private float _returnDelay;
    private bool _attackFinished;

    private void prepareAttack()
    {
        _homePosition = this.transform.position;
        _rotation.IsRotating = true;
        _timedCallbacks.AddCallback(this, attack, _attackDelay);
    }

    private void attack()
    {
        _lerpMovement.MovementSpeed = _attackSpeed;
        _lerpMovement.AddCallback(attackReturn);

        Vector2 targetPosition = _target.position;
        float distance = Vector2.Distance(targetPosition, _homePosition);
        if (distance < this.MinAttackDistance)
        {
            float angle = Mathf.Atan2((targetPosition.y - _homePosition.y), (targetPosition.x - _homePosition.x));
            targetPosition.x = _homePosition.x + this.MinAttackDistance * Mathf.Cos(angle);
            targetPosition.y = _homePosition.y + this.MinAttackDistance * Mathf.Sin(angle);
        }
        _lerpMovement.BeginMovement(targetPosition);
    }

    private void attackReturn(GameObject go)
    {
        _lerpMovement.ClearCallbacks();
        _timedCallbacks.AddCallback(this, beginReturn, _returnDelay);
    }

    private void beginReturn()
    {
        _lerpMovement.AddCallback(hasReturned);
        _lerpMovement.BeginMovement(_homePosition);
    }

    private void hasReturned(GameObject go)
    {
        _attackFinished = true;
        _lerpMovement.ClearCallbacks();
        _rotation.ResetRotation(true);
        _rotation.IsRotating = false;
        this.OnAttackFinished(go);
    }
}
