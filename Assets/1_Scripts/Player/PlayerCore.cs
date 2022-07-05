using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCore : MonoBehaviour
{
    public enum LaneState { Lane1, Lane2, Lane3 }

    [SerializeField] private float speed = 5f;

    public LaneState laneState = LaneState.Lane2;

    private Vector3 v1 = new Vector3(-1, 0, 0);
    private Vector3 v2 = new Vector3(0, 0, 0);
    private Vector3 v3 = new Vector3(1, 0, 0);

    private void OnEnable()
    {
        PlayerController.OnSwipe += Swipe;
    }

    private void OnDisable()
    {
        PlayerController.OnSwipe -= Swipe;
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
                SwitchRightLane();
            }

            else if (delta.x < 0)
            {
                Debug.Log("Left");
                SwitchLeftLane();
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

    private void SwitchRightLane()
    {
        if (laneState == LaneState.Lane1)
        {
            transform.Translate(v2 * speed * Time.deltaTime);
        }
        else if (laneState == LaneState.Lane2)
        {
            transform.Translate(v3 * speed * Time.deltaTime);
        }
    }

    private void SwitchLeftLane()
    {
        if (laneState == LaneState.Lane3)
        {
            transform.Translate(v2 * speed * Time.deltaTime);
        }
        else if (laneState == LaneState.Lane2)
        {
            transform.Translate(v1 * speed * Time.deltaTime);
        }
    }
}
