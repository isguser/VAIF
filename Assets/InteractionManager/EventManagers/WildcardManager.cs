using UnityEngine;
using UnityEngine.Windows.Speech;

public class WildcardManager : MonoBehaviour {

    public InteractionManager interactionManager;
    DictationRecognizer dictationRecognizer;
    protected Wildcard wild;

    // Use this for initialization
    void Start ()
    {
        interactionManager = gameObject.GetComponent<InteractionManager>();
    }

    public void Wildcard(Wildcard wildcard)
    {
        wild = wildcard;
        wild.started = true;
        Debug.Log("Recognizing wildcard...");
        dictationRecognizer = new DictationRecognizer();

        dictationRecognizer.DictationComplete += DictationRecognizer_DictationComplete;
        dictationRecognizer.AutoSilenceTimeoutSeconds = 1f; //seconds after an utterance
        dictationRecognizer.InitialSilenceTimeoutSeconds = wild.timeout; //seconds before utterance 
        dictationRecognizer.Start();

        dictationRecognizer.DictationResult += (text, confidence) =>
        {
            if (wild.annotation)
            {
                Debug.Log("Dictation result: " + text + " @confidence= " + confidence);
            }
        };
        wild.isDone = true;
    }

    private void DictationRecognizer_DictationComplete(DictationCompletionCause cause)
    {
        //Debug.Log("Done talking: "+ cause);
        Debug.Log("Response jump to: " + wild.wildcardJumpID);
        interactionManager.eventIndex = wild.wildcardJumpID;
        dictationRecognizer.DictationComplete -= DictationRecognizer_DictationComplete;
        dictationRecognizer.Dispose();
        interactionManager.stopListening(); //isListening = false;
    }

    public bool isRunning() {
        return (dictationRecognizer.Status == SpeechSystemStatus.Running);
    }

    public void stop() {
        dictationRecognizer.Stop();
    }
}
