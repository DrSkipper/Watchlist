using UnityEngine;
using System.IO;

public static class StaticData
{
    public const string DATA_ROOT = "Data/";
    public const string WEAPON_DATA_PATH = DATA_ROOT + "weapons";
    public const string ENEMY_DATA_PATH = DATA_ROOT + "enemies";
    public const string BOSS_DATA_PATH = DATA_ROOT + "bosses";
    public const string BOSSLOCKS_DATA_PATH = DATA_ROOT + "bosslocks";

    public static WeaponData WeaponData { get {
        if (_weaponData == null) _weaponData = WeaponData.Load(Resources.Load<TextAsset>(WEAPON_DATA_PATH));
        return _weaponData;
    } }

    public static EnemyData EnemyData { get {
        if (_enemyData == null) _enemyData = EnemyData.Load(Resources.Load<TextAsset>(ENEMY_DATA_PATH));
        return _enemyData;
    } }

    public static BossData BossData { get {
        if (_bossData == null) _bossData = BossData.Load(Resources.Load<TextAsset>(BOSS_DATA_PATH));
        return _bossData;
    } }
    
    public static BossLockData BossLockData { get {
        if (_bosslockData == null) _bosslockData = BossLockData.Load(Resources.Load<TextAsset>(BOSSLOCKS_DATA_PATH));
        return _bosslockData;
    } }

    public static void ResetData()
    {
        _weaponData = null;
        _enemyData = null;
        _bossData = null;
        _bosslockData = null;
    }

    /**
     * Private
     */
    private static WeaponData _weaponData;
    private static EnemyData _enemyData;
    private static BossData _bossData;
    private static BossLockData _bosslockData;
}
