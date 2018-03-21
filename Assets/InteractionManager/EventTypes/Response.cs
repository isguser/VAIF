﻿using UnityEngine;

/// <summary>
/// TODO: Make a 2D array representation of this in the editor.
/// TODO: Copy the first event ID to all other elements of the grammar starting where the ID was placed, downwards.
/// </summary>
public class Response : EventIM
{
    [Tooltip("Mandatory. Text version of all reognizable words.\n 'yes' will add similar affirmative responses.\n 'no' will add similar negative responses.\n 'i don't know' will add similar unsure responses.")]
    public string[] responseItems;
    [Tooltip("Mandatory. Event ID's as integers to match each grammar item to its consequence.")]
    public GameObject[] jumpIDs;
    [Tooltip("Optional. How long until the character stops listening (in seconds).")]
    public float timeout;
    [Tooltip("Optional. Jump point if a timeout case occurs.")]
    public GameObject timeoutJumpID;
    [Tooltip("Optional. Counter of missrecognized input. After these many missrecognitions" +
        "the system will jump to the base case jump ID.")]
    public GameObject missrecognitions;
    [Tooltip("Optional. Jump point after a specified set of missrecogntions.")]
    public string baseCaseJumpID;
}
