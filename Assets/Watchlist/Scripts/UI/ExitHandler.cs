using UnityEngine;

public class ExitHandler : VoBehavior
{
    public string Destination = "";

    void Update()
    {
        if (MenuInput.Exit())
        {
            if (this.Destination != "")
                Application.LoadLevel(this.Destination);
            else
                Application.Quit();
        }
    }
}
