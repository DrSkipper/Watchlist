using UnityEngine;

public class EnemyRotatingShooter : VoBehavior
{
    public float InitialAngle = 0.0f;
    public float SpinRange = 20.0f;
    public float ShootRange = 20.0f;
    public float ShotStartDistance = 0.0f;
    public float RotationSpeed = 180.0f;
    public Transform[] Targets;
    public bool YIsUp = false;

    void Start()
    {
        _currentAngle = this.InitialAngle;
        _rotationAxis = this.YIsUp ? new Vector3(0, 0, 1) : new Vector3(0, 1, 0);
        _weapon = this.GetComponent<Weapon>();
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
            _currentAngle = _currentAngle + (this.RotationSpeed * Time.deltaTime) % 360.0f;
            this.transform.localRotation = Quaternion.AngleAxis(_currentAngle % 360.0f, _rotationAxis);

            // If close enough, shoot at the target
            if (_weapon != null && distance < this.ShootRange)
            {
                float rad = _currentAngle * Mathf.PI / 180.0f;
                //float halfPi = Mathf.PI / 2.0f;
                Vector2 forward = new Vector2(Mathf.Cos(rad), Mathf.Sign(rad));
                //Vector2 right = new Vector2(Mathf.Cos(rad + halfPi), Mathf.Sin(rad + halfPi));
                //Vector2 left = new Vector2(Mathf.Cos(rad - halfPi), Mathf.Sin(rad - halfPi));
                //Vector2 back = new Vector2(Mathf.Cos(rad + Mathf.PI), Mathf.Sin(rad + Mathf.PI));

                _weapon.Fire(forward, this.ShotStartDistance);
                //_weapon.Fire(right, this.ShotStartDistance);
                //_weapon.Fire(left, this.ShotStartDistance);
                //_weapon.Fire(back, this.ShotStartDistance);
            }
        }
    }

    /**
     * Private
     */
    private Vector3 _rotationAxis;
    private Weapon _weapon;
    private float _currentAngle;
}
