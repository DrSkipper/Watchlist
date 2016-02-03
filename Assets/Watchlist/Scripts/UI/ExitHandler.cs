using UnityEngine;
using UnityEngine.SceneManagement;

public class ExitHandler : VoBehavior
{
    public string Destination = "";

    void Update()
    {
        if (MenuInput.Exit())
        {
            if (this.Destination != "")
                SceneManager.LoadScene(this.Destination);
            else
                Application.Quit();
        }
    }
}
