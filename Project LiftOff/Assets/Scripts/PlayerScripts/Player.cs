using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// using UnityEngine.InputSystem;

// public class Player : MonoBehaviour
// {
//     //refrences 
//     public BoxCollider2D boxCollider;
//     public Animator anim;
//     public Rigidbody2D player;
//     public Transform groundCheck;
//     public LayerMask groundLayer;

//     [Header("Movement Parameters")]
//     private float horizontal;
//     public float speed = 8f;
//     private bool isFacingRight = true;
    
//     public float jumpingPower = 16f;


//     private void Update()
//     {   
//         if (horizontal != 0)
//         {
//             anim.SetBool("Running", true);
//             player.velocity = new Vector2(horizontal * speed, player.velocity.y);
//         }
//         else
//         {
//             anim.SetBool("Running", false);
//             return;
//         }
        
//         if(!isFacingRight && horizontal > 0f)
//         {
//             Flip();
//         }
//         else if (isFacingRight && horizontal < 0f)
//         {
//             Flip();
//         }
//     }

//     public void Jump (InputAction.CallbackContext context)
//     {
//         //normal jump 
//         if (context.performed && IsGrounded())
//         {
//             player.velocity = new Vector2(player.velocity.x, jumpingPower);
//         }

//         //adjustable height
//         if (context.canceled && player.velocity.y > 0f)
//         {
//             player.velocity = new Vector2(player.velocity.x, player.velocity.y * 0.5f);
//         }
//     }

//     private bool IsGrounded()
//     {
//         return Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);
//     }

//     private void Flip()
//     {
//         isFacingRight = !isFacingRight;
//         Vector3 localScale = transform.localScale;
//         localScale.x *= -1f;
//         transform.localScale = localScale;
//     }

//     public void Move (InputAction.CallbackContext context)
//     {
//         horizontal = context.ReadValue<Vector2>().x;
//     }


// }
