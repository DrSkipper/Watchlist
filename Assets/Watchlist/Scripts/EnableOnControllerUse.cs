using UnityEngine;

public class EnableOnControllerUse : MonoBehaviour
{
    public int PlayerIndex = 0;
    public bool Disable = false;
    public GameObject[] ObjectsToEnable;

    void Start()
    {
        if (DynamicData.GetSessionPlayer(this.PlayerIndex).UsingJoystick)
        {
            for (int i = 0; i < this.ObjectsToEnable.Length; ++i)
            {
                if (this.ObjectsToEnable[i] != null)
                    this.ObjectsToEnable[i].SetActive(!this.Disable);
            }
        }
    }
}
