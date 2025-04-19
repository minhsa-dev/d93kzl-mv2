using System;
using UnityEngine;

// abstract class to allow for different types of events, scriptable objects for modularity, generic event channel value
public abstract class EventChannelSO<T> : ScriptableObject
{
    public event Action<T> OnEventRaised;
    public void RaiseEvent(T value) => OnEventRaised?.Invoke(value);
}
