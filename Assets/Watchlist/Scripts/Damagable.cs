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
    public DeathCallback OnDeath;
    public GameObject GibsPrefab;
    public float ShakeMagnitudeOnDeath;

    public delegate void DeathCallback(Damagable died);

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

            if (!this.Stationary)
            {
                _actor.CollisionMask = _nonInvincibleCollisionMask;
                _actor.RemoveVelocityModifier(VELOCITY_MODIFIER_KEY);
            }

            this.integerCollider.AddToCollisionPool();
            this.localNotifier.SendEvent(new InvincibilityToggleEvent(false));
        }

        if (_markedForDeath)
        {
            die();
        }

        _alreadyHitThisUpdate.Clear();
    }

    public void ReceiveDamage(Damager other)
    {
        _alreadyHitThisUpdate.Add(other.gameObject);

        this.Health -= other.Damage;

        Vector2 impactVector = Vector2.zero;

        if (!this.Stationary)
        {
            Vector2 difference = this.integerPosition - other.integerPosition;
            Actor2D otherActor = other.gameObject.GetComponent<Actor2D>();
            Vector2 otherV = otherActor != null ? otherActor.Velocity : Vector2.zero;
            difference.Normalize();
            otherV.Normalize();
            impactVector = (difference + otherV).normalized;
        }

        if (this.Health <= 0)
        {
            _deathKnockback = other.Knockback;
            _deathImpactVector = impactVector;
            markForDeath();
        }
        else
        {
            if (!this.Stationary)
            {
                _actor.SetVelocityModifier(VELOCITY_MODIFIER_KEY, new VelocityModifier(impactVector * other.Knockback, VelocityModifier.CollisionBehavior.bounce));

                _nonInvincibleCollisionMask = _actor.CollisionMask;
                _actor.CollisionMask = this.InvincibilityCollisionMask;
            }

            this.Invincible = true;
            _invincibilityTimer = other.HitInvincibilityDuration;
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
    private bool _markedForDeath;
    private float _deathKnockback;
    private Vector2 _deathImpactVector;

    private const string VELOCITY_MODIFIER_KEY = "damagable";

    private void markForDeath()
    {
        _markedForDeath = true;
    }

    private void die()
    {
        if (this.OnDeath != null)
            this.OnDeath(this);

        if (this.GibsPrefab != null)
        {
            GameObject gibs = (GameObject)Instantiate(this.GibsPrefab, this.transform.position, this.transform.rotation);
            GibsBehavior gibsBehavior = gibs.GetComponent<GibsBehavior>();
            gibsBehavior.Knockback = _deathKnockback;
            gibsBehavior.ImpactVector = _deathImpactVector;
        }

        if (this.ShakeMagnitudeOnDeath > 0.0f)
            Camera.main.GetComponent<ShakeHandler>().ApplyImpact(this.ShakeMagnitudeOnDeath);

        Destroy(this.gameObject);
    }
}
