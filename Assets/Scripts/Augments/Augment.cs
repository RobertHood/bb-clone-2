using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Augment : MonoBehaviour
{
    public AugmentEffect augmentEffect;

    public void OnClick(Tilemap tilemap)
    {
        Destroy(gameObject);
        augmentEffect.Apply(tilemap.gameObject);
    }
}
