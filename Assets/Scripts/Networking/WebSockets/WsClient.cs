using UnityEngine;
using WebSocketSharp;
using UnityEngine.UI;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine.SceneManagement;
public class WsClient : MonoBehaviour
{
    public static WebSocket ws;
    public int port = 6969;
    public string ip = "127.0.0.1";
    public InputField gameIdField;
    public InputField nameField;
    public static string clientId;
    public static string gameId;
    public static int size;
    public static string color;
    public static JToken game;
    public static string clientName;

    private void Start()
    {
        DontDestroyOnLoad(this);
        ws = new WebSocket($"ws://{ip}:{port}");
        ws.Connect();
        ws.OnMessage += (sender, message) =>
        {
            JObject response = JObject.Parse(message.Data);
            if (response["method"].ToString() == "connect")
            {
                clientId = response["clientId"].ToString();
                Debug.Log($"Client ID set successfully: {clientId}");
            }

            if (response["method"].ToString() == "create")
            {
                JToken game = response["game"];
                gameId = game["id"].ToString();
                size = int.Parse(game["size"].ToString());

                Debug.Log($"game successfully created with ID: {gameId} and size of {size}");

                UnityMainThread.wkr.AddJob(() =>
                {
                    TextEditor te = new TextEditor();
                    te.text = gameId;
                    te.SelectAll();
                    te.Copy();
                });
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
                //sceneLoader.Switch();
                UnityMainThread.wkr.AddJob(() =>
                {
                    SceneManager.LoadScene("GameOnline");
                });
            }
        };
    }

    public void createRoom()
    {
        JObject payload = new JObject();
        payload["method"] = "create";
        ws.Send(payload.ToString());

    }

    public void joinRoom()
    {
        clientName = nameField.text;
        if (gameIdField.text.Trim().Length > 0) gameId = gameIdField.text.Trim();
        if (clientName == "")
        {
            Debug.Log("Put ur name there ");
            return;
        }
        TextEditor te = new TextEditor();
        JObject payload = new JObject();
        payload["method"] = "join";
        payload["clientId"] = clientId;
        payload["name"] = clientName;
        payload["gameId"] = gameId;
        ws.Send(payload.ToString());
    }
}