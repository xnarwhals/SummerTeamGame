using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//In Unity add a animation event (respawn) at the end of the die animation (1 second after last animation)
//
public class PlayerRespawn : MonoBehaviour
{
    //[SerializeField] private AudioClip checkpointSound; //plays when hitting checkpoint
    private Transform currentCheckpoint; //storage of what the last checkpoint is 
    private Health playerHealth; 

    private void Awake()
    {
        playerHealth = GetComponent<Health>();
    }

    public void Respawn()
    {
        transform.position = currentCheckpoint.position; //move player to last checkpoint position
        //restore player health and reset animation

        playerHealth.Respawn(); //Restore player health and reset animation 

        //move camera to checkpoint room(for this to work checkpoint object has to be placed as a child of the room object )
        //Camera.main.GetComponent<CameraController>().MoveToNewRoom(currentCheckpoint.parent);
    
    }

    //Activate Checkpoint 
private void OnTriggerEnter2D(Collider2D collision)
{
    if(collision.transform.tag == "Checkpoint")
    {
        currentCheckpoint = collision.transform; //store the checkpoint you touched as the current one 
        //SoundManager.instance.PlaySound(checkpointSound);
        collision.GetComponent<Collider2D>().enabled = false;//Deactivate checkpoint collider so it cant be triggered by player again
        //Collider2D class encompasses all 2d collider as opposed to the Collider class
        //collision.GetComponent<Animator>().SetTrigger("appear");//trigger checkpoint animation
    }
}
}
