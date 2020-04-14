using UnityEngine;
using System.Collections.Generic;


public class InAgentRange : MonoBehaviour
{
    public float timeInRange;
    protected GameObject PlayerCollider;
    string tagged = "Player";

    /* What to do once we enter an agent's collider? */
    void OnTriggerEnter(Collider other)
    {
        PlayerCollider = other.gameObject;
        //Debug.Log("have: " + PlayerCollider.tag + " but want: " + tagged);
        if (PlayerCollider.tag == tagged)
        {
            Invoke("InCharacterRange", timeInRange);
        }
    }

    /* What to do when we leave an agent's collider? */
    void OnTriggerExit(Collider other)
    {
        PlayerCollider = other.gameObject;
        if (PlayerCollider.tag == tagged && this.gameObject.GetComponentInParent<AgentStatusManager>().name==this.transform.parent.name)
        {
            this.gameObject.GetComponentInParent<AgentStatusManager>().stopInRange();
            //Debug.Log("not in range of: " + this.gameObject.GetComponentInParent<AgentStatusManager>().name);
            CancelInvoke("InCharacterRange");
        }
    }

    /* What to do when we are near an agent's collider? */
    void InCharacterRange()
    {
        if (this.gameObject.GetComponentInParent<AgentStatusManager>().name == this.transform.parent.name)
        {
            this.gameObject.GetComponentInParent<AgentStatusManager>().startInRange();
            //Debug.Log("in range of: " + this.gameObject.GetComponentInParent<AgentStatusManager>().name);
        }
    }
}
