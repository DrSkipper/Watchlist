using UnityEngine;
using System;
using System.Collections.Generic;

public class Bullet : Actor2D
{
    public WeaponType WeaponType;
    public LayerMask BounceLayerMask = Physics2D.DefaultRaycastLayers;
    public GameObject ExplosionPrefab;

    public void LaunchWithWeaponType(Vector2 direction, WeaponType weaponType, AllegianceInfo allegianceInfo)
    {
        this.WeaponType = weaponType;
        this.Velocity = weaponType.TravelType == WeaponType.TRAVEL_TYPE_LASER ? Vector2.zero : 
            new Vector2(direction.x * weaponType.ShotSpeed, direction.y * weaponType.ShotSpeed);

        _allegianceInfo = allegianceInfo;
        AllegianceColorizer allegianceColorizer = this.GetComponent<AllegianceColorizer>();
        if (allegianceColorizer != null)
            allegianceColorizer.UpdateVisual(allegianceInfo);

        _bouncesRemaining = weaponType.MaximumBounces;
        this.localNotifier.Listen(CollisionEvent.NAME, this, this.OnCollide);

        string allegianceString = allegianceInfo.LayerString;
        int ourLayer = LayerMask.NameToLayer(allegianceString + " Missile");
        this.gameObject.layer = ourLayer;
        LayerMask alliedVulnerable = (1 << LayerMask.NameToLayer(allegianceString + " Vulnerable"));
        LayerMask levelGeometryMask = (1 << LayerMask.NameToLayer("Level Geometry"));
        LayerMask pickupMask = (1 << LayerMask.NameToLayer("Pickup"));
        LayerMask everything = int.MaxValue;
        LayerMask nothing = 0;
        LayerMask alliedLayers = alliedVulnerable | GetMissileLayers();

        _damager = this.GetComponent<Damager>();
        _damager.DamagableLayers = (~alliedVulnerable) & GetVulnerableLayers();
        _damager.Damage = weaponType.Damage;
        _damager.Knockback = weaponType.Knockback;
        _damager.HitInvincibilityDuration = weaponType.HitInvincibilityDuration;

        this.BounceLayerMask = weaponType.MaximumBounces > 0 ? levelGeometryMask : nothing;
        this.CollisionMask = everything & (~alliedLayers) & (~pickupMask);
        this.HaltMovementMask = weaponType.TravelType == WeaponType.TRAVEL_TYPE_LASER ? levelGeometryMask : this.CollisionMask;
        
        if (weaponType.TravelType == WeaponType.TRAVEL_TYPE_LASER)
            handleLaserCast(direction);
    }

