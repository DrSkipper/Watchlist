using UnityEngine;

[RequireComponent(typeof(IntegerCircleCollider))]
public class Explosion : VoBehavior
{
    //TODO - Make these more parameters on weapon?
    public float ExplosionDuration = 1.0f;
    public float RadiusToPowerMultiplier = 1.0f;
    public WeaponType WeaponType;

    public void DetonateWithWeaponType(WeaponType weaponType, int layer)
    {
        this.gameObject.layer = layer;
        _finalRadius = this.RadiusToPowerMultiplier * weaponType.SpecialEffectParameter1;
        _growthRate = _finalRadius / this.ExplosionDuration;
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
            ((IntegerCircleCollider)this.integerCollider).Radius = Mathf.RoundToInt(_trueRadius);
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
}
