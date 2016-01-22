using UnityEngine;

public class LootDropper : VoBehavior
{
    public LootManager LootManager;

    void Start()
    {
        this.GetComponent<Damagable>().OnDeathCallbacks.Add(this.OnDeath);
    }

    void OnDeath(Damagable died)
    {
        GameObject toSpawn = LootManager.GenerateLootDrop();
        if (toSpawn != null)
        {
            Instantiate(toSpawn, this.transform.position, Quaternion.identity);
        }
    }
}
