using UnityEngine;
using System;
using System.IO;
using System.Xml;
using System.Text;
using System.Xml.Serialization;
using System.Collections.Generic;

[System.Serializable]
public class BossType
{
    public int Id;
    public string Name;
    public string SceneKey;
    public string PageText1;
    public string PageText2;
}

[XmlRoot("BossData")]
[System.Serializable]
public class BossData
{
    [XmlIgnoreAttribute]
    public Dictionary<int, BossType> BossTypes;

    /**
     * XML
     */
    [XmlArray("BossTypes"), XmlArrayItem("BossType")]
    public BossType[] BossTypeArray;

    public void Save(string path)
    {
        var serializer = new XmlSerializer(typeof(BossData));
        using (StreamWriter stream = new StreamWriter(path, false, Encoding.GetEncoding("UTF-8")))
        {
            serializer.Serialize(stream, this);
        }
    }

    public static BossData Load(TextAsset asset)
    {
        BossData bossData = null;
        var serializer = new XmlSerializer(typeof(BossData));

        using (var stream = new StringReader(asset.text))
        {
            bossData = serializer.Deserialize(stream) as BossData;
        }

        bossData.BossTypes = new Dictionary<int, BossType>();
        foreach (BossType type in bossData.BossTypeArray)
        {
            /*try
            {*/
            bossData.BossTypes.Add(type.Id, type);
            /*}
            catch (ArgumentException e)
            {
                Debug.Log("ArgumentException: " + e.Message);
            }*/
        }

        return bossData;
    }
}
