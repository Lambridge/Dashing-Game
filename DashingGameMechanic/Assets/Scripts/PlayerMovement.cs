using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {

    #region Variables
    Rigidbody2D playerRigidbody;

    //Movement speed variables
    [SerializeField] float walkSpeed;
    [SerializeField] float runSpeed;
    [SerializeField] float jumpVelocity;

    //State variables... maybe make these actual enums
    bool isOnGround;
    bool isRunning;
    bool isAlive;

    //Double Tap Variables
    [SerializeField] float doubleTapTimerMaxValue;
    float doubleTapTimerCurrentValue;
    float previousInputDirection; //Used to check if input direction was pressed on a given frame
    float previousInputDirectionPressed; //What input direction was last pressed

    //Groun detection variables
    [SerializeField] Transform groundDetectPoint;
    [SerializeField] float groundDetectRadius = 0.4f;
    [SerializeField] LayerMask whatIsGround;
    #endregion

    #region Properties
    private bool IsOnGround
    {
        get
        {
            //Check to see if player is on ground
            return UpdateIsOnGround();
        }
    }
    #endregion

    // Use this for initialization
    void Start()
    {
        playerRigidbody = GetComponent<Rigidbody2D>();
        isAlive = true;
        isRunning = false;
    }

    void Update()
    {
        if (isAlive)
        {
            UpdateIsOnGround();
            HandlePlayerInput();
            ChangeSpriteColorDependingOnRunningState();
        }
    }

    void HandlePlayerInput()
    {
        //Check for jump inputs then directional inputs
        //Slightly prioritizes jumping
        HandleJumpInput();
        HandleDirectionalInput();
    }

    private bool UpdateIsOnGround()
    {
        Collider2D[] groundColliders =
            Physics2D.OverlapCircleAll(groundDetectPoint.position, groundDetectRadius, whatIsGround);

        return groundColliders.Length > 0;
    }

    private void HandleJumpInput()
    {
        if (Input.GetButtonDown("Jump") && IsOnGround)
        {
            float xVelocity = playerRigidbody.velocity.x;
            float yVelocity = jumpVelocity; //* Time.deltaTime * 75; WE DON'T NEED THIS SINCE JUMP INPUT IS ON ONE FRAME. THIS IS IMPULSE JUMPING
            Vector2 velocityToSet = new Vector2(xVelocity, yVelocity);

            playerRigidbody.velocity = velocityToSet;
        }
    }

    private void HandleDirectionalInput()
    {

        float newInputDirection = Input.GetAxisRaw("Horizontal");
        if (newInputDirection != 0)
        {
            if (!isRunning)
            {
                //Actually do movement things
                if (InputDirectionPressedThisFrame(newInputDirection) && IsOnGround)
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

        if (isRunning)
        {
            GetComponent<SpriteRenderer>().color = runColor;
        }
        else
        {
            GetComponent<SpriteRenderer>().color = walkColor;
        }
    }

    public void TakeDamage()
    {
        //Subtract from the health that is not yet implemented
    }
    /*
    private void HandleMovementInput()
    {
        float movementInput = Input.GetAxis("Horizontal");
        float xVelocity = movementInput * moveSpeed * Time.deltaTime * 75;
        float yVelocity = myRigidbody2D.velocity.y;
        
        Vector2 velocityToSet = new Vector2(xVelocity, yVelocity);
        myRigidbody2D.velocity = velocityToSet;
    }
    */
}
