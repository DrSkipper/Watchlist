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
        }
        else if (selectionEvent.Action.ToLower() == "choice_b")
        {
            PersistentData.RegisterRefusedMaster();
        }
    }
}
