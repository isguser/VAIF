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
    public List<AgentStatusManager> agents = new List<AgentStatusManager>();
    private List<GameObject> memories = new List<GameObject>();
    public List<EventIM> events = new List<EventIM>();

    private EventIM lastPlayed;
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
        cm = new ConversationManager();

        rm = gameObject.GetComponent<ResponseManager>();
        wm = gameObject.GetComponent<WildcardManager>();
        tm = gameObject.GetComponent<TriggerManager>();
        wwm = gameObject.GetComponent<WaitManager>();
        mcm = gameObject.GetComponent<MemoryCheckManager>();
        cm = gameObject.GetComponent<ConversationManager>();
    }

    private void Update() {
        //if we're waiting on a response
        if ( lastPlayed!=null && eventNeedsResponse(lastPlayed) && lastPlayed.nextEvent==null )
            return;
        //is another event running?
        if ( lastPlayed!=null && !lastPlayed.isDone() )
            return; //wait til the next frame
        EventIM e;
        //find the first Conversation to play this game
        foreach ( Conversation c in cm.conversations ) {
            //is the user in this Conversation?
            if ( !getState(c) )
                continue;
            //is this Conversation complete?
            if ( c.isOver() ) {
                Debug.Log(TAG + c.name + "is over");
                c.finish();
                continue;
            }
            //get the next event to run in this Conversation
            e = nextEvent(c);
            //is another event running?
            if ( !canStartEvent(e) )
                return;
            //if the conditions match, run this Conversation
            if ( getState(e) ) {
                c.start();
                lastPlayed = e; //this event is the most recently played
                //move to the next Conversation
                RunGame(c,e);
                return;
            }
            //is the event done?
            if ( e.isDone() ) {
                if ( e.name=="Response" )
                    c.eventIsDone(e);
                done(e);
            }
        }
    }

    public void RunGame(Conversation c, EventIM e) {
        if ( ( getState(e) && !waitingOnVerbalEvent() ) || e.name == "Trigger") {
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

    /* ********** Accessors: State Changes ********** */
    private bool getState(EventIM e) {
        EventSettingValue esv = new EventSettingValue();
        AgentStatusManager tmp = e.agent.GetComponent<AgentStatusManager>();
        //grabbing author choices from UNITY
        esv.setWantLookedAt(e.wantLookedAt);
        esv.setWantLookedAt(e.wantInRange);
        //storing the current state of the user
        esv.setCurrVerbals(tmp.getVerbalState());
        esv.setCurrPhysical(tmp.getPhysicalState());
        return esv.checkStateMatch();
    }

    private bool getState(Conversation c) {
        ConversationSettingValue csv = new ConversationSettingValue(c.agents.Count);
        //start Conv c if: we're inRange of ANY agent AND we're lookAt ANY agent
        csv.setWantLookedAt(c.wantLookedAt);
        csv.setWantInRange(c.wantInRange);
        csv.setCurrPhysical(c.agents);
        return csv.checkStateMatch();
    }

    private bool waitingOnVerbalEvent() {
        //check if any agent is speaking
        foreach (AgentStatusManager a in agents)
        {
            //Debug.Log(TAG + " " + a.name + " lookedat/inrange: " + a.isLookedAt() + a.isInRange());
            if (a.isSpeaking() || a.isListening())
                return true;
        }
        return false;
    }

    /* ********** Mutators: Event Sequence Behavior ********** */
    private void done(EventIM e)
    {
        if (e.nextEvent != null && e.nextEvent.name == "Response")
            return; //wait until the response is complete to mark this event as done
        Debug.Log(TAG + "Finished Event " + e.name);
        e.finish();
    }

    private bool canStartEvent(EventIM e) {
        //has e started, or can it?
        return ( !e.hasStarted() && !e.isDone() );
    }

    private EventIM nextEvent(Conversation c)
    {
        //start if we have not played any yet
        if (lastPlayed == null)
            return c.getFirstUnfinishedEvent();
        //if the event is conversational, it had multiple nextEvent possibilities
        if ( eventNeedsResponse(lastPlayed) )
            return lastPlayed.nextEvent.GetComponent<EventIM>();
        return c.findNextEvent(lastPlayed);
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

    private bool eventNeedsResponse(EventIM e)
    {
        //of what type is the conversation?
        switch (e.name)
        {
            case "Response":
            case "Wildcard":
                return true;
            default:
                return false;
        }
    }
}
