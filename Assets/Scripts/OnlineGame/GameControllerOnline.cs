using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//using WebSocketSharp;
//using HybridWebSocket;
using NativeWebSocket;
using Newtonsoft.Json.Linq;
using System.Text;
using UnityEngine.SceneManagement;
using System.Collections;
public class GameControllerOnline : MonoBehaviour
{
    public List<Text> buttonL = new List<Text>();
    public static Text[] buttonList;
    public CreateGrid gridCreator;
    private Color disColor = new Color32(53, 51, 51, 105);

    private WebSocket ws;



    public static bool playable;//this checks if the current player can play

    void Awake()
    {
        GameObject[] cardPanels = GameObject.FindGameObjectsWithTag("cardPanel");
        foreach (GameObject cardPanel in cardPanels) cardPanel.SetActive(false);


        disableCards();
        gridCreator.CreateButtons();
        gridCreator.CreateLines();

        //isDisabled = new bool[(int)(Mathf.Pow(gridCreator.getSize(), 2))];
        GameObject[] rawButtonList = GameObject.FindGameObjectsWithTag("gameButton");
        foreach (GameObject button in rawButtonList) buttonL.Add(button.GetComponentInChildren<Text>());

        buttonList = buttonL.ToArray();
        ws = WsClient.ws;
        //SetGameControllerReferenceOnButtons();
    }

    void Start()
    {
        //ws.OnMessage += (sender, message) =>
        ws.OnMessage += (message) =>
        {
            JObject response = JObject.Parse(Encoding.UTF8.GetString(message));
            //JObject response = JObject.Parse(message.Data);
            if (response["method"].ToString() == "win")
            {
                JToken winner = response["winner"];
                string color = winner["color"].ToString();
                string clientId = winner["clientId"].ToString();
                string clientName = winner["clientName"].ToString();
                if (color == WsClient.color) Debug.Log("I've won");

                UnityMainThread.wkr.AddJob(() =>
                {
                    Canvas gameOverCanvas = GameObject.Find("Game Over Canvas").GetComponent<Canvas>();
                    Text gameOverText = gameOverCanvas.GetComponentInChildren<Text>();
                    gameOverCanvas.sortingOrder = 1;
                    gameOverText.text = $"<color={color}>{clientName} won</color>";
                    SetBoardInteractable(false);
                    StartCoroutine(returnToHome());
                });
            }
            if (response["method"].ToString() == "move")
            {
                JToken currentMove = response["currentMove"];
                string color = currentMove["color"].ToString();
                //Debug.Log(color);
                if (color == WsClient.color)
                {
                    playable = true;
                }
                else
                {
                    playable = false;
                }
            }
            if (response["method"].ToString() == "cards")
            {
                string currentColor = response["currentColor"].ToString();

                if (currentColor == WsClient.color) playable = true;
                else playable = false;

                UnityMainThread.wkr.AddJob(() =>
                {

                    string currentColor = response["currentColor"].ToString();
                    string currentName = response["name"].ToString();
                    Canvas mainCanvas = GameObject.Find("Canvas").GetComponent<Canvas>();
                    GameObject cardPanel = mainCanvas.transform.Find("Card Panel").gameObject;
                    cardPanel.SetActive(true);
                    cardPanel.GetComponentInChildren<Text>().text = $"Wildcard for: <color={currentColor}>{currentName}</color>";

                });
            }
            if (response["method"].ToString() == "cardSelection")
            {
                Debug.Log("Card Selected");
                JToken card = response["card"];
                string cardType = card["cardType"].ToString();
                string color = card["color"].ToString();
                bool isActive = (bool)card["active"];

                UnityMainThread.wkr.AddJob(() =>
                {

                    Canvas mainCanvas = GameObject.Find("Canvas").GetComponent<Canvas>();
                    GameObject cardPanel = mainCanvas.transform.Find("Card Panel").gameObject;
                    cardPanel.SetActive(false);

                    toggleCards(color, cardType, true);
                });

            }
            if (response["method"].ToString() == "doomCards")
            {
                string currentColor = response["currentColor"].ToString();

                if (currentColor == WsClient.color) playable = true;
                else playable = false;

                UnityMainThread.wkr.AddJob(() =>
                {

                    string currentColor = response["currentColor"].ToString();
                    string currentName = response["name"].ToString();
                    Canvas mainCanvas = GameObject.Find("Canvas").GetComponent<Canvas>();
                    GameObject cardPanel = mainCanvas.transform.Find("Doom Card Panel").gameObject;
                    cardPanel.SetActive(true);
                    cardPanel.GetComponentInChildren<Text>().text = $"Doom Card for: <color={currentColor}>{currentName}</color>";

                });
            }

            if (response["method"].ToString() == "removeCard")
            {
                string cardType = response["cardType"].ToString();
                string color = response["color"].ToString();

                UnityMainThread.wkr.AddJob(() =>
                {
                    toggleCards(color, cardType, false);
                });
            }
            if (response["method"].ToString() == "deadToken")
            {
                string buttonId = response["button"].ToString();

                UnityMainThread.wkr.AddJob(() =>
                {
                    Button button = GameObject.Find(buttonId).GetComponent<Button>();
                    Color disColor = new Color32(53, 51, 51, 250);
                    ColorBlock cb = button.colors;
                    cb.normalColor = disColor;
                    cb.highlightedColor = disColor;
                    cb.selectedColor = disColor;
                    cb.pressedColor = disColor;
                    button.colors = cb;
                });

            }
            if (response["method"].ToString() == "skipTurn")
            {
                string oldColor = response["oldColor"].ToString();
                string newColor = response["newColor"].ToString();
                if (newColor == WsClient.color) playable = true;
                else playable = false;
                UnityMainThread.wkr.AddJob(() =>
                {
                    Canvas mainCanvas = GameObject.Find("Canvas").GetComponent<Canvas>();
                    GameObject doomCardPanel = mainCanvas.transform.Find("Doom Card Panel").gameObject;
                    doomCardPanel.SetActive(false);
                });
            }
        };
    }

