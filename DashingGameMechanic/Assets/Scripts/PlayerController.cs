using UnityEngine;
using System.Collections;
using System;

public class PlayerController : MonoBehaviour {

    [SerializeField] Transform groundDetectPoint;
    [SerializeField] float groundDetectRadius = 0.4f;
    [SerializeField] LayerMask whatIsGround;

    Rigidbody2D myRigidbody2D;
    //[SerializeField] float moveSpeed = 10;
    [SerializeField] float jumpVelocity = 10;
    bool isOnGround;

    bool isAlive;

    #region Properties
    private bool IsOnGround
    {
        get
        {
            //Whatever code I want to get called when this property is accessed
            return UpdateIsOnGround();
        }
        //set
        //{
        //    //Whatever code I want to get called when this property is assigned a value
        //}
    }
    #endregion

    // Use this for initialization
    void Start () {
        myRigidbody2D = GetComponent<Rigidbody2D>();
        isAlive = true;
	}

    void Update()
    {
        if (isAlive)
        {
            //HandleMovementInput();
            HandleJumpInput();
            UpdateIsOnGround();
        }
        
    }

    private bool UpdateIsOnGround()
    {
        Collider2D[] groundColliders = 
            Physics2D.OverlapCircleAll(groundDetectPoint.position, groundDetectRadius, whatIsGround);

        //isOnGround = groundColliders.Length > 0;

        return groundColliders.Length > 0;
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

    private void HandleJumpInput()
    {
        if (Input.GetButtonDown("Jump") && IsOnGround)
        {
            float xVelocity = myRigidbody2D.velocity.x;
            float yVelocity = jumpVelocity; //* Time.deltaTime * 75; WE DON'T NEED THIS SINCE JUMP INPUT IS ON ONE FRAME. THIS IS IMPULSE JUMPING
            Vector2 velocityToSet = new Vector2(xVelocity, yVelocity);

            myRigidbody2D.velocity = velocityToSet;
        }
    }

    public void TakeDamage()
    {
        isAlive = false;
    }

}
