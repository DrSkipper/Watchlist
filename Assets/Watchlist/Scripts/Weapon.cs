using UnityEngine;

public class Weapon : VoBehavior
{
    public WeaponType WeaponType = new WeaponType();
    public GameObject BulletPrefab;

    public void Fire(Vector2 direction, float shotStartDistance = 0.0f)
    {
        if (_shotCooldown <= 0.0f)
        {
            _shotCooldown = this.WeaponType.ShotCooldown;

            if (direction.x != 0 || direction.y != 0)
            {
                direction.Normalize();

                // Create instance of bullet prefab and set velocity on it's BulletController component
                Vector3 position = this.transform.position + (new Vector3(direction.x, direction.y, 0) * shotStartDistance);
                GameObject bullet = Instantiate(BulletPrefab, position, Quaternion.identity) as GameObject;
                bullet.GetComponent<BulletController>().Velocity = new Vector2(direction.x * this.WeaponType.ShotSpeed, direction.y * this.WeaponType.ShotSpeed);
            }
        }
        else
        {
            _shotCooldown -= Time.deltaTime;
        }
    }

    /**
     * Private
     */
    private float _shotCooldown;
}
