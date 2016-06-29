using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class DiskDataHandler
{
    public static string LoadStreamedAsset(string filePath)
    {
        if (filePath.Contains("://")) {
            WWW www = new WWW(filePath);
            //yield return www;
            return www.text;
        } else
            return System.IO.File.ReadAllText(filePath);
    }
    

    public static void Save(string path, object data)
    {
        PlayerPrefs.SetString(path,JsonUtility.ToJson(data));
    }

    public static T Load<T>(string path)
    {
        string fullPath = Application.persistentDataPath + Path.DirectorySeparatorChar + path;
        if (PlayerPrefs.HasKey(path))
        {

            return JsonUtility.FromJson<T>(PlayerPrefs.GetString(path));
        }
        return default(T);
    }
}
