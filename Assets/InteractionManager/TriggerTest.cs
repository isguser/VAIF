using UnityEngine;

public class TriggerTest : MonoBehaviour
{
    //Every trigger method receives a string parameter array that needs to be typecast
    void changeColor(string[] parameters)
    {
        foreach (string p in parameters)
        {
            Debug.Log(p);
        }
        gameObject.GetComponent<BoxCollider>().enabled = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        // do some stuff
    }
}

