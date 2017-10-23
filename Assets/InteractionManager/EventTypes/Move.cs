using UnityEngine;

public class Move : EventIM
{
    [Tooltip("Mandatory. Follow this game object's transform values. To unfollow use an unfollow event.")]
    public GameObject target;
    [Tooltip("Mandatory. Movement speed, defaults at 1, may need to change" +
        "depending on the scale of your scene.")]
    public float speed = 1;
    [Tooltip("Mandatory. Follow the object if the target moves.")]
    public bool follow = false;
}
