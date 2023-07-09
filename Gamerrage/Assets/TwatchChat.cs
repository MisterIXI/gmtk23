using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
public class TwatchChat : MonoBehaviour
{
    [field: SerializeField] public GameObject ChatPanel { get; private set; }
    [field: SerializeField] public TextMeshProUGUI TwatchTextObject { get; private set; }

    [field: SerializeField] List<Message> MessageList = new List<Message>();
    // public ViewerController ViewerController { get; private set; }
    float nextMessageTime = 0.5f;
    public int MaxMessages = 40;

    string[] colors = { "#00ffffff", "#0000ffff", "#ff00ffff", "#008000ff", "#00ff00ff", "#ffa500ff", "#ff0000ff", "#ffff00ff" };
    string[] usernames = { "sazzles", "nuttyskateboard", "crashanalyst", "weirdzing", "prizewhirr", "doog", "doggo6", "catmash", "batman56", "zeroi", "tweetcurly", "evebrewer", "oinklung", "forecastser", "MisterIXI", "sazzles", "Hyuroki", "countercurl", "Sherpod", "Lollo", "Wayai", "SupremePat3", "Freexis", "Robyes", "Tarall45", "1337Hax0r", "Nippy", "Tnt23", "N1ifgt", "892fgh", "4rchik1l", "Jumbl3", "W4dfhe", "Mirror32", "Luff1", "catnip2", "roflcorny", "ItIsYe", "MrGame", "fifilikescats", "islathetequila", "beefy74", "beb1", "Dirk", "nick3212", "username34", "boi89", "superk1ll3r", "piraz", "Olgnfar", "Omu_sa", "kilwol1", "pandaman237", "manu8l", "piggatsu", "palmtree61", "gemer", "ninjaboi8", "xluvX", "david_lynt", "50knife51", "Damni", "Search55", "y4nsi", "Splinter324", "RealKoll3", "h4X0Rm3an", "jackiii112", "excuton", "lilalulo", "m0m3yx", "ferryas", "asdfghjiko", "1118ee112", "hecke89", "brettchef", "satakeluv", "t00ts111", "jonesie", "reddog", "catnip1", "katti7", "edgelord99", "kawaiihawaii", "nippitwist", "mnvasdf", "sanfterbube2", "saftnase", "sweet11", "1337_AMA_L", "LLAMA4", "letter_box", "breadcat", "salam1", "indioana_dog", "harambes_wife", "number_8ight", "laxsaxmax", "1892klover", "piano1234", "tigerwutz", "spongey1", "bunnydad", "omegalovan1", "sauna0", "squeez" };
    string[] neutral = { "hi", "Hello", "Hi!", "Yooo", "lmao", "haha", "lol", "LUL", "ur muted", "ads wtf", "hahaha", ":D", "is this live?", "whats this game called", "same lol", "how are you today", "can we play something else", "why are we playing this", "no way", "yoo", "what are these graphics", "is this ranked?", "<3", "yooo", "XD", "xd", "uhh", "oh", "yo", "waddup", "im back what did i miss", ":)", "oh man", "classic cha0sgamer haha", "lol", "dude", "this looks like trash tbh", "that splatanim xD", "greetings from germany", "8)", ":3", "im hungry", "OMEGALUL" };
    string[] excited = { "OH", "pausing", "almost there", "Pausechamp", "YOU CAN DO IT", "Pausechamp", "Pausechamp", "Pausechamp", "Pausechamp", "Pausechamp", "Puasechamp", "pausing" };
    string[] fail = { "KEKW", "HAHAHAHHAHAHAHa", "SO BAD", "LOLOLL", "HAHAHHAA", "HAHAAHA", "WASHED UP", "NA jump", "xDDD", "XDDDD", "LMAAOOO", "soooBAD", "LUL", "OMEGALUL", "HAAHAHA", "SOOO BAD", "KEKW KEKW KEKW KEKW", "KEK", "KEKW KEKW KEKW", "KEKW KEKW", "KEKW", "KEKW", "KEKW", "KEKW", "KEKW KEKW", "KEK", "KEK", "LOLOOL", "XDDD", "AHAHAA", "HAHAA", "OMEGALUL", "OMEGALUL", "OMEGALUL", "OMEGALUL" };
    string[] success = { "EZ", "POG", "pog", "LETSGO", "nice", "NICE", "LETSGOO", "POG POG POG POG POG", "poggers", "YOOOOO", "POG", "POG", "POG", "POG", "POG", "POG", "POG", "POG", "poggers", "Pog", "Pog", "YOU DID IT", "LETSGOOOO" };

    Dictionary<int, string[]> MsgDictionary;
    // Update is called once per frame

    void Awake()
    {
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

    void FixedUpdate()
    {
        if (Time.time > nextMessageTime)
        {
            int excitementLvl = getExcitement();
            string username = usernames[(int)Random.Range(0f, usernames.Length)];
            string[] msg = MsgDictionary[excitementLvl];
            int randomizer = (int)Random.Range(1f, 10f);
            switch (randomizer)
            {
                case 1:
                case 2:
                case 3:
                case 4:
                case 5:
                case 6:
                case 7:
                    MessageList.Add(GetTwatchMessage(username, msg[(int)Random.Range(0f, (int)msg.Length)]));
                    break;
                case 8:
                case 9:
                    MessageList.Add(GetTwatchMessage(username, msg[(int)Random.Range(0f, (int)msg.Length)]));
                    MessageList.Add(GetTwatchMessage(username, msg[(int)Random.Range(0f, (int)msg.Length)]));
                    break;
                case 10:
                    MessageList.Add(GetTwatchMessage(username, msg[(int)Random.Range(0f, (int)msg.Length)]));
                    MessageList.Add(GetTwatchMessage(username, msg[(int)Random.Range(0f, (int)msg.Length)]));
                    MessageList.Add(GetTwatchMessage(username, msg[(int)Random.Range(0f, (int)msg.Length)]));
                    break;
            }
            nextMessageTime = Time.time + Random.Range(0.01f, 1f);
        }

        //             if (ViewerController.CheckProgress())
        // {
        //     ViewerController.viewerCounter += Random.Range(4f, 11f);
        // }
        // if (ViewerController.CheckHype())
        // {
        //     ViewerController.viewerCounter *= 1.05f;
        // }
        // if (ViewerController.CheckFall())
        // {
        //     ViewerController.viewerCounter *= 1.5f;
        // }
        // ViewerController.ViewerCounterLabel.text = "<color=\"red\">" + (int)ViewerController.viewerCounter;
    }


    int getExcitement()
    {
        return (int)Random.Range(1f, 4f);
    }

    public Message GetTwatchMessage(string Username, string TxtInput)
    {
        if (MessageList.Count >= MaxMessages)
        {
            Destroy(MessageList[0].MessageTextObject.gameObject);
            MessageList.Remove(MessageList[0]);
        }
        Message NewMessage = new Message();
        NewMessage.User = Username;
        NewMessage.MsgText = TxtInput;
        TextMeshProUGUI NewTMP = Instantiate(TwatchTextObject, ChatPanel.transform);
        NewMessage.MessageTextObject = NewTMP;
        NewMessage.MessageTextObject.text = "<color=" + colors[(int)Random.Range(0f, (int)colors.Length)] + ">" + NewMessage.User + "</color>" + ": " + NewMessage.MsgText;
        return NewMessage;
    }
}

[System.Serializable]
public class Message
{
    public string User;
    public string MsgText;
    public TextMeshProUGUI MessageTextObject;
}

public class TwatchMsg
{
    public string Message;
    public int Category;
}