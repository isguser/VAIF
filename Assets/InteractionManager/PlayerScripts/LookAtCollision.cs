using UnityEngine;
using System.Collections.Generic;


public class LookAtCollision : MonoBehaviour
{
    public float time_on_target;
    protected GameObject camera_LookAt;
    string tagged = "Camera_LookAt";

    /* What to do when we enter an agent's collider? */
    void OnTriggerEnter(Collider other)
    {
        camera_LookAt = other.gameObject; //this is the VR camera

        if (camera_LookAt.tag == tagged)
        {
            Invoke("LookedAt", time_on_target);
        }
    }

    /* What to do when stop looking at an agent? */
    void OnTriggerExit(Collider other)
    {
        camera_LookAt = other.gameObject;
        if (camera_LookAt.tag == tagged && this.gameObject.GetComponent<AgentStatusManager>().name == this.gameObject.name )
        {
            this.gameObject.GetComponent<AgentStatusManager>().stopLookedAt();
            //Debug.Log("not looking at: " + this.gameObject.GetComponent<AgentStatusManager>().name);
            CancelInvoke("LookedAt");
        }
    }

    /* What to do when we are looking at an agent? */
    void LookedAt()
    {
        if (this.gameObject.GetComponent<AgentStatusManager>().name == this.gameObject.name)
        {
            this.gameObject.GetComponent<AgentStatusManager>().startLookedAt();
            //Debug.Log("looking at: " + this.gameObject.GetComponent<AgentStatusManager>().name);
        }
    }
}
