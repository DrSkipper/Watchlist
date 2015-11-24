using UnityEngine;

[System.Serializable]
public struct AllegianceInfo
{
    public Allegiance Allegiance;
    public int MemberId;

    public string LayerString
    {
        get
        {
            switch (this.Allegiance)
            {
                case Allegiance.Player:
                    return "Player";
                case Allegiance.Enemy:
                    return "Enemy";
                default:
                    return "";
            }
        }
    }
}

public enum Allegiance
{
    NeutralPassive,
    NeutralAggressive,
    Player,
    Enemy
}

public enum ColorPaletteState
{
    Main,
    Projectile,
    Explosion,
    Damaged
}
