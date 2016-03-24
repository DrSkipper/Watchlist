using UnityEngine;
using System.Collections.Generic;

public static class ProgressData
{
    public static IntegerVector[] CompletedTiles { get { return _completedTiles.ToArray(); } }
    public static IntegerVector MostRecentTile { get { return _mostRecentTile.HasValue ? _mostRecentTile.Value : IntegerVector.Zero; } }

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

    public static int GetCurrentDifficulty()
    {
        int radius = Mathf.Max(Mathf.Abs(ProgressData.MostRecentTile.X), Mathf.Abs(ProgressData.MostRecentTile.Y));
        if (radius <= 1)
            return 0; // Easy
        if (radius >= 3)
            return 2; // Hard
        return 1; // Medium
    }

    public static void WipeData()
    {
        _completedTiles = new List<IntegerVector>();
        _mostRecentTile = null;
    }

    /**
     * Private
     */
    private static List<IntegerVector> _completedTiles = new List<IntegerVector>();
    private static IntegerVector? _mostRecentTile = null;
}
