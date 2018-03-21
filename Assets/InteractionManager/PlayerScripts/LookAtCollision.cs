using UnityEngine;

public class LookAtCollision : MonoBehaviour
{
    public float time_on_target;
    protected GameObject camera_LookAt;
    string tagged = "Camera_LookAt";

    void OnTriggerEnter(Collider other)
    {
        camera_LookAt = other.gameObject;
        if (camera_LookAt.tag == tagged)
        {
            Invoke("LookedAt", time_on_target);
        }
    }

    void OnTriggerExit(Collider other)
    {
        camera_LookAt = other.gameObject;
        if (camera_LookAt.tag == tagged)
        {
            this.gameObject.GetComponent<AgentStatusManager>().notLookingAt();
            CancelInvoke("LookedAt");
        }
    }

    void LookedAt()
    {
        this.gameObject.GetComponent<AgentStatusManager>().lookingAt();
    }
}
