using UnityEngine;
using UnityEngine.UI;

public class VersionNumber : MonoBehaviour
{
    public const string PREFIX = "v";
    public const int MAJOR = 1;
    public const int MINOR = 3;
    public const int PATCH = 0;
    public const string DOT = ".";

    public Text VersionText; 

    public static string FullVersionString
    {
        get
        {
            return PREFIX + MAJOR + DOT + MINOR + DOT + PATCH;
        }
    }

    private void Start()
    {
        this.VersionText.text = FullVersionString;
    }
}
