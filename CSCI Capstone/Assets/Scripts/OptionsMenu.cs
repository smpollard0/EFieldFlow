using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class OptionsMenu : MonoBehaviour
{

    public TMPro.TMP_Dropdown resolutionDropdown;
    Resolution[] resolutions;
    // this function is called when the scene loads
    void Start() {
        // this method basically has unity figure out what resolutions are allowed for the user's current monitor
        resolutions = Screen.resolutions;

        // clear all options currently in the resolutions dropdown menu
        resolutionDropdown.ClearOptions();

        // create a list of strings which represent the resolutions to add
        List<string> options = new List<string>();

        int currentResolutionIndex = 0;

        // iterate through the resolutions and turn them into strings to put into options
        for (int i = 0; i < resolutions.Length; i++){
            string option = resolutions[i].width + " x " + resolutions[i].height;
            options.Add(option);

            if (resolutions[i].width == Screen.currentResolution.width && 
                resolutions[i].height == Screen.currentResolution.height){
                currentResolutionIndex = i;
            }
        }

        // add the list of strings that represent the options to the dropdown menu
        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = currentResolutionIndex;
        resolutionDropdown.RefreshShownValue();
    }

    public void SetResolution(int resolutionIndex){
        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }

    // this function sets the window to be fullscreen based on the current status of the toggle
    public void SetFullscreen(bool isFullscreen) {
        Screen.fullScreen = isFullscreen;
    }
}
