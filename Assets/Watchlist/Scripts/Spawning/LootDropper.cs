using UnityEngine;

public class LootDropper : VoBehavior
{
    public LootManager LootManager;
    public int AmountToDrop = 1;
    public float MaxDistance = 0.0f;

    void Start()
    {
        this.GetComponent<Damagable>().OnDeathCallbacks.Add(this.OnDeath);
    }

    void OnDeath(Damagable died)
    {
        for (int i = 0; i < this.AmountToDrop; ++i)
        {
            GameObject toSpawn = LootManager.GenerateLootDrop();
            if (toSpawn != null)
            {
                Instantiate(toSpawn, (Vector2)this.transform.position + new Vector2(Random.Range(-this.MaxDistance, this.MaxDistance), Random.Range(-this.MaxDistance, this.MaxDistance)), Quaternion.identity);
            }
        }
    }
}
