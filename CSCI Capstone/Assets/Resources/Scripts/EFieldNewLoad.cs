using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EFieldNewLoad : MonoBehaviour
{
    // this function uses the scene manager to change scene and go to the efield simulation
    public void StartEFieldSimulation(){
        SceneManager.LoadSceneAsync(2);
    }

    // this function uses the scene manager to change scene and go back to the main menu
    public void GoBack(){
        SceneManager.LoadSceneAsync(1);
    }
}
