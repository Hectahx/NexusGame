using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    /*
    Precursor, this was code that was made in a rush and looks like ass
    I haven't had time to clean this up but I will do so after the hack.
    
    TODO:
    Reduce amount of public vars
    Rewrite some functions e.g. the 
    */
    public Canvas gameOverPanel; public Text gameOverText;
    public GameObject redPanel; public GameObject bluePanel;
    public List<Text> buttonL = new List<Text>();
    public Text[] buttonList;
    public CreateGrid gridCreator;
    public WinLogic winLogic;
    public string playerSide;
    string playerString = "Wildcard For: ";
    public bool isReversed { get; set; }
    public bool isEnabled { get; set; }
    bool[] isDisabled;
    string prevPlayer;
    int limit = 5;
    int moves = 0;
    public Player playerRed = new Player("X");
    public Player playerBlue = new Player("O");
    private Color disColor = new Color32(53, 51, 51, 105);
    public Image redRevCard;
    public Image blueRevCard;
    public Image redEnabledCard;
    public Image blueEnabledCard;

    void Awake()
    {
        moves = 0;
        GameObject[] reverseCards = GameObject.FindGameObjectsWithTag("reversedCard");
        foreach (GameObject img in reverseCards)
        {
            if (img.name == "RedReverseCard") { redRevCard = img.GetComponent<Image>(); }
            if (img.name == "BlueReverseCard") { blueRevCard = img.GetComponent<Image>(); }
            if (img.name == "RedEnabledCard") { redEnabledCard = img.GetComponent<Image>(); }
            if (img.name == "BlueEnabledCard") { blueEnabledCard = img.GetComponent<Image>(); }

        }

        redRevCard.enabled = false;
        blueRevCard.enabled = false;
        blueEnabledCard.enabled = false;
        redEnabledCard.enabled = false;
        isDisabled = new bool[(int)(Mathf.Pow(gridCreator.getSize(), 2) + 1)];
        gridCreator.CreateLines();
        gridCreator.CreateButtons();


        GameObject[] rawButtonList =
            GameObject.FindGameObjectsWithTag("gameButton");
        foreach (GameObject button in rawButtonList)
        {
            buttonL.Add(button.GetComponentInChildren<Text>());
        }
        buttonList = buttonL.ToArray();
        SetGameControllerReferenceOnButtons();
        playerSide = "X";
        redPanel.SetActive(true);
        bluePanel.SetActive(false);
    }

    void SetGameControllerReferenceOnButtons()
    {
        for (int i = 0; i < buttonList.Length; i++)
        {
            buttonList[i]
                .GetComponentInParent<GridSpace>()
                .SetGameControllerReference(this);
        }
    }

    public string GetPlayerSide()
    {
        return playerSide;
    }

    void ChangeSides()
    {
        bool done = false;

        var rand = Random.Range(1, 100);


        if (rand >= 85)
        {
            while (!done) //this will loop until the button chosen is empty so it wont try to disable an already selected button
            {
                rand = Random.Range(1, 100);
                if (getButtonText(rand) == "empty")
                {
                    Button button = GameObject.Find(rand.ToString()).GetComponent<Button>();
                    ColorBlock cb = button.colors;
                    cb.normalColor = disColor;
                    cb.highlightedColor = disColor;
                    cb.selectedColor = disColor;
                    cb.pressedColor = disColor;
                    button.colors = cb;
                    //button.interactable = false;
                    isDisabled[rand] = true;
                    done = true;
                }
            }

        }
        bool boardEnabled = true; //used for debugging

        playerSide = (playerSide == playerRed.side) ? playerBlue.side : playerRed.side;

        if (moves % 7 == 0 && boardEnabled == true)
        {
            Canvas cardPanel = GameObject.FindGameObjectWithTag("cardPanel").GetComponent<Canvas>();
            Text whichPlayer = GameObject.FindGameObjectWithTag("whichPlayer").GetComponent<Text>();
            whichPlayer.text = playerString + ((playerSide == "X") ? "<color=red>Red</color>" : "<color=blue>Blue</color>");
            for (int i = 1; i < Mathf.Pow(gridCreator.getSize(), 2); i++)
            {
                if (isDisabled[i] == false)
                {
                    Button button = GameObject.Find(i.ToString()).GetComponent<Button>();
                    button.interactable = false;
                }
            }
            cardPanel.sortingOrder = 1;
            prevPlayer = playerSide;
        }

    }

    public void EndTurn(Button button)
    {
        moves++;
        winLogic = new WinLogic(gridCreator.getSize());
        if (moves >= 9)
        {
            if (winLogic.checkLeftToRight(button) == limit - 1) endGame(); //works
            if (winLogic.checkRightToLeft(button) == limit - 1) endGame(); //works
            if (winLogic.checkDownUp(button) == limit - 1) endGame(); //works
            if (winLogic.checkUpDown(button) == limit - 1) endGame();
            if (winLogic.checkDiagRightToLeftUp(button) == limit - 1) endGame();//works
            if (winLogic.checkDiagLeftToRightDown(button) == limit - 1) endGame();
            if (winLogic.checkDiagRightToLeftDown(button) == limit - 1) endGame();//works
            if (winLogic.checkDiagLeftToRightUp(button) == limit - 1) endGame();
        }
        isDisabled[int.Parse(button.name)] = true;

        if(moves == (gridCreator.getSize() * gridCreator.getSize())){
            endGame(draw:true);
        }
        ChangeSides();
    }

    void SetBoardInteractable(bool toggle)
    {
        for (int i = 0; i < buttonList.Length; i++)
        {
            buttonList[i].GetComponentInParent<Button>().interactable = toggle;
        }
    }

    string getButtonText(int index)
    {
        string buttonText =
            GameObject
                .Find(index.ToString())
                .GetComponent<Button>()
                .GetComponentInChildren<Text>()
                .text;
        buttonText = buttonText == "" ? "empty" : buttonText;
        return buttonText;
    }

    public void endGame(bool draw = false)
    {
        SetBoardInteractable(false);
        string winner = (playerSide == playerRed.side) ? "<color=red>Red</color>" : "<color=blue>Blue</color>";
        if(!draw) setGameOverText(winner + " Wins!");
        else setGameOverText("It's a draw!");
    }

    public void setReversed()
    {
        if (playerSide == playerRed.side) redRevCard.enabled = true; playerRed.isReversed = true;
        if (playerSide == playerBlue.side) blueRevCard.enabled = true; playerBlue.isReversed = true;
        disableCardPanel();
    }

    public void setEnabled()//TODO
    {
        if (playerSide == playerRed.side) redEnabledCard.enabled = true; playerRed.isEnabled = true;
        if (playerSide == playerBlue.side) blueEnabledCard.enabled = true; playerBlue.isEnabled = true;
        disableCardPanel();
    }

    public void allowEnabled()//TODO
    {
        if (playerSide == playerRed.side && playerRed.isEnabled)
        {
            isEnabled = true;
            Debug.Log("Should be enabled");

        }
        if (playerSide == playerBlue.side && playerBlue.isEnabled)
        {
            isEnabled = true;
            Debug.Log("Should be enabled");
        }


    }

    public void allowReversed() //TODO
    {
        if (playerSide == playerRed.side && playerRed.isReversed)
        {
            isReversed = true;
            Debug.Log("Should be enabled");

        }
        if (playerSide == playerBlue.side && playerBlue.isReversed)
        {
            isReversed = true;
            Debug.Log("Should be enabled");
        }
    }

    public void disableCardPanel()
    {
        Canvas cardPanel = GameObject.FindGameObjectWithTag("cardPanel").GetComponent<Canvas>();
        cardPanel.sortingOrder = 0;
        for (int i = 1; i < Mathf.Pow(gridCreator.getSize(), 2); i++)
        {
            if (isDisabled[i] == false)
            {
                Button button = GameObject.Find(i.ToString()).GetComponent<Button>();
                button.interactable = true;
            }
        }
    }

    void setGameOverText(string value)
    {
        gameOverPanel.sortingOrder = 2;
        gameOverText.text = value;
    }

}
