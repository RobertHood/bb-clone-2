using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;
using TMPro;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;
public class Augment : MonoBehaviour
{   
    public AugmentEffect augmentEffect;
    [SerializeField] private GameObject augmentUI;
    [SerializeField] private TextMeshProUGUI nameText; [SerializeField] private TextMeshProUGUI descText;
    [SerializeField] private TextMeshProUGUI scoreMulText;
    [SerializeField] private Button button;

    private GameObject applyTarget;

    void Start()
    {
        if (augmentEffect == null)
        {

        }
        if (nameText == null || descText == null || scoreMulText == null)
        {
            var texts = GetComponentsInChildren<TextMeshProUGUI>(true);
            if (texts.Length > 0 && nameText == null) nameText = texts[0];
            if (texts.Length > 1 && descText == null) descText = texts[1];
            if (texts.Length > 2 && scoreMulText == null) scoreMulText = texts[2];
        }
        RefreshUi();
        if (button == null) button = GetComponentInChildren<Button>(true);
        if (button != null)
        {
            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(OnClick);
        }
    }

    public void Setup(AugmentEffect effect, GameObject uiPanel, GameObject target)
    {
        augmentEffect = effect;
        augmentUI = uiPanel;
        applyTarget = target;
        RefreshUi();
    }
    private void RefreshUi()
    {
        if (augmentEffect == null) return;
        if (nameText != null) nameText.text = augmentEffect.augmentEffectName;
        if (descText != null) descText.text = augmentEffect.augmentEffectDescription;
        if (scoreMulText != null) scoreMulText.text = "Score Multiplier: " + augmentEffect.scoreMultiplier + "x";
    }
    public void OnClick()
    {
        if (augmentUI != null) augmentUI.SetActive(false);

        if (augmentEffect != null && applyTarget != null)
        {
            augmentEffect.Apply(applyTarget);
        }
        Destroy(gameObject);
    }
}
