using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Threading;
using TMPro;
using UnityEngine;

public class PayoutManager : MonoBehaviour
{
    [SerializeField] private float winAmount = 0;
    
    [SerializeField] private BetManager betManager;
    [SerializeField] private TMP_Text winTxt;

    [SerializeField] private WinningPopup winningPopup;
    
    private void Start()
    {
        winTxt.text = winAmount.ToString(CultureInfo.InvariantCulture);
    }

    public void UpdateWin(WinData winData)
    {
        float currentWin = -1f;
        
        switch (winData.symbolCount)
        {
            case 3:
                currentWin = betManager.CurrentBetAmount * winData.symbolOnPayLine.times3Multiplier;
                break;
            
            case 4:
                currentWin = betManager.CurrentBetAmount * winData.symbolOnPayLine.times4Multiplier;
                break;
            
            case 5:
                currentWin = betManager.CurrentBetAmount * winData.symbolOnPayLine.times5Multiplier;
                break;
        }
        
        // Show win popup
        winningPopup.gameObject.SetActive(true);
        winningPopup.ShowWinPopup(currentWin);
        
        betManager.UpdateBalanceAmount(currentWin);
        winAmount += currentWin;
        winTxt.text = winAmount.ToString("F2", CultureInfo.InvariantCulture);
    }

    public void UpdateBalance()
    {
        betManager.UpdateBalanceAmount();
    }
}

[Serializable]
public class WinData
{
    public int lineIndex = -1;
    public Symbol symbolOnPayLine = null;
    public int symbolCount = -1;

    public WinData(int lineIndex, Symbol symbolOnPayLine, int symbolCount)
    {
        this.lineIndex = lineIndex;
        this.symbolOnPayLine = symbolOnPayLine;
        this.symbolCount = symbolCount;
    }
}
