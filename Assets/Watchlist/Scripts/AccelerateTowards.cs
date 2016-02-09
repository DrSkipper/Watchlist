using UnityEngine;

[RequireComponent(typeof(Actor2D))]
public class AccelerateTowards : VoBehavior
{
    public float AccelerationDuration = 0.4f;
    public float MaxSpeed = 260.0f;
    public float RangeToReachTarget = 10.0f;
    public ReachedTargetCallback OnReachedTarget;

    public delegate void ReachedTargetCallback(GameObject go);

    void Awake()
    {
        _actor = this.GetComponent<Actor2D>();
        _acceleration = this.AccelerationDuration > 0 ? this.MaxSpeed / this.AccelerationDuration : this.MaxSpeed * 1000;
    }

    public void SeekTarget(Vector2 target)
    {
        _target = target;
    }

    public void SeekTarget(Vector2 target, ReachedTargetCallback callback)
    {
        this.OnReachedTarget = callback;
        this.SeekTarget(target);
    }

    public void StopSeeking()
    {
        _target = null;
    }

    void Update()
    {
        Vector2 targetVelocity = Vector2.zero;
        float speed = _actor.Velocity.magnitude;

        if (_target.HasValue)
        {
            targetVelocity = _target.Value - (Vector2)this.transform.position;
            targetVelocity.Normalize();
            targetVelocity *= this.MaxSpeed;
        }
        
        float changeX = targetVelocity.x - _actor.Velocity.x;
        float changeY = targetVelocity.y - _actor.Velocity.y;

        if (changeX != 0 || changeY != 0)
        {
            float changeTotal = Mathf.Sqrt(Mathf.Pow(changeX, 2) + Mathf.Pow(changeY, 2));

            if (changeX != 0)
            {
                float ax = Mathf.Abs(_acceleration * changeX / changeTotal);
                _actor.Velocity.x = Mathf.Lerp(_actor.Velocity.x, targetVelocity.x, ax * Time.deltaTime / Mathf.Abs(changeX));
            }

            if (changeY != 0)
            {
                float ay = Mathf.Abs(_acceleration * changeY / changeTotal);
                _actor.Velocity.y = Mathf.Lerp(_actor.Velocity.y, targetVelocity.y, ay * Time.deltaTime / Mathf.Abs(changeY));
            }
        }

        if (_target.HasValue && Vector2.Distance(_target.Value, this.transform.position) <= this.RangeToReachTarget)
        {
            this.OnReachedTarget(this.gameObject);
        }
    }

    /**
     * Private
     */
    private float _acceleration;
    private Vector2? _target;
    private Actor2D _actor;
}
