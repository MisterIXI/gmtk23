using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class TwatchChat : MonoBehaviour
{
    [field: SerializeField] public GameObject ChatPanel { get; private set; }
    [field: SerializeField] public GameObject InputField { get; private set; }

    List<Message> MessageList = new List<Message>();
    public int MaxMessages = 25;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.KeypadEnter)) {
            SendMessageToChat("It worked");
        }
    }

    public void SendMessageToChat(string Text){
        if (MessageList.Count >= MaxMessages){
            MessageList.Remove(MessageList[0]);
        }
        Message NewMessage = new Message();
        NewMessage.Text = Text;
        MessageList.Add(NewMessage);

        // if ()
    }
}

[System.Serializable]
public class Message
{
    public string Text;
    public Text TextObject;
}
