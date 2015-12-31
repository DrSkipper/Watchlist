using System.Collections.Generic;

public static class DynamicData
{
    public const int WEAPON_SLOTS = 4;

    public static IntegerVector[] CompletedTiles { get { return _completedTiles.ToArray(); } }
    public static IntegerVector MostRecentTile { get { return _mostRecentTile.HasValue ? _mostRecentTile.Value : IntegerVector.Zero; } }
    public static WeaponData.Slot[][] WeaponSlotsByPlayer
    {
        get
        {
            WeaponData.Slot[][] playerSlots = new WeaponData.Slot[_weaponSlotsByPlayer.Count][];
            foreach (int player in _weaponSlotsByPlayer.Keys)
            {
                playerSlots[player] = _weaponSlotsByPlayer[player];
            }
            return playerSlots;
        }
    }
    
    public static void CompleteTile(IntegerVector tile)
    {
        if (!_completedTiles.Contains(tile))
            _completedTiles.Add(tile);
        _mostRecentTile = tile;
    }

    public static void SelectTile(IntegerVector tile)
    {
        _mostRecentTile = tile;
    }

    public static void UpdatePlayer(int playerIndex, WeaponData.Slot[] slots)
    {
        _weaponSlotsByPlayer[playerIndex] = slots;
    }

    public static void ResetData()
    {
        _completedTiles = new List<IntegerVector>();
        _mostRecentTile = null;
        _weaponSlotsByPlayer = new Dictionary<int, WeaponData.Slot[]>();
    }

    /**
     * Private
     */
    private static List<IntegerVector> _completedTiles = new List<IntegerVector>();
    private static IntegerVector? _mostRecentTile = null;
    private static Dictionary<int, WeaponData.Slot[]> _weaponSlotsByPlayer = new Dictionary<int, WeaponData.Slot[]>();
}
