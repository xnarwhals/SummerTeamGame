using UnityEngine;


[CreateAssetMenu(menuName = "Text/TextObject")]//creating that menu option for text
public class TextObject : ScriptableObject
{
    [SerializeField] [TextArea] private string[] text;
    [SerializeField] private Response[] responses;

    //make TextUI code less crowded with the boolean 
    public bool HasResponses => Responses != null && Responses.Length > 0;
    public string[] Text => text;
    public Response[] Responses => responses;
}
