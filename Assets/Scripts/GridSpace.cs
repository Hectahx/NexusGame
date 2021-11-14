using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GridSpace : MonoBehaviour
{
    public Button button;
    public Text buttonText;
    private GameController gameController;
    public Sprite Player1;
    public Sprite Player2;

    private Color redColor = Color.red;
    private Color blueColor = Color.blue;
    private Color grayColor = Color.gray;

    private string prevPlayer;

    private Color disColor = new Color32(53, 51, 51, 105);
    private bool redReversed;
    private bool blueReversed;
    private bool setReversed;



    public void SetGameControllerReference(GameController controller)
    {
        gameController = controller;
    }


    public void setSpace()
    {
        ColorBlock cb = button.colors;

        if (cb.normalColor == disColor)
        {
            if (gameController.isEnabled)
            {
                if (gameController.playerRed.isEnabled == true && gameController.playerRed.side == gameController.playerSide)
                {
                    gameController.redEnabledCard.enabled = false;
                    setButton();
                    gameController.playerRed.isEnabled = false;
                    gameController.isEnabled = false;
                }
                if (gameController.playerBlue.isEnabled == true && gameController.playerBlue.side == gameController.playerSide)
                {
                    gameController.blueEnabledCard.enabled = false;
                    setButton();
                    gameController.playerBlue.isEnabled = false;
                    gameController.isEnabled = false;
                }
            }
        }
        else if (gameController.isReversed)
        {
            if (gameController.playerRed.isReversed == true && gameController.playerRed.side == gameController.playerSide)
            {
                setButton();
                redReversed = true;
            }
            if (gameController.playerBlue.isReversed == true && gameController.playerBlue.side == gameController.playerSide)
            {
                setButton();
                blueReversed = true;
            }
            setReversed = true;
        }
        else
        {
            setButton();
        }
    }


    public void setButton()
    {
        ColorBlock cb = button.colors;
        button.interactable = false;

        if (setReversed)
        {
            buttonText.text = prevPlayer;
            Debug.Log(prevPlayer);
            if (blueReversed)
            {
                gameController.blueRevCard.enabled = false;
                gameController.playerBlue.isReversed = false;
                gameController.isReversed = false;
            }
            else if (redReversed)
            {
                gameController.redRevCard.enabled = false;
                gameController.playerRed.isReversed = false;
                gameController.isReversed = false;
            }

        }
        else
        {
            buttonText.text = gameController.GetPlayerSide();
        }

        if (buttonText.text == gameController.playerRed.side) cb.disabledColor = redColor;
        if (buttonText.text == gameController.playerBlue.side) cb.disabledColor = blueColor;

        button.colors = cb;

        //Debug.Log($"Button {button.name} was pressed");
        gameController.EndTurn(button);
        prevPlayer = buttonText.text;

    }


}
