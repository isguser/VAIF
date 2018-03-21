using UnityEngine;

public class AgentStatusManager : MonoBehaviour
{
    public bool speaking;
    public bool listening;
    public bool waiting;
    public bool inRange;
    public bool moving;
    public bool lookedAt;
    private static int properties = 6;
    private static bool[] state = new bool[properties];
    private static string[] stateName = new string[properties];
    private static string LISTEN = "listen";
    private static string SPEAK = "speak";
    private static string WAIT = "wait";
    private static string INRANGE = "inRange";
    private static string MOVE = "move";
    private static string LOOKAT = "lookAt";

    private string TAG = "ASM";

    // Use this for initialization
    void Start()
    {
        state = new bool[properties];
        stateName = new string[properties];
        setStates();
    }

    private void setStates()
    {
        for (int i = 0; i < state.Length; i++)
        {
            switch (i)
            {
                case 0:
                    stateName[i] = LISTEN;
                    break;
                case 1:
                    stateName[i] = SPEAK;
                    break;
                case 2:
                    stateName[i] = WAIT;
                    break;
                case 3:
                    stateName[i] = INRANGE;
                    break;
                case 4:
                    stateName[i] = MOVE;
                    break;
                case 5:
                    stateName[i] = LOOKAT;
                    break;
            }
            state[i] = false;
        }
    }

    // Update is called once per frame
    void Update() { }

    /** ********** Mutators : Begin a Behavior ********** */
    public void startListening()
    {
        state[stateOf(LISTEN)] = true;
        listening = true;
    }

    public void startSpeaking()
    {
        state[stateOf(SPEAK)] = true;
        speaking = true;
    }

    public void startWaiting()
    {
        state[stateOf(WAIT)] = true;
        waiting = true;
    }

    public void startMoving()
    {
        state[stateOf(MOVE)] = true;
        moving = true;
    }

    public void movedNearby()
    {
        state[stateOf(INRANGE)] = true;
        inRange = true;
    }

    public void lookingAt()
    {
        state[stateOf(LOOKAT)] = true;
        lookedAt = true;
    }

    /** ********** Mutators : End a Behavior ********** */
    public void stopListening()
    {
        state[stateOf(LISTEN)] = false;
        listening = false;
    }

    public void stopSpeaking()
    {
        state[stateOf(SPEAK)] = false;
        speaking = false;
    }

    public void stopWaiting()
    {
        state[stateOf(WAIT)] = false;
    }

    public void stopMoving()
    {
        state[stateOf(MOVE)] = false;
        moving = false;
    }

    public void movedAway()
    {
        state[stateOf(INRANGE)] = false;
        inRange = false;
    }

    public void notLookingAt()
    {
        state[stateOf(LOOKAT)] = false;
        lookedAt = false;
    }

    /** ********** Accessors ********** */
    public bool isSpeaking()
    {
        return state[stateOf(SPEAK)];
    }

    public bool isListening()
    {
        return state[stateOf(LISTEN)];
    }

    public bool isWaiting()
    {
        return state[stateOf(WAIT)];
    }

    public bool isMoving()
    {
        return state[stateOf(MOVE)];
    }

    public bool isInRange()
    {
        return state[stateOf(INRANGE)];
    }

    public bool isLookedAt()
    {
        return state[stateOf(LOOKAT)];
    }

    /** ********** Regarding the State as a Whole ********** */
    public int numberOfProperties()
    {
        return properties;
    }

    public bool[] currentState()
    {
        return new bool[] { state[stateOf(LOOKAT)], state[stateOf(INRANGE)] };
    }

    public bool[] getVerbalState()
    {
        return new bool[] { state[stateOf(SPEAK)], state[stateOf(LISTEN)] };
    }

    public int stateOf(string s)
    {
        for (int i = 0; i < stateName.Length; i++)
            if (stateName[i].Equals(s))
                return i;
        return -1;
    }

    public void eventFinished(string s)
    {
        switch (s)
        {
            case "Dialog":
                state[stateOf(SPEAK)] = false;
                break;
            case "Move":
            case "StopMoving":
                state[stateOf(MOVE)] = false;
                break;
            case "Wait":
                state[stateOf(WAIT)] = false;
                break;
            default:
                //Debug.Log(TAG + " Event is another type: " + s);
                break;
        }
    }
}