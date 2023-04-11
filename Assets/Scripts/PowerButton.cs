using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PowerButton : MonoBehaviour
{

    private void OnTriggerEnter(Collider other)
    {
            Debug.Log("Button Pressed");
            PowerButtonEvent.Publish("pressed");
    }
}
public static class PowerButtonEvent
{

    private static Dictionary<Type, List<Action<object>>> listeners = new Dictionary<Type, List<Action<object>>>();

    public static void AddListener<T>(Action<object> listener) where T : class
    {
        if (listeners.ContainsKey(typeof(T)) == false)
            listeners.Add(typeof(T), new List<Action<object>>());

        listeners[typeof(T)].Add(listener);
    }

    public static void RemoveListener<T>(Action<object> listener) where T : class
    {
        listeners[typeof(T)].Remove(listener);

        if (listeners[typeof(T)].Count == 0)
        {
            listeners.Remove(typeof(T));
        }

    }

    public static void Publish<T>(T publishedEvent) where T : class
    {

        if (listeners.ContainsKey(typeof(T)) == false)
        {
            return;
        }

        foreach (var action in listeners[typeof(T)])
        {
            action.Invoke(publishedEvent);
        }

    }

}