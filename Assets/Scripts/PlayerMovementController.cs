using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovementController : MonoBehaviour
{   
    [SerializeField] private float speed = 10f;
    [SerializeField] private float jumpForce = 20.0f;
    [SerializeField] private Transform groundCheck;

    [Range(0f, 10f)]
    [SerializeField] private float fallModifier;

    private bool isJumpInputBuffered = false;
    private float jumpInputBufferTime = 0;
    private float jumpBufferTimer = 0;
    private bool buffered, bufferjump;
    private float horizontal;
    private Rigidbody2D rb;
    private InputAction moveAction, jumpAction;
    private InputSystem playerControls;
    public LayerMask groundLayer;
    private bool isGrounded = false;
    private Vector3 selfScale;
    private Animator anim;
    void Awake()
    {
        playerControls = new InputSystem();
        rb = GetComponent<Rigidbody2D>();
        selfScale = transform.localScale;
        anim = GetComponent<Animator>();
    }
    
    void OnEnable()
    {
        //Enables player controls as part of Unity input system.
        moveAction = playerControls.Player.Move;
        jumpAction = playerControls.Player.Jump;
        jumpAction.Enable();
        moveAction.Enable();
        moveAction.performed += _Move;
        jumpAction.performed += _Jump;
    }

    void OnDisable()
    {
        moveAction.Disable();
        jumpAction.Disable();
        moveAction.performed -= _Move;
        jumpAction.performed -= _Jump;
    }
    // Start is called before the first frame update
    private void Start()
    {

    }

    public void _Move(InputAction.CallbackContext context)
    {
        horizontal = context.ReadValue<Vector2>().x;
    }

    public void _Jump(InputAction.CallbackContext context)
    {
        if(context.performed)
        {
            if(isGrounded)
            {
                rb.velocity = new Vector2(rb.velocity.x,jumpForce);
                // selfScale = new Vector3(.5f,1.5f,1);
                // transform.localScale = selfScale;
                isGrounded = false;
               
            }
            else
            {
                buffered = true;
            }
             print("JUMP");
            
        }
    }
    // Update is called once per frame
    private void Update()
    {  

        if(buffered)
        {
            jumpBufferTimer += Time.deltaTime;
        }

        if(isGrounded && buffered)
        {
            jumpInputBufferTime = jumpBufferTimer;
            print(jumpInputBufferTime);
            jumpBufferTimer = 0;
            //bufferjump = true;
            if(jumpInputBufferTime < .2f)
            {
                bufferjump = true;
            }
            buffered = false;
        }

        UpdateAnimation();

        // selfScale.x = Mathf.Lerp(selfScale.x,1,.015f);
        // selfScale.y = Mathf.Lerp(selfScale.y,1,.015f);
        // transform.localScale = selfScale;   
    }
        

    private void FixedUpdate()
    {
        //This code checks if the player is on their way down, if yes then fall faster. If not, fall regularly.
        if(rb.velocity.y < 0)
        {
            rb.gravityScale = 5.0f + fallModifier;
        }
        else
        {
            rb.gravityScale = 5.0f;
        }


        //Checking the ground
        RaycastHit2D hit = Physics2D.Raycast(groundCheck.position, Vector2.down, 0.1f, groundLayer);

        // If the raycast hits a ground object, consider the character grounded
        isGrounded = hit.collider != null;

        //Moves player horizontally 
        rb.velocity = new Vector2(horizontal*speed,rb.velocity.y);

        //Applies the buffered input jump
        if(bufferjump)
        {
            rb.velocity = new Vector2(rb.velocity.x,jumpForce);
            bufferjump = false;
        }
        
    }

    void UpdateAnimation()
    {
        //Stores if character is moving horizontally and sets the animator parameter.
        bool isRunning = rb.velocity.x != 0;
        anim.SetBool("isRunning", isRunning);

        //This code flips the 2D sprite when running right and left.
        if (isRunning)
        {
            selfScale.x = Mathf.Sign(rb.velocity.x);
            transform.localScale = selfScale; 
        }
    }

  
}
