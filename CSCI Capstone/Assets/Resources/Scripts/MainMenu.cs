using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    // this function uses the scene manager to change the scene to view the simulation select screen
    public void ViewSimulations(){
        SceneManager.LoadSceneAsync(1);
    }

    // this function uses the scene manager to change the scene to view the options
    public void ViewOptions(){
        SceneManager.LoadSceneAsync(3);
    }

    // this function closes the application
    public void QuitGame(){
        Application.Quit();
    }
}
