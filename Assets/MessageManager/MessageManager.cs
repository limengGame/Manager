using System.Collections;
using System.Collections.Generic;

public interface IOnMessage
{
    void OnMessage(IMessage message);
}

public interface IMessage
{
    string Name
    {
        get;
    }
    object Body
    {
        get;
    }
    object Type
    {
        get;
    }
    void Reset();
}

public class Message : IMessage
{
    public static Message Current
    {
        get;set;
    }
    public string Name
    {
        get;set;
    }
    public object Body
    {
        get;set;
    }
    public object Type
    {
        get;set;
    }
    public Message(string Name) : this(Name, null, null)
    {
    }
    public Message(string Name, object Body) : this(Name, Body, null)
    {
    }

    public Message(string Name, object Body, object Type)
    {
        this.Name = Name;
        this.Body = Body;
        this.Type = Type;
        Message.Current = this;
    }

    public void Reset()
    {
        this.Name = "";
        this.Body = null;
        this.Type = null;
        Message.Current = null;
    }
}

public static class MessageManager
{
    private static Dictionary<IOnMessage, List<string>> _messageMap;

    public static bool Init()
    {
        _messageMap = new Dictionary<IOnMessage, List<string>>();
        return true;
    }
    public static void Dispose()
    {
        if (_messageMap == null)
            return;
        _messageMap.Clear();
    }

    public static void RegisterMessage(IOnMessage message, string[] commandNames)
    {
        List<string> messageList = null;
        if (_messageMap.TryGetValue(message, out messageList))
        {
            for (int i = 0; i < commandNames.Length; i++)
            {
                if (messageList.Contains(commandNames[i]))
                    continue;
                messageList.Add(commandNames[i]);
            }
        }
        else
            _messageMap[message] = new List<string>(commandNames);
    }

    public static void RemoveMessage(IOnMessage message, string[] commandNames)
    {
        List<string> messageList = null;
        if (_messageMap.TryGetValue(message, out messageList))
        {
            for (int i = 0; i < commandNames.Length; i++)
            {
                if (messageList.Contains(commandNames[i]))
                    messageList.Remove(commandNames[i]);
            }
        }
    }

    public static void ExecuteMessage(string Name, object Body = null, object Type = null)
    {
        Message.Current.Name = Name;
        Message.Current.Body = Body;
        Message.Current.Type = Type;
        ExecuteMessage(Message.Current);
    }

    private static void ExecuteMessage(IMessage message)
    {
        List<IOnMessage> views = new List<IOnMessage>();
        Dictionary<IOnMessage, List<string>>.Enumerator iter = _messageMap.GetEnumerator();

        while (iter.MoveNext())
        {
            if (iter.Current.Value.Contains(message.Name))
                views.Add(iter.Current.Key);
        }
        if (views.Count <= 0)
            return;
        for (int i = 0; i < views.Count; i++)
        {
            views[i].OnMessage(message);
        }
        views.Clear();
        views = null;
    }

}
