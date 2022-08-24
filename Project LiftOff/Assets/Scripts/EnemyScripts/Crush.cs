using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crush : MonoBehaviour
{
    [SerializeField] private Animator anim;

    private void OnTriggerEnter2D(Collider2D coll)
    {
        if(coll.gameObject.CompareTag("Player"))
        {
            anim.SetTrigger("Crush");
        }
    }

    private void OnTriggerExit2D(Collider2D coll)
    {
        anim.ResetTrigger("Crush");
    }

    
}
