using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpManager : MonoBehaviour
{
    public int[] nextEventID;
    public List<EventIM> events = new List<EventIM>();
    private List<GameObject> nextEvents = new List<GameObject>();

    private string TAG = "JM";

    /** nextEvents stores each event's nextEvent (GameObject)
     * CONVENTION: -1 means there is no next event */

    public JumpManager(List<EventIM> e)
    {
        events = e;
        nextEventID = new int[e.Count];
        init();
    }

    private void init()
    {
        for (int i = 0; i < events.Count; i++)
        {
            //get this event's nextEvent, if any
            if (events[i].nextEvent == null)
            {
                if (i == events.Count - 1)
                    nextEventID[i] = -1; //last event has no jump
                else
                    nextEventID[i] = i + 1; //not last event, so move linearly in list
            }
            else
            {
                nextEvents.Add(events[i].nextEvent);
                //save this nextEvent's EventID
                nextEventID[i] = indexOf(nextEvents[i]);
            }
        }
    }

    private int indexOf(GameObject e)
    {
        if (e == null || e.GetComponent<EventIM>() == null)
            return -1; //no nextEvent or not a type of EventIM
        for (int i = 0; i < events.Count; i++)
            if (events[i] == e.GetComponent<EventIM>())
                return i;
        return -1; //not found
    }

    public int getNextEventIndex(EventIM e)
    {
        //Debug.Log(TAG + " Finding nextEvent for: " + e.name);
        //return e's nextEventID
        for (int i = 0; i < events.Count; i++)
            if (events[i] == e)
                return nextEventID[i];
        return -1; //likely unreachable
    }
}
