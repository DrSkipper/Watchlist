using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class DiskDataHandler
{
    public static void Save(string path, object data)
    {
        string fullPath = Application.persistentDataPath + "/" + path;
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Exists(fullPath) ? File.Open(fullPath, FileMode.Open) : File.Create(fullPath);
        bf.Serialize(file, data);
        file.Close();
    }

    public static T Load<T>(string path)
    {
        string fullPath = Application.persistentDataPath + "/" + path;
        if (File.Exists(fullPath))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(fullPath, FileMode.Open);
            T data = (T)bf.Deserialize(file);
            file.Close();

            return data;
        }
        return default(T);
    }
}
