using System;
using System.Collections.Generic;
using UnityEngine;

public class ConversationManager : MonoBehaviour {
    [Tooltip("Optional. Activate Conversation allows the user to enter different conversations.")]
    public bool activateConversation;
    public string currentTag;
    
    protected InteractionManager interactionManager;
    public Conversation[] conversations; //The amount of conversations to track must be added in editor
    protected Conversation curConvo;

    protected int location; 

    [Tooltip("Mandatory. System uses tags of the conversations to check if they exist in memory. Returns true if the event happened, otherwise false.")]
    public string[] tagsToCheck;
    [Tooltip("Mandatory. Jump to this ID if the conversation HAS occured.")]
    public GameObject[] conversationsOccured;
    [Tooltip("Mandatory. Jump to this ID if the conversation HAS NOT occured.")]
    public GameObject[] conversationsToOccur;

    public void setConversations(List<EventIM> events)
    {
        int i = 0;
        foreach (EventIM e in events)
        {
            Debug.Log(e.name + " - > " + conversations[i]);
            
            i++;
        }
    }

    public List<EventIM> firstEvents()
    {
        List<EventIM> e = new List<EventIM>();
        foreach (Conversation c in conversations)
        {
            Debug.Log("Conversation " + c.conversationName + " w/event 0  -> " + c.events[0].name );
            e.Add(c.events[0]);
        }
        return e; 
    }
}