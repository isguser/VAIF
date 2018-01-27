using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpManager : MonoBehaviour {
    public GameObject[] trigger;
    public int[] jumpToEvent;

    public JumpManager(int size) {
        trigger = new GameObject[size]; //??
        jumpToEvent = new int[size];
        init();
    }

    private void init() {
        //each event jumps to next event by default
        foreach ( int i in jumpToEvent )
            jumpToEvent[i] = i+1;
    }

    public void grabJumps() {
        //TODO get from GUI
        //set Jump instance to started -- needed?
    }
}
