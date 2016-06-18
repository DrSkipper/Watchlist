using UnityEngine;

public class ShootWhenLookingAt : VoBehavior
{
    public LookAtPlayer LookAt;
    public WeaponAutoFire WeaponFire;

    void Update()
    {
        this.WeaponFire.Paused = !this.LookAt.IsLookingAt;
    }
}
