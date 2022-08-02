using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{

    [Header("TextUI")]
    [SerializeField] private TextUI textUI;
    public TextUI TextUI => textUI;
    public Interactable Interactable { get; set; }

    [Header("Movement Parameters")]
    [SerializeField] private float speed;
    [SerializeField] private float speedItemValue;
    [SerializeField] private float jumpPower; 
    [SerializeField] private float jumpPowerItemValue;
    private bool isFacingRight;
    
    [Header("Multiple Jumps")]
    [SerializeField] private int extraJumps;
    [SerializeField] private int extraJumpItemValue;
    private int jumpCounter;

    [Header("Layers")]
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private LayerMask wallLayer;

    [Header("Sounds")]
    [SerializeField] private AudioClip jumpSound;

    [Header("WallJump")]
    [SerializeField] private float wallJumpTime; //avoid slugish gameplay (0.2f) by giving a certain amount of time to perform jump
    [SerializeField] private float wallSlideSpeed; //0.3f
    [SerializeField] private float wallDistance; // 0.5f how far does wall have to be from collider to perform jump
    private bool isWallSliding = false;
    RaycastHit2D WallCheckHit;
    float jumpTime;

    [Header("Dash")]
    [SerializeField] private float dashDistance;//adjustable distance
    private bool isDashing; //how do we know we are dashing 
    // private float dashCooldown = 1f;

    [Header("CoyoteTime")]
    [SerializeField] private float hangTime;
    private float hangCounter = 0;

    //refrences
    private Rigidbody2D body;
    private BoxCollider2D boxCollider;
    private float horizontalInput;
    private Animator anim;

    private void Awake()
    {    
        anim = GetComponent<Animator>();
        body = GetComponent<Rigidbody2D>();
        boxCollider = GetComponent<BoxCollider2D>();
    }

    private void Update()
    {
        if (textUI.IsOpen) return; // stop player movement when active
    
//Horizontal Movement Section
        horizontalInput = Input.GetAxis("Horizontal");//gets the A or D key press

    // (!isDashing) remeber this
        if (horizontalInput != 0 && (!isDashing))
        {
            body.velocity = new Vector2(horizontalInput * speed, body.velocity.y);
            anim.SetBool("Running", true);
        }
        else
        {
            body.velocity = new Vector2(horizontalInput * 0, body.velocity.y);
            anim.SetBool("Running", false);
        }
        
        body.gravityScale = 3f;
        //^careful with this because gravity will always be set to 3 even if I try 
        //to change the gravity in game it will be set back to 3

       
//Flip the player -x/x depending on key press (A or D)
        if (horizontalInput > 0.01f)
        {
            transform.localScale = Vector3.one;
            isFacingRight = true;
        }           
        else if (horizontalInput < -0.01f)
        {
            transform.localScale = new Vector3(-1, 1, 1);
            isFacingRight = false;
        }  

//When on ground reset extra jumps and check coyote time
        if (isGrounded())
        {
            anim.SetBool("Grounded", true);
            anim.SetBool("isJumping", false);
            jumpCounter = extraJumps;  
            hangCounter = hangTime;
            //hang counter = 0.4f
        }
        else
        {
            anim.SetBool("isJumping", true);
            hangCounter -= Time.deltaTime;
            //if not on ground start reducing hang counter (0.4f)
        }

//Jump Section
        if (Input.GetButtonDown("Jump"))
           {
            Jump();
           } 
//Adjustable jump height
        if (Input.GetButtonUp("Jump") && body.velocity.y > 0)
            body.velocity = new Vector2(body.velocity.x, body.velocity.y / 2);   

//Dash Section

//left Dash
    if (Input.GetAxis("DashLeft") == 1) {
            anim.SetTrigger("Dash");
            transform.localScale = new Vector3(-1, 1, 1);
            StartCoroutine(Dash(-1f));             
        }
    

//Right Dash
    if (Input.GetAxis("DashRight") == 1) { 
            anim.SetTrigger("Dash");
            transform.localScale = Vector3.one;
            StartCoroutine(Dash(1f)); 
        }
    

          
//Wall Jump and Slide Section
        if (isFacingRight)
        {   //check the player position (parameter 1) second parameter is casting the Raycast towards right wall
            WallCheckHit = Physics2D.Raycast(transform.position, new Vector2(wallDistance, 0), wallDistance, wallLayer);
            Debug.DrawRay(transform.position, new Vector2(-wallDistance, 0), Color.blue);
        }
        else//cast left wall
        {
            WallCheckHit = Physics2D.Raycast(transform.position, new Vector2(-wallDistance, 0), wallDistance, wallLayer);
            Debug.DrawRay(transform.position, new Vector2(-wallDistance, 0), Color.blue);
        }
 
        if (WallCheckHit && !isGrounded() && horizontalInput != 0)
        {              
            isWallSliding = true;//logic
            //buffer time to reduce slugish time 
            jumpTime = Time.time + wallJumpTime;
        }
        else if(jumpTime < Time.time)
            isWallSliding = false;//if jumpTime is less than current time return false   
            //wallJumptime essentially acts as coyote time! Add it to normal jump!    
        
        if (isWallSliding)
        {
            //slow down player logic
            body.velocity = new Vector2(body.velocity.x, Mathf.Clamp(body.velocity.y, wallSlideSpeed, float.MaxValue));
            //parameter 1 of New Vector 2 is the x position of player
            //parameter 2 is clamp (y position) that takes players y position, speed of sliding down (min), and a max value
        }        
        
//For Interactions With Npc Dialouge
        if (Input.GetButton("Interact"))
        {
            Interactable?.Interact(this);
        }
    }
    
//Dash Function
    IEnumerator Dash (float direction)
    {
        isDashing = true;
        body.velocity = new Vector2(body.velocity.x, 0f); //no vertical movement or gravity 
        body.AddForce(new Vector2(dashDistance * direction, 0f), ForceMode2D.Impulse);//direction is -1 or 1 based on key
        float gravity = body.gravityScale;
        body.gravityScale = 0;
        yield return new WaitForSeconds(1f);
        //^wait this amount of time before the dash distance is covered
        //could be made a variable 
        isDashing = false;
        body.gravityScale = gravity;//once dash is done set bool to false and reset gravity 
    }

//Jump Function
    void Jump()
    {
        //dont forget animation
        SoundManager.instance.PlaySound(jumpSound); //play jump sound
        
        //coyote time --> && hangCounter > 0
        if ((isGrounded()) || isWallSliding && Input.GetButton("Jump"))//check wall jump first
        {
            anim.SetTrigger("takeOff");
            body.velocity = new Vector2(body.velocity.x, jumpPower);//physical jump  
        }          
        else
        {
            if (jumpCounter > 0) //If we have extra jumps then jump and decrease the jump counter
            {
                anim.SetTrigger("DoubleJump");
                body.velocity = new Vector2(body.velocity.x, jumpPower);
                jumpCounter--; 
            }
        }   
    }   

//Items

    // far from efficent but its a way I found working
    void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.gameObject.CompareTag("SpeedItem"))
        {
            Destroy(coll.gameObject);
            speed = speedItemValue;  
        }
        else if (coll.gameObject.CompareTag("ExtraJumpItem"))
        {
            Destroy(coll.gameObject);
            extraJumps = extraJumpItemValue;
        }
        else if (coll.gameObject.CompareTag("JumpForceItem"))
        {
            Destroy(coll.gameObject);
            jumpPower = jumpPowerItemValue;
        } 
    }      
    
//Ground Check
    bool isGrounded()//casts a ray to see if the player is touching ground false or true 
    {
        RaycastHit2D raycastHit = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0, Vector2.down, 0.1f, groundLayer);
        return raycastHit.collider != null;
    }

//Wall Check 
    bool onWall()//casts another ray to see if player is touching wall layer false or true 
    {
        RaycastHit2D raycastHit = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0, new Vector2(transform.localScale.x, 0), 0.1f, wallLayer);
        return raycastHit.collider != null;
    }
               
}
