using System.Collections.Generic;
using UnityEngine;

public class Conversation : EventIM
{
    /**
     * Future implementation: Track if the user left mid-conversation and where. 
     * protected bool startedButNotFinished;
     * protected GameObject whereWeLeftOff; //This cannot be a response/wildcard event
     **/
    public List<EventIM> events;

    private void Start()
    {
        foreach (Transform child in transform)
        {
            //Debug.Log("Adding : " + child.name);
            events.Add(child.GetComponentInChildren<EventIM>());
        }
    }
}