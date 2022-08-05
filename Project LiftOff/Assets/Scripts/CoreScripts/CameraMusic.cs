using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class CameraMusic : MonoBehaviour
{
    public GameObject currentScene;
    // Start is called before the first frame update
    void Start()
    {
        if (currentScene.name == "StartingScreen")
        {
            FindObjectOfType<AudioManager>().Play("MenuTheme");
        }
        else
        {
            FindObjectOfType<AudioManager>().Stop("MenuTheme");
            FindObjectOfType<AudioManager>().Play("BackgroundMusic");
        }

        
    }
}
