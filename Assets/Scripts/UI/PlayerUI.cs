using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Rendering;

public class PlayerUI : MonoBehaviour
{

    [SerializeField] private Button endTurnButton;
    [SerializeField] private TextMeshProUGUI endTurnButtonLabel;

    private const string DEFENCE_BUTTON_LABEL = "END TURN";
    private const string WAIT_BUTTON_LABEL = "WAIT";
    private const string ATTACK_BUTTON_LABEL = "SLAY HER";

    private void Awake()
    {
        endTurnButton.onClick.AddListener(() =>
        {
            PlayerMain.Instance.EndTurn();
        });
    }

    private void Start()
    {
        PlayerMain.Instance.OnWait += PlayerMain_OnWait; //rename on multiplayerRemake
        PlayerMain.Instance.OnAttack += PlayerMain_OnAttack;
        PlayerMain.Instance.OnDefence += PlayerMain_OnDefence;
    }

    private void PlayerMain_OnDefence(object sender, System.EventArgs e)
    {
        endTurnButton.interactable = true;
        endTurnButtonLabel.text = DEFENCE_BUTTON_LABEL;
    }

    private void PlayerMain_OnAttack(object sender, System.EventArgs e)
    {
        endTurnButton.interactable = false;
        endTurnButtonLabel.text = ATTACK_BUTTON_LABEL;
    }

    private void PlayerMain_OnWait(object sender, System.EventArgs e)
    {
        endTurnButton.interactable = false;
        endTurnButtonLabel.text = WAIT_BUTTON_LABEL;
    }
}
