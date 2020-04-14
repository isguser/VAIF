﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventSettingValue : MonoBehaviour
{
    private bool[] want = new bool[] { false, false };
    private bool[] dontcare = new bool[] { false, false };
    private bool[] have;
    private string TAG = "ESM";

    /** CONVENTION
     * want[0], dontcare[0], have[0] hold the isLookedAt EventSetting
     * want[1], dontcare[1], have[1] hold the inRange EventSetting
     * doingVerbals[0] holds the speaking state 
     * doingVerbals[1] holds the listening state */

    public EventSettingValue()
    {
        //don't care
    }

    /* Given the set of want in range, store these conditions */
    public void setWantInRange(EventIM.EventSetting w)
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

    /* Given the set of want looked at, store these conditions */
    public void setWantLookedAt(EventIM.EventSetting w)
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

    /* Given the set of current physical conditions of the agent */
    public void setCurrPhysical(bool[] sysState)
    {
        //store the current physical state of the system
        have = sysState;
    }

    /** Returns TRUE if the agent is neither talking/listening, AND we have what we want in:
      * isLookedAt
      * isInRange
      */
    public bool checkStateMatch()
    {
        //debugMe();
        if (have.Length != want.Length)
            return false; //mismatched sizes
        for (int i = 0; i < dontcare.Length; i++)
            if (dontcare[i]) //if we don't care if T/F, skip it
                continue;
            else
                if (want[i] != have[i])
                return false; //if state doesn't match, return false
        return true;
    }

    /* Reset the values for a new check */
    public void reset()
    {
        //reset for next event
        for (int i = 0; i < want.Length; i++)
        {
            want[i] = false;
            dontcare[i] = false;
        }
    }
}
