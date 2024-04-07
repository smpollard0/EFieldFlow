using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMEnu : MonoBehaviour {
    public static bool GameIsPaused = false;
    
    public GameObject pauseMenuUI;
    public GameObject controlsMenuUI;

    // Update is called once per frame
    void Update() {
        if (Input.GetKeyDown(KeyCode.Escape)){
            if (GameIsPaused){
                Resume();
            }
            else{
                Pause();
            }
        }
    }

    void Pause(){
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        GameIsPaused = true;
    }

    // Button functionalities
    public void Resume(){
        if (pauseMenuUI.activeSelf){
            pauseMenuUI.SetActive(false);
            Time.timeScale = 1f;
            GameIsPaused = false;
        }
        
    }

    public void QuitGame(){
        Time.timeScale = 1f;
        GameIsPaused = false;
        SceneManager.LoadScene(0);
    }
    public void LoadOptionsMenu(){
        Time.timeScale = 1f;
        // Come back and do this teehee
    }

    public void LoadControlsMenu(){
        controlsMenuUI.SetActive(true);
        pauseMenuUI.SetActive(false);
    }

    // Controls menu functionalities
    public void GoBack(){
        controlsMenuUI.SetActive(false);
        pauseMenuUI.SetActive(true);
    }
}
