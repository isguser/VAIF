using UnityEngine;

/// <summary>
/// TODO: Make a 2D array representation of this in the editor.
/// TODO: Copy the first event ID to all other elements of the grammar starting where the ID was placed, downwards.
/// </summary>
public class Response : EventIM
{
    [Tooltip("Mandatory. Text version of all reognizable words.")]
    public string [] grammarItems;
    [Tooltip("Mandatory. Event ID's as integers to match each grammar item to its consequence.")]
    public int [] jumpIDs;
    [Tooltip("Optional. How long until the character stops listening.")]
    public float timeout;
    [Tooltip("Optional. Jump point if a timeout case occurs.")]
    public int timeoutJumpID;
    [Tooltip("Optional. Counter of missrecognized input. After these many missrecognitions" +
        "the system will jump to the base case jump ID.")]
    public int missrecognitions;
    [Tooltip("Optional. Jump point after a specified set of missrecogntions.")]
    public string baseCaseJumpID;
}
