using UnityEngine;
public class Dialog : EventIM
{
    [Tooltip("Mandatory. Audio file object you want to play in this dialog.")]
    public AudioClip audioFile;
    [Tooltip("Optional. Name of the audio file that you want to play, case sensitive.")]
    public string audioFileName;
    [Tooltip("Optional. Text of the dialog contained in the audio file for reference, not used by the system," +
        "only for readability.")]
    public string dialogText;
    [Tooltip("Optional. When not null, this defines the event into which the character jumps if it is interrupted. " +
        "If not set, you cannot interrupt the agent during this dialog.")]
    public string bargeIn;
}
