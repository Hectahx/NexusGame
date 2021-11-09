using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GridSpace : MonoBehaviour
{
    public Button button;
    public Text buttonText;
    public string playerSide;
    private GameController gameController;


    public void SetGameControllerReference(GameController controller)
    {
        gameController = controller;
    }
    public void setSpace()
    {
        button.interactable = false;
        buttonText.text = gameController.GetPlayerSide();
        gameController.EndTurn(button);
    }


}
