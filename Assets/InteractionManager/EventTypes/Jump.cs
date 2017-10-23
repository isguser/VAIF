using UnityEngine;

public class Jump : EventIM
{
    [Tooltip("Mandatory. Jump to this ID when this event is reached. For branching hyse the provided jumpID fields in response, memory" +
        "checks, emotion checks, etc.")]
    public int jumpID;
}
