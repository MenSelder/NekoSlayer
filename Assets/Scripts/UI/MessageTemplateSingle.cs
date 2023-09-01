using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class MessageTemplateSingle : MonoBehaviour
{
    [Header("Self fields")]
    [SerializeField] private Image background;
    [SerializeField] private TextMeshProUGUI senderNameField;
    [SerializeField] private TextMeshProUGUI textField;
    [SerializeField] private TextMeshProUGUI timeField;

    [SerializeField] private Button deleteButton;

    public MessageSystem.Message Message { get; private set; }

    private void Awake()
    {
        deleteButton.onClick.AddListener(() => Message.Delete());
    }

    public void SetMessageUI(MessageSystem.Message message)
    {
        this.Message = message;

        senderNameField.text = message.SenderName;
        textField.text = message.Text;
        timeField.text = message.SendingTime.ToString();

        SetColor();
    }

    private void SetColor()
    {
        if (Message.CharacterSO == null) return;

        var textColor = Message.CharacterSO.MessageTextColor;
        var backgroundColor = Message.CharacterSO.MessageBackgroundColor;

        senderNameField.color = textColor;
        textField.color = textColor;
        timeField.color = textColor;

        background.color = backgroundColor;
    }
}
