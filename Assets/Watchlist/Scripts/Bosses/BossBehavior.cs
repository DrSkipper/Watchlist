using UnityEngine;
using System.Collections.Generic;

public class BossBehavior : VoBehavior
{
    public List<GameObject> OtherBosses;
    public float WeaponCooldown = 0.0332f;
    public float ShotStartDistance = 5.0f;
    public Vector2[] StartingVelocities;
    public float StartingSpeed;
    public int[] PossibleRotationDirections = { 1, -1 };
    public float RotationSpeed = 180.0f;

    void Start()
    {
        _actor = this.GetComponent<Actor2D>();
        _weapon = this.GetComponent<Weapon>();
        _rotationAxis = new Vector3(0, 0, 1);
        _rotationDirection = this.PossibleRotationDirections[Random.Range(0, this.PossibleRotationDirections.Length)];

        Vector2 velocity = this.StartingVelocities[Random.Range(0, this.StartingVelocities.Length)];
        _actor.SetVelocityModifier("boss_normal", new VelocityModifier(this.StartingSpeed * velocity.normalized, VelocityModifier.CollisionBehavior.bounce));
    }

    void Update()
    {
        if (_firing)
        {
            if (_fireDurationCountdown > 0.0f)
            {
                if (_weaponCooldown <= 0.0f)
                {
                    foreach (GameObject boss in this.OtherBosses)
                    {
                        Vector2 diff = boss.transform.position - this.transform.position;
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
}
