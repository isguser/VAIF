using UnityEngine;

public class EventIM : MonoBehaviour
{

    [Tooltip("Default. The nextEvent is how you can access different events through Jump events in the Interaction Manager. It is also" +
        "used in timeouts and parameters as jumps from other events.")]
    public GameObject nextEvent;
    [Tooltip("Optional. The IDescription is how you can describe your events with a simple description ID. EX: Dialog1")]
    public string IDescription;
    public GameObject agent;
    [Tooltip("Required. Do you want the user to be in range to activate this event? DONTCARE allows this event to begin without necessity.")]
    public EventSetting wantInRange;
    [Tooltip("Required. Do you want the user to look at the agent to activate this event? DONTCARE allows this event to begin without necessity.")]
    public EventSetting wantLookedAt;
    public enum EventSetting
    {
        DONTCARE, //can be T/F
        TRUE,
        FALSE
    }
    protected EventSetting eventSetting;
    public enum EventType
    {
        Dialog,
        Animation,
        Response,
        Wildcard,
        Trigger,
        Gaze,
        Jump,
        Wait,
        RotateTo,
        Emote,
        EmotionCheck,
        MemoryCheck,
        Move,
        StopMoving
    }
    protected EventType eventType;
    public bool started = false;
    public bool finished = false;

    public void AddEvent(string type) {
        switch (type)
        {
            case "Dialog":
                GameObject dialog = new GameObject();
                dialog.name = "Dialog";
                dialog.AddComponent<Dialog>();
                dialog.GetComponent<Dialog>().agent = agent;
                dialog.GetComponent<Dialog>().nextEvent = nextEvent;
                dialog.GetComponent<Dialog>().wantInRange = wantInRange;
                eventType = EventType.Dialog;
                SetParent(dialog);
                break;
            case "Animation":
                GameObject animation = new GameObject();
                animation.name = "Animation";
                animation.AddComponent<Animate>();
                animation.GetComponent<Animate>().agent = agent;
                animation.GetComponent<Animate>().nextEvent = nextEvent;
                SetParent(animation);
                eventType = EventType.Animation;
                break;
            case "Response":
                GameObject response = new GameObject();
                response.name = "Response";
                response.AddComponent<Response>();
                response.GetComponent<Response>().agent = agent;
                response.GetComponent<Response>().nextEvent = nextEvent;
                SetParent(response);
                eventType = EventType.Response;
                break;
            case "Wildcard":
                GameObject wildcard = new GameObject();
                wildcard.name = "Wildcard";
                wildcard.AddComponent<Wildcard>();
                wildcard.GetComponent<Wildcard>().agent = agent;
                wildcard.GetComponent<Wildcard>().nextEvent = nextEvent;
                SetParent(wildcard);
                eventType = EventType.Wildcard;
                break;
            case "Trigger":
                GameObject trigger = new GameObject();
                trigger.name = "Trigger";
                trigger.AddComponent<Trigger>();
                //trigger.GetComponent<Trigger>().agent = agent;
                trigger.GetComponent<Trigger>().nextEvent = nextEvent;
                SetParent(trigger);
                eventType = EventType.Trigger;
                break;
            case "Gaze":
                GameObject gaze = new GameObject();
                gaze.name = "Gaze";
                gaze.AddComponent<Gaze>();
                gaze.GetComponent<Gaze>().agent = agent;
                gaze.GetComponent<Gaze>().nextEvent = nextEvent;
                SetParent(gaze);
                eventType = EventType.Gaze;
                break;
            case "Emote":
                GameObject emote = new GameObject();
                emote.name = "Emote";
                emote.AddComponent<Emote>();
                emote.GetComponent<Emote>().agent = agent;
                emote.GetComponent<Emote>().nextEvent = nextEvent;
                SetParent(emote);
                eventType = EventType.Emote;
                break;
            case "Expression":
                GameObject expression = new GameObject();
                expression.name = "Expression";
                expression.AddComponent<Expression>();
                expression.GetComponent<Expression>().agent = agent;
                expression.GetComponent<Expression>().nextEvent = nextEvent;
                SetParent(expression);
                eventType = EventType.Emote; //??
                break;
            case "Jump":
                GameObject jump = new GameObject();
                jump.name = "Jump";
                jump.AddComponent<Jump>();
                jump.GetComponent<Jump>().agent = agent;
                jump.GetComponent<Jump>().nextEvent = nextEvent;
                SetParent(jump);
                eventType = EventType.Jump;
                break;
            case "Wait":
                GameObject wait = new GameObject();
                wait.name = "Wait";
                wait.AddComponent<Wait>();
                wait.GetComponent<Wait>().agent = agent;
                wait.GetComponent<Wait>().nextEvent = nextEvent;
                SetParent(wait);
                eventType = EventType.Wait;
                break;
            case "MemoryCheck":
                GameObject memoryCheck = new GameObject();
                memoryCheck.name = "MemoryCheck";
                memoryCheck.AddComponent<MemoryCheck>();
                memoryCheck.GetComponent<MemoryCheck>().agent = agent;
                memoryCheck.GetComponent<MemoryCheck>().nextEvent = nextEvent;
                SetParent(memoryCheck);
                eventType = EventType.MemoryCheck;
                break;
            case "EmotionCheck":
                GameObject emotionCheck = new GameObject();
                emotionCheck.name = "EmotionCheck";
                emotionCheck.AddComponent<EmotionCheck>();
                emotionCheck.GetComponent<EmotionCheck>().agent = agent;
                emotionCheck.GetComponent<EmotionCheck>().nextEvent = nextEvent;
                SetParent(emotionCheck);
                eventType = EventType.EmotionCheck;
                break;
            case "RotateTo":
                GameObject rotateTo = new GameObject();
                rotateTo.name = "RotateTo";
                rotateTo.AddComponent<RotateTo>();
                rotateTo.GetComponent<RotateTo>().agent = agent;
                rotateTo.GetComponent<RotateTo>().nextEvent = nextEvent;
                SetParent(rotateTo);
                eventType = EventType.RotateTo;
                break;
            case "Move":
                GameObject move = new GameObject();
                move.name = "Move";
                move.AddComponent<Move>();
                move.GetComponent<Move>().agent = agent;
                move.GetComponent<Move>().nextEvent = nextEvent;
                SetParent(move);
                eventType = EventType.Move;
                break;
            case "StopMoving":
                GameObject stopMoving = new GameObject();
                stopMoving.name = "StopMoving";
                stopMoving.AddComponent<StopMoving>();
                stopMoving.GetComponent<StopMoving>().agent = agent;
                stopMoving.GetComponent<StopMoving>().nextEvent = nextEvent;
                SetParent(stopMoving);
                eventType = EventType.StopMoving;
                break;
        }
    }

    //Invoked when a new event is created.
    public void SetParent(GameObject eventInstance)
    {
        //Makes the GameObject "newParent" the parent of the GameObject.
        eventInstance.transform.parent = gameObject.transform;
    }

    public void start()
    {
        started = true;
    }
    public bool hasStarted()
    {
        return started;
    }
    public void finish()
    {
        finished = true;
    }
    public bool isDone()
    {
        return finished;
    }

    public EventType getType()
    {
        return eventType;
    }
}
