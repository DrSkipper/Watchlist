using UnityEngine;

public class GenericEnemy : VoBehavior
{
    public float MaxSpeed = 140.0f;
    public float Acceleration = 280.0f;
    public float SpinRange = 40.0f;
    public float LookAtRange = 30.0f;
    public float ShootRange = 20.0f;
    public float ShotStartDistance = 0.0f;
    public float RotationSpeed = 180.0f;
    public float RotationOffset = 45.0f;
    public bool OnlyShootOnPause = false;
    public float PauseAngle = 45.0f;
    public float PauseDuration = 0.0f;
    public int[] PossibleRotationDirections = { 1, -1 };
    public Vector2[] PossibleStartingVelocities;
    public Transform[] Targets;
    public LayerMask BounceLayerMask;
    public WeaponType CollisionWeapon;

    public const float MAX_DISTANCE = 500000.0f;

    void Start()
    {
        _currentAngle = this.RotationOffset;
        _rotationAxis = new Vector3(0, 0, 1);
        _weapon = this.GetComponent<Weapon>();
        _rotationDirection = this.PossibleRotationDirections[Random.Range(0, this.PossibleRotationDirections.Length)];

        _actor = this.GetComponent<Actor2D>();
        if (_actor != null && this.MaxSpeed > 0.0f)
        {
            _actor.Velocity = this.PossibleStartingVelocities.Length > 0 ? this.PossibleStartingVelocities[Random.Range(0, this.PossibleStartingVelocities.Length)] : Vector2.zero;
            _rotationDirection = this.PossibleRotationDirections[Random.Range(0, this.PossibleRotationDirections.Length)];
            this.localNotifier.Listen(CollisionEvent.NAME, this, this.OnCollide);
        }

        Damager damager = this.GetComponent<Damager>();
        if (damager != null)
        {
            damager.Damage = this.CollisionWeapon.Damage;
            damager.Knockback = this.CollisionWeapon.Knockback;
            damager.HitInvincibilityDuration = this.CollisionWeapon.HitInvincibilityDuration;
        }
    }

    void Update()
    {
        float distance = MAX_DISTANCE;
        Vector2 aimAxis = Vector2.zero;

        // Find the closest target
        if (this.Targets.Length > 0)
        {
            Transform closest = this.Targets[0];
            Vector2 ourPosition = new Vector2(this.transform.position.x, this.transform.position.y);
            Vector2 theirPosition = new Vector2(closest.position.x, closest.position.y);
            aimAxis = theirPosition - ourPosition;
            distance = aimAxis.magnitude;

            for (int i = 1; i < this.Targets.Length; ++i)
            {
                Transform current = this.Targets[i];
                Vector2 position = new Vector2(current.position.x, current.position.y);
                float currentDistance = Vector2.Distance(ourPosition, position);
                if (currentDistance < distance)
                {
                    closest = current;
                    theirPosition = position;
                    distance = currentDistance;
                }
            }
        }

        // Accelerate
        if (_actor != null && this.MaxSpeed > 0.0f)
        {
            float vMag = _actor.Velocity.magnitude;
            if (vMag < this.MaxSpeed)
            {
                vMag += this.Acceleration * Time.deltaTime;
                if (vMag > this.MaxSpeed)
                    vMag = this.MaxSpeed;

                _actor.Velocity = vMag * _actor.Velocity.normalized;
            }
        }

        // If close enough, turn to face the target
        if (distance < this.LookAtRange)
        {
            aimAxis.Normalize();

            _currentAngle = Vector2.Angle(Vector2.up, aimAxis);
            if (aimAxis.x > 0.0f)
                _currentAngle = -_currentAngle;
            _currentAngle += this.RotationOffset;

            this.transform.localRotation = Quaternion.AngleAxis(_currentAngle, _rotationAxis);
        }

        // Rotate by rotation speed
        else if (distance < this.SpinRange)
        {
            if (!_paused)
            {
                float additionalAngle = _rotationDirection * this.RotationSpeed * Time.deltaTime;
                float distSincePause = _distanceSincePause + additionalAngle;

                if (_usesPauses && distSincePause >= this.PauseAngle)
                {
                    additionalAngle = this.PauseAngle - _distanceSincePause;
                    _distanceSincePause = 0.0f;
                    _pauseTimer = this.PauseDuration;
                }
                else
                {
                    _distanceSincePause = distSincePause;
                }

                _currentAngle = (_currentAngle + additionalAngle) % 360.0f;
                this.transform.localRotation = Quaternion.AngleAxis(_currentAngle, _rotationAxis);
            }
            else
            {
                _pauseTimer -= Time.deltaTime;
            }
        }

        // If close enough, shoot at the target
        if (_weapon != null && distance < this.ShootRange && (!this.OnlyShootOnPause || (this.OnlyShootOnPause && _paused)))
        {
            Vector2 forward = aimAxis;
            if (forward.x == 0.0f && forward.y == 0.0f)
            {
                float rad = (_currentAngle + this.RotationOffset) * Mathf.Deg2Rad;
                forward = new Vector2(Mathf.Sin(rad), -Mathf.Cos(rad)).normalized;
            }
            _weapon.Fire(forward, this.ShotStartDistance);
        }
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
    private Vector3 _rotationAxis;
    private Weapon _weapon;
    private float _currentAngle;
    private float _distanceSincePause;
    private bool _pausing;
    private float _pauseTimer;
    private int _rotationDirection;

    private bool _usesPauses { get { return this.PauseDuration > 0.0f; } }
    private bool _paused { get { return _pauseTimer > 0.0f; } }
}
