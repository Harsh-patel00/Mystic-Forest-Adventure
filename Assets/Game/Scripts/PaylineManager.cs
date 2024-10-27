using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaylineManager : MonoBehaviour
{
    [SerializeField] private DataScriptableObject gameDataSO;
    [SerializeField] private PayoutManager payoutManager;
    [SerializeField] private LineRenderer paylineRenderer;
    
    private readonly int _minComboRequired = 3;
    
    [SerializeField] private List<WinData> _winDatum = new List<WinData>();

    private Symbol[,] symbolsCache = new Symbol[5,3];

    public void CheckAllPaylines(Symbol[,] symbols)
    {
        Debug.Log("Checking all paylines");
        
        symbolsCache = symbols;
        
        // Will run 20 times
        for (var lineIndex = 0; lineIndex < gameDataSO.payLinesData.Count; lineIndex++)
        {
            var payLines = gameDataSO.payLinesData[lineIndex];
            Debug.Log($"<color=red>Payline :: {lineIndex+1}</color>");
            int comboCount = 1;
            Symbol symbolOnPayLine = null;
            Symbol prevSymbol = null;
            Symbol initialSymbol = null;

            // This will run 5 times as there are 5 elements on the payline to check
            for (var i = 0; i < payLines.lineAddress.Count; i++)
            {
                var lineAddr = payLines.lineAddress[i];

                Debug.Log($"--- PayLine checking slot :: {i}");
                Debug.Log($"--- PayLine address :: {lineAddr}");

                // This is done to convert from line address (1, 0, -1) to result matrix coordinates
                int j = lineAddr + 1;

                symbolOnPayLine = symbols[i, j];
                
                Debug.Log($"Current Symbol Type :: {symbolOnPayLine.type}");

                if (i == 0)
                {
                    initialSymbol = symbolOnPayLine;
                    if(symbolOnPayLine.type is SymbolType.BONUS or SymbolType.SCATTER) break;
                }
                
                if (i > 0 && prevSymbol != null)
                {
                    Debug.Log($"Previous Symbol Type :: {prevSymbol.type}");
                    
                    if (symbolOnPayLine.type == prevSymbol.type || (symbolOnPayLine.type == SymbolType.WILD || prevSymbol.type == SymbolType.WILD))
                    {
                        comboCount++;
                        
                        Debug.Log($"Combo :: {comboCount}");
                    }
                    
                    if (symbolOnPayLine.type != prevSymbol.type)
                    {
                        Debug.Log($"Breaking Loop");
                        break;
                    }
                }
                
                Debug.Log($"Symbol on payline: {symbolOnPayLine.ToString()}");
                
                prevSymbol = symbolOnPayLine;
            }
            
            if (comboCount >= _minComboRequired)
            {
                _winDatum.Add(new WinData(lineIndex, initialSymbol, comboCount));
            }
        }
        
        // Check for the highest win
        CheckForHighestWin();
    }
    
    public void ResetWinData()
    {
        // Clear win data and update the balance
        payoutManager.UpdateBalance();
        paylineRenderer.gameObject.SetActive(false);
        _winDatum.Clear();
        paylineRenderer.positionCount = 0;
    }
    
    private void CheckForHighestWin()
    {
        if(_winDatum.Count == 0) return;
        
        if (_winDatum.Count == 1)
        {
            AnimatePayLine(_winDatum[0]);
            payoutManager.UpdateWin(_winDatum[0]);
        }
        else
        {
            float highestMultiplier = float.MinValue;
            int highestMultiplierIndex = -1;

            for (var index = 0; index < _winDatum.Count; index++)
            {
                var winData = _winDatum[index];
                float currentMultiplier = 0f;

                switch (winData.symbolCount)
                {
                    case 3:
                        currentMultiplier = winData.symbolOnPayLine.times3Multiplier;
                        break;

                    case 4:
                        currentMultiplier = winData.symbolOnPayLine.times4Multiplier;
                        break;

                    case 5:
                        currentMultiplier = winData.symbolOnPayLine.times5Multiplier;
                        break;
                }

                if (highestMultiplier < currentMultiplier)
                {
                    highestMultiplier = currentMultiplier;
                    highestMultiplierIndex = index;
                }
            }
            
            Debug.Log($"Highest Win: {_winDatum[highestMultiplierIndex].symbolOnPayLine.type}");
            
            AnimatePayLine(_winDatum[highestMultiplierIndex]);
            payoutManager.UpdateWin(_winDatum[highestMultiplierIndex]);
        }
    }

    private void AnimatePayLine(WinData winData)
    {
        var payLines = gameDataSO.payLinesData[winData.lineIndex];
        
        List<Vector3> positions = new List<Vector3>();
        
        for (var i = 0; i < payLines.lineAddress.Count; i++)
        {
            var lineAddr = payLines.lineAddress[i];

            // This is done to convert from line address (1, 0, -1) to result matrix coordinates
            int j = lineAddr + 1;

            Symbol symbol = symbolsCache[i, j];
            
            var finalPos = GetWorldPositionOfUIElement(symbol.gameObject.GetComponent<RectTransform>());
            
            symbol.PlayWinAnimation();
            SoundManager.instance.PlayWinSound();

            switch (symbol.type)
            {
                case SymbolType.LOW1:
                case SymbolType.LOW2:
                case SymbolType.LOW3:
                case SymbolType.LOW4:
                case SymbolType.LOW5:
                    SoundManager.instance.PlayLPSound();
                    break;
                case SymbolType.HIGH1:
                    SoundManager.instance.PlayHP1Sound();
                    break;
                case SymbolType.HIGH2:
                    SoundManager.instance.PlayHP2Sound();
                    break;
                case SymbolType.HIGH3:
                    SoundManager.instance.PlayHP3Sound();
                    break;
                case SymbolType.HIGH4:
                    SoundManager.instance.PlayHP4Sound();
                    break;
                case SymbolType.WILD:
                    // Fill in on requirement
                    break;
                case SymbolType.BONUS:
                    // Fill in on requirement
                    break;
                case SymbolType.SCATTER:
                    // Fill in on requirement
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            
            positions.Add(finalPos);
        }
        
        paylineRenderer.gameObject.SetActive(true);
        
        paylineRenderer.positionCount++;
        paylineRenderer.SetPosition(0, positions[0]);
        
        StartCoroutine(AnimatePayline(positions));
        
    }

    IEnumerator AnimatePayline(List<Vector3> finalPositions)
    {
        // Ignore 1st position as it's already set
        for (var i = 1; i < finalPositions.Count; i++)
        {
            paylineRenderer.positionCount++;
            paylineRenderer.SetPosition(i, finalPositions[i]);
            yield return new WaitForSeconds(0.05f);
        }
    }
    
    Vector3 GetWorldPositionOfUIElement(RectTransform uiElement)
    {
        Vector3 screenPos = RectTransformUtility.WorldToScreenPoint(Camera.main, uiElement.position);
        Vector3 worldPos;
        RectTransformUtility.ScreenPointToWorldPointInRectangle(uiElement, screenPos, Camera.main, out worldPos);
        return worldPos;
    }
}
