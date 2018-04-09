using System.Collections.Generic;
using UnityEngine;

public class Conversation : MonoBehaviour// : EventIM
{
    [Tooltip("The nextConversation in the Timeline to be played after this Conversation.")]
    public GameObject nextConversation;
    [Tooltip("Optional. The IDescription is how you can describe your Conversation with a simple description ID. EX: Conversation1")]
    public string IDescription;
    [Tooltip("Required. Which Agent(s) are involved in this interaction?")]
    public List<AgentStatusManager> agents = new List<AgentStatusManager>();
    [Tooltip("Required (SAME SIZE OF AGENTS). Do you want the user to be inRange of ANY Agent to activate this Conversation? DONTCARE allows it to begin without necessity.")]
    public List<ConversationSetting> wantInRange = new List<ConversationSetting>();
    [Tooltip("Required (SAME SIZE OF AGENTS). Do you want the user to lookAt ANY Agent to activate this Conversation? DONTCARE allows it to begin without necessity.")]
    public List<ConversationSetting> wantLookedAt = new List<ConversationSetting>();
    [Tooltip("This will be filled at runtime based on the EventIM objects under this level of the heirarchy.")]
    public List<EventIM> events;
    public bool started = false;
    public bool finished = false;

    public enum ConversationSetting
    {
        TRUE,
        DONTCARE, //can be T/F
        FALSE
    }
    /**
     * Future implementation: Track if the user left mid-conversation and where. 
     * protected bool startedButNotFinished;
     * protected GameObject whereWeLeftOff; //This cannot be a response/wildcard event
     **/
    private JumpManager jm;

    private void Start()
    {
        //load the events
        foreach (Transform child in transform)
            events.Add(child.GetComponentInChildren<EventIM>());
        jm = gameObject.GetComponent<JumpManager>();
        jm.load(events);

        if (agents.Count != wantInRange.Count && agents.Count != wantLookedAt.Count)
            Debug.Log("List of agents must match lists of: wantInRange and wantLookedAt");
    }

    public bool isOver()
    {
        /* Verify if all events in this Conversation are started and done */
        foreach ( EventIM e in events )
            if ( e.isLastEvent && e.isDone() )
                return true;
        return false;
    }

    public bool isTheLastEvent(EventIM e) {
        //return ( e==events[events.Count-1] ); //todo -- it can have a jump?
        return e.isLastEvent;
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

    public void eventIsDone(EventIM e) {
        //mark the previous event for this Response as done, now
        for ( int i=0; i<events.Count; i++ )
            if ( events[i]==e && i>0 ) {
                events[i-1].finish();
                return;
            }
    }

    /* ********** Accessors: State Changes ********** */
    public void start()
    {
        started = true;
    }
    public bool hasStarted()
    {
        return started;
    }
    public void finish()
    {
        finished = true;
    }
    public bool isDone()
    {
        return finished;
    }
}