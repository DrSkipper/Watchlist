using UnityEngine;
using System.Collections.Generic;

public class Damager : VoBehavior
{
    public LayerMask DamagableLayers = Physics2D.DefaultRaycastLayers;
    public int Damage = 1;
    public int Knockback = 1;
    public float HitInvincibilityDuration = 1.0f;

    void Awake()
    {
        this.localNotifier.Listen(HitEvent.NAME, this, this.OnHit);
    }

    public void OnHit(LocalEventNotifier.Event localEvent)
    {
        GameObject hit = ((HitEvent)localEvent).Hit;
        if (((1 << hit.layer) & this.DamagableLayers) != 0 && !_alreadyHitThisUpdate.Contains(hit))
        {
            Damagable damagable = hit.GetComponent<Damagable>();
            if (damagable != null && !damagable.Invincible)
            {
                damagable.ReceiveDamage(this);
                this.ApplyDamage(damagable);
            }
        }
    }

    void LateUpdate()
    {
        _alreadyHitThisUpdate.Clear();
    }

    public void ApplyDamage(Damagable other)
    {
        _alreadyHitThisUpdate.Add(other.gameObject);
    }

    /**
     * Private
     */
    private List<GameObject> _alreadyHitThisUpdate = new List<GameObject>();
}
