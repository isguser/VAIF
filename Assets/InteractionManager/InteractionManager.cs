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
    public GameObject eventIndex; //Used for jumps
    public AgentStatusManager[] agents;
    protected int eventID; //Internal ID
    private string TAG = "IM";

    protected bool matches = false;

    public List<GameObject> memories = new List<GameObject>();
    public List<EventIM> events = new List<EventIM>();
    public List<EventIM> convoEvents = new List<EventIM>();

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
    protected JumpManager jm;
    protected ConversationManager cm;

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
        foreach (Transform child in transform)
            events.Add(child.GetComponentInChildren<EventIM>());

        jm = new JumpManager(events); //fixme: this JM handles next eventID value
    }

    private void Update() {
        EventIM e = events.ElementAt(eventID);
        if ( eventInBounds() && canStartEvent(e) ) {
            if (!cm.activateConversation || cm.inConversation())
                RunGame(e);
            else
                conversationCheck(); //If we are also not in a current conversation else runGame (Normally)
        }
        if ( e.isDone() ) //if event finished in this frame
            done(e);
    }

    public void RunGame(EventIM e) {
        if ( cm.activateConversation )
            e = convoEvents.ElementAt(eventID);
        if ( e.name!="Trigger" )
            sm = e.agent.GetComponent<AgentStatusManager>();
        matches = getState(e);
        if ( ( matches && !speaking() ) || e.name == "Trigger" ) {
            memories.Add(eventIndex);
            Debug.Log(TAG + " Playing event " + e.name + " at index " + eventID);
            switch (e.name) {
                case "Dialog":
                    dm = e.agent.GetComponent<DialogManager>();
                    dm.Speak(e.GetComponent<Dialog>());
                    break;
                case "Animation":  //Animation needs to be fixed.. dialog will not play if 105 conditions not met but animation will play..
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

    private bool speaking() {
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
        //eventID++;
        eventID = jm.getNextEventIndex(e);
    }

    public void conversationCheck() {
        foreach (EventIM e in events) {
            /** Note: In editor, Conversation script needs to be first in order
             * for IM to refrence conversation agent and not the agent in EventIM **/
            if (e.hasStarted() && !e.isDone()) //waiting
                return;
            if (e.name != "Trigger")
                sm = e.agent.GetComponent<AgentStatusManager>();
            matches = getState(e); //check the conversation state
            if ((matches && !speaking()) || e.name == "Trigger") //check if anybody is speaking in the scene
            {
                //memories.Add(eventIndex);
                Debug.Log(TAG + " Conversation playing: " + e.name);
                /**if its a conversation then play the first event of that conversation and continue with rungame? 
                 * (w/ the rest of the conversation events while conversation state parameters still true).
                 * conversationCheck will always be checking if any other event has been triggered.. **/
                convoEvents = cm.grabConversationEvents(e);
                e.start();
                RunGame(e);
            }
        }
    }

    private bool eventInBounds() {
        return ( eventID<events.Count && eventID>=0 );
    }

    private bool canStartEvent(EventIM e) {
        foreach ( EventIM t in events )
            if ( t.hasStarted() && !t.isDone() )
                return false; //some event is in progress
        return (!sm.isWaiting() && !e.hasStarted() && !e.isDone()); //event hasn't yet started
    }

    private bool eventIsConversational(EventIM e) { 
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
