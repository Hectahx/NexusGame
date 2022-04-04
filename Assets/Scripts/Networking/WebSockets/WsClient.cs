using UnityEngine;
//using WebSocketSharp;
using UnityEngine.UI;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine.SceneManagement;
//using HybridWebSocket;
using System.Text;
using System.Collections.Generic;
using System.Collections;
using WebSocketSharp;


public class WsClient : MonoBehaviour
{
    public static WebSocket ws;
    public int port = 6969;
    public string localIP = "127.0.0.1";
    public string publicIP = "143.244.213.81";
    public InputField gameIdField;
    public InputField nameField;
    public static string clientId;
    public static string gameId;
    public static int size;
    public static string color;
    public static string gameMode;
    public static JToken game;
    public static string clientName;
    public static float timerValue;
    public TextAsset profanityList;
    public static List<Game> gameList;
    public GameObject test;
    public GameObject reconnectButton;
    public Text connectionText;
    public Dropdown playerSizeDropdown;
    public Dropdown gameModeDropdown;
    public Dropdown timerDropDown;
    bool publicServer = false;
    public InputField passwordInputField;
    public InputField mainMenuPasswordInputField;
    string passwordText;

    private void Start()
    {
        gameId = null;

        DontDestroyOnLoad(this);
        /*
        if (publicServer) ws = new WebSocket($"wss://{publicIP}:{port}");
        else ws = new WebSocket($"wss://{localIP}:{port}");

        ws.SslConfiguration.EnabledSslProtocols = System.Security.Authentication.SslProtocols.Tls12;


        ws.OnOpen += (sender, e) =>
        {
            Debug.Log("Connection open!");
            UnityMainThread.wkr.AddJob(() =>
            {
                connectionText.text = "Server Status <color=green>Open</color>";
                reconnectButton.SetActive(false);
            });
        };
        */
        ws = Login.ws;
        clientId = Login.clientId;
        if (Login.clientName != null)
        {
            clientName = Login.clientName;
            nameField.text = Login.clientName;
            nameField.enabled = false;
        }
        //ws.OnMessage += (message) => //This is for NativeWebSocket
        ws.OnMessage += (sender, e) =>
        {
            //JObject response = JObject.Parse(Encoding.UTF8.GetString(e));
            JObject response;

            if (e.IsText)
            {
                response = JObject.Parse(e.Data);
            }
            else
            {
                response = JObject.Parse(Encoding.UTF8.GetString(e.RawData));
            }

            if (response["method"].ToString() == "connect")
            {
                clientId = response["clientId"].ToString();
                string gameString = response["games"].ToString();
                //Debug.Log(gameString);
                gameList = JsonConvert.DeserializeObject<List<Game>>(gameString);
            }

            if (response["method"].ToString() == "create")//This is the response from the server
            {
                JToken game = response["game"];
                gameId = game["id"].ToString();
                size = int.Parse(game["size"].ToString());

                Debug.Log($"game successfully created with ID: {gameId} and size of {size}");

                UnityMainThread.wkr.AddJob(() =>
                {
                    gameId.CopyToClipboard();
                    joinRoom();
                });

                Debug.Log("Created successfully");
            }

            if (response["method"].ToString() == "join")
            {
                game = response["game"];
                gameMode = response["gameMode"].ToString();
                if (gameMode == "timed")
                {
                    timerValue = int.Parse(game["timeLength"].ToString());
                    Debug.Log(timerValue);
                }
                JArray clients = (JArray)game["clients"];
                size = int.Parse(game["size"].ToString());
                foreach (var client in clients)
                {
                    string cId = client["clientId"].ToString();
                    string clientColor = client["color"].ToString();
                    if (cId == clientId)
                    {
                        color = clientColor;
                        Debug.Log($"{color} is your color");
                    }
                }

                UnityMainThread.wkr.AddJob(() =>
                {
                    SceneManager.LoadScene("GameOnline");
                });
            }
            if (response["method"].ToString() == "serverBrowser")
            {
                string gameString = response["games"].ToString();
                //Debug.Log(gameString);
                WsClient.gameList = JsonConvert.DeserializeObject<List<Game>>(gameString);
            }
            if(response["method"].ToString() == "signupComplete"){
                
            }
        };
        ws.OnClose += (sender, e) =>
        {
            Debug.Log(e.Reason);
            //Debug.Log("The socket has been closed. Please Reconnect");
            UnityMainThread.wkr.AddJob(() =>
            {
                connectionText.text = "Server Status <color=red>Closed</color>";
                reconnectButton.SetActive(true);
            });
        };
        ws.OnError += (sender, e) =>
        {
            //Debug.Log(e.Exception);
        };


        ws.Connect();
    }

    public void createRoom()//This is bound to the create room button in unity 
    {
        var playerIndex = playerSizeDropdown.value;
        var playerSize = int.Parse(playerSizeDropdown.options[playerIndex].text);
        var gameModeIndex = gameModeDropdown.value;
        var gameMode = gameModeDropdown.options[gameModeIndex].text.ToLower();

        passwordText = passwordInputField.text.Trim();



        if (nameCheck())
        {
            JObject payload = new JObject();
            payload["method"] = "create";
            payload["playerSize"] = playerSize;
            payload["gameMode"] = gameMode;
            payload["clientId"] = clientId;
            if (gameMode.Equals("timed")) payload["timeLength"] = timerDropDown.options[timerDropDown.value].text.ToString();

            if (passwordText != "")
            {
                payload["isPrivate"] = true;
                payload["password"] = passwordText;
            }
            else payload["isPrivate"] = false;

            //ws.Send(payload.ToString());
            ws.Send(Encoding.UTF8.GetBytes(payload.ToString()));
        }
    }

    public void joinRoom()
    {
        if (nameCheck())
        {

            if (mainMenuPasswordInputField.text.Trim().Length > 0) passwordText = mainMenuPasswordInputField.text.Trim();

            JObject payload = new JObject();
            payload["method"] = "join";
            payload["clientId"] = clientId;
            payload["name"] = clientName;
            payload["gameId"] = gameId;
            payload["password"] = passwordText;
            //ws.Send(payload.ToString());
            ws.Send(Encoding.UTF8.GetBytes(payload.ToString()));
        }
    }


    public bool nameCheck()
    {
        clientName = nameField.text;

        if (gameIdField.text.Trim().Length > 0) gameId = gameIdField.text.Trim().ToUpper();

        if (clientName == "")
        {
            Debug.Log("Put your name there");
            return false;
        }

        string[] profFilter = profanityList.text.Split('\n');
        List<string> profanity = new List<string>(profFilter);

        foreach (string prof in profanity)
        {
            if (clientName.ToLower().Contains(prof.ToLower().Trim()))
            {
                Debug.Log("This name contains profanity, please enter a new name");
                StartCoroutine(profanityPopup());
                return false;
            }
        }

        return true;
    }
    public void reconnectToServer()
    {
        Start();
    }

    IEnumerator profanityPopup()
    {

        Canvas mainCanvas = GameObject.Find("Canvas").GetComponent<Canvas>();
        GameObject profanityWarning = mainCanvas.transform.Find("Profanity Warning").gameObject;
        profanityWarning.SetActive(true);

        yield return new WaitForSecondsRealtime(3);

        profanityWarning.SetActive(false);

    }


    private void OnApplicationQuit()
    {

        ws.Close();
    }

    private void Update()
    {
    }
}