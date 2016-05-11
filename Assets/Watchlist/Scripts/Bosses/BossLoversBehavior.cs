using UnityEngine;
using System.Collections.Generic;

public class BossLoversBehavior : VoBehavior
{
    public List<GameObject> OtherBosses;
    public float WeaponCooldown = 0.0332f;
    public float ShotStartDistance = 5.0f;
    public Vector2[] StartingVelocities;
    public float StartingSpeed;
    public int[] PossibleRotationDirections = { 1, -1 };
    public float RotationSpeed = 180.0f;
    public float MaxRepelDistance = 10.0f;
    public float MinRepelDistance = 80.0f;
    public float RepelStrength = 100.0f;

    void Start()
    {
        _actor = this.GetComponent<Actor2D>();
        _weapon = this.GetComponent<Weapon>();
        _rotationAxis = new Vector3(0, 0, 1);
        _rotationDirection = this.PossibleRotationDirections[Random.Range(0, this.PossibleRotationDirections.Length)];

        Vector2 velocity = this.StartingVelocities[Random.Range(0, this.StartingVelocities.Length)];
        _actor.SetVelocityModifier(NORMAL_VELOCITY, new VelocityModifier(this.StartingSpeed * velocity.normalized, VelocityModifier.CollisionBehavior.bounce));
    }

    void Update()
    {
        // Weapon
        if (_firing)
        {
            if (_fireDurationCountdown > 0.0f)
            {
                if (_weaponCooldown <= 0.0f)
                {
                    for (int i = 0; i < this.OtherBosses.Count; ++i)
                    {
                        Vector2 diff = this.OtherBosses[i].transform.position - this.transform.position;
                        _weapon.WeaponType.DurationDistance = diff.magnitude;
                        _weapon.Fire(diff.normalized, this.ShotStartDistance);
                    }

                    _weaponCooldown = this.WeaponCooldown;
                }

                _weaponCooldown -= Time.deltaTime;
                _fireDurationCountdown -= Time.deltaTime;
            }
            else
            {
                _firing = false;
            }
        }

        // Flow-Velocity
        Vector2 flowVelocity = Vector2.zero;
        int count = 0;
        for (int i = 0; i < this.OtherBosses.Count; ++i)
        {
            GameObject boss = this.OtherBosses[i];
            Vector2 diff = this.transform.position - this.OtherBosses[i].transform.position;
            float mag = diff.magnitude;
            float strength = 0.0f;
            if (mag <= this.MaxRepelDistance)
            {
                ++count;
                strength = this.RepelStrength * Time.deltaTime;
            }
            else if (mag < this.MinRepelDistance)
            {
                ++count;
                strength = (1.0f - Mathf.Max(0.0f, (mag - this.MaxRepelDistance) / this.MinRepelDistance)) * this.RepelStrength * Time.deltaTime;
            }
            flowVelocity += diff.normalized * strength;
        }
        
        if (count > 0)
            flowVelocity /= count;
        
        VelocityModifier mod = _actor.GetVelocityModifier(NORMAL_VELOCITY);
        mod.Modifier = (mod.Modifier.normalized * Mathf.Max(0.0f, (this.StartingSpeed - flowVelocity.magnitude))) + flowVelocity;

        // Rotation
        float additionalAngle = this.RotationSpeed * Time.deltaTime;
        _currentAngle = (_currentAngle + additionalAngle * _rotationDirection) % 360.0f;
        this.transform.localRotation = Quaternion.AngleAxis(_currentAngle, _rotationAxis);
    }

    public void InitiateFire(float duration)
    {
        _firing = true;
        _fireDurationCountdown = duration;
        _weaponCooldown = 0.0f;
    }

    /**
     * Private
     */
    private Actor2D _actor;
    private Weapon _weapon;
    private bool _firing;
    private float _fireDurationCountdown;
    private float _weaponCooldown;
    private int _rotationDirection;
    private Vector3 _rotationAxis;
    private float _currentAngle;

    private const string NORMAL_VELOCITY = "boss_normal";
}
