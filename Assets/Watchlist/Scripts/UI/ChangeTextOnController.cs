using UnityEngine;
using UnityEngine.UI;

public class ChangeTextOnController : MonoBehaviour
{
    public Text Text;
    public string OnControllerText;

    public void Update()
    {
        if (!onController && MenuInput.ControllerUsed())
        {
            onController = true;
            this.Text.text = this.OnControllerText;
        }
    }

    private bool onController = false;
}
