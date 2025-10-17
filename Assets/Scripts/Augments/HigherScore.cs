using UnityEngine;
[CreateAssetMenu(menuName = "Augments/HigherScore")]
public class HigherScore : AugmentEffect
{
    public override void Apply(GameObject target)
    {
        target.GetComponent<GridManager>().setScoreMultiplier(scoreMultiplier);
    }
}
