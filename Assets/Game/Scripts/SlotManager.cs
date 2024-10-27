using System;
using System.Collections;
using System.Collections.Generic;
using Spine;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class SlotManager : MonoBehaviour
{
    [Header("Spin")]
    [SerializeField] private float spinSpeed = 1f;   
    [SerializeField] private List<SlotSpinner> slotSpinners = new List<SlotSpinner>();
    [SerializeField] private SpinButton spinButton;
    
    [Header("Payout")]
    [SerializeField] private PaylineManager paylineManager;
    
    [SerializeField] private DataScriptableObject gameDataSO;
    
    private int[,] _slots = new int[5,3];
    private const int _numberOfSymbols = 10; // L(1-5), H(1-4), Wild, Bonus, Scatter
    private int _slotStopCounter = 0; // Used to check whether all slots are stopped or not
    private Coroutine _slotSpinRoutine;
    
    private Symbol[,] _resultSlots = new Symbol[5,3];
    
    private void OnEnable()
    {
        spinButton.OnSpin += HandleSpinButtonClick;
    }

    private void OnDisable()
    {
        spinButton.OnSpin -= HandleSpinButtonClick;
    }
    
    private void GenerateRandomOutcome()
    {
        Debug.Log("Slot Length, dim 0");
        Debug.Log(_slots.GetLength(0));
        
        Debug.Log("Slot Length, dim 1");
        Debug.Log(_slots.GetLength(1));
        
        for (int i = 0; i < _slots.GetLength(0); i++)
        {
            for (int j = 0; j < _slots.GetLength(1); j++)
            {
                _slots[i, j] = Random.Range(0, _numberOfSymbols);
            }
        }
        
        Debug.Log($"---- Generated Slots ----");
        
        for (int i = 0; i < _slots.GetLength(0); i++)
        {
            for (int j = 0; j < _slots.GetLength(1); j++)
            {
                Debug.Log($"I :: {i} :: J :: {j} :: Slot Value :: {_slots[i, j]}");
            }
        }
    }

    private void HandleSpinButtonClick(bool startSpin)
    {
        if (!startSpin)
        {
            if (_slotSpinRoutine != null)
            {
                StopCoroutine(_slotSpinRoutine);
                _slotSpinRoutine = null;
            }
            
            StopSlotSpin();    
        }
        else
        {
            EventManager.NotifySpinStart();
            
            paylineManager.ResetWinData();
            
            GenerateRandomOutcome();
            
            SetPredefinedValuesInSlots();

            if (_slotSpinRoutine != null)
            {
                StopCoroutine(_slotSpinRoutine);
                _slotSpinRoutine = null;
            }
            
            _slotSpinRoutine = StartCoroutine(StartSlotSpin());
        }
    }

    private void SetPredefinedValuesInSlots()
    {
        Debug.Log("Setting slots to predefined value...");

        for (int i = 0; i < _slots.GetLength(0); i++)
        {
            List<Symbol> newSlots = new List<Symbol>();
            for (int j = 0; j < _slots.GetLength(1); j++)
            {
                foreach (Symbol storedSymbol in gameDataSO.symbolsData)
                {
                    if (storedSymbol.uniqueID == _slots[i, j])
                    {
                        newSlots.Add(storedSymbol);
                    }
                }
            }
            
            slotSpinners[i].SetLast3Slots(newSlots);
        }
    }

    // This is done to start spinning slots one after another
    private IEnumerator StartSlotSpin()
    {
        spinButton.SetButtonInteractable(false);
        
        foreach (SlotSpinner slotSpinner in slotSpinners)
        {
            slotSpinner.SlotStopped += HandleSlotStop;
            // Divided by 100 because keeping '1' in inspector makes more sense than '0.01'
            slotSpinner.SetSlotSpinSpeed(spinSpeed / 100);
            slotSpinner.StartSpin();
            yield return new WaitForSeconds(0.1f);
        }
        
        spinButton.SetButtonInteractable(true);
    }

    private void StopSlotSpin()
    {
        foreach (SlotSpinner slotSpinner in slotSpinners)
        {
            slotSpinner.StopSpin();
            slotSpinner.SlotStopped -= HandleSlotStop;
        }
    }

    private void HandleSlotStop()
    {
        _slotStopCounter++;

        Debug.Log($"Slot Stopped :: {_slotStopCounter}");
        
        if (_slotStopCounter == slotSpinners.Count)
        {
            foreach (SlotSpinner slotSpinner in slotSpinners)
            {
                slotSpinner.SlotStopped -= HandleSlotStop;
            }
            HandleAllSlotStop();
            _slotStopCounter = 0; // Reset the counter
        }
    }

    private void HandleAllSlotStop()
    {
        // Results will be calculated in this function
        Debug.Log($"All slots are stopped...");
        spinButton.SetSlotSpinningState(false);
        
        Debug.Log($"----- Symbols Displayed On Screen -----");

        for (int i = 0; i < slotSpinners.Count; i++)
        {
            var slotSpinner = slotSpinners[i];
            var slots = slotSpinner.GetTop3Slots();
        
            // Debug.Log($"----- {slotSpinner.name} -----");
        
            for (int j = 0; j < slots.Count; j++)
            {
                var slot = slots[j];
                Symbol symbol = slot.GetComponent<Symbol>();
        
                if (symbol.type is SymbolType.HIGH1 or 
                                   SymbolType.HIGH2 or 
                                   SymbolType.HIGH3 or 
                                   SymbolType.HIGH4 or 
                                   SymbolType.WILD or 
                                   SymbolType.BONUS or
                                   SymbolType.SCATTER)
                {
                    symbol.PlayLandAnimation();
                }
                
                _resultSlots[i, j] = symbol;
            }
        }
        
        EventManager.NotifySpinStop();
        
        paylineManager.CheckAllPaylines(_resultSlots);
    }
}