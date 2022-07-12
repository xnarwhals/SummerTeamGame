using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BouncePad : MonoBehaviour
{
    [SerializeField] private float LaunchForce;
 
    private void OnCollisionEnter2D(Collision2D collision)
        {
            if(collision.gameObject.CompareTag("Player"))
            {
                collision.gameObject.GetComponent<Rigidbody2D>().AddForce(Vector2.up * LaunchForce, ForceMode2D.Impulse);
            }                                
        }

}
