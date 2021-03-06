using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using WebSocketSharp;
//using HybridWebSocket;
//using NativeWebSocket;
using Newtonsoft.Json.Linq;
using System.Text;

public class GridSpaceOnline : MonoBehaviour
{
    public Button button;
    public Text buttonText;
    private GameControllerOnline gameController;
    public Sprite Player1;
    public Sprite Player2;
    private Color playerColor;
    private string prevPlayer;
    private WebSocket ws;
    public static bool[] isDisabled;
    string gameId = WsClient.gameId;
    AudioSource buttonClickSound;
    bool isTimed;
    bool startTimer;
    public float timeValue;
    Text timeText;

    void Awake()
    {
        if (WsClient.gameMode.Equals("timed"))
        {
            isTimed = true; //This is so the timer can be enabled
            Canvas mainCanvas = GameObject.Find("Canvas").GetComponent<Canvas>();
            GameObject timerPanel = mainCanvas.transform.Find("Timer Panel").gameObject;
            timerPanel.SetActive(true);
            timeText = timerPanel.GetComponentInChildren<Text>();
            timeValue = WsClient.timerValue;
        }
    }

    private void Start()
    {
        buttonClickSound = GetComponent<AudioSource>();
        Canvas waitingCanvas = GameObject.Find("Waiting Canvas").GetComponent<Canvas>();
        waitingCanvas.sortingOrder = 1;
        isDisabled = new bool[(int)(Mathf.Pow(WsClient.size, 2))];
        Text redText = GameObject.Find("RedIdentify").GetComponent<Text>();
        Text blueText = GameObject.Find("BlueIdentify").GetComponent<Text>();
        Text purpleText = GameObject.Find("PurpleIdentify").GetComponent<Text>();
        Text greenText = GameObject.Find("GreenIdentify").GetComponent<Text>();
        Text playerText = GameObject.Find("PlayerText").GetComponent<Text>();
        Text gameIdText = GameObject.Find("GameCode").GetComponent<Text>();
        playerText.text = WsClient.clientName;

        JArray clients = (JArray)WsClient.game["clients"];
        foreach (var client in clients)
        {
            string cId = client["clientId"].ToString();
            string cName = client["name"].ToString();
            string clientColor = client["color"].ToString();
            if (clientColor == "red") redText.text = cName;
            if (clientColor == "blue") blueText.text = cName;
            if (clientColor == "purple") purpleText.text = cName;
            if (clientColor == "green") greenText.text = cName;
        }
        gameIdText.text = "Game Code : " + gameId;


        ws = WsClient.ws;

        //ws.OnMessage += (message) =>
        ws.OnMessage += (sender, e) =>
        {
            //JObject response = JObject.Parse(Encoding.UTF8.GetString(message));

            JObject response;

            if (e.IsText)
            {
                response = JObject.Parse(e.Data);
            }
            else
            {
                response = JObject.Parse(Encoding.UTF8.GetString(e.RawData));
            }
            if (response["method"].ToString() == "start")
            {
                startTimer = true;
                UnityMainThread.wkr.AddJob(() =>
                {
                    Canvas waitingCanvas = GameObject.Find("Waiting Canvas").GetComponent<Canvas>();
                    waitingCanvas.sortingOrder = 0;
                });

                if (WsClient.color == "red")
                {
                    GameControllerOnline.playable = true;
                }
            }
            if (response["method"].ToString() == "play")
            {
                JToken move = response["move"];
                int buttonId = int.Parse(move["buttonId"].ToString());
                string color = move["color"].ToString();
                isDisabled[buttonId] = true;

                UnityMainThread.wkr.AddJob(() =>
                {
                    GameObject[] cardPanels = GameObject.FindGameObjectsWithTag("cardPanel");
                    foreach (GameObject cardPanel in cardPanels) cardPanel.SetActive(false);
                    Button button = GameObject.Find(buttonId.ToString()).GetComponent<Button>();
                    if (color == "red") { playerColor = Color.red; }
                    else if (color == "blue") { playerColor = Color.blue; }
                    else if (color == "purple") { playerColor = new Color32(190, 0, 255, 255); }
                    else if (color == "green") { playerColor = Color.green; }
                    ColorBlock cb = button.colors;
                    cb.disabledColor = playerColor;
                    button.colors = cb;
                    button.interactable = false;
                    buttonClickSound.Play();

                });
            }
            if (response["method"].ToString() == "snatch")
            {
                string buttonId = response["button"].ToString();

                UnityMainThread.wkr.AddJob(() =>
                {
                    Button button = GameObject.Find(buttonId.ToString()).GetComponent<Button>();
                    button.interactable = true;
                    Debug.Log($"Token {buttonId} was snatched");
                    Canvas mainCanvas = GameObject.Find("Canvas").GetComponent<Canvas>();
                    GameObject doomCardPanel = mainCanvas.transform.Find("Doom Card Panel").gameObject;
                    doomCardPanel.SetActive(false);
                });
            }
        };

    }

    public void SetGameControllerReference(GameControllerOnline controller)
    {
        gameController = controller;
    }


    public void setSpace()
    {
        if (GameControllerOnline.playable)
        {
            setButton();
        }
    }


    public void setButton()
    {
        //Debug.Log($"Button {button.name} was pressed");
        JObject payload = new JObject();
        payload["method"] = "play";
        payload["clientId"] = WsClient.clientId;
        payload["gameId"] = WsClient.gameId;
        payload["buttonId"] = button.name;
        payload["color"] = WsClient.color;
        payload["clientName"] = WsClient.clientName;
        //ws.Send(payload.ToString());
        ws.Send(Encoding.UTF8.GetBytes(payload.ToString()));

    }

    void Update()
    {
        if (isTimed && startTimer)
        {
            if (timeValue > 0f)
            {
                timeValue -= Time.deltaTime;
            }
            else timeValue = 0f;

            DisplayTime(timeValue);
        }

    }

    void DisplayTime(float timeToDisplay)
    {
        if (timeToDisplay < 0)
        {
            timeToDisplay = 0;
        }

        float minutes = Mathf.FloorToInt(timeToDisplay / 60f);
        float seconds = Mathf.FloorToInt(timeToDisplay % 60f);

        timeText.text = string.Format("{0:00}:{1:00}", minutes, seconds);

    }


}
