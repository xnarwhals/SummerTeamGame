using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    [Header("Respawn")]
    public GameObject[] Enemies;
    private Vector3 respawnPoint;

    private void Awake()
    {
        respawnPoint = transform.position;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Enemy")
        {
            //freeze player movement
            //wait 2 seconds
            //play death animation
            //play death noise
            transform.position = respawnPoint;
        }

    }    
}
