using UnityEngine;

public class EnableOnEvent : MonoBehaviour
{
    public LocalEventNotifier Notifier;
    public string EventName;
    public bool Disable = false;
    public GameObject[] ObjectsToEnable;
    public bool RemoveOnceDone = true;

    void Awake()
    {
        this.Notifier.Listen(this.EventName, this, callback);
    }

    void Update()
    {
        if (_done && this.RemoveOnceDone)
        {
            this.Notifier.RemoveAllListenersForOwner(this);
            this.Notifier = null;
            _done = false;
            this.enabled = false;
        }
    }

    private void callback(LocalEventNotifier.Event e)
    {
        for (int i = 0; i < this.ObjectsToEnable.Length; ++i)
        {
            if (this.ObjectsToEnable[i] != null)
                this.ObjectsToEnable[i].SetActive(!this.Disable);
        }
        _done = true;
    }

    private bool _done;
}
