using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScreenManager : MonoBehaviour
{
    public static ScreenManager instance;
    
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        if (PlayerPrefs.HasKey("ShowSplashScreen"))
        {
            if (PlayerPrefs.GetInt("ShowSplashScreen") == 1)
            {
                splashScreenCanvas.enabled = true;
                gameplayCanvasGroup.interactable = false;
            }
            else
            {
                splashScreenCanvas.enabled = false;
                gameplayCanvasGroup.interactable = true;
            }
        }
    }

    [SerializeField] private CanvasGroup gameplayCanvasGroup;
    [SerializeField] private Canvas splashScreenCanvas;
    [SerializeField] private Toggle showSplashScreenToggle;
    [SerializeField] private Button continueButton;

    private void OnEnable()
    {
        showSplashScreenToggle.onValueChanged.AddListener(HandleToggleChange);
        continueButton.onClick.AddListener(HandleContinueClick);
    }

    private void OnDisable()
    {
        showSplashScreenToggle.onValueChanged.RemoveListener(HandleToggleChange);
        continueButton.onClick.RemoveListener(HandleContinueClick);
    }

    private void HandleToggleChange(bool isOn)
    {
        SoundManager.instance.PlayButtonSound();
    }
    
    private void HandleContinueClick()
    {
        splashScreenCanvas.enabled = false;
        gameplayCanvasGroup.interactable = true;
        
        PlayerPrefs.SetInt("ShowSplashScreen", showSplashScreenToggle.isOn ? 0 : 1);
    }
}
