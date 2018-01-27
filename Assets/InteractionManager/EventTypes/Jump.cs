using UnityEngine;

public class Jump : EventIM
{
    [Tooltip("Mandatory. Jump to this ID when this event is reached. For branching drag the event to jump to." +
        "checks, emotion checks, etc.")]
    public GameObject jumpTo;
}
