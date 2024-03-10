using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SimulationsMenu : MonoBehaviour
{
    // this function uses the scene manager to change scene and go to the efield simulation new/load menu
    public void EFieldNewOrLoad(){
        SceneManager.LoadSceneAsync(4);
    }

    // this function uses the scene manager to change scene and go back to the main menu
    public void GoBack(){
        SceneManager.LoadSceneAsync(0);
    }
}
