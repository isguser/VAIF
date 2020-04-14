using UnityEngine;
using System.Collections;

public class EyeMovement : MonoBehaviour
{
    public Transform target;
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(target.position);
    }

    void rotateEye(Vector3 x, Vector3 y)
    {
        //Quaternion target = Quaternion.Euler(x, y, 0);
    }
}