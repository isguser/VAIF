using UnityEngine;
using System;
using UnityEngine.Windows.Speech;
using System.Linq;
using System.Collections.Generic;

public class GrammarMapper : IEquatable<GrammarMapper>
{
    public string item { get; set; }
    public int jumpTo { get; set; }

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
    public InteractionManager interactionManager;

    Response response;

    void Start()
    {
        interactionManager = gameObject.GetComponentInParent<InteractionManager>();
    }


    public void Respond(Response response)
    {
        interactionManager.isListening = true;
        this.response = response;
        Debug.Log("Respond");
        int id = 0;
        foreach (string g in this.response.grammarItems)
        {
            gMapper.Add(new GrammarMapper() { item = g, jumpTo = this.response.jumpIDs[id] });
            keywordDictionary.Add(g, () => { });
            id++;
        }
        Debug.Log(this.response.grammarItems[0]);
        keywordRecognizer = new KeywordRecognizer(keywordDictionary.Keys.ToArray());
        keywordRecognizer.OnPhraseRecognized += KeywordRecognizer_OnPhraseRecognized;
        keywordRecognizer.Start();

        if (this.response.timeout > 0)
        {
            Invoke("ResponseTimeout", this.response.timeout);
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
                if (gMapper[i].Equals(new GrammarMapper { item = args.text, jumpTo = 0 }))
                {
                    Debug.Log("Response jump to: " + gMapper[i].jumpTo);
                    interactionManager.eventIndex = gMapper[i].jumpTo;
                    keywordRecognizer.Stop();
                    keywordRecognizer.Dispose();
                    interactionManager.isListening = false;
                    keywordDictionary.Clear();
                    gMapper.Clear();
                    keywordAction.Invoke();
                }
            }
        }
    }

    //where and when do we call this
    public void stopKeywordRecognizer()
    {
        PhraseRecognitionSystem.Shutdown();
        if (keywordRecognizer != null)
        {
            keywordRecognizer.Stop();
            keywordRecognizer.Dispose();
        }
    }
}

