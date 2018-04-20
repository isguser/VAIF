using UnityEngine;
using System;
using UnityEngine.Windows.Speech;
using System.Linq;
using System.Collections.Generic;

public class GrammarMapper : IEquatable<GrammarMapper>
{
    public string item { get; set; }
    public GameObject jumpTo { get; set; }
    private int jumpToEvent;

    public override string ToString()
    {
        return "item: " + item + "   jump to: " + jumpTo;
    }

    public override bool Equals(object obj)
    {
        if (obj == null) return false;
        GrammarMapper objAsPart = obj as GrammarMapper;
        if (objAsPart == null) return false;
        else return Equals(objAsPart);
    }

    public bool Equals(GrammarMapper other)
    {
        if (other == null) return false;
        return (item.Equals(other.item));
    }

    public override int GetHashCode()
    {
        return base.GetHashCode();
    }
}

public class ResponseManager : MonoBehaviour
{
    KeywordRecognizer keywordRecognizer;
    Dictionary<string, Action> keywordDictionary = new Dictionary<string, Action>();
    List<GrammarMapper> gMapper = new List<GrammarMapper>();
    private AgentStatusManager agentStatus;
    //private InteractionManager interactionManager;

    Response response;
    private List<String> repetition = new List<String>();
    private List<String> affirmation = new List<String>();
    private List<String> negation = new List<String>();
    private List<String> unsure = new List<String>();

    private GameObject nextEvent;

    private string TAG = "RM ";

    void Start() {  }


    /* Agent will start listening for a response (starts recognition). */
    public void Respond(Response r)
    {
        response = r;
        agentStatus = response.agent.GetComponent<AgentStatusManager>();
        agentStatus.startListening();
        response.start();
        addResponses();
        //words adding to dictionary
        keywordRecognizer = new KeywordRecognizer(keywordDictionary.Keys.ToArray());
        keywordRecognizer.OnPhraseRecognized += KeywordRecognizer_OnPhraseRecognized;
        keywordRecognizer.Start();
        Debug.Log(TAG + agentStatus.name + " is listening...");
        if (this.response.timeout > 0)
        {
            Invoke("ResponseTimeout", this.response.timeout);
        }
    }

    /* Add the list of responses (key phrases to recognize) */
    private void addResponses()
    {
        int id = 0;
        foreach (string g in this.response.responseItems)
        {
            gMapper.Add(new GrammarMapper() { item = g, jumpTo = this.response.jumpIDs[id] });
            //Debug.Log(TAG + g + " jumps to: " + response.jumpIDs[id].GetComponent<EventIM>().IDescription);
            if (keywordDictionary.ContainsKey(g)) continue; //no duplicates
            keywordDictionary.Add(g, () => { });
            if (g.Equals("yes", StringComparison.InvariantCultureIgnoreCase))
                addAffirmations(id);
            else if (g.Equals("no", StringComparison.InvariantCultureIgnoreCase))
                addNegatives(id);
            else if (g.Equals("i don't know", StringComparison.InvariantCultureIgnoreCase))
                addUnsures(id);
            id++;
        }
        addRepeat();
    }

    /* Add to the list of responses, request a replay of agent's last utterance */
    private void addRepeat()
    {
        //asking the agent similar 'what' responses incurs a repetition of current event
        buildRepetitions();
        foreach (String s in repetition)
        {
            gMapper.Add(new GrammarMapper() { item = s, jumpTo = this.response.nextEvent });
            if (keywordDictionary.ContainsKey(s)) return; //no duplicates
            keywordDictionary.Add(s, () => { });
        }
    }

    /* Add the list of responses (similar phrases to 'yes') */
    private void addAffirmations(int id)
    {
        foreach (string g in affirmation)
        {
            gMapper.Add(new GrammarMapper() { item = g, jumpTo = this.response.jumpIDs[id] });
            if (keywordDictionary.ContainsKey(g)) return; //no duplicates
            keywordDictionary.Add(g, () => { });
        }
    }

    /* Add the list of responses (similar phrases to 'no') */
    private void addNegatives(int id)
    {
        foreach (string g in negation)
        {
            gMapper.Add(new GrammarMapper() { item = g, jumpTo = this.response.jumpIDs[id] });
            if (keywordDictionary.ContainsKey(g)) return; //no duplicates
            keywordDictionary.Add(g, () => { });
        }
    }

