using UnityEngine;

public class MoveManager : MonoBehaviour
{
    Move move = new Move();
    protected bool moving = false;

    protected Vector3 moveTarget;

    /* Start moving the agent. */
    public void StartMoving(Move target)
    {
        move = target;
        moveTarget = move.target.transform.position;
        Debug.Log(moveTarget);
        moving = true;
        target.finish();
    }

    /* Stop the agent's motion. */
    public void Stop()
    {
        moving = false;
        move.finish();
    }

    /* Called on every frame */
    private void Update()
    {
        if (move.follow && moving)
        {
            move.agent.transform.position = Vector3.MoveTowards(move.agent.transform.position, move.target.transform.position, move.speed);
        }
        else if (!move.follow && moving)
        {
            Debug.Log(moveTarget);
            move.agent.transform.position = Vector3.MoveTowards(move.agent.transform.position, moveTarget, move.speed);
        }
    }
}
