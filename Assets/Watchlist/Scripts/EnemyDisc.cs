using UnityEngine;

[RequireComponent(typeof(Actor2D))]
public class EnemyDisc : VoBehavior
{
    public float MaxSpeed = 140.0f;
    public float Acceleration = 280.0f;
    public float RotationSpeed = 90.0f;
    public Vector2[] PossibleStartingVelocities;
    public int[] PossibleRotationDirections = { 1, -1 };
    public LayerMask BounceLayerMask;
    public WeaponType WeaponType;

    void Start()
    {
        _actor = this.GetComponent<Actor2D>();
        _actor.Velocity = this.PossibleStartingVelocities[Random.Range(0, this.PossibleStartingVelocities.Length)];
        _rotationDirection = this.PossibleRotationDirections[Random.Range(0, this.PossibleRotationDirections.Length)];
        this.localNotifier.Listen(CollisionEvent.NAME, this, this.OnCollide);

        Damager damager = this.GetComponent<Damager>();
        if (damager != null)
        {
            damager.Damage = this.WeaponType.Damage;
            damager.Knockback = this.WeaponType.Knockback;
            damager.HitInvincibilityDuration = this.WeaponType.HitInvincibilityDuration;
        }
    }

    void Update()
    {
        float vMag = _actor.Velocity.magnitude;
        if (vMag < this.MaxSpeed)
        {
            vMag += this.Acceleration * Time.deltaTime;
            if (vMag > this.MaxSpeed)
                vMag = this.MaxSpeed;

            _actor.Velocity = vMag * _actor.Velocity.normalized;
        }

        this.transform.localRotation = Quaternion.AngleAxis(_currentAngle, new Vector3(0, 0, 1));
        _currentAngle = (_currentAngle + _rotationDirection * this.RotationSpeed * Time.deltaTime) % 360.0f;
    }

    public void OnCollide(LocalEventNotifier.Event localEvent)
    {
        CollisionEvent collision = localEvent as CollisionEvent;
        foreach (GameObject hit in collision.Hits)
        {
            if (((1 << hit.layer) & this.BounceLayerMask) != 0)
            {
                _actor.Bounce(hit, collision.VelocityAtHit, collision.VelocityApplied, this.BounceLayerMask, 0.0f);
                break;
            }
        }
    }

    /**
     * Private
     */
    private Actor2D _actor;
    private float _currentAngle;
    private int _rotationDirection;
}
