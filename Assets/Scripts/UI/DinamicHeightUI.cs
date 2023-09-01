using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class DinamicHeightUI : MonoBehaviour
{
    private RectTransform rectTransform;

    private Vector2 sizeDeltaBase;
    private float spacing;

    private int prevChildCount;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        sizeDeltaBase = rectTransform.sizeDelta;

        var verticalLayoutGroup = GetComponent<VerticalLayoutGroup>();
        spacing = verticalLayoutGroup.spacing;

    }

    /*
     * Also make DraggableUI:
     * Use interfaces at EventSystem: IBeginDragHandler, IDragHandler, ...End
     */

    private void Update()
    {
        //subscribe onMessagePublish event??
        var childs = GetComponentsInChildren<MessageTemplateSingle>(); // -> transform.childCount?

        if (childs.Length == prevChildCount) return;

        var childsRectTransform = childs.ToList().Select(i => i.gameObject.GetComponent<RectTransform>()).ToList();

        float height = GetUIObjectsHightSum(childsRectTransform) + spacing * (childsRectTransform.Count - 1);

        rectTransform.sizeDelta = new Vector2(sizeDeltaBase.x, height);


        prevChildCount = childs.Length;
    }

    private float GetUIObjectsHightSum(List<RectTransform> rectTransformList)
    {
        float hightSum = 0f;

        foreach (var rectTransform in rectTransformList)
        {
            hightSum += rectTransform.sizeDelta.y;
        }

        return hightSum;
    }
}
