using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using Spine;
using Spine.Unity;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WinningPopup : MonoBehaviour
{
    public SkeletonGraphic winPopupGraphic;
    public AnimationReferenceAsset winIntro;
    public AnimationReferenceAsset winLoop;
    public AnimationReferenceAsset winOutro;
    
    public Button closeButton;
    public TMP_Text winText;

    private void OnEnable()
    {
        closeButton.onClick.AddListener(HandleClosePopup);
    }

    private void OnDisable()
    {
        closeButton.onClick.RemoveListener(HandleClosePopup);
    }

    private void HandleClosePopup()
    {
        CloseWinPopup();
    }

    public void ShowWinPopup(float winAmount)
    {
        winText.text = "$" + winAmount.ToString("F2", CultureInfo.InvariantCulture);
        
        winPopupGraphic.AnimationState.SetAnimation(0, winIntro.Animation, false);
        winPopupGraphic.AnimationState.AddAnimation(0, winLoop.Animation, true, 0);
    }

    public void CloseWinPopup()
    {
        winPopupGraphic.AnimationState.SetAnimation(0, winOutro.Animation, false).Complete += HandleClosePopup2;
    }

    private void HandleClosePopup2(TrackEntry trackentry)
    {
        gameObject.SetActive(false);
    }
}
