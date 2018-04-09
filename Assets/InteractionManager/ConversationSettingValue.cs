using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConversationSettingValue : MonoBehaviour
{
    private bool[] wantIR; //want inRange
    private bool[] wantLA; //want lookAt
    private bool[] dontcareIR;
    private bool[] dontcareLA;
    private bool[] haveIR;
    private bool[] haveLA;
    private string TAG = "CSM ";
    private int numAgents;
    private bool[] match;
    List<AgentStatusManager> agents;

    /** CONVENTION
     * wantIR[i], dontcareIR[i], haveIR[i] for agent[i]
     * wantLA[i], dontcareIR[i], haveLA[i] for agent[i]
     */
    public ConversationSettingValue(int a) {
        numAgents = a;
        match = new bool[a];
        wantIR = new bool[a];
        wantLA = new bool[a];
        dontcareIR = new bool[a];
        dontcareLA = new bool[a];
    }


    public void setWantInRange(List<Conversation.ConversationSetting> w) {
        for ( int i=0 ; i<w.Count ; i++ )
            //store what the author wants
            switch (w[i]) {
                case (Conversation.ConversationSetting.TRUE):
                    wantIR[i] = true;
                    break;
                case (Conversation.ConversationSetting.FALSE):
                    wantIR[i] = false;
                    break;
                case (Conversation.ConversationSetting.DONTCARE):
                    dontcareIR[i] = true;
                    break;
                default:
                    dontcareIR[i] = false;
                    break;
            }
    }

    public void setWantLookedAt(List<Conversation.ConversationSetting> w)
    {
        for ( int i=0; i<w.Count; i++ )
        //store what the author wants
        switch (w[i])
        {
            case (Conversation.ConversationSetting.TRUE):
                wantLA[i] = true;
                break;
            case (Conversation.ConversationSetting.FALSE):
                wantLA[i] = false;
                break;
            case (Conversation.ConversationSetting.DONTCARE):
                dontcareLA[i] = true;
                break;
            default:
                dontcareLA[i] = false;
                break;
        }
    }

    public void setCurrPhysical(List<AgentStatusManager> a) {
        //store the current physical state of the system
        agents = a;
    }

    public bool checkStateMatch()
    {
        /** Returns TRUE if the agent is neither talking/listening, AND we have what we want in:
          * isLookedAt
          * isInRange
          */
        if ( wantIR.Length != wantLA.Length )
            return false; //mismatched sizes
        bool pass = true;
        for ( int i = 0; i < agents.Count ; i++ )
            if ( dontcareIR[i] )  {//if we don't care if T/F, skip it
                if ( dontcareLA[i] )//if we don't care if T/F, skip it
                    continue;
                if ( wantLA[i] != agents[i].isLookedAt() )
                    pass = false; //if state doesn't match, return false
            } else {
                if ( wantIR[i] != agents[i].isInRange() )
                    pass = false; //if state doesn't match, return false
                if ( dontcareLA[i] )//if we don't care if T/F, skip it
                    continue;
                if ( wantLA[i] != agents[i].isLookedAt() )
                    pass = false; //if state doesn't match, return false
            }
        return pass;
    }

    public void reset()
    {
        //reset for next event
        for (int i = 0; i < wantIR.Length; i++)
        {
            wantIR[i] = false;
            dontcareIR[i] = false;
            wantLA[i] = false;
            dontcareLA[i] = false;

            haveIR[i] = false;
            haveLA[i] = false;
        }
    }
}
