using UnityEngine;

public class FollowCamera : MonoBehaviour {

    public GameObject camera_Eye;
    private Vector3 followRotaion = new Vector3();

	// Use this for initialization
	void Start () {
        transform.position = camera_Eye.transform.position;
        followRotaion.x = (camera_Eye.transform.eulerAngles.z-(camera_Eye.transform.eulerAngles.z*2));
        followRotaion.y = camera_Eye.transform.eulerAngles.y;// +90; 
        followRotaion.z = camera_Eye.transform.eulerAngles.x;
        transform.eulerAngles = followRotaion;
    }
	
	// Update is called once per frame
	void Update () {
        transform.position = camera_Eye.transform.position;
        followRotaion.x = (camera_Eye.transform.eulerAngles.z - (camera_Eye.transform.eulerAngles.z * 2));
        followRotaion.y = camera_Eye.transform.eulerAngles.y;// + 90;
        followRotaion.z = camera_Eye.transform.eulerAngles.x;
        transform.eulerAngles = followRotaion;
    }
}
