using UnityEngine;

public class MoveToFrontOnEvent : MonoBehaviour
{
    public Transform Target;
    public string EventName;

    void Start()
    {
        GlobalEvents.Notifier.Listen(this.EventName, this, eventFired);
    }

    private void eventFired(LocalEventNotifier.Event e)
    {
        this.Target.SetAsLastSibling();
    }
}
