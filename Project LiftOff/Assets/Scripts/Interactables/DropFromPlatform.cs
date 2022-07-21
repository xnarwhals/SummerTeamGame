using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropFromPlatform : MonoBehaviour
{
    private PlatformEffector2D effector;
    [SerializeField] private float waitTime;

    private void Start()
    {
        effector = GetComponent<PlatformEffector2D>();
    }

    private void Update()
    {

        if (Input.GetKey(KeyCode.S))
        {
            if( waitTime <= 0)
            {
                effector.rotationalOffset = 180f;
                waitTime = waitTime;
            }
            else
            {
                waitTime -= Time.deltaTime;
            }
        }

        if (Input.GetKey(KeyCode.Q))
        {
            effector.rotationalOffset = 0;
        }
    }
}
