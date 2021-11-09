using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class GameController : MonoBehaviour
{
    //public GameObject[] rawButtonList;
    public List<Text> buttonL = new List<Text>();
    public Text[] buttonList;
    public CreateGrid gridCreator;
    public string playerSide;
    int moves = 0;
    int limit = 5;


    void Awake()
    {
        gridCreator.CreateLines();
        gridCreator.CreateButtons();
        GameObject[] rawButtonList = GameObject.FindGameObjectsWithTag("gameButton");
        foreach (GameObject button in rawButtonList)
        {
            buttonL.Add(button.GetComponentInChildren<Text>());
        }
        buttonList = buttonL.ToArray();

        SetGameControllerReferenceOnButtons();
        playerSide = "X";
    }

    void SetGameControllerReferenceOnButtons()
    {
        for (int i = 0; i < buttonList.Length; i++)
        {
            buttonList[i].GetComponentInParent<GridSpace>().SetGameControllerReference(this);
        }
    }

    public string GetPlayerSide()
    {
        return playerSide;
    }

    void ChangeSides()
    {
        playerSide = (playerSide == "X") ? "O" : "X"; // Note: Capital Letters for "X" and "O"
    }

    public void EndTurn(Button button)
    {
        moves++;
        //Debug.Log(button.name + " was pressed");
        //if (moves >= 9)
        //{
        CheckWin(button);
        //}

        ChangeSides();
    }

    void CheckWin(Button button)
    {
        bool isConnected = true;
        int index = int.Parse(button.name);//gets the index of the button that was clicked
        int lines = gridCreator.getSize();//gets size of the grid
        int diagRight = lines + 1;
        int diagLeft = lines - 1;

        int tempIndex = index;
        Debug.Log(index);

        if (index % lines >= limit)
        {
            Debug.Log("Checking Right to Left");
            for (int i = 0; i < limit - 1; i++)
            {
                Debug.Log(index - i);
            }

        }

        if (lines - (index % lines) >= limit - 1)
        {
            Debug.Log("Checking Left to Right");
            for (int i = 0; i < limit - 1; i++)
            {
                Debug.Log(index + i);
            }
        }


        if (isConnected)
        {
            //Debug.Log("You've Won! 2");
            //SetBoardInteractable(true);
            return;
        }

        if (index % lines > limit - 1 && (index / lines) > limit - 1) Debug.Log("This is not corner");

        tempIndex = index;


        /*
        string diagIndex = (index - diagRight).ToString();
        GameObject diagBut = GameObject.Find(diagIndex);
        //Debug.Log(diagBut.name);
        */
    }


    void SetBoardInteractable(bool toggle)
    {
        for (int i = 0; i < buttonList.Length; i++)
        {
            buttonList[i].GetComponentInParent<Button>().interactable = toggle;
        }
    }


}
