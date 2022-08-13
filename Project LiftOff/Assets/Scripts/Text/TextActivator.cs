using UnityEngine;


public class TextActivator : MonoBehaviour, Interactable
{
    //put this script on the npc and make sure he has a reasonable collider hitbox
    [SerializeField] private TextObject textObject; 

    private void OnTriggerEnter2D(Collider2D other)
    {
        //does it have player tag and component 
        if (other.CompareTag("Player") && other.TryGetComponent(out RevisedMovement player))
        {
            player.Interactable = this;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player") && other.TryGetComponent(out RevisedMovement player))
        {
            if (player.Interactable is TextActivator textActivator && textActivator == this)
            {
                player.Interactable = null;
            }
        }

    }



    public void Interact(RevisedMovement player)
    {
        player.TextUI.ShowText(textObject);
    }
}
