using UnityEngine;
//using WebSocketSharp;
using UnityEngine.UI;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine.SceneManagement;
//using HybridWebSocket;
using NativeWebSocket;
using System.Text;
using System.Collections.Generic;

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
    public static JToken game;
    public static string clientName;
    public TextAsset profanityList;
    public static List<Game> gameList;
    public GameObject test;

    private async void Start()
    {

        gameId = null;

        DontDestroyOnLoad(this);

        ws = new WebSocket($"ws://{localIP}:{port}");


        ws.OnOpen += () =>
        {
            Debug.Log("Connection open!");
        };
        ws.OnMessage += (message) =>
        {
            JObject response = JObject.Parse(Encoding.UTF8.GetString(message));
            if (response["method"].ToString() == "connect")
            {
                clientId = response["clientId"].ToString();
                //string games = "[\n" + response["games"].ToString() + "\n ]";
                string gameString = response["games"].ToString();
                Debug.Log(gameString);
                gameList = JsonConvert.DeserializeObject<List<Game>>(gameString);
            }

            if (response["method"].ToString() == "create")
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
        };

        await ws.Connect();

    }

    public void createRoom()
    {
        JObject payload = new JObject();
        payload["method"] = "create";
        //ws.Send(payload.ToString());
        ws.Send(Encoding.UTF8.GetBytes(payload.ToString()));
    }

    public void joinRoom()
    {
        clientName = nameField.text;
        if (gameIdField.text.Trim().Length > 0) gameId = gameIdField.text.Trim();
        if (clientName == "")
        {
            Debug.Log("Put your name there ");
            return;
        }

        string[] profFilter = profanityList.text.Split('\n');

        foreach (string prof in profFilter)
        {
            if (clientName.Contains(prof))
            {
                Debug.Log("This name contains profanity, please enter a new name");
                return;
            }
        }

        JObject payload = new JObject();
        payload["method"] = "join";
        payload["clientId"] = clientId;
        payload["name"] = clientName;
        payload["gameId"] = gameId;
        //ws.Send(payload.ToString());
        ws.Send(Encoding.UTF8.GetBytes(payload.ToString()));
    }

    private async void OnApplicationQuit()
    {
        await ws.Close();
    }

    private void Update()
    {
#if !UNITY_WEBGL || UNITY_EDITOR
        ws.DispatchMessageQueue();
#endif
    }
}