using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovementController : MonoBehaviour
{   
    public float speed = 10f;
    public float jumpforce = 20.0f;
    [SerializeField] private Transform groundCheck;

    [Range(0f, 10f)]
    [SerializeField] private float fallModifier;

    private bool isJumpInputBuffered = false;
    private float jumpInputBufferTime = 0;
    private float jumpBufferTimer = 0;
    private bool buffered = false;
    private bool bufferjump = false;
    public float horizontal;
    private Rigidbody2D rb;
    private InputAction move;
    private InputAction jump;
    private InputSystem playerControls;
    public LayerMask groundLayer;
    private bool isGrounded = false;
    private Vector3 selfScale;
    void Awake()
    {
        playerControls = new InputSystem();
        rb = GetComponent<Rigidbody2D>();
        selfScale = transform.localScale;
    }
    
    void OnEnable()
    {
        move = playerControls.Player.Move;
        jump = playerControls.Player.Jump;
        jump.Enable();
        move.Enable();
    }

    void OnDisable()
    {
        move.Disable();
        jump.Disable();
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    public void _move(InputAction.CallbackContext context)
    {
        horizontal = context.ReadValue<Vector2>().x;
    }

    public void _jump(InputAction.CallbackContext context)
    {
        if(context.performed)
        {
            if(isGrounded)
            {
                rb.velocity = new Vector2(rb.velocity.x,jumpforce);
                selfScale = new Vector3(.5f,1.5f,1);
                transform.localScale = selfScale;
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
    void Update()
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

        selfScale.x = Mathf.Lerp(selfScale.x,1,.015f);
        selfScale.y = Mathf.Lerp(selfScale.y,1,.015f);
        transform.localScale = selfScale;   
    }
        

    void FixedUpdate()
    {
        if(rb.velocity.y < 0)
        {
            rb.gravityScale = 5.0f + fallModifier;
        }
        else{
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
            rb.velocity = new Vector2(rb.velocity.x,jumpforce);
            bufferjump = false;
        }
        
    }

  
}
