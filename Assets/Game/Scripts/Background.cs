using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Background : IAnimatable
{
    protected override void Animate(string animationName)
    {
        skeletonGraphic.AnimationState.SetAnimation(0, animationName, true);
    }
}
