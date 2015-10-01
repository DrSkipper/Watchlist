using UnityEngine;
using System.Collections.Generic;

public class LocalEventNotifier : MonoBehaviour
{
    public delegate void EventCallback(Event localEvent);

    public class Event
    {
        public string Name;
    }

    public void Listen(string eventName, MonoBehaviour owner, EventCallback callback)
    {
        if (_listenersByEventName == null)
            _listenersByEventName = new Dictionary<string, List<Listener>>();

        List<Listener> listeners = _listenersByEventName[eventName];
        if (listeners == null)
        {
            listeners = new List<Listener>();
            _listenersByEventName[eventName] = listeners;
        }

        listeners.Add(new Listener(owner, callback));
    }

    public void RemoveAllListenersForOwner(MonoBehaviour owner)
    {
        if (_listenersByEventName == null)
            return;

        foreach (List<Listener> listeners in _listenersByEventName.Values)
        {
            listeners.RemoveAll(listener => listener.Owner == owner);
        }
    }

    public void SendEvent(Event localEvent)
    {
        if (_listenersByEventName == null)
            return;

        List<Listener> listeners = _listenersByEventName[localEvent.Name];
        if (listeners != null)
        {
            foreach (Listener listener in listeners)
            {
                listener.Callback(localEvent);
            }
        }
    }

    /**
     * Private
     */
    private Dictionary<string, List<Listener>> _listenersByEventName;

    private class Listener
    {
        public MonoBehaviour Owner;
        public EventCallback Callback;

        public Listener(MonoBehaviour owner, EventCallback callback)
        {
            this.Owner = owner;
            this.Callback = callback;
        }
    }
}
