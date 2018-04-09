using System;
using System.Collections.Generic;
using UnityEngine;

public class ConversationManager : MonoBehaviour {
    //[Tooltip("Optional. Activate Conversation allows the user to enter different conversations.")]
    //public bool activateConversation;
    //[Tooltip("Optional. Jump to which Conversation once this Conversation completes.")]
    //public GameObject[] conversationsNeededAfterThis;
    //[Tooltip("Optional. Jump to which Conversation before beginning this Conversation.")]
    //public GameObject[] conversationsNeededBeforeThis;
    private List<EventIM> firsts;
    public Conversation[] conversations; //The amount of conversations to track must be added in editor

    private string TAG = "CM";

    private void Start()
    {
        setFirstEvents();
    }

    private void setFirstEvents()
    {
        /* Grab the first events of each conversation */
        firsts = new List<EventIM>();
        foreach (Conversation c in conversations)
        {
            firsts.Add(c.getFirstUnfinishedEvent()); //c.events[0]);
        }
    }

    public bool inConversation()
    {
        /* Check if the game is in a conversation already */
        foreach (Conversation c in conversations)
            if (c.hasStarted() && !c.isDone())
            {
                Debug.Log(TAG + " We are in a conversation");
                return true;
            }
        Debug.Log(TAG + " We are NOT in a conversation");
        return false;
    }

    public List<EventIM> grabConversationEvents(EventIM e)
    {
        /* Grab the events in e's Conversation's events */
        Debug.Log(TAG + " grab this convo's events");
        foreach (Conversation c in conversations)
        {
            if (c.name.Equals(e.name))
                return c.events;
        }
        Debug.Log(TAG + " \tERROR: Conversation not found!");
        return null;
    }
}