    void SetGameControllerReferenceOnButtons()
    {
        for (int i = 0; i < buttonList.Length; i++)
        {
            buttonList[i].GetComponentInParent<GridSpaceOnline>().SetGameControllerReference(this);
        }
    }

    static void SetBoardInteractable(bool toggle)
    {
        for (int i = 0; i < buttonList.Length; i++)
        {
            buttonList[i].GetComponentInParent<Button>().interactable = toggle;
        }
    }

    void disableCards()
    {
        GameObject.Find("RedExtra").GetComponent<Image>().enabled = false;
        GameObject.Find("BlueExtra").GetComponent<Image>().enabled = false;
        GameObject.Find("GreenExtra").GetComponent<Image>().enabled = false;
        GameObject.Find("PurpleExtra").GetComponent<Image>().enabled = false;
        /////////////////////////////////////////////////////////////////////
        GameObject.Find("RedEnabled").GetComponent<Image>().enabled = false;
        GameObject.Find("BlueEnabled").GetComponent<Image>().enabled = false;
        GameObject.Find("GreenEnabled").GetComponent<Image>().enabled = false;
        GameObject.Find("PurpleEnabled").GetComponent<Image>().enabled = false;

    }


    void toggleCards(string color, string cardType, bool toggle)
    {
        /*
        This is used to toggle the cards on and off of the playing field
        */
        if (color == "red")
        {
            if (cardType == "extraCard") GameObject.Find("RedExtra").GetComponent<Image>().enabled = toggle;
            else if (cardType == "enabledCard") GameObject.Find("RedEnabled").GetComponent<Image>().enabled = toggle;
        }
        else if (color == "blue")
        {
            if (cardType == "extraCard") GameObject.Find("BlueExtra").GetComponent<Image>().enabled = toggle;
            else if (cardType == "enabledCard") GameObject.Find("BlueEnabled").GetComponent<Image>().enabled = toggle;
        }
        else if (color == "purple")
        {
            if (cardType == "extraCard") GameObject.Find("PurpleExtra").GetComponent<Image>().enabled = toggle;
            else if (cardType == "enabledCard") GameObject.Find("PurpleEnabled").GetComponent<Image>().enabled = toggle;
        }
        else if (color == "green")
        {
            if (cardType == "extraCard") GameObject.Find("GreenExtra").GetComponent<Image>().enabled = toggle;
            else if (cardType == "enabledCard") GameObject.Find("GreenEnabled").GetComponent<Image>().enabled = toggle;
        }
    }


    IEnumerator returnToHome()
    {
        yield return new WaitForSecondsRealtime(5);

        SceneManager.LoadSceneAsync("MainDevelop");
    }

        void Update()
    {
    #if !UNITY_WEBGL || UNITY_EDITOR
        ws.DispatchMessageQueue();
    #endif
    }

}
