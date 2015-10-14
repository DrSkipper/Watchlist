using UnityEngine;
using System.Collections.Generic;

public class Damagable : VoBehavior
{
    public LayerMask DamagerLayers = Physics2D.DefaultRaycastLayers;
    public LayerMask InvincibilityCollisionMask = Physics2D.DefaultRaycastLayers;
    public int Health = 1;
    public bool Stationary = false;
    public float Friction = 1.0f;
    public bool Invincible;

    void Start()
    {
        this.localNotifier.Listen(HitEvent.NAME, this, this.OnHit);
    }

    public void OnHit(LocalEventNotifier.Event localEvent)
    {
        if (this.Invincible)
            return;

        GameObject hit = ((HitEvent)localEvent).Hit;
        if (((1 << hit.layer) & this.DamagerLayers) != 0 && !_alreadyHitThisUpdate.Contains(hit))
        {
            Damager damager = hit.GetComponent<Damager>();
            if (damager != null)
            {
                damager.ApplyDamage(this);
                this.ReceiveDamage(damager);
            }
        }
    }

    void Update()
    {
        if (_invincibilityTimer > 0.0f)
            _invincibilityTimer -= Time.deltaTime;

        //TODO - Update actor's velocity modifier according to friction
    }

    void LateUpdate()
    {
        if (this.Invincible && _invincibilityTimer <= 0.0f)
        {
            this.Invincible = false;
            this.GetComponent<Actor2D>().CollisionMask = _nonInvincibleCollisionMask;

            //TODO - Have integer collider add self back into collision pool
        }

        _alreadyHitThisUpdate.Clear();
    }

    public void ReceiveDamage(Damager other)
    {
        _alreadyHitThisUpdate.Add(other.gameObject);

        //this.Health -= other.Damage;

        if (this.Health <= 0)
        {
            //TODO - Die gracefully
            Destroy(this.gameObject);
        }
        else
        {
            Actor2D actor = this.GetComponent<Actor2D>();
            actor.SetVelocityModifier("damagable", Vector2.up);

            this.Invincible = true;
            _invincibilityTimer = other.HitInvincibilityDuration;
            _nonInvincibleCollisionMask = actor.CollisionMask;
            actor.CollisionMask = this.InvincibilityCollisionMask;

            //TODO - Have integer collider remove self from collision pool
        }
    }

    /**
     * Private
     */
    private List<GameObject> _alreadyHitThisUpdate = new List<GameObject>();
    private float _invincibilityTimer;
    private LayerMask _nonInvincibleCollisionMask;
}
