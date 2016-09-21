using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(IntegerCircleCollider))]
public class Explosion : VoBehavior
{
    //TODO - Make these more parameters on weapon?
    public float ExplosionDuration = 1.0f;
    public float RadiusToPowerMultiplier = 1.0f;
    public WeaponType WeaponType;
    public bool LineOfSight = false;
    public LayerMask LineOfSightBlockers = 0;
    public string ObjectPoolKey = "bomb";

    public void DetonateWithWeaponType(WeaponType weaponType, int layer, LayerMask damagableLayers, AllegianceInfo allegianceInfo)
    {
        this.gameObject.layer = layer;
        _finalRadius = this.RadiusToPowerMultiplier * weaponType.SpecialEffectParameter1;
        _growthRate = _finalRadius / this.ExplosionDuration;

        if (_circleRenderer == null)
            _circleRenderer = this.GetComponent<CircleRenderer>();
        if (_damager == null)
            _damager = this.GetComponent<Damager>();
        _damager.DamagableLayers = damagableLayers;
        _damager.Damage = weaponType.Damage;
        _damager.Knockback = weaponType.Knockback;
        _damager.HitInvincibilityDuration = weaponType.HitInvincibilityDuration;

        _allegianceInfo = allegianceInfo;
        if (allegianceInfo.Allegiance == Allegiance.Player)
            _damager.AddAttackLandedCallback(landedAttack);

        this.GetComponent<AllegianceColorizer>().UpdateVisual(allegianceInfo);
        if (_collider == null)
            _collider = this.GetComponent<IntegerCollider>();
        else
            _collider.AddToCollisionPool();

        ExplosionSounder.Detonate();
    }

    void Update()
    {
        if (_trueRadius >= _finalRadius)
        {
            _destructionScheduled = true;
        }
        else
        {
            _trueRadius += _growthRate * Time.deltaTime;
            IntegerCircleCollider circleCollider = this.integerCollider as IntegerCircleCollider;
            circleCollider.Radius = Mathf.RoundToInt(_trueRadius);
            _circleRenderer.Radius = _trueRadius;

            int prevCount = _collisions.Count;
            circleCollider.Collide(_collisions, 0, 0, _damager.DamagableLayers);

            for (int i = prevCount; i < _collisions.Count; ++i)
            {
                Damagable damagable = _collisions[i].GetComponent<Damagable>();
                if (damagable != null && !damagable.Invincible)
                {
                    if (!this.LineOfSight)
                        this.localNotifier.SendEvent(new HitEvent(_collisions[i]));
                    else
                    {
                        Vector2 direction = (_collisions[i].transform.position - this.transform.position).normalized;
                        float distance = Vector2.Distance(this.transform.position, _collisions[i].transform.position);
                        if (distance < 2.0f)
                        {
                            this.localNotifier.SendEvent(new HitEvent(_collisions[i]));
                        }
                        else
                        {
                            Vector2 start = (Vector2)this.transform.position + direction * Mathf.Max(1.0f, Mathf.Min(Mathf.Min(_trueRadius / 10.0f, 4.5f), distance));
                            distance = Vector2.Distance(start, _collisions[i].transform.position);
                            if (distance < 2.0f || !CollisionManager.RaycastFirst(start, direction, distance, this.LineOfSightBlockers).Collided)
                                this.localNotifier.SendEvent(new HitEvent(_collisions[i]));
                        }
                    }
                }
            }
        }
    }

    void LateUpdate()
    {
        if (_destructionScheduled)
            reset();
    }

    /**
     * Private
     */
    private float _growthRate;
    private float _trueRadius;
    private float _finalRadius;
    private bool _destructionScheduled;
    private Damager _damager;
    private CircleRenderer _circleRenderer;
    private List<GameObject> _collisions = new List<GameObject>();
    private AllegianceInfo _allegianceInfo;
    private IntegerCollider _collider;

    private void reset()
    {
        if (_collider != null)
            _collider.RemoveFromCollisionPool();
        if (_allegianceInfo.Allegiance == Allegiance.Player)
            _damager.RemoveAttackLandedCallback(landedAttack);
        _trueRadius = 0.0f;
        _destructionScheduled = false;
        _collisions.Clear();
        ObjectPools.ReturnPooledObject(this.ObjectPoolKey, this.gameObject);
    }

    private void landedAttack(Damager damager, int damageDone, bool killingBlow)
    {
        int points = killingBlow ? ProgressData.POINTS_FOR_KILL : ProgressData.POINTS_FOR_HIT;
        ProgressData.ApplyPointsDeltaForPlayer(_allegianceInfo.MemberId, points);
        GlobalEvents.Notifier.SendEvent(new PlayerPointsReceivedEvent(_allegianceInfo.MemberId, points));
    }
}
