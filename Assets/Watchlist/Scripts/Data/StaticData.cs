using UnityEngine;
using System.IO;

public static class StaticData
{
    public const string DATA_ROOT = "Data/";
    public const string WEAPON_DATA_PATH = DATA_ROOT + "weapons.xml";
    public const string ENEMY_DATA_PATH = DATA_ROOT + "enemies.xml";
    public const string BOSS_DATA_PATH = DATA_ROOT + "bosses.xml";

    public static WeaponData WeaponData { get {
        if (_weaponData == null) _weaponData = WeaponData.Load(Path.Combine(Application.streamingAssetsPath, WEAPON_DATA_PATH));
        return _weaponData;
    } }

    public static EnemyData EnemyData { get {
        if (_enemyData == null) _enemyData = EnemyData.Load(Path.Combine(Application.streamingAssetsPath, ENEMY_DATA_PATH));
        return _enemyData;
    } }

    public static BossData BossData { get {
        if (_bossData == null) _bossData = BossData.Load(Path.Combine(Application.streamingAssetsPath, BOSS_DATA_PATH));
        return _bossData;
    } }

    public static void ResetData()
    {
        _weaponData = null;
        _enemyData = null;
    }

    /**
     * Private
     */
    private static WeaponData _weaponData;
    private static EnemyData _enemyData;
    private static BossData _bossData;
}
