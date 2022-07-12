using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class ResponseTemplate : MonoBehaviour
{
    [SerializeField] private RectTransform responseBox;
    [SerializeField] private RectTransform responseButtonTemplate;
    [SerializeField] private RectTransform responseContainer;

    private List<GameObject> tempResponseButtons = new List<GameObject>();
    //refrences
    private TextUI textUI;

    private void Start()
    {
        textUI = GetComponent<TextUI>();
    }

    //function to display responses takes an index of the Response Array and the responses
    public void ShowResponses(Response[] responses)
    {
        float responseBoxHeight = 0;
        //^the height changes with each response 
        //increments of 50 pixles

        foreach (Response response in responses)
        {
            //this code can just be done manually in unity but we love code so...
            GameObject responseButton = Instantiate(responseButtonTemplate.gameObject, responseContainer);
            responseButton.gameObject.SetActive(true);
            responseButton.GetComponent<TMP_Text>().text = response.ResponseText;
            responseButton.GetComponent<Button>().onClick.AddListener(() => OnPickedResponse(response));
            //^add event call back when button is clcked

            tempResponseButtons.Add(responseButton);
            //^everytime we create a button it gets added to the list
            responseBoxHeight += responseButtonTemplate.sizeDelta.y;
        }

        responseBox.sizeDelta = new Vector2(responseBox.sizeDelta.x, responseBoxHeight);
        responseBox.gameObject.SetActive(true);
        //^after the foreach loop is done this code
        //physically changes the height of the response box
    }

    //when the response is clicked we deactiavte the box and destroy the temporary buttons
    public void OnPickedResponse(Response response)
    {
        responseBox.gameObject.SetActive(false);

        foreach (GameObject button in tempResponseButtons)
        {
            Destroy(button);
        }
        tempResponseButtons.Clear();
        //reset the temporary list 

        textUI.ShowText(response.TextObject);
    }
}
