using UnityEngine;

public class WeaponPickup : VoBehavior
{
    [System.Serializable]
    public enum PickupType
    {
        WeaponSlot,
        HealthRefill
    }

    [System.Serializable]
    public struct Contents
    {
        public PickupType Type;
        public int Parameter;
    }

    public Contents PickupContents;
}
