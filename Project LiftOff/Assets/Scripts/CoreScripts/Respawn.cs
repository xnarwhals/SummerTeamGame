using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Respawn : UpdateFallingPlatform
{
    [Header("Respawn")]
    private Vector3 respawnPoint; //changes as progress is made
    public GameObject fallDetector;

    private void Awake()
    {
        respawnPoint = transform.position;
    }
    
    private void Update()
    {
        fallDetector.transform.position = new Vector2(transform.position.x, fallDetector.transform.position.y); 
    }

    private void OnTriggerEnter2D(Collider2D collision)//respawn
    {
        if (collision.tag == "Enemy")
        {
            FindObjectOfType<AudioManager>().Play("Death");
            transform.position = respawnPoint; 

        }
        else if (collision.tag == "Checkpoint")
        {
            FindObjectOfType<AudioManager>().Play("Death");
            respawnPoint = transform.position;
        } 
    }
}
