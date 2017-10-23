using UnityEngine;

public class Animate : EventIM {

    [Tooltip("Mandatory. Name of the animation file you want to play, case sensitive." +
    "Animations need to be set at the AnimationManager located in the agent's parent.")]
    public new string animation;

    [Tooltip("Optional. Loop this animation until another animation is called. The actual animation object must be set to loop.")]
    public bool loop = false;

    [Tooltip("Optional. Interrupt the animation at this given time (in other words, play the animation for this" +
        "length of time.")]
    public float timeout = 0;

    [Tooltip("Optional. Start the animation after this amount of seconds.")]
    public float delay = 0;

    [Tooltip("Optional. Switch animations by going through the idle state.")]
    public bool toIdle = true;
}
