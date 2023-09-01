using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HighlightNewMessageUI : MonoBehaviour
{

    [SerializeField] private Image background;

    [SerializeField] private float highlightTime;
    [SerializeField] private float fadeOutTime;

    private float highlightTimeCounter = 0f;
    private float fadeOutTimeCounter = 0f;

    private Color baseColor;
    private float baseAlpha;

    private Color highlightedColor;

    private void Start()
    {
        baseColor = background.color;
        baseAlpha = baseColor.a;

        float maxAlpha = 1f;
        highlightedColor = new Color(baseColor.r, baseColor.g, baseColor.b, maxAlpha);

        //get send time...
        var currentTime = Time.time;
        var messageTime = GetComponent<MessageTemplateSingle>().Message.Time;

        highlightTimeCounter = currentTime - messageTime;
        fadeOutTimeCounter = currentTime - (messageTime + highlightTimeCounter);
        
    }

    private void Update()
    {
        if (highlightTimeCounter >= highlightTime && fadeOutTimeCounter >= fadeOutTime) return;

        //highlight
        if (highlightTimeCounter <= 0) background.color = highlightedColor;

        if (highlightTimeCounter < highlightTime)
        {
            highlightTimeCounter += Time.deltaTime;
            return;
        }

        //fadeout
        if (fadeOutTime <= 0) return;

        float fadedAlpha = Mathf.Lerp(background.color.a, baseAlpha, fadeOutTimeCounter / fadeOutTime);
        background.color = new Color(baseColor.r, baseColor.g, baseColor.b, fadedAlpha);

        fadeOutTimeCounter += Time.deltaTime;
    }

}
