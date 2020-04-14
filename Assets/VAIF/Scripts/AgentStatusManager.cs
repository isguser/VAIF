using UnityEngine;
using System.Collections.Generic;

public class AgentStatusManager : MonoBehaviour {
    [Tooltip("No action needed. Checked if this agent is speaking.")]
    public bool speaking;
    [Tooltip("No action needed. Checked if this agent is listening (expecting a response from the user).")]
    public bool listening;
    [Tooltip("No action needed. Checked if this agent is waiting on some action/event.")]
    public bool waiting;
    [Tooltip("No action needed. Checked if the user is nearby this agent.")]
    public bool inRange;
    [Tooltip("No action needed. Checked if the agent is performing a Moving event.")]
    public bool moving;
    [Tooltip("No action needed. Checked if the user is looking at this agent.")]
    public bool lookedAt;
    [Tooltip("Mandatory (if using Text-to-Speech (TTS)). Un-check this box if this agent is a male." +
        "\nThis helps keep the TTS engine voice(s) to match the agent's gender." +
        "\nChecked by default.")]
    public bool isFemale = true;
    private int properties = 6;
    private bool[] state;// = new bool[properties];
    private string[] stateName;// = new string[properties];
    private string LISTEN = "listen";
    private string SPEAK = "speak";
    private string WAIT = "wait";
    private string INRANGE = "inRange";
    private string MOVE = "move";
    private string LOOKAT = "lookAt";

    private string TAG = "ASM ";

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

    public void startInRange() {
        this.state[stateOf(INRANGE)] = true;
        this.inRange = true;
    }

    public void startLookedAt()
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

    public void stopInRange()
    {
        state[stateOf(INRANGE)] = false;
        inRange = false;
    }

    public void stopLookedAt()
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

    public bool[] getPhysicalState()
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