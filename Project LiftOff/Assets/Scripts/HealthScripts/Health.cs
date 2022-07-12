using UnityEngine;
using System.Collections;


public class Health : MonoBehaviour
{
    [Header ("Health")]
    [SerializeField] private float startingHealth;
    public float currentHealth;//{ get; private set; }//this is so no hackers get silly with your game :)
    private Animator anim;
    private bool dead;

    [Header("iFrames")]
    [SerializeField] private float iFramesDuration;
    [SerializeField] private int numberOfFlashes;
    private SpriteRenderer spriteRend;

    [Header("Components")]
    [SerializeField] private Behaviour[] components;
    private bool invulnerable;
    
    [Header("DeathSound")]
    [SerializeField] private AudioClip deathSound; 
    [SerializeField] private AudioClip hurtSound;


    private void Awake()
    {
        currentHealth = startingHealth;
        anim = GetComponent<Animator>();
        spriteRend = GetComponent<SpriteRenderer>();
    }

    public void TakeDamage(float _damage) //takes in two parameters
    {
        if(invulnerable) return;
        currentHealth = Mathf.Clamp(currentHealth - _damage, 0, startingHealth);

        if (currentHealth > 0) 
        {
            anim.SetTrigger("Hurt");
            StartCoroutine(Invulnerability());
            //iframes^^^
            SoundManager.instance.PlaySound(hurtSound);
        }
        else
        {
            if (!dead)
            {
                

                // //Player death stop movement
                // if (GetComponent<PlayerMovement>() != null)
                //     GetComponent<PlayerMovement>().enabled = false;

                // //Enemy death stop movement
                // if (GetComponentInParent<EnemyPatrol>() != null)
                //     GetComponentInParent<EnemyPatrol>().enabled = false;

                // if (GetComponent<EnemyKnight>() != null)    
                //     GetComponent<EnemyKnight>().enabled = false;
                
                //deactivate all attached components this makes it more simple
                foreach (Behaviour component in components)
                    component.enabled = false;
                
                anim.SetBool("grounded", true);//if they die in the air the game acts as if they are on the ground so the animation still plays
                anim.SetTrigger("Die");

                dead = true;
                SoundManager.instance.PlaySound(deathSound);
            }
        }
    }
    public void AddHealth(float _value)
    {
        currentHealth = Mathf.Clamp(currentHealth + _value, 0, startingHealth);
    }


    public void Respawn()
    {
        dead = false; // hes not dead anymore so.....
        AddHealth(startingHealth);
        anim.ResetTrigger("die");//make sure trigger is not active 
        anim.Play("idle");//words have to be exact 
        StartCoroutine(Invulnerability()); //player cant get hurt after spawning

        //Ativate all attached component classes
        foreach (Behaviour component in components)
             component.enabled = true;
    }

    private IEnumerator Invulnerability()
    {
        //invulnerable to layer 11 (Enemy) 
        invulnerable = true;
        Physics2D.IgnoreLayerCollision(10, 11, true);
        for (int i = 0; i < numberOfFlashes; i++)
        {
            spriteRend.color = new Color(1,0,0, 0.5f);
            yield return new WaitForSeconds(iFramesDuration / (numberOfFlashes * 2));
            spriteRend.color = Color.white;
            yield return new WaitForSeconds(iFramesDuration / (numberOfFlashes * 2));
        } 
        //not invulnerable 
        Physics2D.IgnoreLayerCollision(10, 11, false);
        invulnerable = false;
    }

    private void Deactiavte()
    {
        gameObject.SetActive(false);
    }

}
