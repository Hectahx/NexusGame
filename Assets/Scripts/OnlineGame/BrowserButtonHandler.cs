using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Newtonsoft.Json.Linq;
using System.Text;

public class BrowserButtonHandler : MonoBehaviour
{
    public Game game;
    public Text gameID;
    public Text gridSize;
    public Text playerCount;


    public void JoinGame()
    {
        var clientName = WsClient.clientName;
        if (clientName == "")
        {
            Debug.Log("Put your name there");
            return;
        }

        JObject payload = new JObject();
        payload["method"] = "join";
        payload["clientId"] = WsClient.clientId;
        payload["name"] = WsClient.clientName;
        payload["gameId"] = game.id;
        //ws.Send(payload.ToString());
        WsClient.ws.Send(Encoding.UTF8.GetBytes(payload.ToString()));
        WsClient.gameId = game.id;
    }
    public void setAlltext()
    {
        gameID.text = $"Game ID : {game.id}";
        playerCount.text = $"Player Count : {game.clients.Count} / {game.limit}";
        gridSize.text = $"Grid Size: {game.size}";
    }
}
