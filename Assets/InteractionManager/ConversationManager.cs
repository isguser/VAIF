using System;
using UnityEngine;

public class ConversationManager : MonoBehaviour {
    [Tooltip("Optional. Activate Conversation allows the user to enter different conversations.")]
    public bool activateConversation;
    public string currentTag;
    
    protected InteractionManager interactionManager;
    protected Conversation[] conversations; //The amount of conversations to track
    protected Conversation curConvo;

    protected int location; 

    [Tooltip("Mandatory. System uses tags of the conversations to check if they exist in memory. Returns true if the event happened, otherwise false.")]
    public string[] tagsToCheck;
    [Tooltip("Mandatory. Jump to this ID if the conversation HAS occured.")]
    public GameObject[] conversationsOccured;
    [Tooltip("Mandatory. Jump to this ID if the conversation HAS NOT occured.")]
    public GameObject[] conversationsToOccur;

}