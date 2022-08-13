using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RevisedMovement : MonoBehaviour
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

    [Header("WallJump")]
    [SerializeField] private float wallJumpTime; //avoid slugish gameplay (0.2f) by giving a certain amount of time to perform jump
    [SerializeField] private float wallSlideSpeed; //0.3f
    [SerializeField] private float wallDistance; // 0.5f how far does wall have to be from collider to perform jump
    private bool isWallSliding = false;
	RaycastHit2D WallCheckHit;
    float jumpTime;

	[Header("Dash")] 
	[SerializeField] float dashCooldownDuration = 0.5f;
    [SerializeField] private float dashSpeed = 18;//adjustable distance
	[SerializeField] float dashDuration = 0.5f;
	float dashCooldownTimer = 0;
	Coroutine dashRoutine;
	bool lastFrameRightTrigger;
	bool lastFrameLeftTrigger;
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
		 }
		 else
		 {
			 anim.SetBool("isJumping", true);
			 anim.SetBool("Grounded", false);
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
			FindObjectOfType<AudioManager>().Play("WallSliding");
			body.velocity = new Vector2(body.velocity.x, Mathf.Clamp(body.velocity.y, -wallSlideSpeed, float.MaxValue));
			//slow player when wall sliding
			//parameter 1 of New Vector 2 is the x position of player
			//parameter 2 is clamp (y position) that takes players y position, speed of sliding down (min), and a max value
		}        
		
		//For Interactions With Npc Dialouge
		if (Input.GetButton("Interact"))
		{
			Interactable?.Interact(this);
		}
		
		// are the dash triggers pressed now?
		bool leftTriggerIsPressed = Input.GetAxis("DashLeft") == 1;
		bool rightTriggerIsPressed = Input.GetAxis("DashRight") == 1;

		horizontalInput = Input.GetAxis("Horizontal");


		bool isGliding = false;
		float glidingGravityScale = 0.1f;
		float dashingGravityScale = 0;
		float defaultGravityScale = 2;
		
		if (isGliding)
		{
			body.gravityScale = glidingGravityScale;
		}
		else if (dashRoutine != null)
		{
			body.gravityScale = dashingGravityScale;
		}
		else
		{
			body.gravityScale = defaultGravityScale;
		}
		

		// if we're not dashing...
		if (dashRoutine == null)
		{
			// move based on horizontal input
			anim.SetBool("Running", horizontalInput != 0);
			body.velocity = new Vector2(horizontalInput * speed, body.velocity.y);
			
			// jump if grounded
			if (isGrounded() && Input.GetButtonDown("Jump") || Input.GetButtonDown("Jump") && isWallSliding)
			{           
				FindObjectOfType<AudioManager>().Play("Jump");
            	anim.SetTrigger("takeOff");
				body.velocity = new Vector2(body.velocity.x, jumpPower);
			}
			else
			{
				if (Input.GetButtonDown("Jump") && jumpCounter > 0)
				{
					FindObjectOfType<AudioManager>().Play("Jump");
					anim.SetTrigger("DoubleJump");
                	body.velocity = new Vector2(body.velocity.x, jumpPower);
                	jumpCounter--; 
				}
			}
			
			// cut vertical speed in half if they just released the jump button
			// NOTE: if you press jump while in air, then release jump button, it will cut velocity in half again...
			if (Input.GetButtonUp("Jump") && body.velocity.y > 0)
			{
				body.velocity = new Vector2(body.velocity.x, body.velocity.y / 2);
			}

			// add to cooldown timer
			dashCooldownTimer += Time.deltaTime;

			// once cooldown timer is over...
			if (dashCooldownTimer >= dashCooldownDuration)
			{
				// did we JUST pressed the triggers this frame?
				bool leftTriggerDown = leftTriggerIsPressed && !lastFrameLeftTrigger;
				bool rightTriggerDown = rightTriggerIsPressed && !lastFrameRightTrigger; 
				
				// dash if left trigger is down
				if (leftTriggerDown)
				{
					transform.localScale = new Vector2(-1,1);
					dashRoutine = StartCoroutine(Dash(-1f));
				}

				// dash if right trigger is down
				if (rightTriggerDown)
				{
					transform.localScale = Vector3.one;
					dashRoutine = StartCoroutine(Dash(1f));
				}
			}
		}

		lastFrameLeftTrigger = leftTriggerIsPressed;
		lastFrameRightTrigger = rightTriggerIsPressed;
	}
    
	//Dash Function
	IEnumerator Dash(float direction)
	{
		FindObjectOfType<AudioManager>().Play("Dash");
		anim.SetBool("Dashing", true);
		body.velocity = new Vector2(dashSpeed * direction, 0f);
		
		yield return new WaitForSeconds(dashDuration);

		dashCooldownTimer = 0;
		dashRoutine = null;
		anim.SetBool("Dashing", false);
	}

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

	public void RunSound()
    {
        FindObjectOfType<AudioManager>().Play("Run");
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