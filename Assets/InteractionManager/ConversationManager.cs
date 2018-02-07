using System;
using UnityEngine;

public class ConversationManager : MonoBehaviour {
    public string currentTag;
    [Tooltip("Optional. Enabling this allows systems to jump to a specific EventID based on if conversations (or several events) have occurred.")]
    public bool activateConversations; //To be used in Interaction Manager in dialog case(?) : if(conversations){ do a checkConvos } else { run dialog event normally }
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

    // Use this for initialization
    void Awake ()
    {
        interactionManager = FindObjectOfType<InteractionManager>();
        //conversations = new Conversation[tagsToCheck.Length];
        fillConversations(); //Sets the conversations with their appropiate events... CHECK: if conversations have correct events!
    }

    public void checkConversations(EventIM curEvent)
    {
        //Debug.Log("CONVERSATION CHECK!");
        if (findConversation(curEvent))
        {

            /**if 
             * conversation has not started
             *      startConversation, jump toOccur
             * conversation has started but not finished
             *      if we are in an event with that conversation type then continue
             *      if we are in the last event of the conversation, finishConversation
             * conversation has started and finished 
             *      jump toOccured
             * conversation does not exist
             *      continue
            **/
            if (!curConvo.start)
            {
                //CHECK: do we need this if...
                if (curConvo.checkTag(currentTag))
                {
                    Debug.Log("Conversation " + currentTag + " found jumping to " + curConvo.tagLocation + " in conversationsOccured!");
                    curConvo.startConversation();
                    //Set the currentEvent in IM to this new current event in conversatiosOccured[conversation.tagLocation]
                    //return conversationsOccured[conversation.tagLocation];
                }
                else
                {
                    Debug.Log("Conversation " + currentTag + " not found jumping to " + curConvo.tagLocation + " in conversationsToOccur!");
                    //Set the currentEvent in IM to this new current event in conversatiosOccured[conversation.tagLocation]
                    //return conversationsOccured[conversation.tagLocation];
                }
            }
            else if (curConvo.start && !curConvo.finish)
            {
                Debug.Log("                 Started but not finished!");
                curConvo.finishConversation(curEvent);
            }
            else if(curConvo.start && curConvo.finish)
            {
                Debug.Log("                 Started && finished!");
            }
        }
        else
        {
            Debug.Log("ERROR: Conversation not found!");
        }
    }

    private void fillConversations()
    {
        int i = 0; 
        conversations = new Conversation[tagsToCheck.Length];
        foreach (String c in tagsToCheck)
        {
            Conversation convo = new Conversation();
            convo.setTag(tagsToCheck[i]);
            convo.getEvents();
            conversations[i] = convo;
            //Debug.Log("Tag: " + convo.convoTag);
            convo.displayEventIDInstances();
            i++;
        }
        Debug.Log("Conversations filled..");
    }

    //Check if the conversation exists..
    private bool findConversation(EventIM curEvent)
    {
        location = 0; 
        foreach (Conversation c in conversations)
        {
            //Debug.Log("TEST: " + c.convoTag);
            if (c.convoTag == curEvent.eventID.tag)
            {
                currentTag = tagsToCheck[location];
                curConvo = c;
                return true;
            }
            location++;
        }
        return false;
    }

    //CHECKME: Consult final version of system's jump!!
    void convoJump(GameObject jumpID)
    {
        //TOFIX? Not a proper description... Will only display: Jump to: Animation
        Debug.Log("Jump to: " + (jumpID.name));
        interactionManager.eventIndex = jumpID;
    }
}