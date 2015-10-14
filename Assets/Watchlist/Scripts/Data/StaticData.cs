using UnityEngine;
using System.IO;
using System.Collections.Generic;

public static class StaticData
{
    public const string DATA_ROOT = "Data/";
    public const string WEAPON_DATA_PATH = DATA_ROOT + "weapons.xml";

    public static WeaponData WeaponData { get {
        if (_weaponData == null) _weaponData = WeaponData.Load(Path.Combine(Application.streamingAssetsPath, WEAPON_DATA_PATH));
        return _weaponData;
    } }

    public static void ResetData()
    {
        _weaponData = null;
    }

    /**
     * Private
     */
    private static WeaponData _weaponData;
}
