using UnityEngine;

public class EmotionCheck : EventIM
{
    //[Tooltip("Mandatory. Emotion to check.")]
    //public Emotion editEmotion;
    [Tooltip("Mandatory. If the emotion is less than this.")]
    public float greaterThan;
    [Tooltip("Mandatory. And the emotion is greater than this.")]
    public float lessThan;
    [Tooltip("Mandatory. Jump here. For equality checks write the same value in the previous fields.")]
    public string jumpID;
}
