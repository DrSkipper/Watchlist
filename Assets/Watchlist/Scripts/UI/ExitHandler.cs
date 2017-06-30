using UnityEngine;
using UnityEngine.SceneManagement;

public class ExitHandler : VoBehavior
{
    public string Destination = "";
    public ExitInput Input = ExitInput.Exit;
    public string EventToSend = "";

    [System.Serializable]
    public enum ExitInput
    {
        Exit,
        Pause,
        Cancel
    }

    void Update()
    {
        bool pressed = false;
        switch (this.Input)
        {
            default:
            case ExitInput.Exit:
                pressed = MenuInput.Exit();
                break;
            case ExitInput.Pause:
                pressed = MenuInput.Pause();
                break;
            case ExitInput.Cancel:
                pressed = MenuInput.Cancel();
                break;
        }
        if (pressed)
        {
            if (this.EventToSend != null && this.EventToSend != "")
            {
                LocalEventNotifier.Event e = new LocalEventNotifier.Event();
                e.Name = this.EventToSend;
                GlobalEvents.Notifier.SendEvent(e);
            }

            if (this.Destination != "")
            {
                ProgressData.LoadFromDisk(true);
                SceneManager.LoadScene(this.Destination);
                Destroy(this.gameObject);
            }
            else
            {
                Application.Quit();
            }
        }
    }
}
