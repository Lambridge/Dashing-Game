using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoubleTapToDash : MonoBehaviour {

    #region Variables
    Rigidbody2D playerRigidbody;
    bool isRunning;

    [SerializeField] float walkSpeed;
    [SerializeField] float runSpeed;

    //Double Tap Timer Variables
    [SerializeField] float doubleTapTimerMaxValue;
    float doubleTapTimerCurrentValue;

    float previousInputDirection; //Used to check if input direction was pressed on a given frame
    float previousInputDirectionPressed; //What input direction was last pressed
    #endregion

    // Use this for initialization
    void Start () {
        playerRigidbody = GetComponent<Rigidbody2D>();
        isRunning = false;
    }
	
	// Update is called once per frame
	void Update () {
        CheckForDirectionalInput();
        ChangeSpriteColorDependingOnRunningState();
	}

    private void CheckForDirectionalInput()
    {
        float newInputDirection = Input.GetAxisRaw("Horizontal");
        if (newInputDirection != 0)
        {
            if(!isRunning)
            {
                //Actually do movement things
                if (InputDirectionPressedThisFrame(newInputDirection))
                {
                    if (DoubleTapWithinTimeWindow(newInputDirection))
                    {
                        BeginDashingInGivenDirection(newInputDirection);
                    }
                }
                else
                {
                    float newVelocity = walkSpeed * newInputDirection;
                    playerRigidbody.velocity = new Vector2(newVelocity, playerRigidbody.velocity.y);
                    isRunning = false;
                }
            }
            else
            {
                //Run in the given direction
                float newVelocity = runSpeed * newInputDirection;
                playerRigidbody.velocity = new Vector2(newVelocity, playerRigidbody.velocity.y);
            }

            //After everything record what input was last 
            previousInputDirectionPressed = newInputDirection;
            //Set timer to max since input was pressed
            doubleTapTimerCurrentValue = doubleTapTimerMaxValue;
        }
        else
        {
            isRunning = false;
        }

        //Update the doubleTapTimer
        doubleTapTimerCurrentValue += Time.deltaTime * -1;

        //The new input becomes the previous input, to be used with latter checks
        previousInputDirection = newInputDirection;
    }

    bool InputDirectionPressedThisFrame(float newInputDirection)
    {
        bool newInputPressed;
        //Was an input pressed after there was no input
        if (newInputDirection != 0 && previousInputDirection == 0)
        {
            newInputPressed = true;
            
            Debug.Log("New directional press");
            return newInputPressed;
        }
        else
        {
            newInputPressed = false;
            return newInputPressed;
        }
    }

    bool DoubleTapWithinTimeWindow(float newInputDirection)
    {
        bool InputIsSameDirection = (newInputDirection == previousInputDirectionPressed);
        Debug.Log("Is input in the same direction? " + InputIsSameDirection);
        bool DoubleTapTimerNotZeroed = (doubleTapTimerCurrentValue > 0);
        //Debug.Log("Is double tap timer NOT zeroed? " + DoubleTapTimerNotZeroed);

        if (InputIsSameDirection && DoubleTapTimerNotZeroed)
        {
            Debug.Log("Double tapped within time window");
            return true;
        }
        else
        {
            return false;
        }
    }

    void BeginDashingInGivenDirection(float dashDirectionFromInput)
    {
        //Set velocity to dash velocity and make isRunning true
        float newVelocity = runSpeed * dashDirectionFromInput;
        playerRigidbody.velocity = new Vector2(newVelocity, playerRigidbody.velocity.y);
        isRunning = true;
        Debug.Log("Player began running");
    }

    void ChangeSpriteColorDependingOnRunningState()
    {
        Color walkColor = Color.blue;
        Color runColor = Color.red;
        //Color currentColor = GetComponent<SpriteRenderer>().color;

        if(isRunning)
        {
            GetComponent<SpriteRenderer>().color = runColor;
        }
        else
        {
            GetComponent<SpriteRenderer>().color = walkColor;
        }
    }

}
