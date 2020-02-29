using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    //Movement
    public float speed;
    public float dashSpeed;
    public float jump;
    float moveVelocity;
    public int doubleClickTimer;

    bool isGrounded = true;
    bool isDoubleJumpUsed = false;
    public DashDirection lastInputDirection;

    public GameObject self;
    private Rigidbody rb;

    public enum DashDirection
    {
        None,
        Right,
        Left
    }

    void Start() {
        rb = GetComponent<Rigidbody>();
    }

    void Update () 
    {
        //Jumping
        if (isJumpKeyDown && !isDoubleJumpUsed && !isGrounded) 
        {
            Jump();
            isDoubleJumpUsed = true;
        }
        if (isJumpKeyDown && isGrounded) 
        {
            Jump();
            isGrounded = false;
        }

        

        moveVelocity -= moveVelocity + 0.01f;

        //Left Right Movement
        if (leftKeyDown)
        {
            moveVelocity = -speed;
        }
        if (rightKeyDown)
        {
            moveVelocity = speed;
        }
        
        if(lastInputDirection == DashDirection.Right && rightKeyUp){
            Dash(true);
        }

        if(lastInputDirection == DashDirection.Left && leftKeyUp){
            Dash(false);
        }
        

        if(doubleClickTimer > 0){
            doubleClickTimer -= 1;
        }else {
            lastInputDirection = DashDirection.None;
        }

        //Dashing
        if(leftKeyUp){
            doubleClickTimer = 100;
            if(doubleClickTimer > 0){
                lastInputDirection = DashDirection.Left;
            }
            
        }
        
        if(rightKeyUp){
            doubleClickTimer = 100;
            if(doubleClickTimer > 0){
                lastInputDirection = DashDirection.Right;
            }
        }

        Move();
    }

    void OnCollisionEnter(Collision other) {
        isGrounded = true;
        isDoubleJumpUsed = false;
    }

    bool isJumpKeyDown {
        get { return Input.GetKeyDown (KeyCode.Space) || Input.GetKeyDown (KeyCode.UpArrow) || Input.GetKeyDown (KeyCode.Z) || Input.GetKeyDown (KeyCode.W); }
    }

    bool leftKeyDown {
        get { return Input.GetKey (KeyCode.LeftArrow) || Input.GetKey (KeyCode.A); }
    }

    bool rightKeyDown {
        get { 
            return Input.GetKey (KeyCode.RightArrow) || Input.GetKey (KeyCode.D); 
        }
    }

    bool leftKeyUp {
        get { return Input.GetKeyUp (KeyCode.LeftArrow) || Input.GetKeyUp (KeyCode.A); }
    }

    bool rightKeyUp {
        get { 
            return Input.GetKeyUp (KeyCode.RightArrow) || Input.GetKeyUp (KeyCode.D); 
        }
    }

    private void Jump(){
        rb.velocity = new Vector2 (rb.velocity.z, jump);
    }

    private void Move(){
        Vector3 movementVector = new Vector3 (0, rb.velocity.y, moveVelocity);
        rb.velocity = movementVector;
    }

    private void Dash(bool right) {
        float dash = right ? dashSpeed : -dashSpeed;
        rb.AddForce(0, rb.velocity.y, dash, ForceMode.Impulse);
    }
}
