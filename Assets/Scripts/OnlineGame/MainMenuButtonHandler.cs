using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuButtonHandler : MonoBehaviour
{
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

}
