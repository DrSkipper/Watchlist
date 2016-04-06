using System.IO;
using System.Text;
using System.Xml.Serialization;
using System.Collections.Generic;

[System.Serializable]
public class EnemyType
{
    public string Name = "blank enemy";
    public int Id;
    public string PrefabName = "generic";
    public string SpriteName = "enemy_02";
    public int Difficulty = 0;
    public int ChoiceWeight = 10;
    public int ChoiceWeightIncrease = 5;
    public int Health = 5;
    public float Friction = 150.0f;
    public float MaxSpeed = 140.0f;
    public float Acceleration = 280.0f;
    public float SpinRange = 40.0f;
    public float LookAtRange = 30.0f;
    public float ShootRange = 20.0f;
    public float ShotStartDistance = 0.0f;
    public int MovementType = 0;
    public float TargetDistance = 0.0f;
    public float RotationSpeed = 180.0f;
    public float RotationOffset = 45.0f;
    public bool OnlyShootOnPause = false;
    public float PauseAngle = 45.0f;
    public float PauseDuration = 0.0f;
    public int WeaponTypeId = 0;
    public int CollisionWeaponTypeId = 0;
}

[XmlRoot("EnemyData")]
[System.Serializable]
public class EnemyData
{
    [XmlIgnoreAttribute]
    public Dictionary<int, EnemyType> EnemyTypes;

    /**
     * XML
     */
    [XmlArray("EnemyTypes"), XmlArrayItem("EnemyType")]
    public EnemyType[] EnemyTypeArray;

    public void Save(string path)
    {
        var serializer = new XmlSerializer(typeof(EnemyData));
        using (StreamWriter stream = new StreamWriter(path, false, Encoding.GetEncoding("UTF-8")))
        {
            serializer.Serialize(stream, this);
        }
    }

    public static EnemyData Load(string path)
    {
        EnemyData enemyData = null;
        var serializer = new XmlSerializer(typeof(EnemyData));

        using (var stream = new FileStream(path, FileMode.Open))
        {
            enemyData = serializer.Deserialize(stream) as EnemyData;
        }

        enemyData.EnemyTypes = new Dictionary<int, EnemyType>();
        foreach (EnemyType type in enemyData.EnemyTypeArray)
        {
            /*try
            {*/
            enemyData.EnemyTypes.Add(type.Id, type);
            /*}
            catch (ArgumentException e)
            {
                Debug.Log("ArgumentException: " + e.Message);
            }*/
        }

        return enemyData;
    }
}
