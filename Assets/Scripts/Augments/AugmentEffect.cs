using System;
using TMPro;
using UnityEngine;

public abstract class AugmentEffect : ScriptableObject
{
    public string augmentEffectName;
    public string augmentEffectDescription;
    public int scoreMultiplier;
    public abstract void Apply(GameObject target);
}
