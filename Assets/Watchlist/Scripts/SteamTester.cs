using UnityEngine;
using Steamworks;

public class SteamTester : MonoBehaviour
{
    void Start()
    {
#if !UNITY_EDITOR
        if (SteamAPI.RestartAppIfNecessary((AppId_t)SteamData.AppId))
            Application.Quit();
        else
#endif
        Debug.Log("Logged in steam user: " + SteamData.UserDisplayName);
    }
}
