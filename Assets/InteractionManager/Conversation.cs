using System.Collections.Generic;
using UnityEngine;

public class Conversation : MonoBehaviour
{
    //TODO
    [Tooltip("Optional. The nextConversation of the Timeline's list of conversations to be played after this Conversation completes. WORK IN PROGRESS.")]
    public GameObject nextConversation;
    [Tooltip("Optional. The IDescription is how you can describe each unique Conversation with a simple description ID. EX: Conversation1")]
    public string IDescription;
    [Tooltip("No action needed. This is used as a condition for this conversation to be played. For example, proximity to agent.")]
    public EventIM.EventSetting wantInRange;
    [Tooltip("No action needed. This is used as a condition for this conversation to be played. For example, gazing at agent.")]
    public EventIM.EventSetting wantLookedAt;
    [Tooltip("No action needed. This is a list of the events that will be loaded at runtime.")]
    public List<EventIM> events;
    public bool started = false;
    public bool finished = false;
    private bool leftConv = false;
    private EventIM currEvent;
    public EventIM prevEvent;
    private EventIM nextEvent;
    private AgentStatusManager agent; //agent for the wantInRange/wantLookedAt

    /**
     * Future implementation: Track if the user left mid-conversation and where. 
     * protected bool startedButNotFinished;
     * protected GameObject whereWeLeftOff; //This cannot be a response/wildcard event
     **/
    private JumpManager jm;
    private string TAG = "C ";

    /* Called on build */
    private void Start() {
        TAG += name + " ";

        //load the events
        foreach (Transform child in transform)
            events.Add(child.GetComponentInChildren<EventIM>());
        jm = gameObject.GetComponent<JumpManager>();
        jm.load(events);

        currEvent = jm.getFirstEvent();
        nextEvent = jm.getNextEvent(currEvent);
        setWants();
    }

    /* Called on every frame */
    private void Update() {
        //this conversation is active
        if ( currEvent.hasStarted() )
            started = true;

        if ( leftConv ) {
            prevEvent = currEvent;
            return;
        }

        //this means we are NOT able to play another event
        if ( isPlayingSameType(currEvent) )
            return;

        prevEvent = currEvent;

        switch ( currEvent.name ) {
            case "Dialog":
                if (nextEvent != null && neededResponse(currEvent)) {
                    currEvent.waitForResponse(); //mark it 'unfinished'
                    return; //wait...
                }
                if ( nextEvent!=null && !isConversational(currEvent) )
                    playNextKnown(); //play the next event without wait
                if ( nextEvent==null )
                    Debug.Log(TAG + "Error: need next event after Response/Wildcard");
                break;
            case "Response":
            case "Wildcard":
                //Debug.Log(TAG + currEvent.name + " is a Resp/WC");
                if ( nextEvent == null )
                    Debug.Log(TAG + "Error: need next event after Response/Wildcard");
                if ( !currEvent.isDone() )
                    return; //wait until nextEvent is known
                else
                    playNextFind(); //play the next event when
                break;
            default:
                //not a conversational event (i.e. it can play concurrently)
                if (nextEvent == null)
                    Debug.Log(TAG + "Error: need next event after Response/Wildcard");
                if ( nextEvent!=null )
                    playNextKnown();
                else {
                    //Debug.Log(TAG + "is done");
                    finished = true;
                }
                break;
        }
    }

    /* Play the next event concurrently with other active events */
    private void playNextKnown() {
        currEvent = jm.getNextEvent(prevEvent); //we already know what it's next was
        setWants();
    }

    /* Play the next event concurrently with other active events */
    private void playNextFind() {
        prevEvent.finish();
        currEvent = prevEvent.nextEvent.GetComponent<EventIM>(); //we had to wait to find next
        setWants();
    }

    /* Get the currently played event's next event to play. */
    public EventIM getNextEvent() {
        if (!currEvent.hasStarted())
        {
            setWants();
            return currEvent;
        }
        if (neededResponse(currEvent))
        {
            currEvent = currEvent.nextEvent.GetComponent<EventIM>();
            setWants();
            return currEvent;
        }
        currEvent = jm.getNextEvent(currEvent);
        setWants();
        return currEvent;
    }

    /* Is another event playing of this same type? */
    private bool isPlayingSameType(EventIM e) {
        int counter = 0;
        foreach ( EventIM E in events )
            if ( e.name==E.name && !E.isActive() )
                counter++;
        if ( counter>0 )
            return true;
        return false;
    }

    /* Is this event's type a conversational one? */
    private bool isConversational(EventIM e ) {
        switch (e.name) {
            case "Diagonal":
            case "Response":
            case "Wildcard":
                return true;
            default:
                return false;
        }
    }

    /* Only Response and Wildcards require a recognition from user. */
    private bool neededResponse(EventIM e) {
        //assuming that the event is complete TO GET its nextEvent
        switch (e.name) {
            case "Response":
            case "Wildcard":
                return true;
            default:
                return false;
        }
    }


    /* ********** Accessors: State Changes ********** */
    public bool hasStarted() {
        return started;
    }

    public bool isDone() {
        return started && finished;
    }

    public bool isActive() {
        return started && !finished;
    }

    /* User moved to another conversation */
    public void leftConversation() {
        leftConv = true;
    }

    /* User returned this conversation from another conversation */
    public void returnedToConversation() {
        leftConv = false;
        currEvent = prevEvent;
        currEvent.wantToReplay(); //neither started/finished
    }

    /* User changed conversations */
    public void changedConversations() {
        if ( leftConv ) //we had already left this conversation
            returnedToConversation();
        else
            leftConversation(); //we are leaving this conversation
    }


    /* ********** Accessors: Event Matches ********** */
    private void setWants() {
        if ( currEvent==null || currEvent.isLastEvent )
        {
            finished = true;
            Debug.Log(TAG + "ended this conversation.");
            return;
        }
        agent = currEvent.agent.GetComponent<AgentStatusManager>();
        wantInRange = currEvent.wantInRange;
        wantLookedAt = currEvent.wantLookedAt;
    }

    public AgentStatusManager getAgent() {
        return agent;
    }

    public EventIM.EventSetting getWantLookedAt() {
        return wantLookedAt;
    }

    public EventIM.EventSetting getWantInRange() {
        return wantInRange;
    }
}