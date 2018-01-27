using UnityEngine;
using System;
using UnityEngine.Windows.Speech;
using System.Linq;
using System.Collections.Generic;

public class GrammarMapper : IEquatable<GrammarMapper>
{
    public string item { get; set; }
    public GameObject jumpTo { get; set; }

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
    private bool respond = false;
    public AgentStatusManager agentStatus;
    public InteractionManager interactionManager;

    Response response;

    void Start()
    {
        interactionManager = gameObject.GetComponent<InteractionManager>();
    }

    public void Respond(Response r) {
        response = r;
        agentStatus = response.agent.GetComponent<AgentStatusManager>();
        agentStatus.isListening = true;
        response.started = true;
        Debug.Log("Respond");
        addResponses();
        keywordRecognizer = new KeywordRecognizer(keywordDictionary.Keys.ToArray());
        keywordRecognizer.OnPhraseRecognized += KeywordRecognizer_OnPhraseRecognized;
        keywordRecognizer.Start();

        if (this.response.timeout > 0)
        {
            Invoke("ResponseTimeout", this.response.timeout);
        }
    }
    private void addResponses() {
        //untested --moved from line 58
        int id = 0;
        foreach (string g in this.response.grammarItems)
        {
            gMapper.Add(new GrammarMapper() { item = g, jumpTo = this.response.jumpIDs[id] });
            keywordDictionary.Add(g, () => { });
            if ( g.Equals("yes",StringComparison.InvariantCultureIgnoreCase) )
                addAffirmations(id); //TESTME
            else if ( g.Equals("no", StringComparison.InvariantCultureIgnoreCase) )
                addNegatives(id); //TESTME
            else if ( g.Equals("i don't know",StringComparison.InvariantCultureIgnoreCase) )
                addUnsures(id); //TESTME
            id++;
        }
        //addRepeat(); //TESTME
    }
    private void addRepeat() {
        String[] resp = {"can you repeat that","what","what did you say","huh","can you repeat that"};
        int id = 1;
        foreach (string g in resp) {
            gMapper.Add(new GrammarMapper() { item = g, jumpTo = this.response.jumpIDs[id-1] });
            keywordDictionary.Add(g, () => { });
            id++;
        }
    }
    private void addAffirmations(int id) {
        String[] aff = {"yeah", "yup", "yep", "okay", "that's okay", "sure", "sounds good"};
        foreach (string g in aff) {
            gMapper.Add(new GrammarMapper() { item = g, jumpTo = this.response.jumpIDs[id] });
            keywordDictionary.Add(g, () => { });
        }
    }
    private void addNegatives(int id) {
        String[] aff = {"nah","nope","no way","no, thank you"};
        foreach (string g in aff) {
            gMapper.Add(new GrammarMapper() { item = g, jumpTo = this.response.jumpIDs[id] });
            keywordDictionary.Add(g, () => { });
        }
    }
    private void addUnsures(int id) {
        String[] aff = {"i dunno","i'm unsure","i'm not sure","maybe","i guess"};
        foreach (string g in aff) {
            gMapper.Add(new GrammarMapper() { item = g, jumpTo = this.response.jumpIDs[id] });
            keywordDictionary.Add(g, () => { });
        }
    }

    protected bool ResponseTimeout()
    {
        interactionManager.eventIndex = response.timeoutJumpID;
        return false;
    }

    private void KeywordRecognizer_OnPhraseRecognized(PhraseRecognizedEventArgs args)
    {
        Action keywordAction;
        // if the keyword recognized is in our dictionary, call that Action.
        if (keywordDictionary.TryGetValue(args.text, out keywordAction))
        {
            Debug.Log(args.text + " recognized. Action invoked " + keywordAction.ToString());
            //foreach (GrammarMapper gm in gMapper) { Debug.Log(gm.ToString()); }
            for (int i = 0; i < gMapper.Count; i++)
            {
                if (gMapper[i].Equals(new GrammarMapper { item = args.text, jumpTo = null }))
                {
                    Debug.Log("Response jump to: " + gMapper[i].jumpTo);
                    interactionManager.eventIndex = gMapper[i].jumpTo;
                    keywordRecognizer.Stop();
                    keywordRecognizer.Dispose();
                    agentStatus.isListening = false;
                    keywordDictionary.Clear();
                    gMapper.Clear();
                    this.stopKeywordRecognizer();
                    keywordAction.Invoke();
                }
            }
        }
        response.isDone = true;
    }

    public void stopKeywordRecognizer()
    {
        PhraseRecognitionSystem.Shutdown();
        if (keywordRecognizer != null)
        {
            keywordRecognizer.Stop();
            keywordRecognizer.Dispose();
        }
    }

    public bool isRunning()
    {
        if (keywordRecognizer!=null)
            return keywordRecognizer.IsRunning;
        return false;
    }

    public void stop()
    {
        keywordRecognizer.Stop();
    }
}

