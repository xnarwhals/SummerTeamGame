using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPatrol : MonoBehaviour
{
    //refrences tha need to be assigned in Unity 
    [Header ("Patrol Poinnts")]
    [SerializeField] private Transform leftEdge;
    [SerializeField] private Transform rightEdge;

    [Header ("Enemy")]
    [SerializeField] private Transform enemy;

    [Header ("Movement parameters")]
    [SerializeField] private float speed; 
    private Vector3 initScale;
    private bool movingLeft;

    [Header ("Idle Behavior")]
    [SerializeField] private float idleDuration;
    private float idleTimer;


    [Header ("Enemy Animation")]
    [SerializeField] private Animator anim;


    private void Awake()
    {
        initScale = enemy.localScale;
    }

    private void OnDisable()
    {
        anim.SetBool("moving", false);
        //on disable is called/used anytime an object becomes disabled or destroyed 
    }

    private void Update()
    {
        if(movingLeft)
        {
            if(enemy.position.x >= leftEdge.position.x)
                MoveInDirection(-1);
            else
            {
                //Change Direction
                DirectionChange();
            }
        }
        else
        {
            if(enemy.position.x <= rightEdge.position.x)
                MoveInDirection(1);
                else
                {
                    //change direction
                    DirectionChange();
                }

            
        }

    }

    private void MoveInDirection(int _direction)
    {
        idleTimer = 0;
        anim.SetBool("move", true);
        //animation to make enemy visually move

        //step 1 face correct direction
        enemy.localScale = new Vector3(Mathf.Abs(initScale.x) * _direction,
         initScale.y, initScale.z);

        //step 2 move in said direction
        enemy.position = new Vector3(enemy.position.x + Time.deltaTime * _direction * speed,
            enemy.position.y, enemy.position.z);         
    }

    private void DirectionChange()
    {
        anim.SetBool("moving", false);
        idleTimer += Time.deltaTime;

        if(idleTimer > idleDuration)
            movingLeft = !movingLeft;

        movingLeft = !movingLeft;
        //! swaps the value to the opposite of what it is
    }
}