    /* Add the list of responses (similar phrases to 'i don't know') */
    private void addUnsures(int id)
    {
        foreach (string g in unsure)
        {
            gMapper.Add(new GrammarMapper() { item = g, jumpTo = this.response.jumpIDs[id] });
            if (keywordDictionary.ContainsKey(g)) return; //no duplicates
            keywordDictionary.Add(g, () => { });
        }
    }

    /* Set the timeout after an utterance is made by the user to stop listening. */
    protected bool ResponseTimeout()
    {
        //interactionManager.eventIndex = response.timeoutJumpID;
        nextEvent = response.timeoutJumpID;
        response.nextEvent = response.timeoutJumpID;
        Debug.Log(TAG + "timeout is " + response.timeout);
        return false;
    }

    /* What to do when an utterance from the user is recognized. */
    private void KeywordRecognizer_OnPhraseRecognized(PhraseRecognizedEventArgs args)
    {
        Action keywordAction;
        // if the keyword recognized is in our dictionary, call that Action.
        if (keywordDictionary.TryGetValue(args.text, out keywordAction))
        {
            Debug.Log(TAG + args.text + " recognized; invoked: " + keywordAction.ToString());
            //foreach (GrammarMapper gm in gMapper) { Debug.Log(gm.ToString()); }
            for (int i = 0; i < gMapper.Count; i++)
            {
                if (gMapper[i].Equals(new GrammarMapper { item = args.text, jumpTo = null }))
                {
                    Debug.Log(TAG + "Response jump to: " + gMapper[i].jumpTo.GetComponent<EventIM>().name + " " + gMapper[i].jumpTo.GetComponent<EventIM>().IDescription);
                    //interactionManager.eventIndex = gMapper[i].jumpTo;
                    nextEvent = gMapper[i].jumpTo;
                    response.finish();
                    response.nextEvent = gMapper[i].jumpTo;
                    keywordRecognizer.Stop();
                    keywordRecognizer.Dispose();
                    agentStatus.stopListening();
                    keywordDictionary.Clear();
                    gMapper.Clear();
                    this.stopKeywordRecognizer();
                    keywordAction.Invoke();
                }
            }
        }
    }

    /* Access the nextEvent when the response/recognition is successfully completed. */
    public EventIM getNextEvent()
    {
        //we want to know which event to play next
        return nextEvent.GetComponent<EventIM>();
    }

    /* Stop the recognizer to be able to run other events. (called on recognition) */
    public void stopKeywordRecognizer()
    {
        PhraseRecognitionSystem.Shutdown();
        if (keywordRecognizer != null)
        {
            response.finish();
            keywordRecognizer.Stop();
            keywordRecognizer.Dispose();
        }
    }

    /* Check if this recognizer is running. */
    public bool isRunning()
    {
        if (keywordRecognizer != null)
            return keywordRecognizer.IsRunning;
        return false;
    }

    /* *********** The list of possibile user-responses for 'repeats'. *********** */
    private void buildRepetitions()
    {
        repetition.Add("What");
        repetition.Add("Huh");
        repetition.Add("I don't know what you said");
        repetition.Add("What was that");
        repetition.Add("What did you say");
        repetition.Add("Say that again");
        repetition.Add("Say it again");
        repetition.Add("Say again");
        repetition.Add("Come again");
        repetition.Add("Can you repeat that");
        repetition.Add("Can you repeat what you said");
        repetition.Add("Can you repeat what you just said");
        repetition.Add("Repeat it");
        repetition.Add("Repeat that");
        repetition.Add("Repeat yourself");
        repetition.Add("Repeat what you said");
        repetition.Add("Repeat what you just said");
    }

    /* *********** The list of possibile user-responses for 'yes'. *********** */
    private void buildAffirmations()
    {
        affirmation.Add("yeah");
        affirmation.Add("yup");
        affirmation.Add("yep");
        affirmation.Add("okay");
        affirmation.Add("sure");
        affirmation.Add("sounds good");
        affirmation.Add("that sounds good");
        affirmation.Add("uh huh");
    }

    /* *********** The list of possibile user-responses for 'no'. *********** */
    private void buildNegations()
    {
        negation.Add("nah");
        negation.Add("nope");
        negation.Add("no way");
        negation.Add("no, thank you");
        negation.Add("no, thanks");
    }

    /* *********** The list of possibile user-responses for 'i don't know'. *********** */
    private void buildUnsure()
    {
        unsure.Add("i'm unsure");
        unsure.Add("i'm not sure");
        unsure.Add("i dunno");
        unsure.Add("i have no idea");
        unsure.Add("i have no clue");
        unsure.Add("maybe");
        unsure.Add("i guess");
    }
}

