using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{

    public void Update()
    {
        if (Input.GetButton("Start"))//and i want the current scene to have the name "StartingScreen"
        {
            LoadVillageView();
        }
    }

    public void LoadVillageView (){
        SceneManager.LoadScene("VillageOverview");
    }
}
