using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MessageName
{
    public const string SHOWVIEW = "showview";
}

public class MessageView : MonoBehaviour, IOnMessage
{
    string[] messageNames = new string[]
    {
        MessageName.SHOWVIEW,
    };
	void Start () {
        MessageManager.RegisterMessage(this,messageNames);
	}

    void Show()
    {
        MessageManager.ExecuteMessage(MessageName.SHOWVIEW);
    }

    public void OnMessage(IMessage msg)
    {
        switch (msg.Name)
        {
            case MessageName.SHOWVIEW:
                object body = msg.Body;
                break;
            default:
                break;
        }
    }

}
