using UnityEngine;

public class MoveManager : MonoBehaviour
{
    Move move = new Move();
    protected bool moving = false;

    protected Vector3 moveTarget;

    public void StartMoving(Move target)
    {
        move = target;
        moveTarget = move.target.transform.position;
        Debug.Log(moveTarget);
        moving = true;
    }

    public void Stop()
    {
        moving = false;
    }

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
