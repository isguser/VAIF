using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnableSection : EventIM
{
    [Tooltip("Mandatory. Event ID where system will jump to. WORK IN PROGRESS")]
    public int jumpID;
    [Tooltip("Optional. Name of the audio file that you want to play, case sensitive.")]
    public bool toChange;
    [Tooltip("Optional. Text of the dialog contained in the audio file for reference, not used by the system," +
        "only for readability.")]
    public string dialogText;
    [Tooltip("Optional. When not null, this defines the event into which the character jumps if it is interrupted. " +
        "If not set, you cannot interrupt the agent during this dialog.")]
    public string bargeIn;
}
