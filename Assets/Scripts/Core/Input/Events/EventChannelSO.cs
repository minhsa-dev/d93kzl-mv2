using System;
using UnityEngine;

public abstract class EventChannelSO<T> : ScriptableObject
{
    public event Action<T> OnEventRaised;
    public void RaiseEvent(T value) => OnEventRaised?.Invoke(value);
}
