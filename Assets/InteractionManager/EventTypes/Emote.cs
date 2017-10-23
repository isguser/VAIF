using UnityEngine;

public class Emote : EventIM
{
    [Tooltip("Mandatory. Choose the emotion to express. Needs to match the main emotion labels and have a blendshape.")]
    public string emotion;
    [Tooltip("Mandatory. Show the blendshape of the emotion at this value.")]
    public float emotionalValue;
}
