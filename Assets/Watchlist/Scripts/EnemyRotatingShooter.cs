using UnityEngine;

public class EnemyRotatingShooter : VoBehavior
{
    public float InitialAngle = 0.0f;
    public float SpinRange = 20.0f;
    public float ShootRange = 20.0f;
    public float ShotStartDistance = 0.0f;
    public float RotationSpeed = 180.0f;
    public float ShotRotationOffset = 45.0f;
    public int[] PossibleRotationDirections = { 1, -1 };
    public Transform[] Targets;
    public float PauseAngle = 45.0f;
    public float PauseDuration = 0.0f;
    public bool OnlyShootOnPause = false;
    public bool YIsUp = false;

    void Start()
    {
        _currentAngle = this.InitialAngle;
        _rotationAxis = this.YIsUp ? new Vector3(0, 0, 1) : new Vector3(0, 1, 0);
        _weapon = this.GetComponent<Weapon>();
        _rotationDirection = this.PossibleRotationDirections[Random.Range(0, this.PossibleRotationDirections.Length)];
    }

    void Update()
    {
        if (this.Targets.Length == 0)
            return;

        // Find the closest target
        Transform closest = this.Targets[0];
        Vector2 ourPosition = new Vector2(this.transform.position.x, this.YIsUp ? this.transform.position.y : this.transform.position.z);
        Vector2 theirPosition = new Vector2(closest.position.x, this.YIsUp ? closest.position.y : closest.position.z);
        float distance = Vector2.Distance(ourPosition, theirPosition);

        for (int i = 1; i < this.Targets.Length; ++i)
        {
            Transform current = this.Targets[i];
            Vector2 position = new Vector2(current.position.x, this.YIsUp ? current.position.y : current.position.z);
            float currentDistance = Vector2.Distance(ourPosition, position);
            if (currentDistance < distance)
            {
                closest = current;
                theirPosition = position;
                distance = currentDistance;
            }
        }

        // Rotate by rotation speed
        if (distance < this.SpinRange)
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

            // If close enough, shoot at the target
            if (_weapon != null && distance < this.ShootRange && (!this.OnlyShootOnPause || (this.OnlyShootOnPause && _paused)))
            {
                float rad = (_currentAngle + this.ShotRotationOffset) * Mathf.Deg2Rad;
                Vector2 forward = new Vector2(Mathf.Sin(rad), -Mathf.Cos(rad)).normalized;
                _weapon.Fire(forward, this.ShotStartDistance);
            }
        }
    }

    /**
     * Private
     */
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
