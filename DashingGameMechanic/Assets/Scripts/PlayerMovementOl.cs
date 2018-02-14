using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementOld : MonoBehaviour {

    Rigidbody2D playerRigidbody;

    enum movementState { normal, running }
    bool isRunning;

    movementState currentMovementState;
    [SerializeField] float walkSpeed;
    [SerializeField] float runSpeed;

    float inputDirection;

    //Currently unused WIP code
    [SerializeField] float doubleTapTimer;

    // Use this for initialization
    void Start () {
        playerRigidbody = GetComponent<Rigidbody2D>();
	}
	
	// Update is called once per frame
	void Update () {
        CheckForInput();
	}

    private void CheckForInput()
    {
        inputDirection = Input.GetAxisRaw("Horizontal");

        //If not running and the run button is pressed, start running

        if (currentMovementState == movementState.normal)
        {
            if(Input.GetKeyDown(KeyCode.Space))
            {
                StartDash();
            }
            else if(inputDirection != 0)
            {
                playerRigidbody.velocity = new Vector2(walkSpeed * inputDirection, 0);
            }
        }
        //
        if(currentMovementState == movementState.running)
        {
            if (Input.GetKey(KeyCode.Space))
            {
                playerRigidbody.velocity = new Vector2(runSpeed * inputDirection, 0);
            }
            else 
            {
                currentMovementState = movementState.normal;
            }

        }
    }

    void StartDash()
    {
        inputDirection = Input.GetAxisRaw("Horizontal");
        if (inputDirection == 0) inputDirection = 1;
        playerRigidbody.velocity = new Vector2(runSpeed * inputDirection, 0);
        currentMovementState = movementState.running;
    }
    /*
    void CheckDoubleTap()
    {
        if(doubleTapTimer > 0 && Input.GetKeyDown(KeyCode.LeftArrow))
        {
            StartDash();
        }
        else if(doubleTapTimer > 0 && Input.GetKeyDown(KeyCode.RightArrow))
        {
            StartDash();
        }
        doubleTapTimer -= Time.deltaTime;
    }
    */
}
