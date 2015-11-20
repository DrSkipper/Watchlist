using UnityEngine;

[System.Serializable]
public struct AllegianceInfo
{
    public Allegiance Allegiance;
    public int MemberId;
}

public enum Allegiance
{
    NeutralPassive,
    NeutralAggressive,
    Player,
    Enemy
}

public enum ColorPaletteClass
{
    Main,
    Projectile,
    Explosion,
    Damaged
}
