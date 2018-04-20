using UnityEngine;
using System;

public class WaitManager : MonoBehaviour
{
    private Wait wait;

    void Start()
    {

    }

    /* Start a wait event for a defined time from Unity */
    public void Waiting(Wait w)
    {
        wait = w;
        wait.start();
        Invoke("StopWaiting", w.waitTime);
    }

    /* Stop a wait event after the defined time from Unity */
    public void StopWaiting()
    {
        wait.finish();
    }
}
