using UnityEngine;
using UnityEngine.Windows.Speech;

public class WildcardManager : MonoBehaviour
{
    DictationRecognizer dictationRecognizer;
    protected Wildcard wild;
    private GameObject jumpToEvent;

    private string TAG = "WM";

    // Use this for initialization
    void Start() { }

    public void Wildcard(Wildcard wildcard)
    {
        jumpToEvent = null;
        wild = wildcard;
        wild.start();
        Debug.Log(TAG + " Recognizing...");
        dictationRecognizer = new DictationRecognizer();

        dictationRecognizer.DictationComplete += DictationRecognizer_DictationComplete;
        dictationRecognizer.AutoSilenceTimeoutSeconds = 1f; //seconds after an utterance
        dictationRecognizer.InitialSilenceTimeoutSeconds = wild.timeout; //seconds before utterance 
        dictationRecognizer.Start();

        dictationRecognizer.DictationResult += (text, confidence) =>
        {
            if (wild.annotation)
            {
                Debug.Log(TAG + " Dictation result: " + text + " @confidence= " + confidence);
                wild.finish();
            }
        };
    }

    private void DictationRecognizer_DictationComplete(DictationCompletionCause cause)
    {
        wild.finish();
        Debug.Log(TAG + " Response jump to: " + wild.wildcardJumpID);
        jumpToEvent = wild.wildcardJumpID;
        dictationRecognizer.DictationComplete -= DictationRecognizer_DictationComplete;
        dictationRecognizer.Dispose();
    }

    public bool isRunning()
    {
        return (dictationRecognizer.Status == SpeechSystemStatus.Running);
    }

    public void stop()
    {
        wild.finish();
        dictationRecognizer.Stop();
    }

    public GameObject getJumpEvent()
    {
        return jumpToEvent;
    }
}
