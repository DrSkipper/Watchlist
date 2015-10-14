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
        _actor = this.GetComponent<Actor2D>();
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
        
        if (this.Invincible && !this.Stationary)
        {
            VelocityModifier v = _actor.GetVelocityModifier(VELOCITY_MODIFIER_KEY);
            float vMag = v.Modifier.magnitude;
            vMag -= this.Friction * Time.deltaTime;
            if (vMag <= 0.0f)
            {
                _actor.RemoveVelocityModifier(VELOCITY_MODIFIER_KEY);
            }
            else
            {
                v.Modifier = v.Modifier.normalized * vMag;
                _actor.SetVelocityModifier(VELOCITY_MODIFIER_KEY, v);
            }
        }
    }

    void LateUpdate()
    {
        if (this.Invincible && _invincibilityTimer <= 0.0f)
        {
            this.Invincible = false;
            _actor.CollisionMask = _nonInvincibleCollisionMask;
            _actor.RemoveVelocityModifier(VELOCITY_MODIFIER_KEY);
            this.integerCollider.AddToCollisionPool();
            this.localNotifier.SendEvent(new InvincibilityToggleEvent(false));
        }

        _alreadyHitThisUpdate.Clear();
    }

    public void ReceiveDamage(Damager other)
    {
        _alreadyHitThisUpdate.Add(other.gameObject);

        this.Health -= other.Damage;

        if (this.Health <= 0)
        {
            //TODO - Die gracefully
            Destroy(this.gameObject);
        }
        else
        {
            if (!this.Stationary)
            {
                Vector2 difference = this.integerPosition - other.integerPosition;
                Vector2 otherV = other.gameObject.GetComponent<Actor2D>().Velocity;
                difference.Normalize();
                otherV.Normalize();
                _actor.SetVelocityModifier(VELOCITY_MODIFIER_KEY, new VelocityModifier((difference + otherV).normalized * other.Knockback, VelocityModifier.CollisionBehavior.bounce));
            }

            this.Invincible = true;
            _invincibilityTimer = other.HitInvincibilityDuration;
            _nonInvincibleCollisionMask = _actor.CollisionMask;
            _actor.CollisionMask = this.InvincibilityCollisionMask;
            this.integerCollider.RemoveFromCollisionPool();

            this.localNotifier.SendEvent(new InvincibilityToggleEvent(true));
        }
    }

    /**
     * Private
     */
    private List<GameObject> _alreadyHitThisUpdate = new List<GameObject>();
    private float _invincibilityTimer;
    private LayerMask _nonInvincibleCollisionMask;
    private Actor2D _actor;

    private const string VELOCITY_MODIFIER_KEY = "damagable";
}
