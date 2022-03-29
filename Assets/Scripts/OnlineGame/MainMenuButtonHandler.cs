using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuButtonHandler : MonoBehaviour
{
    public GameObject serverButtonPrefab;
    public InputField nameField;
    public void OpenHelpMenu()
    {
        Canvas mainCanvas = GameObject.Find("Canvas").GetComponent<Canvas>();
        GameObject mainUI = mainCanvas.transform.Find("MainUIHolder").gameObject;
        GameObject helpMenu = mainCanvas.transform.Find("HelpMenuHolder").gameObject;
        mainUI.SetActive(false);
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
        WsClient.clientName = nameField.text;

        Canvas mainCanvas = GameObject.Find("Canvas").GetComponent<Canvas>();
        GameObject mainUI = mainCanvas.transform.Find("MainUIHolder").gameObject;
        GameObject serverBrowser = mainCanvas.transform.Find("ServerBrowserList").gameObject;
        mainUI.SetActive(false);
        serverBrowser.SetActive(true);
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


}
