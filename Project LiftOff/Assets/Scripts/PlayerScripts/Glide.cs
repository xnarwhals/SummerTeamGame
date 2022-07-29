using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Glide : MonoBehaviour
{
    [SerializeField] private float glidingGravity;
    private float _initialGravityScale;
    private Rigidbody2D _ridgidbody;
    
    //at the start of game we get the players ridgidbody 
    //and set the private float for gravity scale to the same as
    //the gravity scale of the ridgidbody
    private void Start()
    {
        _ridgidbody = GetComponent<Rigidbody2D>();
        _initialGravityScale = _ridgidbody.gravityScale;
    }

    //every frame we do whats inside :) 
    private void Update()
    {
        //new variable that is the equal to the Inmput button for jump
        var glidingInput = Input.GetButton("Glide");

        //if space is pressed and the y velocity is less than
        //or equal to zero, we set the ridgidbody gravity to zero 
        //and then give the body a velocity of its current x velocicty 
        //with the gliding speed going down for y 
        if (glidingInput && _ridgidbody.velocity.y <= 0)
        {
            _ridgidbody.gravityScale = 0;
            _ridgidbody.velocity = new Vector2(_ridgidbody.velocity.x, -glidingGravity);
        }
        else    //if not gliding the gravity of the player is normal 
        {
            _ridgidbody.gravityScale = _initialGravityScale;
        }
    }
}
