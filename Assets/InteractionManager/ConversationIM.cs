using System.Collections.Generic;
using UnityEngine;

public class ConversationIM : MonoBehaviour
{
    [Tooltip("Mandatory. Drag the Conversations to this list that you want to run.")]
    public List<Conversation> conversation;

    public enum ConversationType
    {
        InteractionConversation,
        OtherConversation
    }
    
    public void AddConversation(string type) {
        switch (type) {
            case "InteractionConversation":
                GameObject n_conversation = new GameObject();
                n_conversation.name = "InteractionConversation";
                n_conversation.AddComponent<Conversation>();
                n_conversation.AddComponent<JumpManager>();
                n_conversation.AddComponent<EventIM>();
                SetParent(n_conversation);
                break;
            case "OtherConversation":
                GameObject abconversation = new GameObject();
                abconversation.name = "OtherConversation";
                abconversation.AddComponent<Conversation>();
                abconversation.AddComponent<JumpManager>();
                abconversation.AddComponent<EventIM>();
                SetParent(abconversation);
                break;
        }
    }

    //Invoked when a new event is created.
    public void SetParent(GameObject eventInstance)
    {
        //Makes the GameObject "newParent" the parent of the GameObject.
        eventInstance.transform.parent = gameObject.transform;
    }
}
