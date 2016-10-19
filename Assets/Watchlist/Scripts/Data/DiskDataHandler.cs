using UnityEngine;
using System.IO;
using System.Text;
using Newtonsoft.Json;
using System.Xml;
using System.Xml.Serialization;

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
        /*var serializer = new XmlSerializer(data.GetType());
        using (StringWriter sw = new StringWriter())
        {
            serializer.Serialize(sw, data);
        }*/

        PlayerPrefs.SetString(path, JsonConvert.SerializeObject(data));
    }

    public static T Load<T>(string path)
    {
        //string fullPath = Application.persistentDataPath + Path.DirectorySeparatorChar + path;
        if (PlayerPrefs.HasKey(path))
        {
            /*var serializer = new XmlSerializer(typeof(T));
            return (T)serializer.Deserialize(new StringReader(PlayerPrefs.GetString(path)));*/
            return JsonConvert.DeserializeObject<T>(PlayerPrefs.GetString(path));
        }
        return default(T);
    }
}
