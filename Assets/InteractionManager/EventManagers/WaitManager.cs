using UnityEngine;
using System;

public class WaitManager : MonoBehaviour {

    public InteractionManager interactionManager;
    private Wait wait;

    void Start()
    {
        interactionManager = GameObject.Find("Timeline").GetComponent<InteractionManager>();
    }
    public void Waiting(Wait w)
    {
        wait = w;
        wait.started = true;
        interactionManager.startWaiting();
        Invoke("StopWaiting", w.waitTime);
    }

    public void StopWaiting() {
        interactionManager.stopWaiting();
        wait.isDone = true;
    }
}
