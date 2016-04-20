using UnityEngine;
using System;
using System.IO;
using System.Xml;
using System.Text;
using System.Xml.Serialization;
using System.Collections.Generic;

[System.Serializable]
public class WeaponType : ICloneable
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

    public object Clone()
    {
        return this.MemberwiseClone();
    }
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

    private const int NORMAL_ID = 1000000;

    public static WeaponType NewWeaponTypeFromSlots(Slot[] slots)
    {
        List<Slot> slotsList = new List<Slot>(slots);
        slotsList.RemoveAll(slot => slot == Slot.Empty);
        slotsList.Sort();

        WeaponType normalType = StaticData.WeaponData.WeaponTypes[NORMAL_ID];
        int spreadId = NORMAL_ID;
        int spreadMultiplier = 1;
        int bombId = NORMAL_ID;
        int bombMultiplier = 1;
        int bounceId = NORMAL_ID;
        int bounceMultiplier = 1;
        int laserId = NORMAL_ID;
        int laserMultiplier = 1;

        foreach (Slot slot in slotsList)
        {
            switch (slot)
            {
                default:
                    break;
                case Slot.Spreadshot:
                    spreadId += spreadMultiplier * (int)slot;
                    spreadMultiplier *= 10;
                    break;
                case Slot.Bomb:
                    bombId += bombMultiplier * (int)slot;
                    bombMultiplier *= 10;
                    break;
                case Slot.Bounce:
                    bounceId += bounceMultiplier * (int)slot;
                    bounceMultiplier *= 10;
                    break;
                case Slot.Laser:
                    laserId += laserMultiplier * (int)slot;
                    laserMultiplier *= 10;
                    break;
            }
        }

        WeaponType newType = (WeaponType)normalType.Clone();
        WeaponType spreadType = StaticData.WeaponData.WeaponTypes[spreadId];
        WeaponType bombType = StaticData.WeaponData.WeaponTypes[bombId];
        WeaponType bounceType = StaticData.WeaponData.WeaponTypes[bounceId];
        WeaponType laserType = StaticData.WeaponData.WeaponTypes[laserId];

        newType.ShotCount = spreadType.ShotCount;
        newType.SpecialEffect = bombType.SpecialEffect;
        newType.SpecialEffectParameter1 = bombType.SpecialEffectParameter1;
        newType.SpecialEffectParameter2 = bombType.SpecialEffectParameter2;
        newType.ShotSpeed = bombType.ShotSpeed;
        newType.VelocityFallOff = bombType.VelocityFallOff;
        newType.ShotCooldown = bombType.ShotCooldown;
        newType.DurationTime = bombType.DurationTime;
        newType.Knockback = bombType.Knockback;
        newType.MaximumBounces = bounceType.MaximumBounces;
        newType.MinimumBounceAngle = bounceType.MinimumBounceAngle;
        newType.TravelType = laserType.TravelType;
        newType.TravelTypeParameter = laserType.TravelTypeParameter;

        if (newType.TravelType == WeaponType.TRAVEL_TYPE_LASER)
        {
            newType.ShotSpeed = laserType.ShotSpeed;
            newType.VelocityFallOff = laserType.VelocityFallOff;
            newType.ShotCooldown = laserType.ShotCooldown;
            newType.DurationTime = laserType.DurationTime;
            newType.HitInvincibilityDuration = laserType.HitInvincibilityDuration;
            newType.Knockback = laserType.Knockback;
            newType.AudioIndex = laserType.AudioIndex;
            newType.AudioCooldown = laserType.AudioCooldown;
        }

        return newType;
    }

    private static Dictionary<Slot, int> _maxSlotsByType;
    
    public static Dictionary<Slot, int> GetMaxSlotsByType()
    {
        if (_maxSlotsByType == null)
        {
            _maxSlotsByType = new Dictionary<Slot, int>();
            _maxSlotsByType[Slot.Spreadshot] = 3;
            _maxSlotsByType[Slot.Bomb] = 3;
            _maxSlotsByType[Slot.Bounce] = 2;
            _maxSlotsByType[Slot.Laser] = 1;
        }
        return _maxSlotsByType;
    }

    private static Dictionary<Slot, int> _slotDurationsByType;

    public static Dictionary<Slot, int> GetSlotDurationsByType()
    {
        if (_slotDurationsByType == null)
        {
            _slotDurationsByType = new Dictionary<Slot, int>();
            _slotDurationsByType[Slot.Bomb] = 25;
            _slotDurationsByType[Slot.Spreadshot] = 30;
            _slotDurationsByType[Slot.Bounce] = 35;
            _slotDurationsByType[Slot.Laser] = 100;
        }
        return _slotDurationsByType;
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
