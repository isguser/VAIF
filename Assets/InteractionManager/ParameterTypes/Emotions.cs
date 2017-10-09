using UnityEngine;


public class Emotions : MonoBehaviour
{
    //https://simple.wikipedia.org/wiki/List_of_emotions#/media/File:Plutchik-wheel.svg
    [Tooltip("Interest -> Anticipation -> Vigilance")]
    [Range(0.0f, 100.0f)]
    public float anticipation;

    [Tooltip("Serenity -> Joy -> Ecstasy")]
    [Range(0.0f, 100.0f)]
    public float joy;

    [Tooltip("Acceptance -> Trust -> Admiration")]
    [Range(0.0f, 100.0f)]
    public float trust;

    [Tooltip("Apprehension -> Fear -> Terror")]
    [Range(0.0f, 100.0f)]
    public float fear;

    [Tooltip("Distraction -> Surprise -> Amazement")]
    [Range(0.0f, 100.0f)]
    public float surprise;

    [Tooltip("Pensiveness -> Sadness -> Grief")]
    [Range(0.0f, 100.0f)]
    public float sadness;

    [Tooltip("Boredom -> Disgust -> Loathing")]
    [Range(0.0f, 100.0f)]
    public float disgust;

    [Tooltip("Annoyance -> Anger -> Rage")]
    [Range(0.0f, 100.0f)]
    public float anger;

    /*
    "Aggresiveness = Anger + Anticipation"
    "Optimism = Anticipation + Joy"
    "Love = Joy + Trust"
    "Submission = Trust + Fear"
    "Awe = Fear + Surprise"
    "Dsiapproval = Surprise + Sadness"
    "Remorse = Sadness + Disgust"
    "Contempt = Disgust + Anger"
    */
}

