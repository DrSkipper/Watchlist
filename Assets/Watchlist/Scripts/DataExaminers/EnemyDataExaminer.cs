using UnityEngine;
using System.IO;

[ExecuteInEditMode]
public class EnemyDataExaminer : MonoBehaviour
{
    public EnemyType[] EnemyTypes;

    public void Load()
    {
        this.EnemyTypes = StaticData.EnemyData.EnemyTypeArray;
    }

    public void Save()
    {
        StaticData.EnemyData.EnemyTypeArray = this.EnemyTypes;
        StaticData.EnemyData.Save(Path.Combine(Application.streamingAssetsPath, StaticData.ENEMY_DATA_PATH));
        StaticData.ResetData();
    }
}
