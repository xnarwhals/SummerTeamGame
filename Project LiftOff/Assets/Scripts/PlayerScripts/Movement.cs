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
    [SerializeField] private float jumpPower; 
    private bool isFacingRight;

    [Header("Multiple Jumps")]
    [SerializeField] private int extraJumps;
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


    //refrences
    private Rigidbody2D body;
    private BoxCollider2D boxCollider;
    private float horizontalInput;

    private void Awake()
    {    
        body = GetComponent<Rigidbody2D>();
        boxCollider = GetComponent<BoxCollider2D>();
    }

    private void Update()
    {
        if (textUI.IsOpen) return; // stop player movement when active
        
        horizontalInput = Input.GetAxis("Horizontal");//gets the A or D key press

        body.velocity = new Vector2(horizontalInput * speed, body.velocity.y);
        body.gravityScale = 1.5f;
        //^careful with this because gravity will always be set to 1.5 even if I try 
        //to change the gravity in game it will be set back to 1.5
       
        //Flip the player -x/x depending on key press good for when we have a character
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

        //Jump
        if (Input.GetKeyDown(KeyCode.Q))
            Jump();
        
        //Adjustable jump height
        if (Input.GetKeyUp(KeyCode.Q) && body.velocity.y > 0)
            body.velocity = new Vector2(body.velocity.x, body.velocity.y / 2);    
        
        //Wall Jump and Slide Section
        if (isGrounded())
            jumpCounter = extraJumps;           

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
        
        //for interactions with npc
        if (Input.GetKeyDown(KeyCode.E))
        {
            Interactable?.Interact(this);
        }
    }
        //Enemy.transform.position = new Vector2(transform.position.x, Enemy.transform.position.y); 
        //^^^possible code if I want collider to follow player on x axis 

    private void Jump()
    {
        //dont forget animation
        SoundManager.instance.PlaySound(jumpSound); //play jump sound
        
        if (isGrounded() || isWallSliding && Input.GetKeyDown(KeyCode.Q))
            body.velocity = new Vector2(body.velocity.x, jumpPower);//physical jump
        else
        {
            if (jumpCounter > 0) //If we have extra jumps then jump and decrease the jump counter
            {
                body.velocity = new Vector2(body.velocity.x, jumpPower);
                jumpCounter--;
            }
        }    
    }        
    

    private bool isGrounded()//casts a ray to see if the player is touching ground false or true 
    {
        RaycastHit2D raycastHit = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0, Vector2.down, 0.1f, groundLayer);
        return raycastHit.collider != null;
    }

    private bool onWall()//casts another ray to see if player is touching wall layer false or true 
    {
        RaycastHit2D raycastHit = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0, new Vector2(transform.localScale.x, 0), 0.1f, wallLayer);
        return raycastHit.collider != null;
    }
               
}
