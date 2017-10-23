using UnityEngine;
/// <summary>
/// TODO: Check if we should move this as part of the Repsonse class.
/// Either way, would this work or does it also have to be mved
/// as a corroutine to dialog manager?
/// </summary>
public class Wildcard : EventIM
{
    [Tooltip("Mandatory. Jump to this ID afdter the microphone pics up any noise.")]
    public int wildcardJumpID;
    [Tooltip("Mandatory. Seconds before a wildcard timeout occurs due to lack of audio input.")]
    public float timeout;
    [Tooltip("Optional. Annotate whatever is picked up by the dictation software.")]
    public bool annotation = false;
}
