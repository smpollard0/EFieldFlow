using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SimulationsMenu : MonoBehaviour
{
    public void StartEFieldSimulation(){
        SceneManager.LoadSceneAsync(2);
    }
}
