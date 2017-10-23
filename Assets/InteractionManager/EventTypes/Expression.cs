using UnityEngine;

public class Expression : EventIM
{
    [Tooltip("Mandatory. Based on your model's blendshapes, configure this to the " +
        "blendshape index of the emotion you want top edit.")]
    public int expression;
    [Tooltip("Mandatory. Intensity of the blendshape. The face will interpolate until it reaches the desired " +
        "value. For complex emotions use several expression comands to affect more than one blendshape.")]
    public bool magnitude = false;
}
