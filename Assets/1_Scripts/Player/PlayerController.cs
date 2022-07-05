using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerController : MonoBehaviour
{
    public static event Action<Vector2> OnSwipe;

    [SerializeField] float swipeDeadZone;
    [SerializeField] float swipeDuration;
    float firstTapTime;

    Vector3 firstTouchPos;

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
                if (OnSwipe != null)
                    OnSwipe.Invoke(swipeDelta);
            }

        }
    }
}
