using UnityEngine;

public class PlatformerMovement : MonoBehaviour
{

    // Used for dashing and moving.
    public enum HorizontalDirection
    {
        None,
        Right,
        Left
    }

    // Holds all the movement variables
    [System.Serializable]
    public class Movement
    {
        public bool isMoving = false;
        public float walkSpeed = 8.0f;
        public float runSpeed = 14.0f;
        public float rotationSpeed = 1.5f;
        public float acceleration = 1.5f;
        public InputDirection lastInputDirection = InputDirection.None;
    }

    // Holds all the jumping variables
    [System.Serializable]
    public class Jumping
    {
        public float strength = 14f;
        public bool isGrounded = false;
        public bool doubleJumping = false;
        public bool canDoubleJump = false;
        public float doubleJumpStrength = 1.0f;

        // Last time the jump button was clicked down
        [System.NonSerialized]
        public float lastButtonTime = -10.0f;

        // Time we started a jump
        [System.NonSerialized]
        public float lastTime = -1.0f;

        // Mid air start height for double jump
        [System.NonSerialized]
        public float lastStartHeight = 0.0f;
    }

    // Variables
    private Rigidbody rb;
    public Movement movement;
    public Jumping jumping;
    Vector2 moveVelocity;

    // Does this script currently respond to Input?
    public bool canControl = true;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        moveVelocity = new Vector2(0, 0);
        jumping = new Jumping();
        movement = new Movement();
    }

    private void Update()
    {
        moveVelocity.x = 0f;
        moveVelocity.y = -0.1f;

        // Horizontal movement
        moveVelocity.x = keyDown == InputDirection.Right ? movement.walkSpeed : moveVelocity.x;
        moveVelocity.x = keyDown == InputDirection.Left ? -movement.walkSpeed : moveVelocity.x;

        if (leftKeyUp && movement.lastInputDirection == InputDirection.Left)
        {
            Dash();
        }
        if (rightKeyUp && movement.lastInputDirection == InputDirection.Right)
        {
            Dash();
        }
        if (keyUp == InputDirection.Right || keyUp == InputDirection.Left)
        {
            movement.lastInputDirection = keyUp;
        }

        // Vertical Movement
        if (isJumpKeyDown && jumping.isGrounded)
            Jump();
        else if (isJumpKeyDown && jumping.canDoubleJump)
            Jump();


        ApplyMovement();
    }

    private void Jump()
    {
        //Double jumping
        if (!jumping.isGrounded)
        {
            jumping.doubleJumping = true;
            jumping.canDoubleJump = false;
        }

        jumping.isGrounded = false;
        moveVelocity.y += jumping.doubleJumping ? jumping.strength + jumping.doubleJumpStrength : jumping.strength;
    }

    private void Dash()
    {
        // TODO - Uncontrollable? dash somehow? 
        moveVelocity.x += horizontalVelocity + movement.runSpeed;
    }

    // X axis is disabled//locked, Only update movement in terms of Y and Z
    // Y = Vertical
    // Z = Horizontal
    void ApplyMovement()
    {
        Vector3 movementVector = new Vector3(0, jumpVelocity, horizontalVelocity);
        rb.velocity = movementVector;
    }

    // Velocity combining Walking and Jumping
    float horizontalVelocity
    {
        get { return moveVelocity.x; }
    }

    float jumpVelocity
    {
        get { return rb.velocity.y + moveVelocity.y; }
    }

    //
    // Collisions
    //

    void OnCollisionEnter(Collision other)
    {
        //TODO - Tagging against collider? 
        jumping.isGrounded = true;
        jumping.canDoubleJump = true;
        jumping.doubleJumping = false;
    }

    //
    // Input Detection
    //
    public enum InputDirection
    {
        None,
        Left,
        Right
    }

    public InputDirection keyDown
    {
        get
        {
            if (rightKeyDown)
                return InputDirection.Right;
            if (leftKeyDown)
                return InputDirection.Left;
            return InputDirection.None;
        }
    }

    InputDirection keyUp
    {
        get
        {
            if (rightKeyUp)
                return InputDirection.Right;
            if (leftKeyUp)
                return InputDirection.Left;
            return InputDirection.None;
        }
    }

    bool isJumpKeyDown
    {
        get { return Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.Z) || Input.GetKeyDown(KeyCode.W); }
    }

    bool leftKeyDown
    {
        get { return Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A); }
    }

    bool rightKeyDown
    {
        get
        {
            return Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D);
        }
    }

    bool leftKeyUp
    {
        get { return Input.GetKeyUp(KeyCode.LeftArrow) || Input.GetKeyUp(KeyCode.A); }
    }

    bool rightKeyUp
    {
        get
        {
            return Input.GetKeyUp(KeyCode.RightArrow) || Input.GetKeyUp(KeyCode.D);
        }
    }
}