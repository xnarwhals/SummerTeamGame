using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdateFallingPlatform : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private float fallDelay;
    [SerializeField] private float respawnTime;
    [SerializeField] private GameObject RespawnPoint;

    private bool isFalling = false;
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(isFalling = true)
            return;
        else if(collision.gameObject.CompareTag("Player"))
        {
            StartCoroutine(Fall());
        }
    }

    private IEnumerator Fall()
    {
        isFalling = true;
        rb.bodyType = RigidbodyType2D.Dynamic;
        yield return new WaitForSeconds(fallDelay);
        yield return new WaitForSeconds(respawnTime);
        transform.position = new Vector3(RespawnPoint.transform.position.x, RespawnPoint.transform.position.y, 0);
        rb.bodyType = RigidbodyType2D.Kinematic;
        isFalling = false;
    }
}
