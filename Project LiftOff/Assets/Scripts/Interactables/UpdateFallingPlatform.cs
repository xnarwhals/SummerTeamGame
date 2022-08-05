using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdateFallingPlatform : MonoBehaviour
{
    private Rigidbody2D rb;

    private float timeBeforeFall = 1.2f;
 
    private float timeBeforeRespawn = 2f;
 
    private bool isFalling = false;
 
    private Vector2 initialposition;
 

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    private void Start()
    {
         initialposition = transform.position;
    }
 
    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            Invoke("Fall", timeBeforeFall);
        }
    }
 
    void Fall()
    {
        rb.isKinematic = false;
        isFalling = true;
    }
   
    void respawn()
    {
        StartCoroutine(respawnF());
    }
 
    IEnumerator respawnF()
    {
        yield return new WaitForSeconds(timeBeforeRespawn);
        isFalling = false;
        rb.isKinematic = true;
        transform.position = initialposition;
        rb.velocity = Vector2.zero;
    }
 
    void OnTriggerEnter2D(Collider2D col)
    {
        if(col.tag == "Enemy")
        {
            respawn();
        }
    }
}

