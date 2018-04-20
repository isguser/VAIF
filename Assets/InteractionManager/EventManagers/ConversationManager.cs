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
    [Tooltip("Drag each conversation to be played in the desired order.")]
    public Conversation[] conversation; //The amount of conversations to track must be added in editor

    private string TAG = "CM ";

    private void Start()
    {

    }

    private void Update()
    {
    }

    public bool inConversation()
    {
        /* Check if the user is in a conversation already */
        foreach (Conversation c in conversation)
            if ( c.isActive() )
            {
                //Debug.Log(TAG + " We are in a conversation");
                return true;
            }
        //Debug.Log(TAG + " We are NOT in a conversation");
        return false;
    }

    public List<EventIM> grabConversationEvents(EventIM e)
    {
        /* Grab the events in e's Conversation's events */
        //Debug.Log(TAG + " grab this convo's events");
        foreach (Conversation c in conversation)
        {
            if (c.name.Equals(e.name))
                return c.events;
        }
        Debug.Log(TAG + " \tERROR: Conversation not found!");
        return null;
    }
}