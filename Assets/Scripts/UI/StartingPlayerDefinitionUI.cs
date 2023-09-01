using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

public class StartingPlayerDefinitionUI : MonoBehaviour
{

    [SerializeField] private GameObject StartingPlayerScreen;

    [SerializeField] private TextMeshProUGUI playerOrderLabel;
    [SerializeField] private Image background;
    
    private CanvasGroup canvasGroup;

    private float showTime;
    private float showTimeMax;

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();        
    }

    private void Start()
    {
        GameManager.Instance.OnStartingPlayerDefinition += GameManager_OnStartingPlayerDefinition;

        Hide();
    }



    private void GameManager_OnStartingPlayerDefinition(object sender, GameManager.OnStartingPlayerDefinitionEventArgs e)
    {
        showTimeMax = GameManager.Instance.StartingPlayerDefinitionTimer;

        if (PlayerMain.Instance == e.StartingPlayer)
        {
            YouGoFirst();
        }
        else
        {
            EnemyGoFirst();
        }

        Show();
    }

    private void YouGoFirst()
    {
        playerOrderLabel.text = "You Go First";
        playerOrderLabel.color = Color.green;
    }

    private void EnemyGoFirst()
    {
        playerOrderLabel.text = "Enemy Go First";
        playerOrderLabel.color = Color.red;

        //background.color = Color.gray;
    }

    private void Update()
    {
        if (showTime > 0)
        {
            //use function or curve?
            showTime -= Time.deltaTime;
            canvasGroup.alpha = showTime / showTimeMax;
            return;
        }

        showTime = 0;
        Hide();
    }

    private void Hide()
    {
        StartingPlayerScreen.SetActive(false);
    }

    private void Show()
    {
        StartingPlayerScreen.SetActive(true);

        showTime = showTimeMax;
    }
}
