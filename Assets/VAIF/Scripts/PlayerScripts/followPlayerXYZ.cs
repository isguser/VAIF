using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class followPlayerXYZ : MonoBehaviour {

    public GameObject camera_Eye;
    private Vector3 followRotation = new Vector3();

    // Used for player colliders: Only tracks positional XYZ and rotational Y
    void Start () {
        transform.position = camera_Eye.transform.position;
        followRotation.y = camera_Eye.transform.eulerAngles.y + 90;
        transform.eulerAngles = followRotation;
    }
	void Update () {
        transform.position = camera_Eye.transform.position;
        followRotation.y = camera_Eye.transform.eulerAngles.y + 90;
        transform.eulerAngles = followRotation;
    }
}
