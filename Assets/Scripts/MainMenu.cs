using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    
    // Options
    public GameObject OptionsScreen;
    //public GameObject OptionsMenu;

    
    
    public string firstLevelName;

    public void StartGame()
    {
        SceneManager.LoadScene(firstLevelName);
        Time.timeScale = 1f;
    }

    
    
    
    public void ShowHideOptions()
    {
        if (!OptionsScreen.activeInHierarchy)
        {
            OptionsScreen.SetActive(true);
        }
        else
        {
            OptionsScreen.SetActive(false);
        } 
        /*if (!OptionsMenu.activeInHierarchy)
        {
            //OptionsMenu.SetActive(true);
        }
        else
        {
            OptionsMenu.SetActive(false);
        }*/
    }
    
    public void OpenOption()
    {
        OptionsScreen.SetActive(true);
    }

    public void CloseOption()
    {
        OptionsScreen.SetActive(false);
    }
    
    
    
    public void Options()
    {
        // Add Options Here`                     
        Debug.Log("No Options YETTT?"); 
        ShowHideOptions();
    }
    
    /*if (OptionsScreen.activeInHierarchy && Cursor.lockState != CursorLockMode.None)
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }*/ 
        
        
        

    public void QuitGame()
    {
        Application.Quit();
        
        Debug.Log("I'm Quitting");
    }

}
