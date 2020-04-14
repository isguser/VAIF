using UnityEngine;

public class Follow_Camera_Eye : MonoBehaviour {

    public GameObject camera_Eye;
    private Vector3 followRotation = new Vector3();

	// Use this for initialization
	void Start () {
        transform.position = camera_Eye.transform.position;
        followRotation.x = (camera_Eye.transform.eulerAngles.z-(camera_Eye.transform.eulerAngles.z*2));
        followRotation.y = camera_Eye.transform.eulerAngles.y+90; 
        followRotation.z = camera_Eye.transform.eulerAngles.x;
        transform.eulerAngles = followRotation;
    }
	
	// Update is called once per frame
	void Update () {
        transform.position = camera_Eye.transform.position;
        followRotation.x = (camera_Eye.transform.eulerAngles.z - (camera_Eye.transform.eulerAngles.z * 2));
        followRotation.y = camera_Eye.transform.eulerAngles.y + 90;
        followRotation.z = camera_Eye.transform.eulerAngles.x;
        transform.eulerAngles = followRotation;
    }
}
