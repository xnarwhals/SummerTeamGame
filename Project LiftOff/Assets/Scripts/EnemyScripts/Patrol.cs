using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Patrol : MonoBehaviour
{
    [SerializeField] private Transform groundDetection;
    [SerializeField] private float speed;
    [SerializeField] private float RaycastLength;
    private bool movingRight = true;
    
    private void Update()
    {
        transform.Translate(Vector2.right * speed * Time.deltaTime);

        RaycastHit2D groundInfo = Physics2D.Raycast(groundDetection.position, Vector2.down, RaycastLength);//takes orgin, direction, distance
        Debug.DrawRay(groundDetection.position, Vector2.down, Color.blue);
        if(groundInfo.collider == false)
        {
            if(movingRight == true)
            {
                transform.eulerAngles = new Vector3(0, -180, 0);// one way of moving left is to do a 180 rotation on the y axis 
                movingRight = false;
            }
            else
            {
                transform.eulerAngles = new Vector3(0, 0, 0);
                movingRight = true;
            }
        }
    }
}
