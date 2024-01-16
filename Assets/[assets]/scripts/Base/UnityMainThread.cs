// from https://stackoverflow.com/questions/53916533/setactive-can-only-be-called-from-the-main-thread

using System;
using System.Collections.Generic;
using UnityEngine;

internal class UnityMainThread : MonoBehaviour
{
    internal static UnityMainThread wkr;
    Queue<Action> jobs = new Queue<Action>();

    void Awake()
    {
        if (!wkr)
            wkr = this;
    }

    void Update()
    {
        while (jobs.Count > 0)
            jobs.Dequeue().Invoke();
    }

    static internal void AddJob(Action newJob)
    {
        if (!wkr)
        {
            var go = new GameObject("MainThreadWorker");
            wkr = go.AddComponent<UnityMainThread>();
        }
        wkr.jobs.Enqueue(newJob);
    }
}