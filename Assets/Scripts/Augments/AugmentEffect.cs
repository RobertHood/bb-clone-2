using System;
using TMPro;
using UnityEngine;

public abstract class AugmentEffect : ScriptableObject
{
    public abstract void Apply(GameObject target);
}
