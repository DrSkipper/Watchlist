using UnityEngine;
using System.IO;

[ExecuteInEditMode]
public class BossDataExaminer : MonoBehaviour
{
    public BossType[] BossTypes;

    public void Load()
    {
        this.BossTypes = StaticData.BossData.BossTypeArray;
    }

    public void Save()
    {
        StaticData.BossData.BossTypeArray = this.BossTypes;
        StaticData.BossData.Save(Path.Combine(Application.streamingAssetsPath, StaticData.BOSS_DATA_PATH));
        StaticData.ResetData();
    }
}
