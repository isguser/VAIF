using UnityEngine;

public class MemoryCheckManager : MonoBehaviour {

    protected InteractionManager interactionManager;
    protected bool currentMemories = true;
    protected MemoryCheck memories;

    /**********************************************************
    IF YOU CAN NOT MESS WITH THIS THAT WOULD BE AWESOME
        only add what is NEEDED for 'visited this convo already'
        behaviors
    @misha is stealing for a class project: how to use mems in
        convos
    thank you :)
    ***********************************************************/

    // Use this for initialization
    void Start ()
    {
        interactionManager = FindObjectOfType<InteractionManager>();
    }

    public void CheckMemories(MemoryCheck memories)
    {
        memories.started = true;
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
        foreach(GameObject m in memories.memoriesToCheck)
        {
            Debug.Log("MEMORY CHECK: " + m);
            if (!interactionManager.memories.Contains(m))
            {
                Debug.Log("Memory " + m + " not found");
                memories.isDone = true;
                return false;
            }
        }
        memories.isDone = true;
        return true;
    }

    void MemoryJump(GameObject jumpID)
    {
        //TOFIX? Not a proper description... Will only display: Jump to: Animation
        Debug.Log("Jump to: " + (jumpID.name) );
        interactionManager.eventIndex = jumpID;
        //TODO grab JM and go from there
    }
}
