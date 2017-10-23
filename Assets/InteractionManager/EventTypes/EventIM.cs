using UnityEngine;

public class EventIM : MonoBehaviour {

    [Tooltip("Default. The EventID is how you can access different events through Jump events in the Interaction Manager. It is also" +
        "used in timeouts and parameters as jumps from other events. Generated as sequential integers by default.")]
    public int eventID;
    public GameObject agent;
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

    public void AddEvent(string type)
    {

        switch (type)
        {
            case "Dialog":
                GameObject dialog = new GameObject();
                dialog.name = "Dialog";
                dialog.AddComponent<Dialog>();
                dialog.GetComponent<Dialog>().agent = agent;
                dialog.GetComponent<Dialog>().eventID = eventID;
                SetParent(dialog);
                break;
            case "Animation":
                GameObject animation = new GameObject();
                animation.name = "Animation";
                animation.AddComponent<Animate>();
                animation.GetComponent<Animate>().agent = agent;
                animation.GetComponent<Animate>().eventID = eventID;
                SetParent(animation);
                break;
            case "Response":
                GameObject response = new GameObject();
                response.name = "Response";
                response.AddComponent<Response>();
                response.GetComponent<Response>().agent = agent;
                response.GetComponent<Response>().eventID = eventID;
                SetParent(response);
                break;
            case "Wildcard":
                GameObject wildcard = new GameObject();
                wildcard.name = "Wildcard";
                wildcard.AddComponent<Wildcard>();
                wildcard.GetComponent<Wildcard>().agent = agent;
                wildcard.GetComponent<Wildcard>().eventID = eventID;
                SetParent(wildcard);
                break;
            case "Trigger":
                GameObject trigger = new GameObject();
                trigger.name = "Trigger";
                trigger.AddComponent<Trigger>();
                //trigger.GetComponent<Trigger>().agent = agent;
                trigger.GetComponent<Trigger>().eventID = eventID;
                SetParent(trigger);
                break;
            case "Gaze":
                GameObject gaze = new GameObject();
                gaze.name = "Gaze";
                gaze.AddComponent<Gaze>();
                gaze.GetComponent<Gaze>().agent = agent;
                gaze.GetComponent<Gaze>().eventID = eventID;
                SetParent(gaze);
                break;
            case "Emote":
                GameObject emote = new GameObject();
                emote.name = "Emote";
                emote.AddComponent<Emote>();
                emote.GetComponent<Emote>().agent = agent;
                emote.GetComponent<Emote>().eventID = eventID;
                SetParent(emote);
                break;
            case "Expression":
                GameObject expression = new GameObject();
                expression.name = "Expression";
                expression.AddComponent<Expression>();
                expression.GetComponent<Expression>().agent = agent;
                expression.GetComponent<Expression>().eventID = eventID;
                SetParent(expression);
                break;
            case "Jump":
                GameObject jump = new GameObject();
                jump.name = "Jump";
                jump.AddComponent<Jump>();
                jump.GetComponent<Jump>().agent = agent;
                jump.GetComponent<Jump>().eventID = eventID;
                SetParent(jump);
                break;
            case "Wait":
                GameObject wait = new GameObject();
                wait.name = "Wait";
                wait.AddComponent<Wait>();
                wait.GetComponent<Wait>().agent = agent;
                wait.GetComponent<Wait>().eventID = eventID;
                SetParent(wait);
                break;
            case "MemoryCheck":
                GameObject memoryCheck = new GameObject();
                memoryCheck.name = "MemoryCheck";
                memoryCheck.AddComponent<MemoryCheck>();
                memoryCheck.GetComponent<MemoryCheck>().agent = agent;
                memoryCheck.GetComponent<MemoryCheck>().eventID = eventID;
                SetParent(memoryCheck);
                break;
            case "EmotionCheck":
                GameObject emotionCheck = new GameObject();
                emotionCheck.name = "EmotionCheck";
                emotionCheck.AddComponent<EmotionCheck>();
                emotionCheck.GetComponent<EmotionCheck>().agent = agent;
                emotionCheck.GetComponent<EmotionCheck>().eventID = eventID;
                SetParent(emotionCheck);
                break;
            case "RotateTo":
                GameObject rotateTo = new GameObject();
                rotateTo.name = "RotateTo";
                rotateTo.AddComponent<RotateTo>();
                rotateTo.GetComponent<RotateTo>().agent = agent;
                rotateTo.GetComponent<RotateTo>().eventID = eventID;
                SetParent(rotateTo);
                break;
            case "Move":
                GameObject move = new GameObject();
                move.name = "Move";
                move.AddComponent<Move>();
                move.GetComponent<Move>().agent = agent;
                move.GetComponent<Move>().eventID = eventID;
                SetParent(move);
                break;
            case "StopMoving":
                GameObject stopMoving = new GameObject();
                stopMoving.name = "StopMoving";
                stopMoving.AddComponent<StopMoving>();
                stopMoving.GetComponent<StopMoving>().agent = agent;
                stopMoving.GetComponent<StopMoving>().eventID = eventID;
                SetParent(stopMoving);
                break;
        }
        eventID++;
    }

    //Invoked when a new event is created.
    public void SetParent(GameObject eventInstance)
    {
        //Makes the GameObject "newParent" the parent of the GameObject.
        eventInstance.transform.parent = gameObject.transform;
    }

}
