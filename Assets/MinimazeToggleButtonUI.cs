using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MinimazeToggleButtonUI : MonoBehaviour
{
    [SerializeField] private RectTransform scrollView;
    [SerializeField] private RectTransform MessageTemplate;
    [SerializeField] private TextMeshProUGUI buttonText;

    [SerializeField] private float animationTime;
    [SerializeField] private Ease ease; //Ease.OutCubic

    private const string ARROW_UP = "\u25B2";
    private const string ARROW_DOWN = "\u25BC";

    private Button minimazeToggleButton;
    private Vector2 viewportSizeDeltaBase;
    private bool isMinimazed = false;

    private void Awake()
    {
        minimazeToggleButton = GetComponent<Button>();

        minimazeToggleButton.onClick.AddListener(() =>
        {
            if (isMinimazed)
            {
                Maximaze();
                return;
            }
            
            Minimaze();
        });
    }

    private void Start()
    {
        viewportSizeDeltaBase = scrollView.sizeDelta;
        Maximaze();
    }

    private void Minimaze()
    {
        var minimazedRect = new Vector2(scrollView.sizeDelta.x, MessageTemplate.sizeDelta.y);
        DOTween.To(() => scrollView.sizeDelta, x => scrollView.sizeDelta = x, minimazedRect, animationTime)
            .SetEase(ease);

        buttonText.text = ARROW_DOWN;
        isMinimazed = true;
    }

    private void Maximaze()
    {
        DOTween.To(() => scrollView.sizeDelta, x => scrollView.sizeDelta = x, viewportSizeDeltaBase, animationTime)
            .SetEase(ease);

        buttonText.text = ARROW_UP;
        isMinimazed = false;
    }

}
