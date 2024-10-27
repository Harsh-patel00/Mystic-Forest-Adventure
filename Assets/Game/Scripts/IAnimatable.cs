using System;
using System.Collections;
using System.Collections.Generic;
using Spine.Unity;
using UnityEngine;

public abstract class IAnimatable: MonoBehaviour
{
    public SkeletonGraphic skeletonGraphic;
    
    public bool animateOnStart = false;
    public string initialAnimationName;
    
    public string winAnimationName;
    public string landAnimationName;
    public string stopAnimationName;
    
    protected abstract void Animate(string animationName);

    protected void WinAnimation()
    {
        Animate(winAnimationName);
    }
    
    protected void LandAnimation()
    {
        Animate(landAnimationName);
    }
    
    protected void StopAnimation()
    {
        Animate(stopAnimationName);
    }

    public void Start()
    {
        if(animateOnStart)
            Animate(initialAnimationName);
    }
}
