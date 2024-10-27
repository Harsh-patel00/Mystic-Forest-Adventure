using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BetManager : MonoBehaviour
{
    private float _minBetAmount = 0.1f;
    private float _maxBetAmount = 25.0f;
    private readonly float _betAmountStep = 0.1f;
    
    [SerializeField] private float balanceAmount = 100;
    
    [SerializeField] private Button increaseBetButton;
    [SerializeField] private Button decreaseBetButton;
    
    [SerializeField] private TMP_Text betAmountText;
    [SerializeField] private TMP_Text balanceAmountText;

    public float CurrentBetAmount { get; private set; }

    private void Awake()
    {
        EventManager.SpinStart += HandleSpinStart;
        EventManager.SpinStop += HandleSpinStop;
    }

    private void OnEnable()
    {
        increaseBetButton.onClick.AddListener(HandleIncreaseBet);
        decreaseBetButton.onClick.AddListener(HandleDecreaseBet);
    }

    private void OnDisable()
    {
        increaseBetButton.onClick.RemoveListener(HandleIncreaseBet);
        decreaseBetButton.onClick.RemoveListener(HandleDecreaseBet);
    }

    private void Start()
    {
        CurrentBetAmount = _minBetAmount;
        
        betAmountText.text = CurrentBetAmount.ToString("F1", CultureInfo.InvariantCulture);
        balanceAmountText.text = balanceAmount.ToString("F2", CultureInfo.InvariantCulture);
    }

    private void HandleIncreaseBet()
    {
        SoundManager.instance.PlayButtonSound();
        
        if ((CurrentBetAmount + _betAmountStep) >= _maxBetAmount)
        {
            CurrentBetAmount = _maxBetAmount;
        }
        else if (CurrentBetAmount + _betAmountStep >= balanceAmount)
        {
            CurrentBetAmount = balanceAmount;
        }
        else
        {
            CurrentBetAmount += _betAmountStep;
        }
        
        betAmountText.text = CurrentBetAmount.ToString("F1", CultureInfo.InvariantCulture);
    }
    
    private void HandleDecreaseBet()
    {
        SoundManager.instance.PlayButtonSound();
        
        if ((CurrentBetAmount - _betAmountStep) <= _minBetAmount)
        {
            CurrentBetAmount = _minBetAmount;
        }
        else
        {
            CurrentBetAmount -= _betAmountStep;
        }
        
        betAmountText.text = CurrentBetAmount.ToString("F1", CultureInfo.InvariantCulture);
    }

    public void UpdateBalanceAmount()
    {
        if (balanceAmount - CurrentBetAmount <= 0)
        {
            balanceAmount = 0;
        }
        else
        {
            balanceAmount -= CurrentBetAmount;
        }
        
        balanceAmountText.text = balanceAmount.ToString("F2", CultureInfo.InvariantCulture);
    }
    
    public void UpdateBalanceAmount(float amountToAdd)
    {
        balanceAmount += amountToAdd;
        balanceAmountText.text = balanceAmount.ToString("F2", CultureInfo.InvariantCulture);
    }
    
    private void HandleSpinStop()
    {
        increaseBetButton.interactable = true;
        decreaseBetButton.interactable = true;
    }

    private void HandleSpinStart()
    {
        increaseBetButton.interactable = false;
        decreaseBetButton.interactable = false;
    }
}
