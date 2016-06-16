using UnityEngine;

public class LootManager : ScriptableObject
{
    [System.Serializable]
    public struct LootType
    {
        public GameObject LootPrefab;
        public int DropWeight;
        public int AdditionalPlayerWeightMod;

        public int GetCurrentWeight(int numAdditionalPlayers)
        {
            return this.DropWeight + this.AdditionalPlayerWeightMod * numAdditionalPlayers;
        }
    }

    public LootType[] LootTable;

    public GameObject GenerateLootDrop()
    {
        int totalWeight = 0;
        int numAdditionalPlayers = DynamicData.NumJoinedPlayers() - 1;
        foreach (LootType lootType in this.LootTable)
        {
            totalWeight += lootType.GetCurrentWeight(numAdditionalPlayers);
        }

        int choice = Random.Range(0, totalWeight);

        foreach (LootType lootType in this.LootTable)
        {
            choice -= lootType.GetCurrentWeight(numAdditionalPlayers);
            if (choice <= 0)
                return lootType.LootPrefab;
        }
        return null;
    }
}
