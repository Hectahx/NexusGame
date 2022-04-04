using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using WebSocketSharp;
//using HybridWebSocket;
//using NativeWebSocket;
using Newtonsoft.Json.Linq;
using UnityEngine.SceneManagement;
using System.Text;
using System.Collections;


public class DisconnectHandler : MonoBehaviour
{
    //private WebSocket ws;
    private WebSocket ws;
    private void Start()
    {
        ws = WsClient.ws;

        //ws.OnMessage += (sender, message) =>
        ws.OnMessage += (sender, e) =>
        {
            //JObject response = JObject.Parse(Encoding.UTF8.GetString(message)); For NativeWebSocket

            JObject response;

            if (e.IsText)
            {
                response = JObject.Parse(e.Data);
            }
            else
            {
                response = JObject.Parse(Encoding.UTF8.GetString(e.RawData));
            }

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

                    //StartCoroutine(returnToHome());
                    SceneManager.LoadSceneAsync("MainDevelop");
                });
            }
            if (response["method"].ToString() == "noShow")
            {
                SceneManager.LoadSceneAsync("MainDevelop");
            }


        };
    }

    public void ContinueButton()
    {
        JObject payload = new JObject();
        payload["method"] = "timeout";
        payload["gameId"] = WsClient.gameId;
        payload["color"] = WsClient.color;
        //ws.Send(payload.ToString());
        ws.Send(Encoding.UTF8.GetBytes(payload.ToString()));
    }

    public IEnumerator returnToHome()
    {
        yield return new WaitForSecondsRealtime(5);

        SceneManager.LoadSceneAsync("MainDevelop");
    }

    public void leaveGame()
    {
        SceneManager.LoadSceneAsync("MainDevelop");
        JObject payload = new JObject();
        payload["method"] = "leaveGame";
        payload["gameId"] = WsClient.gameId;
        payload["color"] = WsClient.color;
        payload["clientId"] = WsClient.clientId;
        ws.Send(Encoding.UTF8.GetBytes(payload.ToString()));
    }

    void Update()
    {

    }
}
