using System.Collections.Generic;
using UnityEngine;
using System.Linq;

/*
 * -------------------------------------------------------------------------------------------------------------------------------
 * States: dialog, animations, responses, triggers, gaze, emotions, dialog memory, environmental memory, agent memory, permanent memory, lookat
 * TODO dialog memory: remembers where the player left off with the agent
 * TODO environmental memory: agents can remember what they saw or heard you do
 * TODO update the dictation to store what the player said
 * TODO permamnent memory (across scenes and sessions) by implementing a save file 
 * TODO update look at system (turn gaze, then head, then body)
 * TODO controller interaction (what we allow the player to do)
 * TODO network (multiplayer & multiagent)
 * TODO modify dialog prosody
 * TODO default dialog cases for misunderstandings or misrecognitions
 * TODO agent memory: agents can remember other interactions with agents
 * TODO set initial personality, could be used to weight emotions or alter prosody
 * -------------------------------------------------------------------------------------------------------------------------------
 * INTERNAL FUNCTIONS
 * save/load
 * continue interaction when approaching same character
 * add internal ID as states are being added (numerical index)
 * proximity check (3 types: passing, arriving, interacting, leaving)
 * -------------------------------------------------------------------------------------------------------------------------------
 * OTHER
 * multiplayer VR network
 * Player to Player interaction
 */

public class InteractionManager : MonoBehaviour
{
    //public AnimationManager animations;
    public int eventIndex = 0;
    protected bool start = true;
    protected bool matches = false;
    protected bool isInEvent = false;

    public List<int> memories = new List<int>();
    public List<EventIM> events = new List<EventIM>();
    protected EventSettingValue esv; //state comparisons to GUI

    protected DialogManager dm;
    protected AnimationManager am;
    protected ResponseManager rm;
    protected WildcardManager wm;
    protected TriggerManager tm;
    protected WaitManager wwm;
    protected MoveManager mm;
    protected EmoteManager em;
    protected MemoryCheckManager mcm;
    protected AgentStatusManager sm;

    protected Queue<EventIM> q = new Queue<EventIM>();

    private void Start()
    {
        dm = new DialogManager();
        am = new AnimationManager();
        rm = new ResponseManager();
        wm = new WildcardManager();
        tm = new TriggerManager();
        wwm = new WaitManager();
        mm = new MoveManager();
        em = new EmoteManager();
        mcm = new MemoryCheckManager();
        sm = new AgentStatusManager();
        esv = new EventSettingValue();

        rm = gameObject.GetComponent<ResponseManager>();
        wm = gameObject.GetComponent<WildcardManager>();
        tm = gameObject.GetComponent<TriggerManager>();
        wwm = gameObject.GetComponent<WaitManager>();
        mcm = gameObject.GetComponent<MemoryCheckManager>();
        foreach (Transform child in transform) {
            events.Add(child.GetComponentInChildren<EventIM>());
        }
        start = true;
    }

    private void Update() {
        if (eventIndex < events.Count && start && !sm.isWaiting )
        {
            RunGame();
        }
    }

    public void RunGame()
    {
        EventIM e = events.ElementAt(eventIndex); //get curr event
        if ( e.name != "Trigger") {
            sm = e.agent.GetComponent<AgentStatusManager>();
        }
        matches = getState(e);
        Debug.Log(matches);
        if ( matches )
            isInEvent = true;
        else
            isInEvent = false;
        if ( matches || e.name == "Trigger" )
        {
            memories.Add(eventIndex);
            Debug.Log("Event index playing..." + eventIndex);
            switch (e.name)
            {
                //TODO fix the 'done' to each eventtype
                case "Dialog":
                    dm = e.agent.GetComponent<DialogManager>();
                    dm.Speak(e.GetComponent<Dialog>());
                    done();
                    break;
                case "Animation":  //Animation needs to be fixed.. dialog will not play if 105 conditions not met but animation will play..
                    am = e.agent.GetComponent<AnimationManager>();
                    am.PlayAnimation(e.GetComponent<Animate>());
                    done();
                    break;
                case "Response":
                    if (wm.isRunning())
                        wm.stop();
                    rm.Respond(e.GetComponent<Response>());
                    done();
                    break;
                case "Jump":
                    //TODO jump across events
                    eventIndex = e.GetComponent<Jump>().jumpID - 1;
                    done();
                    break;
                case "Wildcard":
                    if (rm.isRunning())
                        rm.stop();
                    wm.Wildcard(e.GetComponent<Wildcard>());
                    done();
                    break;
                case "Trigger":
                    tm.Trigger(e.GetComponent<Trigger>());
                    done();
                    break;
                case "Wait":
                    wwm.Waiting(e.GetComponent<Wait>().waitTime);
                    done();
                    break;
                case "Move":
                    mm = e.agent.GetComponent<MoveManager>();
                    mm.StartMoving(e.GetComponent<Move>());
                    done();
                    break;
                case "StopMoving":
                    mm = e.agent.GetComponent<MoveManager>();
                    mm.Stop();
                    done();
                    break;
                case "Emote":
                    em = e.agent.GetComponent<EmoteManager>();
                    em.EmotionBlendshape(e.GetComponent<Emote>());
                    done();
                    break;
                case "MemoryCheck":
                    mcm.CheckMemories(e.GetComponent<MemoryCheck>());
                    done();
                    break;
            }
            eventIndex++;
        }
    }

    public void startListening()
    {
        sm.isListening = true;
    }
    public void stopListening()
    {
        sm.isListening = false;
    }
    public void startWaiting()
    {
        sm.isWaiting = true;
    }
    public void stopWaiting()
    {
        sm.isWaiting = false;
    }
    public void startSpeaking()
    {
        sm.isSpeaking = true;
    }
    public void stopSpeaking()
    {
        sm.isSpeaking = false;
    }

    private void setupESV(EventIM e) {
        //grabbing from UNITY
        esv.setValues(e.isInRange, e.isLookedAt, sm.isSpeaking, sm.isListening);
    }
    private bool getState(EventIM e) {
        setupESV(e);
        //gets the current isInRange and isLookedAt state
        bool[] b = new bool[2];
        b[0] = sm.isInRange;
        b[1] = sm.isLookedAt;
        return esv.checkStateMatch(b);
    }
    private void done()
    {
        isInEvent = false;
        esv.reset();
    }
}
