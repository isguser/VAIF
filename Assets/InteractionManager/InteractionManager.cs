using System.Collections.Generic;
using UnityEngine;
using System.Linq;

/*
 * --------------------------------------------------------------------------------
 * States: dialog, animations, responses, triggers, gaze, emotions, dialog memory,
 *      environmental memory, agent memory, permanent memory, lookat
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
 * --------------------------------------------------------------------------------
 * INTERNAL FUNCTIONS
 * save/load
 * continue interaction when approaching same character
 * add internal ID as states are being added (numerical index)
 * proximity check (3 types: passing, arriving, interacting, leaving)
 * --------------------------------------------------------------------------------
 * OTHER
 * multiplayer VR network
 * Player to Player interaction
 */

public class InteractionManager : MonoBehaviour
{
    //public AnimationManager animations;
    public GameObject eventIndex; //Used for jumps
    public AgentStatusManager[] agents;
    private string TAG = "IM";

    protected bool matches = false;

    public List<GameObject> memories = new List<GameObject>();
    public List<EventIM> events = new List<EventIM>();
    public List<EventIM> convoEvents = new List<EventIM>();
    private EventIM lastPlayed;

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
    protected ConversationManager cm;

    private Conversation conversation;

    private void Start() {
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
        cm = new ConversationManager();

        rm = gameObject.GetComponent<ResponseManager>();
        wm = gameObject.GetComponent<WildcardManager>();
        tm = gameObject.GetComponent<TriggerManager>();
        wwm = gameObject.GetComponent<WaitManager>();
        mcm = gameObject.GetComponent<MemoryCheckManager>();
        cm = gameObject.GetComponent<ConversationManager>();

        conversation = cm.conversations.ElementAt(0);
    }

    private void Update() {
        EventIM e;
        //find the first Conversation to play this game
        foreach ( Conversation c in cm.conversations ) {
            Debug.Log(TAG + " anything?");
            //is the Conversation complete?
            if ( c.isOver() ) {
                c.finish();
                continue;
            }
            conversation = c;
            //get the next event to run in this Conversation
            e = c.findNextEvent(lastPlayed);
            //e = c.getFirstUnfinishedEvent();
            //is another event running?
            if ( !canStartEvent(e) )
                return; //wait til the next frame
            //if the conditions match, run this Conversation
            if ( getState(c)==getState(e) && canStartEvent(e) ) {
                lastPlayed = e; //this is event is the most recently played
                //move to the next Conversation
                RunGame(e);
                return;
            }
            //is the event done?
            if ( e.isDone() )
                done(e);
        }
    }

    public void RunGame(EventIM e) {
        if ( e.name!="Trigger" )
            sm = e.agent.GetComponent<AgentStatusManager>();
        matches = getState(e);
        if ( ( matches && !speaking() ) || e.name == "Trigger") {
            //memories.Add(eventIndex);
            Debug.Log(TAG + " Playing Conversation/Event: " + conversation.name + "/" + e.name);
            switch ( e.name ) {
                case "Dialog":
                    dm = e.agent.GetComponent<DialogManager>();
                    dm.Speak(e.GetComponent<Dialog>());
                    break;
                case "Animation":
                    //Animation needs to be fixed...
                    //dialog will not play if 105 conditions not met
                    //but animation will play...
                    am = e.agent.GetComponent<AnimationManager>();
                    am.PlayAnimation(e.GetComponent<Animate>());
                    break;
                case "Response":
                    if ( wm.isRunning() )
                        wm.stop();
                    rm.Respond(e.GetComponent<Response>());
                    break;
                case "Jump":
                    //TODO get JumpToIDs from GUI (GameObject)
                    eventIndex = e.GetComponent<Jump>().jumpTo;
                    break;
                case "Wildcard":
                    if (rm.isRunning())
                        rm.stopKeywordRecognizer();
                    wm.Wildcard(e.GetComponent<Wildcard>());
                    break;
                case "Trigger":
                    tm.Trigger(e.GetComponent<Trigger>());
                    break;
                case "Wait":
                    wwm.Waiting(e.GetComponent<Wait>());
                    break;
                case "Move":
                    mm = e.agent.GetComponent<MoveManager>();
                    mm.StartMoving(e.GetComponent<Move>());
                    break;
                case "StopMoving":
                    mm = e.agent.GetComponent<MoveManager>();
                    mm.Stop();
                    break;
                case "Emote":
                    em = e.agent.GetComponent<EmoteManager>();
                    em.EmotionBlendshape(e.GetComponent<Emote>());
                    break;
                case "MemoryCheck":
                    mcm.CheckMemories(e.GetComponent<MemoryCheck>());
                    break;
            }
            if ( !eventIsConversational(e) )
                e.finish();
        }
    }

    /* ********** Mutators: State Changes ********** */
    public void startListening()
    {
        sm.startListening();
    }
    public void stopListening()
    {
        sm.stopListening();
    }
    public void startWaiting()
    {
        sm.startWaiting();
    }
    public void stopWaiting()
    {
        sm.stopWaiting();
    }
    public void startSpeaking()
    {
        sm.startSpeaking();
    }
    public void stopSpeaking(AgentStatusManager curAgent) {
        if (curAgent.name == sm.name)
            sm.stopSpeaking();
        else
            curAgent.stopSpeaking();
    }

    /* ********** Accessors: State Changes ********** */
    private bool getState(EventIM e) {
        //grabbing author choices from UNITY
        esv.setLookedAt(e.wantLookedAt);
        esv.setInRange(e.wantInRange);
        //storing the current state of the user
        esv.setVerbals(sm.getVerbalState());
        return esv.checkStateMatch(sm.currentState());
    }

    private bool getState(Conversation e) {
        //grabbing author choices from UNITY
        esv.setLookedAt(e.wantLookedAt);
        esv.setInRange(e.wantInRange);
        //storing the current state of the user
        esv.setVerbals(sm.getVerbalState());
        return esv.checkStateMatch(sm.currentState());
    }

    private bool speaking() {
        //check if any agent is speaking
        foreach (AgentStatusManager A in agents)
            if (A.isSpeaking())
                return true;
        return false;
    }

    /* ********** Mutators: Event Sequence Behavior ********** */
    private void done(EventIM e)
    {
        Debug.Log(TAG + " Finished Event " + e.name);
        sm.stopSpeaking();
        esv.reset();
        e.finish();
    }

    private bool canStartEvent(EventIM e) {
        //has e started, or can it?
        return (!sm.isWaiting() && !e.hasStarted() && !e.isDone());
    }

    private bool eventIsConversational(EventIM e) {
        //of what type is the conversation?
        switch ( e.name ) {
            case "Dialog":
            case "Response":
            case "Wildcard":
                return true;
            default:
                return false;
        }
    }
}
