using System;
using Spine.Unity;
using UnityEngine;

[RequireComponent(typeof(SkeletonGraphic))]
public class Symbol: IAnimatable
{
    public SymbolType type;
    public float times3Multiplier;
    public float times4Multiplier;
    public float times5Multiplier;
    public Sprite sprite;

    // This will be used to identify symbols uniquely using a matrix
    public int uniqueID;
    
    protected override void Animate(string animationName)
    {
        if(string.IsNullOrEmpty(animationName)) return;
        
        skeletonGraphic.AnimationState.SetAnimation(0, animationName, false);
    }
    
    public void PlayWinAnimation()
    {
        WinAnimation();
    }

    public void PlayLandAnimation()
    {
        LandAnimation();
    }
    
    public void PlayStopAnimation()
    {
        StopAnimation();
    }

    public void UpdateWithNewData(Symbol newSymbol)
    {
        name = "Predefined_" + newSymbol.name;
        
        type = newSymbol.type;
        times3Multiplier = newSymbol.times3Multiplier;
        times4Multiplier = newSymbol.times4Multiplier;
        times5Multiplier = newSymbol.times5Multiplier;
        sprite = newSymbol.sprite;
        uniqueID = newSymbol.uniqueID;
        
        skeletonGraphic.skeletonDataAsset = newSymbol.skeletonGraphic.skeletonDataAsset;
        skeletonGraphic.material = newSymbol.skeletonGraphic.material;
        animateOnStart = newSymbol.animateOnStart;
        initialAnimationName = newSymbol.initialAnimationName;
        winAnimationName = newSymbol.winAnimationName;
        landAnimationName = newSymbol.landAnimationName;
        stopAnimationName = newSymbol.stopAnimationName;
        
        skeletonGraphic.Initialize(true);
    }
}

public enum SymbolType
{
    LOW1,
    LOW2,
    LOW3,
    LOW4,
    LOW5,
    
    HIGH1,
    HIGH2,
    HIGH3,
    HIGH4,
    
    WILD,
    BONUS,
    SCATTER
}
