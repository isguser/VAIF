using System.Collections.Generic;
using UnityEngine;

public class Conversation : EventIM
{
    /**
     * Future implementation: Track if the user left mid-conversation and where. 
     * protected bool startedButNotFinished;
     * protected GameObject whereWeLeftOff; //This cannot be a response/wildcard event
     **/
    private JumpManager jm;
    public List<EventIM> events;

    private void Start()
    {
        //load the events
        foreach (Transform child in transform)
            events.Add(child.GetComponentInChildren<EventIM>());
        jm = gameObject.GetComponent<JumpManager>();
        jm.load(events);
    }

    public bool isOver()
    {
        /* Verify if all events in this Conversation are started and done */
        foreach ( EventIM e in events )
            if ( !e.isDone() )
                return false;
        return true;
    }

    public bool isTheLastEvent(EventIM e) {
        return ( e==events[events.Count-1] ); //todo -- it can have a jump?
    }

    public List<EventIM> getEvents()
    {
        return events;
    }

    public EventIM getFirstUnfinishedEvent()
    {
        /* Returns the first event in this conversation that has not been finished */
        foreach (EventIM e in events)
            if (!e.isDone())
                return e;
        return events[events.Count - 1];
    }

    public EventIM findNextEvent(EventIM e)
    {
        //start if we have not played any yet
        if ( e==null )
            return getFirstUnfinishedEvent();
        //is there a nextEvent to jump to?
        EventIM tmp = jm.getNextEvent(e);
        if ( tmp==null )
            return getFirstUnfinishedEvent();
        return tmp;
    }
}