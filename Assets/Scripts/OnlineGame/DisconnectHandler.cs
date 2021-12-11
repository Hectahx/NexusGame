using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using WebSocketSharp;
using Newtonsoft.Json.Linq;
using UnityEngine.SceneManagement;


public class DisconnectHandler : MonoBehaviour
{
    private WebSocket ws;
    private void Start()
    {
        ws = WsClient.ws;

        ws.OnMessage += (sender, message) =>
        {
            JObject response = JObject.Parse(message.Data);
            if (response["method"].ToString() == "timeout")
            {
                string color = response["color"].ToString();
                string name = response["name"].ToString();

                if (color == WsClient.color)
                {
                    Debug.Log("You are AFK");
                }

                UnityMainThread.wkr.AddJob(() =>
                {
                    Canvas timeoutCanvas = GameObject.Find("Timeout Canvas").GetComponent<Canvas>();
                    timeoutCanvas.sortingOrder = 1;
                    GameObject waitingPanel = timeoutCanvas.transform.Find("WaitingPanel").gameObject;
                    GameObject waitingObjectsHolder = waitingPanel.transform.Find("WaitingObjectsHolder").gameObject;
                    GameObject afkObjectsHolder = waitingPanel.transform.Find("AFKObjectsHolder").gameObject;
                    if (color == WsClient.color)
                    {
                        waitingObjectsHolder.SetActive(false);
                        afkObjectsHolder.SetActive(true);

                    }
                    else
                    {
                        waitingObjectsHolder.SetActive(true);
                        waitingObjectsHolder.GetComponentInChildren<Text>().text = $"Waiting on <color={color}>{name}</color>";
                        afkObjectsHolder.SetActive(false);
                    }
                });

            }

            if (response["method"].ToString() == "disconnect")
            {
                string color = response["color"].ToString();
                bool cont = ((bool)response["continue"]);


                if (WsClient.color == color)
                {
                    Debug.Log("you will be disconnected");
                    UnityMainThread.wkr.AddJob(() =>
                    {
                        SceneManager.LoadScene("MainDevelop");
                    });
                }
                else
                {
                    UnityMainThread.wkr.AddJob(() =>
                    {
                        Canvas timeoutCanvas = GameObject.Find("Timeout Canvas").GetComponent<Canvas>();
                        GameObject waitingPanel = timeoutCanvas.transform.Find("WaitingPanel").gameObject;
                        GameObject waitingObjectsHolder = waitingPanel.transform.Find("WaitingObjectsHolder").gameObject;
                        GameObject afkObjectsHolder = waitingPanel.transform.Find("AFKObjectsHolder").gameObject;
                        waitingObjectsHolder.SetActive(false);
                        afkObjectsHolder.SetActive(false);
                        timeoutCanvas.sortingOrder = 0;
                        if (!cont)
                        {
                            Canvas gameOverCanvas = GameObject.Find("Game Over Canvas").GetComponent<Canvas>();
                            Text gameOverText = gameOverCanvas.GetComponentInChildren<Text>();
                            gameOverCanvas.sortingOrder = 1;
                            gameOverText.text = $"You have won by default";

                            Debug.Log($"You have won by default");
                        }
                    });
                }

            }

            if (response["method"].ToString() == "continue")
            {

                UnityMainThread.wkr.AddJob(() =>
                {
                    Canvas timeoutCanvas = GameObject.Find("Timeout Canvas").GetComponent<Canvas>();
                    GameObject waitingPanel = timeoutCanvas.transform.Find("WaitingPanel").gameObject;
                    GameObject waitingObjectsHolder = waitingPanel.transform.Find("WaitingObjectsHolder").gameObject;
                    GameObject afkObjectsHolder = waitingPanel.transform.Find("AFKObjectsHolder").gameObject;
                    waitingObjectsHolder.SetActive(false);
                    afkObjectsHolder.SetActive(false);
                    timeoutCanvas.sortingOrder = 0;
                });
            }
            if (response["method"].ToString() == "winByDefault")
            {
                JToken winner = response["winner"];
                string color = winner["color"].ToString();
                string clientId = winner["clientId"].ToString();
                string clientName = winner["clientName"].ToString();

                UnityMainThread.wkr.AddJob(() =>
                {

                    Canvas timeoutCanvas = GameObject.Find("Timeout Canvas").GetComponent<Canvas>();
                    timeoutCanvas.sortingOrder = 0;

                    Canvas gameOverCanvas = GameObject.Find("Game Over Canvas").GetComponent<Canvas>();
                    Text gameOverText = gameOverCanvas.GetComponentInChildren<Text>();
                    gameOverCanvas.sortingOrder = 1;
                    gameOverText.text = $"You have won by default";

                    Debug.Log($"You have won by default");


                });
            }


        };
    }

    public void ContinueButton()
    {
        JObject payload = new JObject();
        payload["method"] = "timeout";
        payload["gameId"] = WsClient.gameId;
        payload["color"] = WsClient.color;
        ws.Send(payload.ToString());
    }
}
