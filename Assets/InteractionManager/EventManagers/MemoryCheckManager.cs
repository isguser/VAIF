using UnityEngine;

public class MemoryCheckManager : MonoBehaviour {

    protected InteractionManager interactionManager;
    protected bool currentMemories = true;
    protected MemoryCheck memories;

    // Use this for initialization
    void Start ()
    {
        interactionManager = FindObjectOfType<InteractionManager>();
    }

    public void CheckMemories(MemoryCheck memories)
    {
        this.memories = memories;
        if(CheckMemory(memories))
        {
            MemoryJump(memories.ifIRemember);
        }
        else
        {
            MemoryJump(memories.ifIDontRemember);
        }
    }

    bool CheckMemory(MemoryCheck memories)
    {
        foreach(int m in memories.memoriesToCheck)
        {
            Debug.Log("MEMORY CHECK: " + m);
            if (!interactionManager.memories.Contains(m))
            {
                Debug.Log("Memory " + m + " not found");
                return false;
            }
        }
        return true;
    }

    void MemoryJump(int jumpID)
    {
        Debug.Log("Jump to: " + (jumpID - 1) );
        interactionManager.eventIndex = jumpID-1;
    }
}
