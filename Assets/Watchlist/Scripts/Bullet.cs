using UnityEngine;
using System.Collections.Generic;

public class Bullet : Actor2D
{
    public WeaponType WeaponType = new WeaponType();
    public LayerMask BounceLayerMask = Physics2D.DefaultRaycastLayers;

    public void LaunchWithWeaponType(Vector2 direction, WeaponType weaponType, string allegiance)
    {
        this.WeaponType = weaponType;
        this.Velocity = new Vector2(direction.x * weaponType.ShotSpeed, direction.y * weaponType.ShotSpeed);
        this.localNotifier.Listen(CollisionEvent.NAME, this, this.OnCollide);

        this.gameObject.layer = LayerMask.NameToLayer(allegiance + " Missile");
        LayerMask levelGeometryMask = (1 << LayerMask.NameToLayer("Level Geometry"));
        LayerMask everything = int.MaxValue;
        LayerMask nothing = 0;
        LayerMask alliedLayers = (1 << LayerMask.NameToLayer(allegiance + " Vulnerable")) | GetMissileLayers();

        this.BounceLayerMask = weaponType.MaximumBounces > 0 ? levelGeometryMask : nothing;
        this.CollisionMask = everything & (~alliedLayers);
        this.HaltMovementMask = weaponType.TravelType == WeaponType.TRAVEL_TYPE_LASER ? levelGeometryMask : this.CollisionMask;
    }

    public override void Update()
    {
        base.Update();

        _lifetime += Time.deltaTime;
        _distance += this.Velocity * Time.deltaTime;

        if (_lifetime >= this.WeaponType.DurationTime || _distance.magnitude >= this.WeaponType.DurationDistance)
        {
            _destructionScheduled = true;
        }
        else if (this.WeaponType.VelocityFallOff > 0.0f)
        {
            float magnitude = this.Velocity.magnitude;
            if (magnitude > 0.0f)
            {
                Vector2 normalizedVelocity = this.Velocity.normalized;
                this.Velocity = normalizedVelocity * Mathf.Max((magnitude - this.WeaponType.VelocityFallOff * Time.deltaTime), 0.0f);
            }
        }
    }

    void LateUpdate()
    {
        if (_destructionScheduled)
        {
            this.localNotifier.RemoveAllListenersForOwner(this);
            Destroy(this.gameObject);
        }
    }

    public void OnCollide(LocalEventNotifier.Event localEvent)
    {
        CollisionEvent collision = localEvent as CollisionEvent;

        foreach (GameObject hit in collision.Hits)
        {
            if (((1 << hit.layer) & this.HaltMovementMask) != 0)
            {
                _destructionScheduled = true;
                break;
            }
        }
    }

    /**
     * Private
     */
    private float _lifetime = 0.0f;
    private bool _destructionScheduled;
    private Vector2 _distance = Vector2.zero;

    private static LayerMask MISSILE_LAYERS = 0;
    private static LayerMask GetMissileLayers()
    {
        if (MISSILE_LAYERS == 0)
        {
            List<string> missileLayerNames = new List<string>();
            for (int i = 8; i < 32; ++i)
            {
                string layerName = LayerMask.LayerToName(i);
                if (layerName.Contains("Missile"))
                    missileLayerNames.Add(layerName);
            }
            MISSILE_LAYERS = LayerMask.GetMask(missileLayerNames.ToArray());
        }

        return MISSILE_LAYERS;
    }
}
