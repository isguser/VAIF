using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpManager : MonoBehaviour
{
    private List<EventIM> events = new List<EventIM>();
    private List<EventIM> next = new List<EventIM>();

    private string TAG = "JM ";

    /** nextEvents stores each event's nextEvent (GameObject)
     * CONVENTION: null means there is no next specified 'next' */

    public JumpManager() { }


    /* Store the events. */
    public void load(List<EventIM> e)
    {
        events = e;
        init();
    }

    /* Access each event's nextEvent to play next. */
    private void init() {
        //get each event's nextEvent GameObject, if any
        for (int i = 0; i < events.Count; i++) {
            if ( events[i].nextEvent!=null )
                next.Add(events[i].nextEvent.GetComponent<EventIM>());
            else
                next.Add(new EventIM()); //never equal an empty object
        }
    }

    /* Get an event's nextEvent. */
    public EventIM getNextEvent(EventIM e)
    {
        //return e's nextEvent
        for (int i = 0; i < events.Count; i++)
            if ( events[i]==e ) {
                return next[i];
            }
        return null; //likely unreachable
    }

    /* Get the first event. */
    public EventIM getFirstEvent() {
        return events[0];
    }
}
