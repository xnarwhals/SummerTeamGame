using UnityEngine;
using System.Collections;


public class Health : MonoBehaviour
{
    [Header ("Health")]
    [SerializeField] private float startingHealth;
    public float currentHealth;//{ get; private set; }//this is so no hackers get silly with your game :)
    private Animator anim;
    private bool dead;

    [Header("Components")]
    [SerializeField] private Behaviour[] components;

    [Header("DeathSound")]
    [SerializeField] private AudioClip deathSound; 

    private void Awake()
    {
        currentHealth = startingHealth; //health 1 = 1
        anim = GetComponent<Animator>(); //get animator
    }

    public void TakeDamage(float _damage) //takes in two parameters
    {
        currentHealth = Mathf.Clamp(currentHealth - _damage, 0, startingHealth);
        
        if (!dead)//if not dead = false?
        {
            foreach (Behaviour component in components)
                component.enabled = false;
            //deactivate all attached components this makes it more simple

            anim.SetBool("grounded", true);//if they die in the air the game acts as if they are on the ground so the animation still plays
            anim.SetTrigger("Die");//play dead animation when we have it 

            dead = true;
            SoundManager.instance.PlaySound(deathSound);
            // Player death stop movement
            // if (GetComponent<PlayerMovement>() != null)
            // GetComponent<PlayerMovement>().enabled = false;
        }
    }

    public void Respawn()
    {
        dead = false; // hes not dead anymore so.....
        startingHealth = 1;//add back 1 hp
        anim.ResetTrigger("Die");//make sure trigger is not active 
        anim.Play("Idle");//words have to be exact idle is normal state
        
        //Activate all attached component classes so he can move and interact 
        foreach (Behaviour component in components)
             component.enabled = true;
    }


}
