﻿using UnityEngine;

public class Weapon : VoBehavior
{
    public WeaponType WeaponType;
    public AllegianceInfo AllegianceInfo;
    public GameObject BulletPrefab;
    public GameObject LaserPrefab;
    public GameObject WeaponAudioObject;
    public AudioClip AudioOnFire;
    public float AudioCooldown = 0.5f;

    public void Fire(Vector2 direction, float shotStartDistance = 0.0f)
    {
        if (_shotCooldown <= 0.0f)
        {
            if (direction.x != 0 || direction.y != 0)
            {
                GameObject prefab = this.WeaponType.TravelType == WeaponType.TRAVEL_TYPE_LASER ? this.LaserPrefab : this.BulletPrefab;

                _shotCooldown = this.WeaponType.ShotCooldown;
                direction.Normalize();

                int shotCount = this.WeaponType.ShotCount;
                float shotAngle = this.WeaponType.AngleBetweenShots;
                bool oddCount = shotCount % 2 == 1;

                if (oddCount)
                {
                    createBullet(direction, shotStartDistance, prefab);
                    shotCount -= 1;
                }
                else
                {
                    shotAngle /= 2.0f;
                }

                while (shotCount > 0)
                {
                    createBullet(direction.VectorAtAngle(shotAngle), shotStartDistance, prefab);
                    createBullet(direction.VectorAtAngle(-shotAngle), shotStartDistance, prefab);

                    shotCount -= 2;
                    shotAngle += this.WeaponType.AngleBetweenShots;
                }

                if (this.WeaponAudioObject != null && this.AudioOnFire != null && _audioCooldown <= 0.0f)
                {
                    _audioCooldown = this.AudioCooldown;
                    AudioSource audio = this.WeaponAudioObject.GetComponent<AudioSource>();
                    audio.clip = this.AudioOnFire;
                    audio.Play();
                }
            }
        }
    }

    void Update()
    {
        if (_shotCooldown > 0.0f)
            _shotCooldown -= Time.deltaTime;

        if (_audioCooldown > 0.0f)
            _audioCooldown -= Time.deltaTime;
    }

    /**
     * Private
     */
    private float _shotCooldown;
    private float _audioCooldown;

    private void createBullet(Vector2 direction, float shotStartDistance, GameObject prefab)
    {
        // Create instance of bullet prefab and set it on its way
        Vector3 position = this.transform.position + (new Vector3(direction.x, direction.y, 0) * shotStartDistance);
        GameObject bullet = Instantiate(prefab, position, Quaternion.identity) as GameObject;
        bullet.GetComponent<Bullet>().LaunchWithWeaponType(direction, this.WeaponType, this.AllegianceInfo);
    }
}
