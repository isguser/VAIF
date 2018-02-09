using System.Collections.Generic;
using UnityEngine;

public class Conversation : EventIM
{
    public bool conversationStart;
    public bool conversationFinish;
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
            events.Add(child.GetComponentInChildren<EventIM>());
        }
    }
}