using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.ThirdPerson;

public class RotateManager : MonoBehaviour {
    RotateTo rotate = new RotateTo();
    [Tooltip("For testing purposes!")]
    protected bool rotating;
    protected ThirdPersonCharacter character;
    protected Vector3 direction;
    protected Quaternion lookRotation;
    protected float turnAmt;
    public GameObject player;
    public float speedToPlayer;
    [Tooltip("Activate if you would like the AGENT to turn as soon as the user walks inRange")]
    public bool rotateInRange;

    public void StartRotating (RotateTo target)
    {
        rotate = target;
        rotate.start();
        Debug.Log("Rotate Started!");
        character = target.agent.GetComponent<ThirdPersonCharacter>();
        rotating = true;
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (rotating)
        {
            Debug.Log("Rotation for agent is activated. Calling rotateToDestination");
            rotateToDestination(rotate);
        }
    }

    void rotateToDestination(RotateTo target)
    {
        Debug.Log(gameObject.name + " is rotating!");
        direction = (target.rotateToObject.transform.position - transform.position).normalized;
        direction.y = 0;
        lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));    // flattens the vector3
        turnAmt = character.Turn(direction, rotate.speed, false, false);
        if (turnAmt <= .01f)
        {
            rotating = false;
            rotate.finish();
        }
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * target.speed);
        Debug.Log("Finished Rotation: " + transform.rotation + " && " + target.rotateToObject.transform.rotation);
    }

    public void rotateToUser()
    {
        Debug.Log(gameObject.name + " is rotating TO USER!");
        direction = (player.transform.localPosition - transform.position).normalized;
        direction.y = 0;
        lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));    // flattens the vector3
        turnAmt = character.Turn(direction, speedToPlayer, false, false);
        if (turnAmt <= .01f)
            rotating = false;
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * speedToPlayer);
        Debug.Log("Finished Rotation: " + transform.rotation + " && " + player.transform.rotation);
    }
}