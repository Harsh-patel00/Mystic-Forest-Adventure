using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class SpinButton : MonoBehaviour
{
    [SerializeField] private Button button;
    
    [Header("Spin Button")] 
    [SerializeField] private Sprite spinIdle;
    [SerializeField] private Sprite spinHover;
    [SerializeField] private Sprite spinPressed;
    [SerializeField] private Sprite spinDisabled;
    
    [Header("Stop Button")]
    [SerializeField] private Sprite stopIdle;
    [SerializeField] private Sprite stopHover;
    [SerializeField] private Sprite stopPressed;
    [SerializeField] private Sprite stopDisabled;

    [SerializeField] private bool _isSpinning = false;
    
    public Action<bool> OnSpin;
    
    protected void OnEnable()
    {
        GetComponent<Image>().sprite = spinIdle;
        
        var state = button.spriteState;
        state.highlightedSprite = spinHover;
        state.pressedSprite = spinPressed;
        state.disabledSprite = spinDisabled;
        button.spriteState = state;
        
        button.onClick.AddListener(HandleSpinClick);
    }

    private void OnDisable()
    {
        button.onClick.RemoveListener(HandleSpinClick);
    }

    private void HandleSpinClick()
    {
        SoundManager.instance.PlayButtonSound();
        
        if (_isSpinning)
        {
            SetSpritesToStartSpin();
            _isSpinning = false;
            // This is where it'll signal stop spin
            OnSpin?.Invoke(false);
        }
        else
        {
            SetSpritesToStopSpin();
            _isSpinning = true;
            OnSpin?.Invoke(true);
        }
    }

    private void SetSpritesToStartSpin()
    {
        GetComponent<Image>().sprite = spinIdle;
        
        var state = button.spriteState;
        state.highlightedSprite = spinHover;
        state.pressedSprite = spinPressed;
        state.disabledSprite = spinDisabled;
        button.spriteState = state;
    }

    private void SetSpritesToStopSpin()
    {
        GetComponent<Image>().sprite = stopIdle;
        
        var state = button.spriteState;
        state.highlightedSprite = stopHover;
        state.pressedSprite = stopPressed;
        state.disabledSprite = stopDisabled;
        button.spriteState = state;
    }

    public void SetSlotSpinningState(bool isSpinning)
    {
        _isSpinning = isSpinning;

        if (_isSpinning)
        {
            SetSpritesToStopSpin();
        }
        else
        {
            SetSpritesToStartSpin();
        }
    }

    public void SetButtonInteractable(bool isInteractable)
    {
        button.interactable = isInteractable;
    }
}
