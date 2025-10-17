using System.Collections.Generic;
using UnityEngine;

public class AugmentSpawner : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public GameObject augment;
    public GameObject augmentUIPanel;
    public GameObject effectTarget;
    private Vector3 firstSlot = new Vector3 (-51, 109 ,0);
    private Vector3 secondSlot =
    new Vector3 (-51, -320, 0);
    private Vector3 thirdSlot = new Vector3(-51, -765, 0);

    public List<AugmentEffect> augmentEffects = new List<AugmentEffect>();

    void Start()
    {
        var options = new List<AugmentEffect>(augmentEffects);
        for (int i = 0; i < options.Count; i++)
        {
            int rand = UnityEngine.Random.Range(i, options.Count);
            var temp = options[i];
            options[i] = options[rand];
            options[rand] = temp;
        }

        var aug1 = Instantiate(augment, transform);
        var aug2 = Instantiate(augment, transform);
        var aug3 = Instantiate(augment, transform);

        aug1.GetComponent<RectTransform>().localPosition = firstSlot;

        aug2.GetComponent<RectTransform>().localPosition = secondSlot;

        aug3.GetComponent<RectTransform>().localPosition = thirdSlot;

        var aug1Comp = aug1.GetComponent<Augment>();
        var aug2Comp = aug1.GetComponent<Augment>();
        var aug3Comp = aug1.GetComponent<Augment>();

        if (options.Count > 0 && aug1Comp != null) aug1Comp.Setup(options[0], augmentUIPanel, effectTarget);
        if (options.Count > 1 && aug2Comp != null) aug2Comp.Setup(options[1], augmentUIPanel, effectTarget);
        if (options.Count > 2 && aug3Comp != null) aug3Comp.Setup(options[2], augmentUIPanel, effectTarget);
    }


    void Update()
    {
        
    }
}
