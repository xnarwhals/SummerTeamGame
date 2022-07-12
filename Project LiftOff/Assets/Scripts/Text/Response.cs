using UnityEngine;

[System.Serializable]

public class Response
{
    [SerializeField] private string responseText;
    [SerializeField] private TextObject textObject;

    public string ResponseText => responseText;
    public TextObject TextObject => textObject;


}
