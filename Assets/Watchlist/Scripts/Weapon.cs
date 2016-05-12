using UnityEngine;

public class Weapon : VoBehavior
{
    public WeaponType WeaponType;
    public AllegianceInfo AllegianceInfo;
    public GameObject BulletPrefab;
    public GameObject LaserPrefab;
    public GameObject WeaponAudioObject;
    public AudioClip[] FireAudio;
    public ShotFiredDelegate ShotFiredCallback;
    public float ShotAngleOffset = 0.0f;

    public delegate void ShotFiredDelegate(bool ignoreExplosions);

    void Start()
    {
        _audio = this.WeaponAudioObject.GetComponent<AudioSource>();
    }

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
                
                bool ignoreExplosions = false;
                if (this.WeaponType.TravelType == WeaponType.TRAVEL_TYPE_LASER && this.WeaponType.SpecialEffect == WeaponType.SPECIAL_EXPLOSION)
                {
                    if (_explosionCooldown > 0.0f)
                        ignoreExplosions = true;
                    else
                        _explosionCooldown = this.WeaponType.SpecialEffectParameter2;
                }

                if (oddCount)
                {
                    createBullet(direction.VectorAtAngle(this.ShotAngleOffset), shotStartDistance, prefab, ignoreExplosions);
                    shotCount -= 1;
                }
                else
                {
                    shotAngle /= 2.0f;
                }

                while (shotCount > 0)
                {
                    createBullet(direction.VectorAtAngle(shotAngle + this.ShotAngleOffset), shotStartDistance, prefab, ignoreExplosions);
                    createBullet(direction.VectorAtAngle(-shotAngle + this.ShotAngleOffset), shotStartDistance, prefab, ignoreExplosions);

                    shotCount -= 2;
                    shotAngle += this.WeaponType.AngleBetweenShots;
                }

                if (this.WeaponAudioObject != null && _audioCooldown <= 0.0f && this.FireAudio.Length > this.WeaponType.AudioIndex)
                {
                    AudioClip clip = this.FireAudio[this.WeaponType.AudioIndex];
                    _audioCooldown = this.WeaponType.AudioCooldown <= 0.0f ? clip.length : this.WeaponType.AudioCooldown;
                    _audio.clip = clip;
                    _audio.Play();
                }

                if (this.ShotFiredCallback != null)
                    this.ShotFiredCallback(ignoreExplosions);
            }
        }
    }

    void Update()
    {
        if (_shotCooldown > 0.0f)
            _shotCooldown -= Time.deltaTime;

        if (_audioCooldown > 0.0f)
            _audioCooldown -= Time.deltaTime;

        if (_explosionCooldown > 0.0f)
            _explosionCooldown -= Time.deltaTime;
    }

    /**
     * Private
     */
    private float _shotCooldown;
    private float _audioCooldown;
    private float _explosionCooldown;
    private AudioSource _audio;

    private void createBullet(Vector2 direction, float shotStartDistance, GameObject prefab, bool ignoreExplosions)
    {
        // Create instance of bullet prefab and set it on its way
        Vector3 position = this.transform.position + (new Vector3(direction.x, direction.y, 0) * shotStartDistance);
        GameObject bullet = Instantiate(prefab, position, Quaternion.identity) as GameObject;
        bullet.GetComponent<Bullet>().LaunchWithWeaponType(direction, this.WeaponType, this.AllegianceInfo, ignoreExplosions);
    }
}