    public override void Update()
    {
        base.Update();

        _lifetime += Time.deltaTime;
        _distance += this.Velocity * Time.deltaTime;

        if (_lifetime >= this.WeaponType.DurationTime || _distance.magnitude >= this.WeaponType.DurationDistance)
        {
            scheduleDestruction(this.transform.position);
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
            Destroy(this.gameObject);
    }

    public void OnCollide(LocalEventNotifier.Event localEvent)
    {
        CollisionEvent collision = localEvent as CollisionEvent;

        bool bounced = false;
        foreach (GameObject hit in collision.Hits)
        {
            LayerMask hitLayerMask = (1 << hit.layer);
            if (_bouncesRemaining > 0 && (hitLayerMask & this.BounceLayerMask) != 0)
            {
                if (!bounced)
                    bounce(hit, collision.VelocityAtHit, collision.VelocityApplied);
            }
            else if ((hitLayerMask & this.HaltMovementMask) != 0)
            {
                scheduleDestruction(this.transform.position);
            }
        }
    }

    /**
     * Private
     */
    private float _lifetime = 0.0f;
    private bool _destructionScheduled;
    private Vector2 _distance = Vector2.zero;
    private int _bouncesRemaining;
    private bool _hasExploded;
    private Damager _damager;
    private AllegianceInfo _allegianceInfo;

    private void scheduleDestruction(Vector3 location)
    {
        _destructionScheduled = true;

        if (!_hasExploded && this.WeaponType.SpecialEffect == WeaponType.SPECIAL_EXPLOSION)
            triggerExplosion(location);
    }

    private void bounce(GameObject hit, Vector2 origVelocity, Vector2 appliedVelocity)
    {
        --_bouncesRemaining;
        if (!this.Bounce(hit, origVelocity, appliedVelocity, this.BounceLayerMask, this.WeaponType.MinimumBounceAngle))
            scheduleDestruction(this.transform.position);
    }

    private void handleLaserCast(Vector2 direction)
    {
        IntegerVector origin = this.integerPosition;
        CollisionManager.RaycastResult raycast = this.CollisionManager.RaycastFirst(origin, direction, this.WeaponType.DurationDistance, this.HaltMovementMask | this.BounceLayerMask);
        this.localNotifier.SendEvent(new LaserCastEvent(raycast, origin, _allegianceInfo));

        Vector2 castDifference = raycast.FarthestPointReached - origin;
        CollisionManager.RaycastResult passThroughCast = this.CollisionManager.Raycast(origin, castDifference.normalized, castDifference.magnitude, this.CollisionMask);
        foreach (CollisionManager.RaycastCollision collision in passThroughCast.Collisions)
            this.localNotifier.SendEvent(new HitEvent(collision.CollidedObject));

        if (raycast.Collided && _bouncesRemaining > 0 && ((1 << raycast.Collisions[0].CollidedObject.layer) & this.BounceLayerMask) != 0)
        {
            IntegerVector distanceTravelled = raycast.FarthestPointReached - origin;
            float distanceSoFar = new Vector2(distanceTravelled.X, distanceTravelled.Y).magnitude;

            while (raycast.Collided && _bouncesRemaining > 0 && distanceSoFar < this.WeaponType.DurationDistance && ((1 << raycast.Collisions[0].CollidedObject.layer) & this.BounceLayerMask) != 0)
            {
                --_bouncesRemaining;
                origin = raycast.FarthestPointReached;
                Vector2 origDirection = direction;

                if (raycast.Collisions[0].CollidedX)
                    direction.x = -direction.x;
                if (raycast.Collisions[0].CollidedY)
                    direction.y = -direction.y;

                // Only continue the bounce if our angle is within bounce range
                if (Mathf.Abs(180.0f - Vector2.Angle(origDirection, direction)) < this.WeaponType.MinimumBounceAngle)
                    break;

                // Find point along bounce path that is at least 1 unit forward in both x and y, to prevent immediate collision with same object
                IntegerVector raycastOrigin = origin;
                float min = Mathf.Min(Mathf.Abs(direction.x), Mathf.Abs(direction.y));
                float multiplier = 1.0f / min;
                Vector2 multipliedDirection = direction * multiplier;
                float distanceRemaining = this.WeaponType.DurationDistance - distanceSoFar;
                float multipliedMagnitude = multipliedDirection.magnitude;
                if (multipliedMagnitude + 0.5f < distanceRemaining)
                {
                    raycastOrigin = new IntegerVector(Mathf.RoundToInt(direction.x * multiplier), Mathf.RoundToInt(direction.y * multiplier)) + origin;
                    distanceRemaining -= multipliedMagnitude;
                }

                // Raycast the bounce shot
                raycast = this.CollisionManager.RaycastFirst(raycastOrigin, direction, distanceRemaining, this.HaltMovementMask | this.BounceLayerMask);
                this.localNotifier.SendEvent(new LaserCastEvent(raycast, origin, _allegianceInfo));

                castDifference = raycast.FarthestPointReached - raycastOrigin;
                passThroughCast = this.CollisionManager.Raycast(raycastOrigin, castDifference.normalized, castDifference.magnitude, this.CollisionMask);
                foreach (CollisionManager.RaycastCollision collision in passThroughCast.Collisions)
                    this.localNotifier.SendEvent(new HitEvent(collision.CollidedObject));

                distanceTravelled = raycast.FarthestPointReached - origin;
                distanceSoFar += new Vector2(distanceTravelled.X, distanceTravelled.Y).magnitude;
            }
        }
    }

    private void triggerExplosion(Vector3 position)
    {
        _hasExploded = true;
        GameObject explosion = Instantiate(this.ExplosionPrefab, position, Quaternion.identity) as GameObject;
        explosion.GetComponent<Explosion>().DetonateWithWeaponType(this.WeaponType, this.gameObject.layer, _damager.DamagableLayers);
        explosion.GetComponent<AllegianceColorizer>().UpdateVisual(_allegianceInfo);
    }
    
    private static LayerMask MISSILE_LAYERS = 0;
    private static LayerMask VULNERABLE_LAYERS = 0;

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

    private static LayerMask GetVulnerableLayers()
    {
        if (VULNERABLE_LAYERS == 0)
        {
            List<string> vulnerableLayerNames = new List<string>();
            for (int i = 8; i < 32; ++i)
            {
                string layerName = LayerMask.LayerToName(i);
                if (layerName.Contains("Vulnerable"))
                    vulnerableLayerNames.Add(layerName);
            }
            VULNERABLE_LAYERS = LayerMask.GetMask(vulnerableLayerNames.ToArray());
        }

        return VULNERABLE_LAYERS;
    }
}
