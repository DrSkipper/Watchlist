using UnityEngine;

public class EnemyShooter : VoBehavior
{
    public float InitialAngle = 0.0f;
    public float LookAtRange = 30.0f;
    public float ShootRange = 20.0f;
    public float ShotCooldown = 0.5f;
    public float ShotSpeed = 5.0f;
    public float ShotStartDistance = 0.0f;
    public GameObject BulletPrefab;
    public Transform[] Targets;
    public bool YIsUp = false;

    void Start()
    {
        _rotationAxis = this.YIsUp ? new Vector3(0, 0, 1) : new Vector3(0, 1, 0);
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

        // If close enough, turn to face the target
        if (distance < this.LookAtRange)
        {
            Vector2 aimAxis = theirPosition - ourPosition;
            aimAxis.Normalize();

            float angle = Vector2.Angle(Vector2.up, aimAxis);
            //float angle = Vector2.Angle(ourPosition, theirPosition);
            if ((this.YIsUp && theirPosition.x - ourPosition.x > 0.0f) || (!this.YIsUp && theirPosition.x - ourPosition.x < 0.0f))
                angle = -angle;

            this.transform.localRotation = Quaternion.AngleAxis(angle + this.InitialAngle, _rotationAxis);

            // If close enough, shoot at the target
            if (distance < this.ShootRange)
            {
                if (_shotCooldown <= 0.0f)
                {
                    _shotCooldown = this.ShotCooldown;

                    // Create instance of bullet prefab and set velocity on it's BulletController component
                    Vector3 aimAxis3d = this.YIsUp ? new Vector3(aimAxis.x, aimAxis.y, 0) : new Vector3(aimAxis.x, 0, aimAxis.y);

                    Vector3 position = this.transform.position + (aimAxis3d * this.ShotStartDistance);
                    GameObject bullet = Instantiate(BulletPrefab, position, Quaternion.identity) as GameObject;
                    bullet.GetComponent<BulletController>().Velocity = new Vector2(aimAxis.x * this.ShotSpeed, aimAxis.y * this.ShotSpeed);
                }
                else
                {
                    _shotCooldown -= Time.deltaTime;
                }
            }
        }
    }

    /**
     * Private
     */
    private Vector3 _rotationAxis;
    private float _shotCooldown;
}
