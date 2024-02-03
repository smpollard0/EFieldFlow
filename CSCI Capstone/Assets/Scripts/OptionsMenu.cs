using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;

public class OptionsMenu : MonoBehaviour
{

    public TMPro.TMP_Dropdown resolutionDropdown;
    private Resolution[] resolutions;
    private List<Resolution> filteredResolutions;
    private double currentRefreshRate;
    int currentResolutionIndex = 0;

    // this function is called when the scene loads
    void Start() {
        // this method basically has unity figure out what resolutions are allowed for the user's current monitor
        resolutions = Screen.resolutions;
        filteredResolutions = new List<Resolution>();

        // clear all options currently in the resolutions dropdown menu
        resolutionDropdown.ClearOptions();
        currentRefreshRate = Screen.currentResolution.refreshRateRatio.value;

        // iterate through the resolutions and turn them into strings to put into options
        for (int i = 0; i < resolutions.Length; i++){
            if (resolutions[i].refreshRateRatio.value == currentRefreshRate){
                filteredResolutions.Add(resolutions[i]);
            }
        }

        List<string> options = new List<string>();

        for (int i = 0; i < filteredResolutions.Count; i++){
            string resolutionOption = filteredResolutions[i].width + " x " + filteredResolutions[i].height + " " + Math.Round(filteredResolutions[i].refreshRateRatio.value) + " Hz";
            options.Add(resolutionOption);
            if (filteredResolutions[i].width == Screen.width && 
                filteredResolutions[i].height == Screen.height){
                currentResolutionIndex = i;
            }
        }

        // add the list of strings that represent the options to the dropdown menu
        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = currentResolutionIndex;
        resolutionDropdown.RefreshShownValue();
    }

    public void SetResolution(int resolutionIndex){
        Resolution resolution = filteredResolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }

    // this function sets the window to be fullscreen based on the current status of the toggle
    public void SetFullscreen(bool isFullscreen) {
        Screen.fullScreen = isFullscreen;
    }
}
