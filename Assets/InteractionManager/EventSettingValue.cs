using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventSettingValue : MonoBehaviour
{
    private bool[] want = new bool[] { false, false };
    private bool[] dontcare = new bool[] { false, false };
    private bool[] doingVerbals = new bool[] { false, false };
    private string TAG = "ESM";

    /** CONVENTION
     * want[0], dontcare[0] hold the inRange EventSetting
     * want[1], dontcare[1] hold the isLookedAt EventSetting
     * doingVerbals[0] holds the speaking state 
     * doingVerbals[1] holds the listening state */

    public EventSettingValue()
    {
        //don't care
    }


    public void setInRange(EventIM.EventSetting w)
    {
        //store what the author wants
        switch (w)
        {
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
    }

    public void setLookedAt(EventIM.EventSetting w)
    {
        //store what the author wants
        switch (w)
        {
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
    }

    public void setVerbals(bool[] sysState)
    {
        //store the current verbal state of the system
        doingVerbals[0] = sysState[0];
        doingVerbals[1] = sysState[1];
    }

    public bool checkStateMatch(bool[] have)
    {
        /** Returns TRUE if the agent is neither talking/listening, AND we have what we want in:
          * isLookedAt
          * isInRange
          */
        if (have.Length != want.Length)
            return false; //mismatched sizes
        if (talkingOrListening())
            return false; //agent is talk-/listen-ing
        for (int i = 0; i < dontcare.Length; i++)
            if (dontcare[i]) //if we don't care if T/F, skip it
                continue;
            else
                if (want[i] != have[i])
                return false; //if state doesn't match, return false
        return true;
    }

    private bool talkingOrListening()
    {
        for (int i = 0; i < doingVerbals.Length; i++)
            if (doingVerbals[i])
                return true;
        return false;
    }

    private void debugMe(bool[] have)
    {
        //This code is for testing purposes only
        for (int i = 0; i < want.Length; i++)
        {
            Debug.Log(TAG + " i=" + i + " Dontcare=" + dontcare[i] +
                " Want=" + want[i] +
                " Have=" + have[i]);
        }
        Debug.Log(TAG + "isSpeaking: " + doingVerbals[0] + " isListening: " + doingVerbals[1]);
    }

    public void reset()
    {
        //reset for next event
        for (int i = 0; i < want.Length; i++)
        {
            want[i] = false;
            dontcare[i] = false;
            if (i < doingVerbals.Length)
                doingVerbals[i] = false;
        }
    }
}
