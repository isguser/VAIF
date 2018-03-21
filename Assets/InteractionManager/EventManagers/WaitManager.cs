using UnityEngine;
using System;

public class WaitManager : MonoBehaviour
{
    private Wait wait;

    void Start()
    {

    }

    public void Waiting(Wait w)
    {
        wait = w;
        wait.start();
        Invoke("StopWaiting", w.waitTime);
    }

    public void StopWaiting()
    {
        wait.finish();
    }
}
