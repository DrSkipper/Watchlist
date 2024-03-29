﻿using UnityEngine;
using System.IO;
using System.Xml;
using System.Text;
using System.Xml.Serialization;
using System.Collections.Generic;

[System.Serializable]
public class BossLock
{
    public int BossId;
    public bool Locked;
}

[XmlRoot("BossLockData")]
[System.Serializable]
public class BossLockData
{
    [XmlIgnoreAttribute]
    public Dictionary<int, bool> BossLocks;

    /**
     * XML
     */
    [XmlArray("BossLocks"), XmlArrayItem("BossLock")]
    public BossLock[] BossLockArray;

    public void Save(string path)
    {
        var serializer = new XmlSerializer(typeof(BossLockData));
        using (StreamWriter stream = new StreamWriter(path, false, Encoding.GetEncoding("UTF-8")))
        {
            serializer.Serialize(stream, this);
        }
    }

    public static BossLockData Load(TextAsset asset)
    {
        BossLockData lockData = null;
        var serializer = new XmlSerializer(typeof(BossLockData));

        using (var stream = new StringReader(asset.text))
        {
            lockData = serializer.Deserialize(stream) as BossLockData;
        }

        lockData.BossLocks = new Dictionary<int, bool>();
        foreach (BossLock bossLock in lockData.BossLockArray)
        {
            /*try
            {*/
            lockData.BossLocks.Add(bossLock.BossId, bossLock.Locked);
            /*}
            catch (ArgumentException e)
            {
                Debug.Log("ArgumentException: " + e.Message);
            }*/
        }

        return lockData;
    }
}
