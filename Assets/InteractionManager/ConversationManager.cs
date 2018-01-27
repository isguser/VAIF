using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConversationManager : MonoBehaviour {
    [Tooltip("Optional. Enabling this allows systems to jump to a specific EventID based on if conversations (or several events) have occurred.")]
    public bool conversations; //To be used in Interaction Manager in dialog case(?) : if(conversations){ do a checkConvos } else { run dialog event normally }
    protected InteractionManager interactionManager;
    protected bool currentMemories = true;
    protected Memory convos;

    [Tooltip("Mandatory. EventID of conversations to check. Returns true if the event happened.")]
    public GameObject[] convosToCheck;
    [Tooltip("Mandatory. Go to this ID if the conversation HAS occured.")]
    public GameObject[] convoOccured;
    [Tooltip("Mandatory. Go to this ID if the conversation HAS NOT occured.")]
    public GameObject[] convoToOccur;

    // Use this for initialization
    void Start ()
    {
        interactionManager = FindObjectOfType<InteractionManager>();
    }

    // Update is called once per frame
    void Update () {
		
	}

    public void checkConvos(Conversation convos)
    {
        //this.convos = convos;
        if (convoCheck(convos))
        {
            //convoJump(convos.ifIRemember);
        }
        else
        {
            //convoJump(convos.ifIDontRemember);
        }
    }

    bool convoCheck(Conversation convos)
    {
        foreach (GameObject c in convosToCheck)
        {
            Debug.Log("CONVERSATION CHECK: " + c);
            if (!interactionManager.memories.Contains(c))
            {
                Debug.Log("Conversation " + c + " not found");
                return false;
            }
        }
        return true;
    }

    void convoJump(GameObject jumpID)
    {
        //TOFIX? Not a proper description... Will only display: Jump to: Animation
        Debug.Log("Jump to: " + (jumpID.name));
        interactionManager.eventIndex = jumpID;
    }
}
