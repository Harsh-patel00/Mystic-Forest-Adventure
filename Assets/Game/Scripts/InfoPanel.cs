using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InfoPanel : MonoBehaviour
{
    [SerializeField] private Sprite selectedPageIcon;
    [SerializeField] private Sprite unselectedPageIcon;
    [SerializeField] private Button nextButton;
    [SerializeField] private Button prevButton;
    
    [SerializeField] private List<Image> pageIcons = new List<Image>();
    [SerializeField] private List<GameObject> pages = new List<GameObject>();
    

    private int _maxPage = 4;
    private int _currentPage = 1;

    private void OnEnable()
    {
        nextButton.onClick.AddListener(HandleNextClick);
        prevButton.onClick.AddListener(HandlePrevClick);
    }

    private void OnDisable()
    {
        nextButton.onClick.RemoveListener(HandleNextClick);
        prevButton.onClick.RemoveListener(HandlePrevClick);
    }

    private void Start()
    {
        pageIcons[0].sprite = selectedPageIcon;
        pages[0].SetActive(true);
    }

    private void HandlePrevClick()
    {
        SoundManager.instance.PlayButtonSound();

        if(_currentPage == 1) return;
        
        pageIcons[_currentPage-1].sprite = unselectedPageIcon;
        pageIcons[_currentPage-2].sprite = selectedPageIcon;
        
        pages[_currentPage-1].SetActive(false);
        pages[_currentPage-2].SetActive(true);
        
        _currentPage--;
    }

    private void HandleNextClick()
    {
        SoundManager.instance.PlayButtonSound();
        
        if(_currentPage == _maxPage) return;
        
        _currentPage++;
        pageIcons[_currentPage-1].sprite = selectedPageIcon;
        pageIcons[_currentPage-2].sprite = unselectedPageIcon;
        
        pages[_currentPage-1].SetActive(true);
        pages[_currentPage-2].SetActive(false);
    }
}
