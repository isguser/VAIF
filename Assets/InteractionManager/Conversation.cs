using UnityEngine;

public class Conversation : EventIM
{
    public bool start;
    public bool finish;
    public GameObject[] conversationEvent; //The amount of events in this instance of conversation
    /**
     * Future implementation: Track if the user left mid-conversation and where. 
     * protected bool startedButNotFinished;
     * protected GameObject whereWeLeftOff;
     **/
    public string convoTag;
    public int tagLocation = 0;

    public void startConversation()
    {
        Debug.Log("Conversation " + convoTag + " started!");
        start = true;
    }

    public void finishConversation(EventIM curEvent)
    {
        Debug.Log("     Length : " + conversationEvent.Length + "     " + conversationEvent[conversationEvent.Length - 1].GetInstanceID() + " : " + curEvent.gameObject.GetInstanceID());
        if (conversationEvent[conversationEvent.Length - 1].GetInstanceID()== curEvent.gameObject.GetInstanceID())   
        {
            Debug.Log("         Conversation " + convoTag + " finished!");
            finish = true;
        }
    }

    public void restartConversation()
    {
        start = false;
    }

    public void getEvents()
    {
        conversationEvent = GameObject.FindGameObjectsWithTag(convoTag);
        //Debug.Log("Conversation " + convoTag + " w/ Length " + conversationEvent.Length);
    }
    public void setTag(string curTag)
    {
        convoTag = curTag;
        //Debug.Log(curTag + " set " + convoTag);
    }

    public bool checkTag(string curTag)
    {
        if (convoTag == curTag)
            return true;
        return false;
    }

    public void displayEventIDInstances()
    {
        foreach (GameObject c in conversationEvent)
        {
            Debug.Log("                         Event " + c.tag + " ID: " + c.GetInstanceID());
        }
    }

}