using UnityEngine;
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

    public int[] GenerateBossesForPlaythrough()
    {
        List<int> possibilities = new List<int>();

        foreach (int bossId in this.BossLocks.Keys)
        {
            if (!this.BossLocks[bossId])
                possibilities.Add(bossId);
        }

        int[] chosen = new int[4];
        for (int i = 0; i < chosen.Length; ++i)
        {
            chosen[i] = possibilities[Random.Range(0, possibilities.Count)];

            if (possibilities.Count > chosen.Length)
                possibilities.Remove(chosen[i]);
        }
        return chosen;
    }

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

    public static BossLockData Load(string path)
    {
        BossLockData lockData = null;
        var serializer = new XmlSerializer(typeof(BossLockData));

        using (var stream = new FileStream(path, FileMode.Open))
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
