using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerController : MonoBehaviour
{
    public enum LaneState { Lane1, Lane2, Lane3 }

    public LaneState laneState = LaneState.Lane2;

    [SerializeField] float swipeDeadZone;
    [SerializeField] float swipeDuration;
    float firstTapTime;

    [SerializeField] private float speed = 5f;
    private float newXPos, xPos;

    private Rigidbody rb;

    Vector3 firstTouchPos;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            firstTapTime = Time.time;
            firstTouchPos = Input.mousePosition;
        }

        if (Input.GetMouseButtonUp(0))
        {
            Vector2 swipeDelta = Input.mousePosition - firstTouchPos;

            if (swipeDelta != Vector2.zero && Time.time - firstTapTime <= swipeDuration)
            {
                Swipe(swipeDelta);
            }
        }
        transform.position = Vector3.Lerp(transform.position, new Vector3(newXPos, transform.position.y, transform.position.z), speed * Time.deltaTime);
    }

    private void Swipe(Vector2 delta)
    {
        float xAbs = Mathf.Abs(delta.x);
        float yAbs = Mathf.Abs(delta.y);

        //Horizontal Swipe
        if (xAbs > yAbs)
        {
            //do left or right
            if (delta.x > 0)
            {
                Debug.Log("Right");
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
            }
            else if (delta.x < 0)
            {
                Debug.Log("Left");
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
            }
        }
        //Vertical swipe
        else
        {
            //up or down
            if (delta.y > 0)
            {
                Debug.Log("Up");
            }

            else if (delta.y < 0)
            {
                Debug.Log("Down");
            }
        }
    }
}
