using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MessageSystem : MonoBehaviour
{
    public class Message
    {
        public string SenderName { get; private set; }
        public string Text { get; private set; }
        public DateTime SendingTime { get; private set; }

        public CharacterSO CharacterSO = null;
        public int Index => Instance.messageList.IndexOf(this);
        public void Delete() => Instance.DeleteMessage(this);

        public float Time { get; private set; }

        public Message(string senderName, string text)
        {
            SenderName = senderName;
            Text = text;
            SendingTime = DateTime.Now;

            Time = UnityEngine.Time.time;

        }
    }

    public static MessageSystem Instance { get; private set; }

    private List<Message> messageList;

    public List<Message> GetMessageList => messageList;

    //public EVENT...
    public event EventHandler OnMessageListChange;
    //public class OnMessagePublishEventArgs : EventArgs
    //{
    //    public Message Message;
    //}

    private void Awake()
    {
        Instance = this;
        messageList = new List<Message>();
    }

    // obj seder = xyeta. delete
    public void PublishMessage(string sourceName, string text, CharacterSO characterSO = null)
    {
        var message = new Message(sourceName, text);
        if (characterSO != null) message.CharacterSO = characterSO;

        messageList.Add(message);

        //invoke event UI
        OnMessageListChange?.Invoke(this, EventArgs.Empty);
    }
    
    public void DeleteMessage(Message message)
    {
        messageList.Remove(message);

        OnMessageListChange?.Invoke(this, EventArgs.Empty);
    }

}
