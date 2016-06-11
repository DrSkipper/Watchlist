using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(IntegerCircleCollider))]
public class Explosion : VoBehavior
{
    //TODO - Make these more parameters on weapon?
    public float ExplosionDuration = 1.0f;
    public float RadiusToPowerMultiplier = 1.0f;
    public WeaponType WeaponType;

    public void DetonateWithWeaponType(WeaponType weaponType, int layer, LayerMask damagableLayers, AllegianceInfo allegianceInfo)
    {
        this.gameObject.layer = layer;
        _finalRadius = this.RadiusToPowerMultiplier * weaponType.SpecialEffectParameter1;
        _growthRate = _finalRadius / this.ExplosionDuration;

        _circleRenderer = this.GetComponent<CircleRenderer>();
        _damager = this.GetComponent<Damager>();
        _damager.DamagableLayers = damagableLayers;
        _damager.Damage = weaponType.Damage;
        _damager.Knockback = weaponType.Knockback;
        _damager.HitInvincibilityDuration = weaponType.HitInvincibilityDuration;

        _allegianceInfo = allegianceInfo;
        if (allegianceInfo.Allegiance == Allegiance.Player)
            _damager.AddAttackLandedCallback(landedAttack);

        this.GetComponent<AllegianceColorizer>().UpdateVisual(allegianceInfo);

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
                this.localNotifier.SendEvent(new HitEvent(_collisions[i]));
        }
    }

    void LateUpdate()
    {
        if (_destructionScheduled)
            Destroy(this.gameObject);
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

    private void landedAttack(Damager damager, int damageDone, bool killingBlow)
    {
        GlobalEvents.Notifier.SendEvent(new PlayerPointsReceivedEvent(_allegianceInfo.MemberId, killingBlow ? ProgressData.POINTS_FOR_KILL : ProgressData.POINTS_FOR_HIT));
    }
}
