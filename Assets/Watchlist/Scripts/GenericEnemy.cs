using UnityEngine;

public class GenericEnemy : VoBehavior
{
    public EnemyType EnemyType;
    public int[] PossibleRotationDirections = { 1, -1 };
    public Vector2[] PossibleStartingVelocities;
    public Transform[] Targets;
    public LayerMask BounceLayerMask;
    public bool UseDebugWeapon = false;

    public WeaponType WeaponType { get {
        if (StaticData.WeaponData.WeaponTypes.ContainsKey(this.EnemyType.WeaponTypeId))
            return StaticData.WeaponData.WeaponTypes[this.EnemyType.WeaponTypeId];
        return null;
    } }

    public WeaponType CollisionWeaponType { get {
        if (StaticData.WeaponData.WeaponTypes.ContainsKey(this.EnemyType.CollisionWeaponTypeId))
            return StaticData.WeaponData.WeaponTypes[this.EnemyType.CollisionWeaponTypeId];
        return null;
    } }

    public const float MAX_DISTANCE = 500000.0f;

    void Start()
    {
        _currentAngle = this.EnemyType.RotationOffset;
        _rotationAxis = new Vector3(0, 0, 1);
        _weapon = this.GetComponent<Weapon>();
        _rotationDirection = this.PossibleRotationDirections[Random.Range(0, this.PossibleRotationDirections.Length)];

        _actor = this.GetComponent<Actor2D>();
        if (_actor != null && this.EnemyType.MaxSpeed > 0.0f)
        {
            _actor.Velocity = this.PossibleStartingVelocities.Length > 0 ? this.PossibleStartingVelocities[Random.Range(0, this.PossibleStartingVelocities.Length)] : Vector2.zero;
            _rotationDirection = this.PossibleRotationDirections[Random.Range(0, this.PossibleRotationDirections.Length)];
            this.localNotifier.Listen(CollisionEvent.NAME, this, this.OnCollide);
        }

        if (!this.UseDebugWeapon && this.WeaponType != null && _weapon != null)
            _weapon.WeaponType = this.WeaponType;

        Damager damager = this.GetComponent<Damager>();
        if (damager != null)
        {
            damager.Damage = this.CollisionWeaponType.Damage;
            damager.Knockback = this.CollisionWeaponType.Knockback;
            damager.HitInvincibilityDuration = this.CollisionWeaponType.HitInvincibilityDuration;
        }

        Damagable damagable = this.GetComponent<Damagable>();
        if (damagable != null)
        {
            damagable.Health = this.EnemyType.Health;
            damagable.Friction = this.EnemyType.Friction;
        }
    }

    void Update()
    {
        float distance = MAX_DISTANCE;
        Vector2 aimAxis = Vector2.zero;
        Vector2 forward = Vector2.zero;

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
        if (_actor != null && this.EnemyType.MaxSpeed > 0.0f)
        {
            float vMag = _actor.Velocity.magnitude;
            if (vMag < this.EnemyType.MaxSpeed)
            {
                vMag += this.EnemyType.Acceleration * Time.deltaTime;
                if (vMag > this.EnemyType.MaxSpeed)
                    vMag = this.EnemyType.MaxSpeed;

                _actor.Velocity = vMag * _actor.Velocity.normalized;
            }
        }

        // If close enough, turn to face the target
        if (distance < this.EnemyType.LookAtRange)
        {
            aimAxis.Normalize();
            forward = aimAxis;

            _currentAngle = Vector2.Angle(Vector2.up, aimAxis);
            if (aimAxis.x > 0.0f)
                _currentAngle = -_currentAngle;
            _currentAngle += this.EnemyType.RotationOffset;

            this.transform.localRotation = Quaternion.AngleAxis(_currentAngle, _rotationAxis);
        }

        // Rotate by rotation speed
        else if (distance < this.EnemyType.SpinRange)
        {
            if (!_paused)
            {
                float additionalAngle = this.EnemyType.RotationSpeed * Time.deltaTime;
                float distSincePause = _distanceSincePause + additionalAngle;

                if (_usesPauses && distSincePause >= this.EnemyType.PauseAngle)
                {
                    additionalAngle = this.EnemyType.PauseAngle - _distanceSincePause;
                    _distanceSincePause = 0.0f;
                    _pauseTimer = this.EnemyType.PauseDuration;
                }
                else
                {
                    _distanceSincePause = distSincePause;
                }
                
                _currentAngle = (_currentAngle + additionalAngle * _rotationDirection) % 360.0f;
                this.transform.localRotation = Quaternion.AngleAxis(_currentAngle, _rotationAxis);
            }
            else
            {
                _pauseTimer -= Time.deltaTime;
            }
        }

        // If close enough, shoot at the target
        if (_weapon != null && distance < this.EnemyType.ShootRange && (!this.EnemyType.OnlyShootOnPause || (this.EnemyType.OnlyShootOnPause && _paused)))
        {
            if (forward.x == 0.0f && forward.y == 0.0f)
            {
                float rad = (_currentAngle + this.EnemyType.RotationOffset + 180.0f) * Mathf.Deg2Rad;
                forward = new Vector2(Mathf.Sin(rad), -Mathf.Cos(rad)).normalized;
            }
            _weapon.Fire(forward, this.EnemyType.ShotStartDistance);
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
    private float _pauseTimer;
    private int _rotationDirection;

    private bool _usesPauses { get { return this.EnemyType.PauseDuration > 0.0f; } }
    private bool _paused { get { return _pauseTimer > 0.0f; } }
}
