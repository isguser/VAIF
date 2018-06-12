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
    [Tooltip("Mandatory. Drag and drop the AgentStatusManager for each of the Agents to this list.")]
    public List<AgentStatusManager> agents = new List<AgentStatusManager>();
    private List<GameObject> memories = new List<GameObject>();
    
    private Conversation currConv;
    private EventIM currEvent;
    private string TAG = "IM ";
    
    protected DialogManager dm;
    protected AnimationManager am;
    protected ResponseManager rm;
    protected WildcardManager wm;
    protected TriggerManager tm;
    protected WaitManager wwm;
    protected MoveManager mm;
    protected EmoteManager em;
    protected MemoryCheckManager mcm;
    protected ConversationIM cm;

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
        cm = new ConversationIM();

        rm = gameObject.GetComponent<ResponseManager>();
        wm = gameObject.GetComponent<WildcardManager>();
        tm = gameObject.GetComponent<TriggerManager>();
        wwm = gameObject.GetComponent<WaitManager>();
        mcm = gameObject.GetComponent<MemoryCheckManager>();
        cm = gameObject.GetComponent<ConversationIM>();
    }

    /* Called once on every frame */
    private void Update() {
        //is another event running?
        if ( needToWait(currEvent) )
            return;
        foreach ( Conversation c in cm.conversation ) {
            //is this Conversation complete OR are we not in this c's bounds?
            if ( c.isDone() || !inConversation(c) )
                continue;
            if ( currConv!=null && currConv!=c ) {
                currConv.changedConversations();
                c.changedConversations();
            }
            EventIM e = c.getNextEvent();
            //can we run this Conversation?
            if ( !getState(e) )
                continue;
            //move to the next Conversation
            RunGame(c, e);
            done(c, e);
            return; //don't check next conversation(s)
        }
    }

    /* Perform the unique actions for each event to run */
    public void RunGame(Conversation c, EventIM e) {
        if ( e.name!="Trigger" ) {
            //memories.Add(eventIndex);
            Debug.Log(TAG + "Playing Conversation/Event: " + c.name + "/" + e.name);
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
                    if ( wm!=null || wm.isRunning() )
                        wm.stop();
                    rm.Respond(e.GetComponent<Response>());
                    break;
                case "Jump":
                    //TODO jump between conversations?
                    break;
                case "Wildcard":
                    if ( rm.isRunning() )
                        rm.stopKeywordRecognizer();
                    if ( wm!=null || wm.isRunning() )
                        wm.stop();
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
        }
    }

    /* ********** Accessors: State Changes ********** */
    /* What is the current state to START the event? */
    private bool getState(EventIM e) {
        EventSettingValue esv = new EventSettingValue();
        AgentStatusManager agent = e.agent.GetComponent<AgentStatusManager>();
        //grabbing author choices from UNITY
        esv.setWantLookedAt(e.wantLookedAt);
        esv.setWantInRange(e.wantInRange);
        //storing the current state of the user
        esv.setCurrPhysical(agent.getPhysicalState());
        return esv.checkStateMatch();
    }

    /* What is the current state to START the conversation?  */
    private bool inConversation(Conversation c) {
        EventSettingValue esv = new EventSettingValue();
        AgentStatusManager agent = c.getAgent();
        esv.setWantLookedAt(c.getWantLookedAt());
        esv.setWantInRange(c.getWantInRange());
        esv.setCurrPhysical(agent.getPhysicalState());
        return esv.checkStateMatch();
    }

    /* ********** Mutators: Event Sequence Behavior ********** */
    /* Save this conversation/event as the currently played one. */
    private void done(Conversation c, EventIM e) {
        currConv = c;
        currEvent = e;
    }

    /* Does this event need to wait before we can run it? */
    private bool needToWait(EventIM e) {
        //is this the first event?
        if ( e==null )
            return false;
        //Debug.Log(TAG + e.IDescription + " is not null");
        //this is a Resp/WC, and we need it to finish first (recognize to know what's next)
        if ( eventNeedsResponse(e) && e.isActive() )
            return true;
        //Debug.Log(TAG + e.IDescription + " is not a resp/wc");
        if ( !bothAreConversational(e) ) {
            currEvent = currConv.getNextEvent(); //to run concurrently
            e = currEvent;
            //Debug.Log(TAG + currEvent.name + " should play now");
        }
        //Debug.Log(TAG + e.IDescription + e.hasStarted() + e.isDone());
        return e.isActive();
    }

    /* Is any agent waiting on a verbal event? */
    private bool waitingOnVerbalEvent()
    {
        //check if any agent is speaking
        foreach (AgentStatusManager a in agents)
            if (a.isSpeaking() || a.isListening())
                return true;
        return false;
    }

    /* Is this event and the currently playing event of a conversational type? */
    private bool bothAreConversational(EventIM e) {
        if (isConversational(e) && isConversational(currEvent))
            return true;
        return false;
    }

    /* Is this event a Dialog/Response/Wildcard? */
    private bool isConversational(EventIM e)
    {
        switch (e.name) {
            case "Dialog":
            case "Response":
            case "Wildcard":
                return true;
            default:
                return false;
        }
    }

    /* Does this event require a response (is it a Response/Wildcard)? */
    private bool eventNeedsResponse(EventIM e) {
        //of what type is the conversation?
        switch (e.name) {
            case "Response":
            case "Wildcard":
                return true;
            default:
                return false;
        }
    }
}
