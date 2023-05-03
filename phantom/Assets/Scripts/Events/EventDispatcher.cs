using System;
using System.Collections.Generic;

public class GameEvent
{
    
}

public class EventDispatcher
{
    private EventDispatcher()
    {
    }

    private static readonly Dictionary<Type, int> Listeners = new Dictionary<Type, int>();
    
    public static EventDispatcher Instance { get; } = new EventDispatcher();

    public delegate void EventDelegate<T>(T e) where T : GameEvent;

    private readonly Dictionary<Type, Delegate> _delegates = new Dictionary<Type, Delegate>();

    public void AddListener<T>(EventDelegate<T> listener) where T : GameEvent
    {
        if (_delegates.TryGetValue(typeof (T), out var d))
        {
            d = Delegate.Remove(d, listener);
            
            _delegates[typeof (T)] = Delegate.Combine(d, listener);
        }
        else
        {
            _delegates[typeof (T)] = listener;
        }

        if (Listeners.ContainsKey (typeof(T)))
        {
            Listeners [typeof(T)] += 1;
        } 
        else
        {
            Listeners.Add (typeof(T), 1);
        }
    }

    public void RemoveListener<T>(EventDelegate<T> listener) where T : GameEvent
    {
        if (_delegates.TryGetValue(typeof (T), out var d))
        {
            Delegate currentDel = Delegate.Remove(d, listener);
            Listeners [typeof(T)] -= 1;

            if (currentDel == null)
            {
                _delegates.Remove(typeof (T));
            }
            else
            {
                _delegates[typeof (T)] = currentDel;
            }
        }

    }

    public void Raise<T>(T ev) where T : GameEvent
    {
        if (ev == null)
        {
            throw new ArgumentNullException(nameof(ev));
        }

        if (!_delegates.TryGetValue(typeof(T), out var d))
        {
            return;
        }

        EventDelegate<T> callback = d as EventDelegate<T>;

        if (callback == null)
        {
            return;
        }

        foreach (EventDelegate<T> singleCallback in callback.GetInvocationList())
        {
            singleCallback?.Invoke(ev);
        }
    }
}
