using UnityEngine;
using System.Collections;

/**
 * 
 * **/

public class GazeManager : MonoBehaviour
{
    public AgentStatusManager manager;
    //public Transform player;
    public Vector3 agentPosition;
    public Vector3[] coordinates;     //coordinates of where we want an agent to be looking at 
    public Transform[] targets;         //other targets for the agent to look at
    public EyeMovement[] eyes;
    public EyeMovement LeftEye;
    public EyeMovement RightEye;
    bool lookingAtTarget = true;
    // Use this for initialization
    void Start()
    {
        coordinates = new Vector3[4];
        populateRandGazeCoord();
        eyes = new EyeMovement[2];
        manager = GetComponent<AgentStatusManager>();
        eyes = GetComponentsInChildren<EyeMovement>(); //get the eyes of this agent 
        if (eyes[0].gameObject.name.Contains("Left"))
        {
            LeftEye = eyes[0];
            RightEye = eyes[1];
        }
        else
        {
            LeftEye = eyes[1];
            RightEye = eyes[0];
        }
        //LeftEye.gameObject.transform.eulerAngles = new Vector3(LeftEye.gameObject.transform.eulerAngles.x + -2, LeftEye.gameObject.transform.eulerAngles.y + 9, LeftEye.gameObject.transform.eulerAngles.z);
        //RightEye.gameObject.transform.eulerAngles = new Vector3(RightEye.gameObject.transform.eulerAngles.x + -2, RightEye.gameObject.transform.eulerAngles.y + 9, RightEye.gameObject.transform.eulerAngles.z);

    }

    // Update is called once per frame
    void LateUpdate()
    {
        
        //Random gaze when speaking 
        if (manager.isSpeaking() && lookingAtTarget)
        {
            int lookindex = Random.Range(0, 3);
            LeftEye.transform.eulerAngles = coordinates[lookindex];
            RightEye.transform.eulerAngles = coordinates[lookindex];
            Invoke("LookBackAtTarget", 2f);
            lookingAtTarget = false;
        }

        if (!manager.isSpeaking() && lookingAtTarget)
        {
            LookBackAtTarget();
        }
        

    }

    void LookBackAtTarget()
    {
        LeftEye.target = targets[0];
        RightEye.target = targets[0];
        lookingAtTarget = true;
    }

    void populateRandGazeCoord()
    {
        agentPosition = this.transform.position;
        //top left
        coordinates[0] = new Vector3(-2, 9, 0);
        //top right 
        coordinates[1] = new Vector3(-2, -9, 0);
        //bottom left
        coordinates[2] = new Vector3(2, 9, 0);
        //bottom right 
        coordinates[3] = new Vector3(2, -9, 0);
    }
}