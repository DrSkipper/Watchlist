using UnityEngine;
using System.Collections.Generic;

public class BossHealth : VoBehavior
{
    public int MaxHealth = 10;
    public int CurrentHealth = 10;
    public List<Damagable> Damagables;
    public List<HealthChangedCallback> Callbacks;
    public float DeathShakeMagnitude = 300.0f;

    public delegate void HealthChangedCallback(int currentHealth);

    void Awake()
    {
        this.Callbacks = new List<HealthChangedCallback>();
    }

    void Start()
    {
        for (int i = 0; i < this.Damagables.Count; ++i)
        {
            this.Damagables[i].OnHealthChangeCallbacks.Add(damagableHit);
            this.Damagables[i].OnDeathCallbacks.Add(damagableDied);
        }

        runCallbacks();
    }

    public void AddDamagable(Damagable damagable)
    {
        this.Damagables.Add(damagable);
        damagable.OnHealthChangeCallbacks.Add(damagableHit);
        damagable.OnDeathCallbacks.Add(damagableDied);
    }

    /**
     * Private
     */
    private void damagableHit(Damagable damgable, int damage)
    {
        this.CurrentHealth -= damage;

        if (this.CurrentHealth <= 0)
        {
            Camera.main.GetComponent<ShakeHandler>().ApplyImpact(this.DeathShakeMagnitude);
        }

        runCallbacks();
    }

    private void damagableDied(Damagable damagable)
    {
        this.Damagables.Remove(damagable);
    }

    private void runCallbacks()
    {
        for (int i = 0; i < this.Callbacks.Count; ++i)
            this.Callbacks[i](this.CurrentHealth);
    }
}
