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

    public void load(List<EventIM> e)
    {
        events = e;
        init();
    }

    private void init() {
        //get each event's nextEvent GameObject, if any
        for (int i = 0; i < events.Count; i++) {
            if ( events[i].nextEvent!=null )
                next.Add(events[i].nextEvent.GetComponent<EventIM>());
            else
                next.Add(new EventIM()); //never equal an empty object
        }
    }

    public EventIM getNextEvent(EventIM e)
    {
        //return e's nextEvent
        for (int i = 0; i < events.Count; i++)
            if ( events[i]==e ) {
                return next[i];
            }
        return null; //likely unreachable
    }

    public EventIM getFirstEvent() {
        return events[0];
    }
}
