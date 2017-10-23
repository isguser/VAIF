using UnityEngine;

public class Gaze : EventIM
{

    [Tooltip("Mandatory. Gaze at this game object (drag from scene hierarchy).")]
    public GameObject pointOfInterest;
    [Tooltip("Optional. Delay for this many seconds looking at the object in question.")]
    public float delay;
    [Tooltip("Optional. Return to the default gaze settings after looking this many seconds at the specified object." +
        "If null, gaze will only be updated when another gaze event occurs.")]
    public float timeout;
    [Tooltip("Optional. If selected, it overrides all other options and turns default random gaze movement.")]
    public bool randomGaze;
}
