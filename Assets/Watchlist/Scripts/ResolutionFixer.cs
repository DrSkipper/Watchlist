using UnityEngine;

public class ResolutionFixer : MonoBehaviour
{
    public const string RES_KEY = "cached_res";

#if !UNITY_EDITOR
    void Awake()
    {
        if (!PlayerPrefs.HasKey(RES_KEY) || PlayerPrefs.GetInt(RES_KEY) != 1)
        {
            Resolution[] resolutions = Screen.resolutions;
            Resolution res = resolutions[resolutions.Length - 1];
            Screen.SetResolution(res.width, res.height, true);
            PlayerPrefs.SetInt(RES_KEY, 1);
        }
    }
#endif
}
