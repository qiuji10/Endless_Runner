using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCore : MonoBehaviour
{
    public enum LaneState { Lane1, Lane2, Lane3 }

    [SerializeField] private float speed = 5f;
    private float newXPos;

    public LaneState laneState = LaneState.Lane2;

    private Vector3 goLeft = new Vector3(-0.8f, 0, 0);
    private Vector3 goRight = new Vector3(0.8f, 0, 0);

    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }    

    private void SwitchRightLane()
    {
        if (laneState == LaneState.Lane1)
        {
            laneState = LaneState.Lane2;
            newXPos = 0;
        }
        else if (laneState == LaneState.Lane2)
        {
            laneState = LaneState.Lane3;
            newXPos = 0.8f;
        }

        //transform.Translate(goRight * speed * Time.fixedDeltaTime);
        //rb.MovePosition(goRight * speed * Time.fixedDeltaTime);
    }

    private void SwitchLeftLane()
    {
        if (laneState == LaneState.Lane3)
        {
            laneState = LaneState.Lane2;
            newXPos = 0;
        }
        else if (laneState == LaneState.Lane2)
        {
            laneState = LaneState.Lane1;
            newXPos = -0.8f;
        }

        //transform.Translate(goLeft * speed * Time.fixedDeltaTime);
        //rb.MovePosition(goRight * speed * Time.fixedDeltaTime);
    }
}
