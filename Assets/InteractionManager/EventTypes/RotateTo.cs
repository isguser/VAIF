using UnityEngine;

public class RotateTo : EventIM
{
    /// <summary>
    /// TODO: Update this with a proper rotation system that blends the rotating animation properly.
    /// </summary>
    [Tooltip("Mandatory. Rotate the agent's body to look at this object.")]
    public GameObject rotateToObject;
    [Tooltip("The speed in which you would like the agent to rotate at.")]
    public float speed;
    //protected EventType eventType = EventType.RotateTo;
}
