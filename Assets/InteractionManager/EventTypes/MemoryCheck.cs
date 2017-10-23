using UnityEngine;

public class MemoryCheck : EventIM
{
    [Tooltip("Mandatory. EventID of memory to check. Returns true if the event happened.")]
    public int [] memoriesToCheck;
    [Tooltip("Mandatory. Go to this EventID if the memory check returns true.")]
    public int ifIRemember;
    [Tooltip("Mandatory. Go to this EventID if the memory check returns false.")]
    public int ifIDontRemember;
    [Tooltip("Optional. Check this box to see if this memory has been recalled before. Returns true if it has.")]
    public bool recall;
}
