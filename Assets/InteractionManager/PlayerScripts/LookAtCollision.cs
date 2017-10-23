using UnityEngine;

public class LookAtCollision : MonoBehaviour {

    public float time_on_target;
    protected GameObject target;
    string tag = "Agent";

    // Use this for initialization
    void Start ()
    {
		
	}
	
    void OnTriggerEnter(Collider other)
    {
        target = other.gameObject;
        if (target.tag == tag)
        {
            Invoke("LookedAt", time_on_target);
        }
    }

    void OnTriggerExit(Collider other)
    {
        target = other.gameObject;
        if (target.tag == tag)
        {
            target.GetComponent<AgentStatusManager>().isLookedAt = false;
            CancelInvoke("LookedAt");
        }
    }

    void LookedAt()
    {
        target.GetComponent<AgentStatusManager>().isLookedAt = true;
    }
}
