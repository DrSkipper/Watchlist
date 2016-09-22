using UnityEngine;

public class MiniBossBehavior : VoBehavior
{
    public Damagable Damagable;
    public LookAtPlayer LookAt;
    public Damagable[] Subs;

    public float HalfSize = 10.0f;
    public float IdleMovementSpeed = 100.0f;
    public float AlertMovementSpeed = 280.0f;
    public float MaxMovementDistance = 300.0f;
    public float MinMovementDistance = 50.0f;
    public float MovementCooldown = 1.0f;
    public float AlertCooldownSpeedMultiplier = 2.0f;
    public int RayDirections = 8;
    public LayerMask HaltMovementMask = 0;
    public Damagable.DeathCallback DeathCallback;
    public string BossGibsPoolKey = "boss_gibs";

    void Start()
    {
        _timedCallbacks = this.GetComponent<TimedCallbacks>();
        _lerpMovement = this.GetComponent<LerpMovement>();
        _lerpMovement.AddCallback(movementComplete);
        this.Damagable.OnDeathCallbacks.Add(onDeath);
        ObjectPools.Preload(this.BossGibsPoolKey, this.Subs.Length);
    }

    void Update()
    {
        if (!_dead)
        {
            if (!_lerpMovement.IsMoving)
            {
                _moveCooldown -= Time.deltaTime * (this.LookAt.IsLookingAt ? this.AlertCooldownSpeedMultiplier : 1.0f);

                if (_moveCooldown <= 0.0f)
                {
                    _lerpMovement.MovementSpeed = this.LookAt.IsLookingAt ? this.AlertMovementSpeed : this.IdleMovementSpeed;

                    Vector2? target = findTarget();

                    if (target.HasValue)
                    {
                        _lerpMovement.BeginMovement(target.Value);
                        _moveCooldown = this.MovementCooldown;
                    }
                }
            }
            else
            {
                _lerpMovement.MovementSpeed = this.LookAt.IsLookingAt ? this.AlertMovementSpeed : this.IdleMovementSpeed;
            }
        }
    }

    /**
     * Private
     */
    private TimedCallbacks _timedCallbacks;
    private LerpMovement _lerpMovement;
    private bool _dead;
    private float _moveCooldown;

    public const float WIGGLE = 2.0f;

    private Vector2? findTarget()
    {
        float angleInc = 360.0f / this.RayDirections;
        int angleDir = Mathf.RoundToInt(Mathf.Sign(1.0f - Random.Range(0.0f, 2.0f)));
        if (angleDir == 0)
            angleDir = 1;
        for (int i = 0; i < this.RayDirections; ++i)
        {
            Vector2 direction = ((Vector2)(this.LookAt.transform.forward)).VectorAtAngle(i * angleInc * angleDir);
            float distance = this.MaxMovementDistance + this.HalfSize;
            CollisionManager.RaycastResult result = CollisionManager.RaycastFirst(new IntegerVector(this.transform.position), direction, distance, this.HaltMovementMask);

            if (!result.Collided)
            {
                distance -= (this.HalfSize + WIGGLE);

                if (distance > this.MinMovementDistance)
                {
                    distance = Random.Range(this.MinMovementDistance, distance);

                    IntegerVector offset = new IntegerVector(direction * distance);

                    if (this.LookAt.integerCollider.CollideFirst(offset.X, offset.Y, this.HaltMovementMask) == null)
                    {
                        return (Vector2)this.transform.position + direction * distance;
                    }
                }
            }
        }

        return null;
    }

    private void movementComplete(GameObject go)
    {

    }

    private void onDeath(Damagable died)
    {
        _dead = true;

        for (int i = 0; i < this.Subs.Length; ++i)
        {
            this.Subs[i].Kill(0.0f);
        }

        _lerpMovement.HaltMovement();
        _timedCallbacks.AddCallback(this, destroy, 0.2f);

        if (this.DeathCallback != null)
            DeathCallback(died);
    }

    private void destroy()
    {
        Destroy(this.gameObject);
    }
}
