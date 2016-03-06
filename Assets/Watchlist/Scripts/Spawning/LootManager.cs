using UnityEngine;

public class LootManager : ScriptableObject
{
    [System.Serializable]
    public struct LootType
    {
        public GameObject LootPrefab;
        public int DropWeight;
    }

    public LootType[] LootTable;

    public GameObject GenerateLootDrop()
    {
        int totalWeight = 0;
        foreach (LootType lootType in this.LootTable)
        {
            totalWeight += lootType.DropWeight;
        }

        int choice = Random.Range(0, totalWeight);

        foreach (LootType lootType in this.LootTable)
        {
            choice -= lootType.DropWeight;
            if (choice <= 0)
                return lootType.LootPrefab;
        }
        return null;
    }
}
