using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingPlatform : MonoBehaviour
{
    private Rigidbody2D platform;
    [SerializeField] private float timeBeforeFall;//after the collision eneter
    [SerializeField] private float timeBeforeGone;//after alloted time, platform dies

    private void Awake()
    {
        platform = GetComponent<Rigidbody2D>();
    }

    private void OnCollisionEnter2D (Collision2D col)
    {
        if (col.gameObject.tag == "Player")
        {
            PlatformManager.Instance.StartCoroutine("SpawnPlatform", new Vector2(transform.position.x, transform.position.y));
            Invoke("Falling", timeBeforeFall);
            Destroy(gameObject, timeBeforeGone);
        }     
    }
    
    private void Falling()
    {
        platform.isKinematic = false;
    }
}
