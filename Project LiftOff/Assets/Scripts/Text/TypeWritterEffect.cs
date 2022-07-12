using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TypeWritterEffect : MonoBehaviour
{
    [SerializeField] private float typeSpeed;

    //responsible for running the text 
    //First parameter is the text we want to type second is where
    //we want the text to be typed on 
    public Coroutine Run(string textToType, TMP_Text textLable)
    {
        return StartCoroutine(TypeText(textToType, textLable));

    }

    //responsible for typing the text 
    private IEnumerator TypeText(string textToType, TMP_Text textLable)
    {
        textLable.text = string.Empty;//leave box blank at start of game

        float t = 0; //elapsed time since typing
        int charIndex = 0; //also intialized 

        while (charIndex < textToType.Length)
        {
            t += Time.deltaTime * typeSpeed;//increment time with speed value
            charIndex = Mathf.FloorToInt(t); //store floor value of updating timer
            charIndex = Mathf.Clamp(charIndex, 0, textToType.Length);//makes sure value is never greater than text to type 

            textLable.text = textToType.Substring(0, charIndex);//physcial typing code

            yield return null;
        }

        textLable.text = textToType;
    }
}
