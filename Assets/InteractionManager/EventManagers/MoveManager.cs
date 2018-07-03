using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityStandardAssets.Characters.ThirdPerson;

public class MoveManager : MonoBehaviour
{
    Move move = new Move();
    protected bool moving = false;

    protected Vector3[] moveTarget;
    protected Vector3 curTarget;
    protected NavMeshAgent navAgent;
    protected ThirdPersonCharacter character;
    protected Animator animator;
    protected float dist;
    protected bool reachedDest;
    protected int destPoint = 0;

    /* Start moving the agent. */
    public void StartMoving(Move target)
    {
        move = target;
        move.start();
        navAgent = target.agent.GetComponent<NavMeshAgent>();
        character = target.agent.GetComponent<ThirdPersonCharacter>();
        animator = gameObject.GetComponent<Animator>();
        navAgent.speed = target.speed;
        getTransform(target);


        curTarget = moveTarget[destPoint];
        //Debug.Log("curTarget = " + moveTarget[destPoint].x + " at " + destPoint);
        navAgent.SetDestination(curTarget);
        moving = true;
    }

    /* Stop the agent's motion. */
    public void Stop()
    {
        moving = false;
        navAgent.isStopped = true;
        animator.SetFloat("Forward", 0.0f);
        reachedDest = true;
        move.finish();
    }

    /* Called on every frame */
    private void Update()
    {
        if (moving)
        {
            moveToDestination();
        }

    }

    /** Depending on the Stopping Distance of the NavMesh Agent, the character will stop from the target's radius. **/
    void moveToDestination()
    {
        float t = Mathf.Round(Mathf.Abs((navAgent.remainingDistance - navAgent.stoppingDistance)));
        //Debug.Log("destPoint = " + destPoint + " Difference : " + t + " RD : " + navAgent.remainingDistance);
        character.Move(navAgent.desiredVelocity, true, false, false);
        if (!navAgent.pathPending && t < 1f)
        {
            //Debug.Log("Path Status is complete for path to destPoint = " + destPoint);
            if (curTarget == moveTarget[moveTarget.Length - 1])
            {
                int temp = moveTarget.Length - 1;
                //Debug.Log("Just completed the last path. destpoint = " + destPoint + " and moveTarget.Length-1 = " + temp);
                Invoke("Stop", .5f);
            }
            else
            {
                //update current target
                destPoint++;
                //Debug.Log("Not final target! \n destPoint = " + destPoint);
                curTarget = moveTarget[destPoint];
                navAgent.SetDestination(curTarget);
                character.Move(navAgent.desiredVelocity, true, false, false);
            }
        }
        //keep moving
    }

    void getTransform(Move target)
    {
        int i = 0;
        //Debug.Log("Total amount of targets: " + move.target.Length);
        moveTarget = new Vector3[move.target.Length];
        foreach (Transform t in move.target) {
            moveTarget[i] = t.position;
            i++;
        }
        //Debug.Log("All targets loaded!");
    }
}
