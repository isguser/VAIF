using UnityEngine;

public class InAgentRange : MonoBehaviour
{
    public float timeInRange;
    protected GameObject target;
    string tag = "AgentRange";

    // Use this for initialization
    void Start ()
    {
		
	}

    void OnTriggerEnter(Collider other)
    {
        target = other.gameObject;
        if (target.tag == tag)
        {
            Invoke("InCharacterRange", timeInRange);
        }
    }

    void OnTriggerExit(Collider other)
    {
        target = other.gameObject;
        if (target.tag == tag)
        {
            target.GetComponentInParent<AgentStatusManager>().isInRange = false;
            CancelInvoke("InCharacterRange");
        }
    }

    void InCharacterRange()
    {
        target.GetComponentInParent<AgentStatusManager>().isInRange = true;
    }
}
