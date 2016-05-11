using UnityEngine;

public class WeaponAutoFire : VoBehavior
{
    public float ShotStartDistance = 4.0f;
    public int WeaponId = 1;

    void Awake()
    {
        _weapon = this.GetComponent<Weapon>();
        _weapon.WeaponType = StaticData.WeaponData.WeaponTypes[this.WeaponId];
    }

    void Update()
    {
        _weapon.Fire(Vector2.up, this.ShotStartDistance);
    }

    /**
     * Private
     */
    private Weapon _weapon;
}
