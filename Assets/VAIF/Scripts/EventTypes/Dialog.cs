using UnityEngine;
using Crosstales.RTVoice;
using System.Collections.Generic;

public class Dialog : EventIM
{
    [Tooltip("Mandatory (if not using Text-to-Speech). Audio file object you want to play in this dialog.")]
    public AudioClip audioFile;
    [Tooltip("Optional. Name of the audio file that you want to play, case sensitive.")]
    public string audioFileName;
    [Tooltip("Optional. Check if you are using a Text-to-Speech engine to voice the dialogs instead of audio files." +
        "\nRequires the RT Voice Pro asset. See documentation for more information." +
        "\nIf checked, you must also assign a different I Description for each Dialog event.")]
    public bool usingTTS;
    [Tooltip("Mandatory (if using Text-to-Speech (TTS)). Text of the dialog contained in the audio file for reference." +
        "\nIf using TTS, you must enter the text format of the utterance for the agent here." +
        " You must also assign a different I Description for each Dialog event.")]
    public string dialogText;
    [Tooltip("Optional. When not null, this defines the event into which the character jumps if it is interrupted. " +
        "If not set, you cannot interrupt the agent during this dialog.")]
    public string bargeIn;
    //protected EventType eventType = EventType.Dialog;
}
