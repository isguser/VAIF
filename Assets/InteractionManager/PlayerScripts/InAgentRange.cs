using UnityEngine;

public class InAgentRange : MonoBehaviour
{
    public float timeInRange;
    protected GameObject PlayerCollider;
    string tagged = "Player";

    void OnTriggerEnter(Collider other)
    {
        PlayerCollider = other.gameObject;
        //Debug.Log(PlayerCollider.tag + " " + tagged);
        if (PlayerCollider.tag == tagged)
        {
            Invoke("InCharacterRange", timeInRange);
        }
    }

    void OnTriggerExit(Collider other)
    {
        PlayerCollider = other.gameObject;
        if (PlayerCollider.tag == tagged)
        {
            this.gameObject.GetComponentInParent<AgentStatusManager>().movedAway();
            CancelInvoke("InCharacterRange");
        }
    }

    void InCharacterRange()
    {
        this.gameObject.GetComponentInParent<AgentStatusManager>().movedNearby();
    }
}
