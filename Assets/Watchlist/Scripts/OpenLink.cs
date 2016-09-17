using UnityEngine;

public class OpenLink : MonoBehaviour
{
    public string URL = "";

    public void Open()
    {
        Application.OpenURL(this.URL);
    }
}
