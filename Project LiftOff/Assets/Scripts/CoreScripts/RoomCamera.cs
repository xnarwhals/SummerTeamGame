using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomCamera : MonoBehaviour
{
    public GameObject virtualCam;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Player") && !other.isTrigger)//Compare tag same as collison.tag == "Player"
        {
            virtualCam.SetActive(true);
        }    
    }
    
    private void OnTriggerExit2D(Collider2D other)
    {
        if(other.CompareTag("Player") && !other.isTrigger)//Compare tag same as collison.tag == "Player"
        {
            virtualCam.SetActive(false);
        }    
    }

}
