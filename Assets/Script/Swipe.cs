using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Swipe : MonoBehaviour {

    // Use this for initialization
    private bool tap, swipeUp, swipeDown;
    private Vector2 startTouch, swipeDelta;
    private bool isdragging = false;

    // Update is called once per frame
    private void Update() {
        tap = swipeUp = swipeDown = false;

        #region Desktop Inputs
        if (Input.GetMouseButtonDown(0))
        {
            tap = true;
            isdragging = true;
            startTouch = Input.mousePosition;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            isdragging = false; 
            Reset();
        }
        #endregion

        #region MobileInputs

        if (Input.touches.Length > 0)
        {
           
            if(Input.touches[0].phase == TouchPhase.Began)
            {
                tap = true;
                isdragging = true;
                startTouch = Input.touches[0].position;
            }
            else if (Input.touches[0].phase == TouchPhase.Ended || Input.touches[0].phase == TouchPhase.Canceled)
            {
                isdragging = false;
                Reset();
            }
        }

        swipeDelta = Vector2.zero;
        if(isdragging)
        {
            if (Input.touches.Length > 0)
                swipeDelta = Input.touches[0].position - startTouch;
            else if (Input.GetMouseButton(0))
                swipeDelta = (Vector2)Input.mousePosition - startTouch;
        }

        //cross deadzone?
        if(swipeDelta.magnitude > 150)
        {
            float x = swipeDelta.x;
            float y = swipeDelta.y;

            if(Mathf.Abs(x) < Mathf.Abs(y))
            {
                // Up and down
                if(y < 0)
                {
                    swipeDown = true;
                }
                else
                {
                    swipeUp = true;
                }
            }

            Reset();
        }
    }
    #endregion

    private void Reset()
    {
        startTouch = swipeDelta = Vector2.zero;
        isdragging = false;
    }

    public Vector2 SwipeDelta { get { return swipeDelta; } }
    public bool SwipeUp { get{ return swipeUp; } }
    public bool SwipeDown {get { return swipeDown; } }

}
