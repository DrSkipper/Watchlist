using UnityEngine;
using System;
using System.IO;
using System.Xml;
using System.Text;
using System.Xml.Serialization;
using System.Collections.Generic;

[System.Serializable]
public class WeaponType
{
    public const string TRAVEL_TYPE_NORMAL = "normal";
    public const string TRAVEL_TYPE_LASER = "laser";

    public const string SPECIAL_NONE = "none";
    public const string SPECIAL_EXPLOSION = "explosion";
    
    public string Name = "blank";
    public int Id = 1;
    public int Damage = 1;
    public int Knockback = 1;
    public float HitInvincibilityDuration = 1.0f;
    public float ShotSpeed = 1.0f;
    public float ShotCooldown = 1.0f;
    public int ShotCount = 1;
    public float AngleBetweenShots = 10.0f;
    public int MaximumBounces = 0;
    public float MinimumBounceAngle = 0.0f;
    public float VelocityFallOff = 0.0f;
    public float DurationTime = 5.0f;
    public float DurationDistance = 500.0f;
    public string TravelType = TRAVEL_TYPE_NORMAL;
    public int TravelTypeParameter = 0;
    public string SpecialEffect = SPECIAL_NONE;
    public float SpecialEffectParameter1 = 0.0f;
    public float SpecialEffectParameter2 = 0.0f;
    public int AudioIndex = 0;
    public float AudioCooldown = 0.0f;
}

[XmlRoot("WeaponData")]
[System.Serializable]
public class WeaponData
{
    public enum Slot
    {
        Empty = 0,
        Bounce = 1,
        Spreadshot = 2,
        Laser = 3,
        Bomb = 4
    };

    [XmlIgnoreAttribute]
    public Dictionary<int, WeaponType> WeaponTypes;

    public static int WeaponTypeIdFromSlots(Slot[] slots)
    {
        List<Slot> slotsList = new List<Slot>(slots);
        slotsList.RemoveAll(slot => slot == Slot.Empty);
        slotsList.Sort();

        int id = 1000000;
        int multiplier = 1;
        foreach (Slot slot in slotsList)
        {
            id += ((int)slot) * multiplier;
            multiplier *= 10;
        }
        return id;
    }

    /**
     * XML
     */
    [XmlArray("WeaponTypes"), XmlArrayItem("WeaponType")]
    public WeaponType[] WeaponTypeArray;

    public void Save(string path)
    {
        var serializer = new XmlSerializer(typeof(WeaponData));
        using (StreamWriter stream = new StreamWriter(path, false, Encoding.GetEncoding("UTF-8")))
        {
            serializer.Serialize(stream, this);
        }
    }

    public static WeaponData Load(string path)
    {
        WeaponData weaponData = null;
        var serializer = new XmlSerializer(typeof(WeaponData));

        using (var stream = new FileStream(path, FileMode.Open))
        {
            weaponData = serializer.Deserialize(stream) as WeaponData;
        }

        weaponData.WeaponTypes = new Dictionary<int, WeaponType>();
        foreach (WeaponType type in weaponData.WeaponTypeArray)
        {
            /*try
            {*/
                weaponData.WeaponTypes.Add(type.Id, type);
            /*}
            catch (ArgumentException e)
            {
                Debug.Log("ArgumentException: " + e.Message);
            }*/
        }

        return weaponData;
    }
}
