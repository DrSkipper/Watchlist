using UnityEngine;
using System.IO;

[ExecuteInEditMode]
public class WeaponDataExaminer : MonoBehaviour
{
    public WeaponType[] WeaponTypes;

    public void Load()
    {
        this.WeaponTypes = StaticData.WeaponData.WeaponTypeArray;
    }
    
    public void Save()
    {
        StaticData.WeaponData.WeaponTypeArray = this.WeaponTypes;
        StaticData.WeaponData.Save(Path.Combine(Application.streamingAssetsPath, StaticData.WEAPON_DATA_PATH));
        StaticData.ResetData();
    }
}
