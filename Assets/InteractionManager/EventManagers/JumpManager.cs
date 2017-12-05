using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpManager : MonoBehaviour {
    public bool[] Event;
    public GameObject[] trigger;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public JumpManager(int size)
    {
        Event = new bool[size];
        trigger = new GameObject[size];
    }
}
