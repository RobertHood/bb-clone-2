using UnityEngine;
[CreateAssetMenu(menuName = "Augments/LuckyClear")]
public class LuckyClear : AugmentEffect
{
    public float chance = 0.05f;
    public override void Apply(GameObject target)
    {
        if (target == null) return;
        var ctrl = target.GetComponent<LuckyClearController>();
        if (ctrl == null) ctrl = target.AddComponent<LuckyClearController>();
        ctrl.chance = chance;
        ctrl.enabled = true;
        target.GetComponent<GridManager>().setScoreMultiplier(scoreMultiplier);
    }
}
