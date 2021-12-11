using System;
using System.Collections.Generic;
using UnityEngine;

internal class UnityMainThread : MonoBehaviour
{
    internal static UnityMainThread wkr;
    Queue<Action> jobs = new Queue<Action>();

    void Awake()
    {
        Application.runInBackground = true;
        DontDestroyOnLoad(this);
        wkr = this;
    }

    void Update()
    {
        while (jobs.Count > 0)
            try
            {
                jobs.Dequeue().Invoke();
            }
            catch (Exception ex)
            {
                //Debug.Log("Thread helper shit itself again");
                Debug.Log(ex);
            }
    }

    internal void AddJob(Action newJob)
    {
        jobs.Enqueue(newJob);
    }
}
