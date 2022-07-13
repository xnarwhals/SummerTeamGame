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

    [Header("Multiple Jumps")]
    [SerializeField] private int extraJumps;
    private int jumpCounter;

    [Header("Layers")]
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private LayerMask wallLayer;

    [Header("Sounds")]
    [SerializeField] private AudioClip jumpSound;

    [Header("WallClimb")]
    [SerializeField] private float wallclimbSpeed;


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
        
        //for interactions with npc
        if (Input.GetKeyDown(KeyCode.E))
        {
            Interactable?.Interact(this);
        }
        
        if (textUI.IsOpen) return; // stop player movement when active

        horizontalInput = Input.GetAxis("Horizontal");//gets the A oe D key press

        //Flip the player -x/x depending on key press
         if (horizontalInput > 0.01f)
            transform.localScale = Vector3.one;
        else if (horizontalInput < -0.01f)
            transform.localScale = new Vector3(-1, 1, 1);
        
        //Jump
        if (Input.GetKeyDown(KeyCode.Q))
            Jump();
        
        //Adjustable jump height
        if (Input.GetKeyUp(KeyCode.Q) && body.velocity.y > 0)
            body.velocity = new Vector2(body.velocity.x, body.velocity.y / 2);    
        
        if(onWall() && Input.GetKeyDown(KeyCode.W))//if on the wall then have the player fall and not stick (As long as player not holding horizontal key input)
        {
            WallClimb();
        }
        else//if not on wall this code physically allows player to jump
        {
            body.gravityScale = 1.5f;
            body.velocity = new Vector2(horizontalInput * speed, body.velocity.y);   
        }


        if (isGrounded())
        {
            jumpCounter = extraJumps;
        }

        //Enemy.transform.position = new Vector2(transform.position.x, Enemy.transform.position.y); //possible code if I want collider to follow player on x axis 
    }

    private void Jump()
    {
        //dont forget animation
        SoundManager.instance.PlaySound(jumpSound); //play jump sound

        if (isGrounded())
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



    private void WallClimb()
    {
        //dont forget animation
        body.gravityScale = 0;
        body.velocity = Vector2.zero;
        //body.velocity = new Vector2(0, body.velocity.y * wallclimbSpeed);
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
