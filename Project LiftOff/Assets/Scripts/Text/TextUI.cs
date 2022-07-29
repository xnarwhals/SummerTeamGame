using TMPro;
using UnityEngine;
using System.Collections;

public class TextUI : MonoBehaviour
{
    [SerializeField] private GameObject textBox;
    [SerializeField] private TMP_Text textLable;
    
    public bool IsOpen { get; private set; }

    //Refrences
    private ResponseTemplate responseTemplate;
    private TypeWritterEffect typeWritterEffect;//case sensitve pay attention to the T and t

    private void Start()
    {
        responseTemplate = GetComponent<ResponseTemplate>();
        typeWritterEffect = GetComponent<TypeWritterEffect>();

        CloseText();//hide text until we want to show it
        
    }

    public void ShowText(TextObject textObject)
    {
        IsOpen = true; //set bool to true
        textBox.SetActive(true);
        StartCoroutine(StepThroughText(textObject));
    }

    private IEnumerator StepThroughText(TextObject textObject)
    {
        for (int i = 0; i < textObject.Text.Length; i++)
        {
            string text = textObject.Text[i];
            yield return typeWritterEffect.Run(text, textLable);

            //check if at end of text
            if(i == textObject.Text.Length - 1 && textObject.HasResponses) break;//stop the loop

            yield return new WaitUntil(() => Input.GetButtonDown("Interact"));
            //^remember to change the key code or button to the one they want 
        }

        if (textObject.HasResponses)
        {
            responseTemplate.ShowResponses(textObject.Responses);
        }
        else//if theres a response avliable show it else close the textBox
        {
            CloseText();//once the for loop is finished do close text    
        }

        
    }

    private void CloseText()
    {
        IsOpen = false;
        textBox.SetActive (false);//deactivate the text box
        textLable.text = string.Empty;//empty the words in the lable
    }
}
