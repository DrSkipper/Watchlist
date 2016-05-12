using UnityEngine;

public class WeaponAutoFire : VoBehavior
{
    public float ShotStartDistance = 4.0f;
    public int WeaponId = 1;
    public bool UseRotation = false;
    public bool Paused = false;

    void Awake()
    {
        _weapon = this.GetComponent<Weapon>();
        _weapon.WeaponType = StaticData.WeaponData.WeaponTypes[this.WeaponId];
    }

    void Update()
    {
        if (!this.Paused)
            _weapon.Fire(this.UseRotation ? (Vector2)this.transform.right : Vector2.up, this.ShotStartDistance);
    }

    /**
     * Private
     */
    private Weapon _weapon;
}
