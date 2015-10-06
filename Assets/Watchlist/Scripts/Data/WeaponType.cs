using System.IO;
using System.Xml;
using System.Xml.Serialization;

[System.Serializable]
public class WeaponType
{
    public const string TRAVEL_TYPE_NORMAL = "normal";
    public const string TRAVEL_TYPE_LASER = "laser";

    public const string SPECIAL_NONE = "none";
    public const string SPECIAL_EXPLOSION = "explosion";

    [XmlAttribute("name")]
    public string Name = "blank";
    public float ShotSpeed = 1.0f;
    public float ShootCooldown = 1.0f;
    public int ShotCount = 1;
    public float AngleBetweenShots = 10.0f;
    public int MaximumBounces = 0;
    public float VelocityFallOff = 0.0f;
    public float DurationTime = 5.0f;
    public float DurationDistance = 100.0f;
    public string TravelType = TRAVEL_TYPE_NORMAL;
    public string SpecialEffect = SPECIAL_NONE;
    public float SpecialEffectParameter = 0.0f;
}

[XmlRoot("WeaponData")]
[System.Serializable]
public class WeaponData
{
    [XmlArray("WeaponTypes"), XmlArrayItem("WeaponType")]
    public WeaponType[] WeaponTypes;

    /*public void Save(string path)
    {
        var serializer = new XmlSerializer(typeof(WeaponData));
        using (var stream = new FileStream(path, FileMode.Create))
        {
            serializer.Serialize(stream, this);
        }
    }*/

    public static WeaponData Load(string path)
    {
        var serializer = new XmlSerializer(typeof(WeaponData));
        using (var stream = new FileStream(path, FileMode.Open))
        {
            return serializer.Deserialize(stream) as WeaponData;
        }
    }
}
