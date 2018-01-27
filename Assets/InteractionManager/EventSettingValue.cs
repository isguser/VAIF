using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventSettingValue : MonoBehaviour {
    private bool[] want = new bool[]{false,false};
    private bool[] dontcare = new bool[] { false, false };
    private bool[] doingVerbals = new bool[] { false, false };

    public EventSettingValue() {
        //don't care
    }

    public void setValues(EventIM.EventSetting isInRange, EventIM.EventSetting isLookedAt, bool isSpeaking, bool isListening) {
        //Use the enums in InteractionManager
        switch ( isInRange ) {
            case (EventIM.EventSetting.TRUE):
                want[0] = true;
                break;
            case (EventIM.EventSetting.FALSE):
                want[0] = false;
                break;
            case (EventIM.EventSetting.DONTCARE):
                dontcare[0] = true;
                break;
        }
        switch (isLookedAt) {
            case (EventIM.EventSetting.TRUE):
                want[1] = true;
                break;
            case (EventIM.EventSetting.FALSE):
                want[1] = false;
                break;
            case (EventIM.EventSetting.DONTCARE):
                dontcare[1] = true;
                break;
        }
        //store if the agent is talking/listening in this event
        doingVerbals[0] = isSpeaking;
        doingVerbals[1] = isListening;
    }

    public bool checkStateMatch(bool[] have) {
        /** Returns TRUE IFF the agent is neither talking/listening, AND we have what we want in:
          * isLookedAt
          * isInRange
          */
        //debugMe(have);
        if (have.Length != want.Length)
        {
            return false; //mismatched sizes
        }
        if (talkingOrListening())
        {
            return false; //agent is talk-/listen-ing
        }
        for (int i = 0; i < dontcare.Length; i++)
            if (dontcare[i]) //if we don't care if T/F, skip it
                continue;
            else
                if (want[i] != have[i])
                {
                    return false; //if state doesn't match, return false
                }
        return true;
    }

    private bool talkingOrListening()
    {
        for ( int i=0; i<doingVerbals.Length; i++ )
            if ( doingVerbals[i] ) return true;
        return false;
    }

    private void debugMe(bool[] have) {
        //This code is for testing purposes only
        for (int i = 0; i < want.Length; i++)
        {
            Debug.Log("Dontcare isInRange=" + dontcare[i] + 
                "\nDontcare isLookedAt=" + dontcare[i] +
                "\nWant isInRange=" + want[i] +
                "\nHave isLookedAt=" + have[i]);
        }
        Debug.Log("isSpeaking: " + doingVerbals[0] +
                "\nisListening: " + doingVerbals[1]);
    }

    public void reset()
    {
        //reset otherwise they gon' overlap for next event
        want = new bool[] { false, false } ;
        dontcare = new bool[] { false, false };
        doingVerbals = new bool[] { false, false };
    }
}
