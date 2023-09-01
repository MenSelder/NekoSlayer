using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FPSCounterUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI FPSCounterText;

    private void LateUpdate()
    {
        float fps = 1f / Time.deltaTime;
        FPSCounterText.text = Mathf.Ceil(fps).ToString();
    }
}
