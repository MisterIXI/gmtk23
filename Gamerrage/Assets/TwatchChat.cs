using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class TwatchChat : MonoBehaviour
{
    [field: SerializeField] public GameObject ChatPanel { get; private set; }
    [field: SerializeField] public TextMeshProUGUI TwatchTextObject { get; private set; }

    [field: SerializeField] List<Message> MessageList = new List<Message>();
    float nextMessageTime = 0.5f;
    public int MaxMessages = 40;

    string[] colors = {"red"};
    string[] usernames = {"sazzles"};
    string[] neutral = {"hi","Hello","Hi!","Yooo","lmao","haha","lol","LUL","ur muted","ads wtf","hahaha",":D","is this live?","whats this game called","same lol","how are you today","can we play something else","why are we playing this","no way","yoo","what are these graphics","is this ranked?","<3","yooo","XD","xd","uhh","oh","yo","waddup","im back what did i miss",":)","oh man","classic cha0sgamer haha","lol","dude","this looks like trash tbh","that splatanim xD","greetings from germany","8)",":3","im hungry","OMEGALUL"};
    string[] excited = {"OH","pausing","almost there","Pausechamp","YOU CAN DO IT","Pausechamp","Pausechamp","Pausechamp","Pausechamp","Pausechamp","Puasechamp","pausing"};
    string[] fail = {"KEKW","HAHAHAHHAHAHAHa","SO BAD","LOLOLL","HAHAHHAA","HAHAAHA","WASHED UP","NA jump","xDDD","XDDDD","LMAAOOO","soooBAD","LUL","OMEGALUL","HAAHAHA","SOOO BAD","KEKW KEKW KEKW KEKW","KEK","KEKW KEKW KEKW","KEKW KEKW","KEKW","KEKW","KEKW","KEKW","KEKW KEKW","KEK","KEK","LOLOOL","XDDD","AHAHAA","HAHAA","OMEGALUL","OMEGALUL","OMEGALUL","OMEGALUL"};
    string[] success = {"EZ","POG","pog","LETSGO","nice","NICE","LETSGOO","POG POG POG POG POG","poggers","YOOOOO","POG","POG","POG","POG","POG","POG","POG","POG","poggers","Pog","Pog","YOU DID IT","LETSGOOOO"};

    Dictionary<int, string[]> MsgDictionary;
    // Update is called once per frame

    void Awake(){
        MsgDictionary = new Dictionary<int, string[]>(){
            {1, neutral},
            {2, excited},
            {3, fail},
            {4, success}
        };
    }

    void Update()
    {
    }

    void FixedUpdate(){
        if(Time.time > nextMessageTime) {
            int excitementLvl = getExcitement();
            string username = usernames[(int)Random.Range(0f,usernames.Length)];
            string[] msg = MsgDictionary[excitementLvl];
            SendTwatchMessage(username, msg[(int)Random.Range(0f,(int)msg.Length)]);
            nextMessageTime = Time.time + 2f;
        }
    }

    int getExcitement(){
        return (int) Random.Range(1f,4f);
    }

    public void SendTwatchMessage(string Username, string TxtInput)
    {
        if (MessageList.Count >= MaxMessages)
        {
            Destroy(MessageList[0].MessageTextObject.gameObject);
            MessageList.Remove(MessageList[0]);
        }
        Message NewMessage = new Message();
        NewMessage.User = Username;
        NewMessage.MsgText = TxtInput;
        TextMeshProUGUI NewTMP = Instantiate(TwatchTextObject,ChatPanel.transform);
        NewMessage.MessageTextObject = NewTMP;
        NewMessage.MessageTextObject.text = "<color=\"" + colors[(int)Random.Range(0f,(int)colors.Length)] + "\">" + NewMessage.User + "</color>" + ": " + NewMessage.MsgText;
        MessageList.Add(NewMessage);
    }
}

[System.Serializable]
public class Message
{
    public string User;
    public string MsgText;
    public TextMeshProUGUI MessageTextObject;
}

public class TwatchMsg{
    public string Message;
    public int Category;
}