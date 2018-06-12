using System.Collections.Generic;
using UnityEngine;

public class ConversationIM : MonoBehaviour
{
    [Tooltip("Mandatory. Drag the Conversations to this list that you want to run.")]
    public List<Conversation> conversation;

    public enum ConversationType
    {
        Conversation
    }
    
    public void AddConversation(string type) {
        switch (type) {
            case "Conversation":
                GameObject n_conversation = new GameObject();
                n_conversation.name = "Conversation";
                n_conversation.AddComponent<Conversation>();
                n_conversation.AddComponent<JumpManager>();
                n_conversation.AddComponent<EventIM>();
                conversation.Add(n_conversation.GetComponent<Conversation>());
                SetParent(n_conversation);
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
