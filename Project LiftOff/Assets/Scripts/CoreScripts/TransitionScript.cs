using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TransitionScript : MonoBehaviour
{
    bool loadingStarted = false;
    float secondsLeft = 0;

    void Start()
    {
        StartCoroutine(DelayLoadLevel(16));
    }

    IEnumerator DelayLoadLevel(float seconds)
    {
        secondsLeft = seconds;
        loadingStarted = true;
        do
        {
            yield return new WaitForSeconds(1);
        } while (--secondsLeft > 0);

        SceneManager.LoadScene(2);
    }

    void OnGUI(){
        if (loadingStarted){     
            GUIStyle style = new GUIStyle(); 
            style.normal.textColor = Color.yellow; 
            style.fontSize = 90;         
            style.alignment = TextAnchor.UpperRight;         
            GUI.Label(new Rect(Screen.width - 100, 0, 100, 40), secondsLeft.ToString(), style); 
        }        
    }
}
