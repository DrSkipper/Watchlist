using UnityEngine;

public class BossMasterChoiceRecorder : MonoBehaviour
{
    void Start()
    {
        GlobalEvents.Notifier.Listen(MenuElementSelectedEvent.NAME, this, choiceMade);
    }

    void choiceMade(LocalEventNotifier.Event e)
    {
        MenuElementSelectedEvent selectionEvent = e as MenuElementSelectedEvent;
        if (selectionEvent.Action.ToLower() == "choice_a")
        {
            PersistentData.RegisterAcceptedMaster();
<<<<<<< Updated upstream
=======
            PersistentData.RecordHighScore();
            PersistentData.SaveToDisk();
>>>>>>> Stashed changes
        }
        else if (selectionEvent.Action.ToLower() == "choice_b")
        {
            GlobalEvents.Notifier.SendEvent(new PlayMusicEvent());
            PersistentData.RegisterRefusedMaster();
        }
    }
}
