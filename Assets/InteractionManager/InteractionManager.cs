using System.Collections.Generic;
using UnityEngine;
using System.Linq;

/*
 * -------------------------------------------------------------------------------------------------------------------------------
 * States: dialog, animations, responses, triggers, gaze, emotions, dialog memory, environmental memory, agent memory, permamnent memory, lookat
 * TODO dialog memory: remembers where the player left off with the agent
 * TODO environmental memory: agents can remember what they saw or heard you do
 * TODO update the dictation to store what the player said
 * TODO permamnent memory (across scenes and sessions)
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
    public bool isSpeaking = false;
    public bool isListening = false;
    public bool isWaiting = false;
    public bool isInRange = false;
    protected bool start = true;

    public List<int> memories = new List<int>();
    public List<EventIM> events = new List<EventIM>();

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

        rm = gameObject.GetComponent<ResponseManager>();
        wm = gameObject.GetComponent<WildcardManager>();
        tm = gameObject.GetComponent<TriggerManager>();
        wwm = gameObject.GetComponent<WaitManager>();
        mcm = gameObject.GetComponent<MemoryCheckManager>();
        foreach (Transform child in transform)
        {
            events.Add(child.GetComponentInChildren<EventIM>());
        }
        start = true;
    }

    private void Update()
    {
        // Need more logic
        //if (!isSpeaking && !isListening && !isWaiting && eventIndex < events.Count && start)
        if (eventIndex < events.Count && start && !isWaiting)
        {
            RunGame();
        }
    }

    public void RunGame()
    {
        EventIM e = events.ElementAt(eventIndex);
        if(e.name != "Trigger")
        {
            sm = e.agent.GetComponent<AgentStatusManager>();
        }
        if ((sm.isInRange && sm.isLookedAt && !sm.isSpeaking) || e.name == "Trigger")
        {
            memories.Add(eventIndex);
            Debug.Log("Event index playing..." + eventIndex);
            switch (e.name)
            {
                case "Dialog":
                    dm = e.agent.GetComponent<DialogManager>();
                    dm.Speak(e.GetComponent<Dialog>());
                    break;
                case "Animation":
                    am = e.agent.GetComponent<AnimationManager>();
                    am.PlayAnimation(e.GetComponent<Animate>());
                    break;
                case "Response":
                    rm.Respond(e.GetComponent<Response>());
                    break;
                case "Jump":
                    eventIndex = e.GetComponent<Jump>().jumpID - 1;
                    break;
                case "Wildcard":
                    wm.Wildcard(e.GetComponent<Wildcard>());
                    break;
                case "Trigger":
                    tm.Trigger(e.GetComponent<Trigger>());
                    break;
                case "Wait":
                    wwm.Waiting(e.GetComponent<Wait>().waitTime);
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
            eventIndex++;
        }
    }
}
