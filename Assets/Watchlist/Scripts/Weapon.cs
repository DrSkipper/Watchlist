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

                int shotCount = this.WeaponType.ShotCount;
                float shotAngle = this.WeaponType.AngleBetweenShots;
                bool oddCount = shotCount % 2 == 1;

                if (oddCount)
                {
                    createBullet(direction, shotStartDistance);
                    shotCount -= 1;
                }
                else
                {
                    shotAngle /= 2.0f;
                }

                while (shotCount > 0)
                {
                    createBullet(direction.VectorAtAngle(shotAngle), shotStartDistance);
                    createBullet(direction.VectorAtAngle(-shotAngle), shotStartDistance);

                    shotCount -= 2;
                    shotAngle += this.WeaponType.AngleBetweenShots;
                }
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

    private void createBullet(Vector2 direction, float shotStartDistance)
    {
        // Create instance of bullet prefab and set velocity on it's BulletController component
        Vector3 position = this.transform.position + (new Vector3(direction.x, direction.y, 0) * shotStartDistance);
        GameObject bullet = Instantiate(BulletPrefab, position, Quaternion.identity) as GameObject;
        bullet.GetComponent<BulletController>().Velocity = new Vector2(direction.x * this.WeaponType.ShotSpeed, direction.y * this.WeaponType.ShotSpeed);
    }
}
