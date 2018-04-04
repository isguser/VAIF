using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpManager : MonoBehaviour
{
    public List<EventIM> events = new List<EventIM>();
    private List<EventIM> next = new List<EventIM>();

    private string TAG = "JM";

    /** nextEvents stores each event's nextEvent (GameObject)
     * CONVENTION: null means there is no next specified 'next' */

    public JumpManager(List<EventIM> e)
    {
        Debug.Log(TAG + " breh");
        load(e);
        Debug.Log(TAG + " wut");
    }

    public void load(List<EventIM> e)
    {
        events = e;
        Debug.Log(TAG + " maybe");
        init();
    }

    private void init() {
        //get each event's nextEvent GameObject, if any
        for (int i = 0; i < events.Count; i++) {
            Debug.Log(TAG + " this" + events[i].name);
        }
    }

    public EventIM getNextEvent(EventIM e)
    {
        //return e's nextEvent
        for (int i = 0; i < events.Count; i++)
            if ( events[i]==e )
                return next[i];
        Debug.Log(TAG + " no next event: " + e.name);
        return null; //likely unreachable
    }
}
