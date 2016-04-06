using UnityEngine;
using System.Collections.Generic;

public class GenericEnemy : VoBehavior
{
    public EnemyType EnemyType;
    public int[] PossibleRotationDirections = { 1, -1 };
    public Vector2[] PossibleStartingVelocities;
    public List<Transform> Targets;
    public LayerMask BounceLayerMask;
    public Texture2D SpriteSheet;
    public GameObject ExplosionPrefab;
    public bool UseDebugWeapon = false;

    public enum MovementType
    {
        Free = 0,
        Seek = 1,
        MoveToward = 2
    }

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

        switch (this.EnemyType.MovementType)
        {
            default:
            case (int)MovementType.Free:
                _movementFunction = freeMovement;
                break;
            case (int)MovementType.Seek:
                _movementFunction = seekMovement;
                break;
            case (int)MovementType.MoveToward:
                _movementFunction = moveTowardMovement;
                break;
        }

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
            
            if (this.CollisionWeaponType != null && this.CollisionWeaponType.SpecialEffect == WeaponType.SPECIAL_EXPLOSION)
            {
                damagable.OnDeathCallbacks.Add(onDeath);
            }
        }

        this.spriteRenderer.sprite = this.SpriteSheet.GetSprites()[this.EnemyType.SpriteName];
        GlobalEvents.Notifier.Listen(PlayerDiedEvent.NAME, this, playerDied);
    }

    void Update()
    {
        float distance = MAX_DISTANCE;
        Vector2 aimAxis = Vector2.zero;
        Vector2 forward = Vector2.zero;

        // Find the closest target
        if (this.Targets.Count > 0)
        {
            Vector2 ourPosition = (Vector2)this.transform.position;

            for (int i = 0; i < this.Targets.Count; ++i)
            {
                Transform current = this.Targets[i];
                Vector2 theirPosition = current.position;
                float d = Vector2.Distance(ourPosition, theirPosition);
                if (d < distance)
                {
                    distance = d;
                    aimAxis = theirPosition - ourPosition;
                }
            }
        }

        aimAxis.Normalize();

        // Accelerate
        if (_actor != null && this.EnemyType.MaxSpeed > 0.0f)
        {
            _movementFunction(aimAxis, distance);
        }

        // If close enough, turn to face the target
        if (distance < this.EnemyType.LookAtRange)
        {
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
            if (this.CollisionWeaponType != null && this.CollisionWeaponType.SpecialEffect == WeaponType.SPECIAL_EXPLOSION && ((1 << hit.layer) & this.GetComponent<Damager>().DamagableLayers) != 0)
            {
                // Explode on impact
                Damagable damagable = this.GetComponent<Damagable>();
                while (damagable.Health > 0)
                {
                    damagable.ReceiveDamage(this.GetComponent<Damager>());
                }
            }
            else if (((1 << hit.layer) & this.BounceLayerMask) != 0)
            {
                _actor.Bounce(hit, collision.VelocityAtHit, collision.VelocityApplied, this.BounceLayerMask, 0.0f);
                break;
            }
        }
    }

    public override void OnDestroy()
    {
        if (GlobalEvents.Notifier != null)
            GlobalEvents.Notifier.RemoveAllListenersForOwner(this);
        base.OnDestroy();
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
    private MovementDelegate _movementFunction;

    private bool _usesPauses { get { return this.EnemyType.PauseDuration > 0.0f; } }
    private bool _paused { get { return _pauseTimer > 0.0f; } }
    private delegate void MovementDelegate(Vector2 aimAxis, float distance);
    private const float WIGGLE_ROOM = 2.0f;

    private void onDeath(Damagable damagable)
    {
        if (this.CollisionWeaponType != null && this.CollisionWeaponType.SpecialEffect == WeaponType.SPECIAL_EXPLOSION)
        {
            AllegianceInfo info = this.GetComponent<AllegianceColorizer>().AllegianceInfo;
            LayerMask damagableLayers = this.GetComponent<Damager>().DamagableLayers;
            Bullet.CreateExplosionEntity(this.transform.position, this.ExplosionPrefab, info, this.gameObject.layer, damagableLayers, this.CollisionWeaponType);
        }
    }

    private void playerDied(LocalEventNotifier.Event playerDiedEvent)
    {
        this.Targets.Remove((playerDiedEvent as PlayerDiedEvent).PlayerObject.transform);
    }

    private void freeMovement(Vector2 aimAxis, float distance)
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

    private void seekMovement(Vector2 aimAxis, float distance)
    {
        float targetX = aimAxis.x * this.EnemyType.MaxSpeed;
        float targetY = aimAxis.y * this.EnemyType.MaxSpeed;

        float changeX = targetX - _actor.Velocity.x;
        float changeY = targetY - _actor.Velocity.y;

        if (changeX != 0 || changeY != 0)
        {
            float changeTotal = Mathf.Sqrt(Mathf.Pow(changeX, 2) + Mathf.Pow(changeY, 2));

            if (changeX != 0)
            {
                float ax = Mathf.Abs(this.EnemyType.Acceleration * changeX / changeTotal);
                _actor.Velocity.x = Mathf.MoveTowards(_actor.Velocity.x, targetX, ax * Time.deltaTime);
            }

            if (changeY != 0)
            {
                float ay = Mathf.Abs(this.EnemyType.Acceleration * changeY / changeTotal);
                _actor.Velocity.y = Mathf.MoveTowards(_actor.Velocity.y, targetY, ay * Time.deltaTime);
            }
        }

        //TODO - Apply TargetDistance
    }

    private void moveTowardMovement(Vector2 aimAxis, float distance)
    {
        float vMag = _actor.Velocity.magnitude;

        if (vMag < this.EnemyType.MaxSpeed)
        {
            vMag += this.EnemyType.Acceleration * Time.deltaTime;
            if (vMag > this.EnemyType.MaxSpeed)
                vMag = this.EnemyType.MaxSpeed;
        }

        if (Mathf.Abs(distance - this.EnemyType.TargetDistance) > WIGGLE_ROOM)
        {
            if (distance < this.EnemyType.TargetDistance)
                aimAxis *= -1;
            _actor.Velocity = aimAxis * vMag;
        }
        else
        {
            _actor.Velocity = Vector2.zero;
        }
    }
}
