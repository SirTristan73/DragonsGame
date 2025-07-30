using System;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : PersistentSingleton<TimeManager>
{
    private readonly List<Action<float>> _fixedUpdateSubscribers = new(128);
    private readonly List<Action<float>> _updateSubscribers = new(128);


    public void RegisterUpdateListener(Action<float> listenerFunc)
    {
        _updateSubscribers.Add(listenerFunc);
    }

    public void RegisterFixedUpdateListener(Action<float> listenerFunc)
    {
        _fixedUpdateSubscribers.Add(listenerFunc);
    }

    public void UnregisterUpdateListener(Action<float> listenerFunc)
    {
        _updateSubscribers.Remove(listenerFunc);
    }

    public void UnregisterFixedUpdateListener(Action<float> listenerFunc)
    {
        _fixedUpdateSubscribers.Remove(listenerFunc);
    }

    private void FixedUpdate()
    {
        var deltaTime = Time.fixedDeltaTime;

        foreach (var listener in _fixedUpdateSubscribers)
        {
            listener.Invoke(deltaTime);
        }
    }

    private void Update()
    {
        var deltaTime = Time.deltaTime;

        foreach (var listener in _updateSubscribers)
        {
            listener.Invoke(deltaTime);
        }
    }

    public void UnregisterAllListeners()
    {
        _updateSubscribers.Clear();
        _fixedUpdateSubscribers.Clear();
    }
}
