using UnityEngine;

public class LuckyClearController : MonoBehaviour
{
    public float chance = 0.05f;

    private GridManager gm;

    void Awake()
    {
        gm = GetComponent<GridManager>();
    }

    public void NotifyClear()
    {
        if (gm == null) return;
        if (Random.value < chance)
        {
            gm.ClearBoard();
        }
    }
}
