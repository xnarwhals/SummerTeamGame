using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallJump : MonoBehaviour
{
    // [Header("WallJump")]
    // [SerializeField] private float wallJumpTime; //avoid slugish gameplay (0.2f) by giving a certain amount of time to perform jump
    // [SerializeField] private float wallSlideSpeed; //0.3f
    // [SerializeField] private float wallDistance; // 0.5f how far does wall have to be from collider to perform jump
    // private bool isWallSliding = false;
    // RaycastHit2D WallCheckHit;
    // float jumpTime;
    

    // private void Update()
    // {
    //     if (isFacingRight)
    //     {   //check the player position (parameter 1) second parameter is casting the Raycast towards right wall
    //         WallCheckHit = Physics2D.Raycast(transform.position, new Vector2(wallDistance, 0), wallDistance, groundLayer);
    //     }
    //     else//cast left wall
    //     {
    //         WallCheckHit = Physics2D.Raycast(transform.position, new Vector2(-wallDistance, 0), wallDistance, groundLayer);
    //     }
           
        
    //     if (WallCheckHit && !isGrounded && horizontalInput != 0)
    //     {              
    //         isWallSliding = true;//logic
    //         //buffer time to reduce slugish time 
    //         jumpTime = Time.time + wallJumpTime;
    //     }
    //     else if(jumpTime < Time.time)
    //         isWallSliding = false;//if jumpTime is less than current time return false   
    //         //wallJumptime essentially acts as coyote time! Add it to normal jump!    
        
    //     if (isWallSliding)
    //     {
    //         //slow down player logic
    //         body.velcoity = new Vector2(body.velocity.x, Mathf.Clamp(body.velocity.y, wallSlideSpeed, float.MaxValue));
    //         //parameter 1 of New Vector 2 is the x position of player
    //         //parameter 2 is clamp (y position) that takes players y position, speed of sliding down (min), and a max value
    //     }
    // }
}
