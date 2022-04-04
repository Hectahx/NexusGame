using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Text;

public class MainMenuButtonHandler : MonoBehaviour
{
    public GameObject serverButtonPrefab;
    public InputField nameField;
    public Dropdown gameModeDropDown;
    public Dropdown timeDropDown;

    void Start()
    {
        gameModeDropDown.onValueChanged.AddListener(delegate
        {
            gameModeDropDownChanged(gameModeDropDown);
        });
    }
    public void OpenHelpMenu() //These show functions are attached to buttons on the UI
    {
        Canvas mainCanvas = GameObject.Find("Canvas").GetComponent<Canvas>(); //This finds the Canvas in the Scene and assigns it to a variable
        GameObject mainUI = mainCanvas.transform.Find("MainUIHolder").gameObject; //These 2 lines get the Main UI and the Help Menu as Game Objects
        GameObject helpMenu = mainCanvas.transform.Find("HelpMenuHolder").gameObject;
        mainUI.SetActive(false);//These two enable/disable them accordingly
        helpMenu.SetActive(true);
    }

    public void CloseHelpMenu()
    {
        Canvas mainCanvas = GameObject.Find("Canvas").GetComponent<Canvas>();
        GameObject mainUI = mainCanvas.transform.Find("MainUIHolder").gameObject;
        GameObject helpMenu = mainCanvas.transform.Find("HelpMenuHolder").gameObject;
        mainUI.SetActive(true);
        helpMenu.SetActive(false);
    }

    public void OpenServerBrowser()
    {
        JObject payload = new JObject();
        payload["method"] = "serverBrowser";
        payload["clientId"] = WsClient.clientId;

        WsClient.ws.Send(Encoding.UTF8.GetBytes(payload.ToString()));

        WsClient.clientName = nameField.text;

        Canvas mainCanvas = GameObject.Find("Canvas").GetComponent<Canvas>();
        GameObject mainUI = mainCanvas.transform.Find("MainUIHolder").gameObject;
        GameObject serverBrowser = mainCanvas.transform.Find("ServerBrowserList").gameObject;
        mainUI.SetActive(false);
        serverBrowser.SetActive(true);
    }

    public void ServerBrowserLoader()
    {
        GameObject listContent = GameObject.FindGameObjectWithTag("ListContent");

        foreach (Game game in WsClient.gameList)
        {

            GameObject serverJoin = Instantiate(serverButtonPrefab) as GameObject;

            serverJoin.transform.SetParent(listContent.transform, false);
            BrowserButtonHandler browHandler = serverJoin.GetComponentInChildren<BrowserButtonHandler>();
            browHandler.game = game;
            browHandler.setAlltext();
        }
    }

    public void CloseServerBrowser()
    {
        Canvas mainCanvas = GameObject.Find("Canvas").GetComponent<Canvas>();
        GameObject mainUI = mainCanvas.transform.Find("MainUIHolder").gameObject;
        GameObject serverBrowser = mainCanvas.transform.Find("ServerBrowserList").gameObject;
        GameObject listContent = GameObject.FindGameObjectWithTag("ListContent");

        foreach (Transform child in listContent.transform)
        {
            GameObject.Destroy(child.gameObject);
        }
        mainUI.SetActive(true);
        serverBrowser.SetActive(false);
    }

    public void OpenCreateGameMenu()
    {
        Canvas mainCanvas = GameObject.Find("Canvas").GetComponent<Canvas>();
        GameObject mainUI = mainCanvas.transform.Find("MainUIHolder").gameObject;
        GameObject createUI = mainCanvas.transform.Find("CreateRoomHolder").gameObject;

        mainUI.SetActive(false);
        createUI.SetActive(true);
    }

    public void CloseCreateGameMenu()
    {
        Canvas mainCanvas = GameObject.Find("Canvas").GetComponent<Canvas>();
        GameObject mainUI = mainCanvas.transform.Find("MainUIHolder").gameObject;
        GameObject createUI = mainCanvas.transform.Find("CreateRoomHolder").gameObject;

        mainUI.SetActive(true);
        createUI.SetActive(false);
    }

    void gameModeDropDownChanged(Dropdown dropdown)
    {
        if (dropdown.options[dropdown.value].text.ToLower().Equals("timed"))
        {
            timeDropDown.gameObject.SetActive(true);
        }
        else
        {
            timeDropDown.gameObject.SetActive(false);
        }
    }


}
