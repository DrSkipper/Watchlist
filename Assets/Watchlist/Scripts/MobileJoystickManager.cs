using UnityEngine;

public class MobileJoystickManager : MonoBehaviour
{
    void Update()
    {
        if (!usingController && MenuInput.ControllerUsed())
        {
            usingController = true;
            gameObject.SetActive(false);
        }
    }

    private bool usingController = false;
}
