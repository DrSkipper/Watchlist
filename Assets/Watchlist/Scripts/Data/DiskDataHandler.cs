using UnityEngine;
using Newtonsoft.Json;

public static class DiskDataHandler
{
    public static void Save(string path, object data)
    {
        PlayerPrefs.SetString(path, JsonConvert.SerializeObject(data));
    }

    public static T Load<T>(string path)
    {
        if (PlayerPrefs.HasKey(path))
        {
            return JsonConvert.DeserializeObject<T>(PlayerPrefs.GetString(path));
        }
        return default(T);
    }
}
