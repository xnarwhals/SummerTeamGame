using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Respawn : MonoBehaviour
{
    [Header("Respawn")]
    private Vector3 respawnPoint; //changes as progress is made

    private void Awake()
    {
        respawnPoint = transform.position;
    }
    
    private void OnTriggerEnter2D(Collider2D collision)//respawn
    {
        if (collision.tag == "Enemy")
        {
            transform.position = respawnPoint;        
        }
        else if (collision.tag == "Checkpoint")
        {
            respawnPoint = transform.position;
        }
    } 
}
