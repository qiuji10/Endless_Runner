using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public enum PlayerState { OnAir, OnRun, OnRoll, Whatever }

public class PlayerController : MonoBehaviour
{
    public enum LaneState { Lane1, Lane2, Lane3 }
    

    public LaneState laneState = LaneState.Lane2;
    public PlayerState playerState = PlayerState.OnRun;
    public ForceMode forceMode;

    [SerializeField] float swipeDeadZone;
    [SerializeField] float swipeDuration;
    [SerializeField] float jumpForce;
    float firstTapTime;
    bool isJump;

    public bool isSlope;

    [SerializeField] private float speed = 5f;
    private float newXPos;

    private Rigidbody rb;
    private Animator anim;

    Vector3 firstTouchPos;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        GameManager.OnGameStart += StartAnimation;
    }

    private void OnDestroy()
    {
        GameManager.OnGameStart -= StartAnimation;
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

    private void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.CompareTag("Ground"))
        {
            isJump = false;
            playerState = PlayerState.OnRun;
        }
    }

    private void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.CompareTag("Slope"))
        {
            Debug.Log("TRIGGER ENTER");
            rb.AddForce(Vector3.up, ForceMode.VelocityChange);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        isSlope = false;
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
                GoRight();
            }
            else if (delta.x < 0)
            {
                GoLeft();
            }
        }
        //Vertical swipe
        else
        {
            //up or down
            if (delta.y > 0 && !isJump)
            {
                playerState = PlayerState.OnAir;
                anim.SetTrigger("onAir");
                isJump = true;
                rb.AddForce(Vector3.up * jumpForce, forceMode);
            }

            else if (delta.y < 0)
            {
                playerState = PlayerState.OnRoll;
                anim.SetTrigger("onRoll");
                rb.AddForce(Vector3.down * jumpForce/2, forceMode);
            }
        }
    }

    private void GoRight()
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
    }

    private void GoLeft()
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
    }

    public void ResetState()
    {
        playerState = PlayerState.OnRun;
    }

    public void StartAnimation()
    {
        anim.SetTrigger("StartGame");
    }
}
