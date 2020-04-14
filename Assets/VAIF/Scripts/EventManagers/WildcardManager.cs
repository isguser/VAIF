using UnityEngine;
using UnityEngine.Windows.Speech;

public class WildcardManager : MonoBehaviour
{
    private DictationRecognizer dictationRecognizer;
    protected Wildcard wild;

    private string TAG = "WM ";

    // Use this for initialization
    void Start() { }

    /* Start the recognizer for dictation events */
    public void Wildcard(Wildcard wildcard)
    {
        wild = wildcard;
        wild.start();
        Debug.Log(TAG + "Recognizing...");
        dictationRecognizer = new DictationRecognizer();

        dictationRecognizer.DictationResult += DictationRecognizer_DictationResult;
        //dictationRecognizer.DictationHypothesis += DictationRecognizer_DictationHypothesis; //used for testing
        dictationRecognizer.DictationComplete += DictationRecognizer_DictationComplete;
        dictationRecognizer.AutoSilenceTimeoutSeconds = 1f; //seconds after an utterance
        dictationRecognizer.InitialSilenceTimeoutSeconds = wild.timeout; //seconds before utterance

        dictationRecognizer.Start();
    }

    /* What to do on a recognized utterance from user */
    private void DictationRecognizer_DictationResult(string text, ConfidenceLevel conf)
    {
        //on a recognized dictation from user...
        wild.finish();
        dictationRecognizer.DictationResult -= DictationRecognizer_DictationResult;

        Debug.Log(TAG + "Dictation result: " + text + " @confidence= " + conf);
    }

    /* What to do while recognizing utterance from user */
    private void DictationRecognizer_DictationHypothesis(string text)
    {
        //on a guess dictation of user...
        dictationRecognizer.DictationHypothesis -= DictationRecognizer_DictationHypothesis;

        //Debug.Log(TAG + "Dictation hypothesized: " + text);
    }

    /* What to do when a recognition completes (also by timeout) */
    private void DictationRecognizer_DictationComplete(DictationCompletionCause cause)
    {
        //on any cause for completion...
        wild.finish();
        dictationRecognizer.DictationComplete -= DictationRecognizer_DictationComplete;

        Debug.Log(TAG + "Dictation complete: " + cause);
    }

    /* Check if this recognizer is active. */
    public bool isRunning()
    {
        return ( dictationRecognizer!=null || (dictationRecognizer.Status == SpeechSystemStatus.Running));
    }

    /* Stop this recognizer. */
    public void stop()
    {
        if ( wild!=null )
            wild.finish();
        if ( dictationRecognizer != null ) {
            dictationRecognizer.Dispose();
            dictationRecognizer.Stop();
        }
    }
}
