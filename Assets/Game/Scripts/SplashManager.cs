using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SplashManager : MonoBehaviour
{
    [SerializeField] private Image logo;
    [SerializeField] private Slider loadingSlider;
    [SerializeField] private GameObject loadingScreen;
    private IEnumerator Start()
    {
        float timer = 1f;
        Color logoColor = logo.color;
        Color logoColorFaded = logo.color;
        logoColorFaded.a = 1;
        
        // Logo Fade-In
        while (timer > 0)
        {
            timer -= Time.deltaTime;
            logo.color = Color.Lerp(logoColorFaded, logoColor, timer);
            yield return new WaitForEndOfFrame();
        }

        yield return new WaitForSeconds(1f);
        
        timer = 2f;

        // Logo Fade-Out
        while (timer > 0)
        {
            timer -= Time.deltaTime;
            logo.color = Color.Lerp(logoColor, logoColorFaded, timer);
            yield return new WaitForEndOfFrame();
        }

        loadingScreen.SetActive(true);
        StartCoroutine(ShowLoadingScreen());
    }

    private IEnumerator ShowLoadingScreen()
    {
        float timer = 5f;

        while (timer > 0)
        {
            timer -= Time.deltaTime;
            loadingSlider.value += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        
        SceneManager.LoadScene("MainScene");
    }
}
