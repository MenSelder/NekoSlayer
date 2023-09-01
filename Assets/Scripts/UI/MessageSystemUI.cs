using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MessageSystemUI : MonoBehaviour
{
    [SerializeField] private Transform messageTemplate;
    [SerializeField] private Transform container;

    private void Awake()
    {
        messageTemplate.gameObject.SetActive(false);
        CleanUpContainer();
    }

    private void Start()
    {
        MessageSystem.Instance.OnMessageListChange += MessageSystem_OnMessagePublish;
    }

    private void MessageSystem_OnMessagePublish(object sender, EventArgs e)
    {
        UpdateMessages();
    }

    private void CleanUpContainer() //except Template
    {
        foreach (Transform child in container)
        {
            if (child == messageTemplate) continue;

            Destroy(child.gameObject);
        }
    }

    private void UpdateMessages()
    {
        CleanUpContainer();

        foreach (MessageSystem.Message message in MessageSystem.Instance.GetMessageList)
        {
            var newMessage = Instantiate(messageTemplate, container);
            newMessage.GetComponent<MessageTemplateSingle>().SetMessageUI(message);
            newMessage.gameObject.SetActive(true);
        } 
    }

}
