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

    void CheckWin(Button button)//i started this on sat/sun and im still not finished on wednesday :/
    {
        //bool isConnected = false;
        int count = 0;
        int index = int.Parse(button.name);//gets the index of the button that was clicked
        int lines = gridCreator.getSize();//gets size of the grid
        int diagRight = lines + 1;
        int diagLeft = lines - 1;

        int tempIndex = index;

        int position = index % lines;
        int row = index / lines;
        //Debug.Log($"Position is {position} and index is {index}");

        if (position >= limit || position == 0) //If true checks the grid from right to left
        {
            //This checks standard right to left
            for (int i = 0; i < limit - 1; i++)
            {
                string buttonText1 = getButtonName(index - i);
                string buttonText2 = getButtonName(index - i - 1);
                if (buttonText1 == "empty" || buttonText2 == "empty")
                {
                    //Debug.Log(buttonText1 == "empty" ? index - i + " is empty" : index - 1 - i + " is empty");
                    //isConnected = false;
                    break;
                }

                if (string.Equals(buttonText1, buttonText2))
                {
                    //Debug.Log($"Index {index - i} is {buttonText1} and index {index - 1 - i} is {buttonText2}");
                    count++;
                }
                else
                {
                    //Debug.Log("They are not the same");
                    //isConnected = false;
                    break;
                }
            }


            if (count != 4)
            {
                //this checks if there is one on the right and 3 on the left
                count = 0;
                if (lines - position > 0 && position != 10 && index != 100)
                {
                    var buttonText1 = getButtonName(index);
                    var buttonText2 = getButtonName(index + 1);
                    if (buttonText1.Equals(buttonText2)) count++;
                    for (int i = 0; i < limit - 2; i++)
                    {
                        buttonText1 = getButtonName(index - i);
                        buttonText2 = getButtonName(index - i - 1);
                        if (buttonText1 == "empty" || buttonText2 == "empty") break;
                        if (buttonText1.Equals(buttonText2)) count++;
                    }

                }
            }
            if (count != 4)
            {
                //this checks if there is 2 on the right and 2 on th left
                count = 0;
                if (lines - position > 1 && position != 10 & index != 100)
                {
                    for (int i = 0; i < limit - 3; i++)
                    {
                        var buttonText1 = getButtonName(index + i);
                        var buttonText2 = getButtonName(index + i + 1);
                        if (buttonText1 == "empty" || buttonText2 == "empty") break;
                        if (buttonText1.Equals(buttonText2)) count++;
                    }
                    for (int i = 0; i < limit - 3; i++)
                    {
                        var buttonText1 = getButtonName(index - i);
                        var buttonText2 = getButtonName(index - i - 1);
                        if (buttonText1 == "empty" || buttonText2 == "empty") break;
                        if (buttonText1.Equals(buttonText2)) count++;
                    }

                }
            }

            //Debug.Log($"count is {count}");

            if (count == limit - 1)
            {
                Debug.Log("You've Won!");
                //SetBoardInteractable(true);
                return;
            }


        }

        count = 0;

        if (lines - position >= limit - 1 && index != 100) //If true checks the grid from left to right
        {
            for (int i = 0; i < limit - 1; i++)
            {
                string buttonText1 = getButtonName(index + i);
                string buttonText2 = getButtonName(index + i + 1);
                if (buttonText1 == "empty" || buttonText2 == "empty")
                {
                    break;
                }

                if (string.Equals(buttonText1, buttonText2))
                {
                    count++;
                }
                else
                {
                    break;
                }
            }

            if (count != 4)
            {
                //this checks if theres one on the left and 3 on the right
                count = 0;
                if (position > 1)
                {
                    var buttonText1 = getButtonName(index);
                    var buttonText2 = getButtonName(index - 1);
                    if (buttonText1.Equals(buttonText2)) count++;
                    for (int i = 0; i < limit - 2; i++)
                    {
                        buttonText1 = getButtonName(index + i);
                        buttonText2 = getButtonName(index + i + 1);
                        if (buttonText1 == "empty" || buttonText2 == "empty") break;
                        if (buttonText1.Equals(buttonText2)) count++;
                    }
                }
            }

            if (count != 4)
            {
                //this checks if there is 2 on the left and 2 on the right
                count = 0;
                if (position > 2)
                {
                    for (int i = 0; i < limit - 3; i++)
                    {
                        var buttonText1 = getButtonName(index + i);
                        var buttonText2 = getButtonName(index + i + 1);
                        if (buttonText1 == "empty" || buttonText2 == "empty") break;
                        if (buttonText1.Equals(buttonText2)) count++;
                    }
                    for (int i = 0; i < limit - 3; i++)
                    {
                        var buttonText1 = getButtonName(index - i);
                        var buttonText2 = getButtonName(index - i - 1);
                        if (buttonText1 == "empty" || buttonText2 == "empty") break;
                        if (buttonText1.Equals(buttonText2)) count++;
                    }
                }
            }

            if (count == limit - 1)
            {
                Debug.Log("You've Won!");
                //SetBoardInteractable(true);
                return;
            }
        }

        count = 0;

        if (row + 1 >= limit) //this checks down up
        {
            for (int i = 0; i < limit - 1; i++)
            {
                string buttonText1 = getButtonName(index - (i * lines));
                string buttonText2 = getButtonName(index - ((i + 1) * lines));
                if (buttonText1 == "empty" || buttonText2 == "empty")
                {
                    break;
                }
                if (string.Equals(buttonText1, buttonText2))
                {
                    count++;
                }
                else
                {
                    break;
                }
            }
        }
        if (count != 4)
        {
            count = 0;
            if (row >= 3 && row <= lines - 2) // this checks if there is one below and 3 above
            {
                string buttonText1 = getButtonName(index);
                string buttonText2 = getButtonName(index + lines);
                if (buttonText1.Equals(buttonText2)) count++;
                for (int i = 0; i < limit - 2; i++)
                {
                    buttonText1 = getButtonName(index - (i * lines));
                    buttonText2 = getButtonName(index - ((i + 1) * lines));
                    if (buttonText1 == "empty" || buttonText2 == "empty") break;
                    if (buttonText1.Equals(buttonText2)) count++;
                }
            }
        }
        if (count != 4)
        {
            count = 0;
            Debug.Log("asd");
            if (row >= 2 && row <= lines - 3) // this checks if there is one below and 3 above
            {
                for (int i = 0; i < limit - 3; i++)
                {
                    var buttonText1 = getButtonName(index - (i * lines));
                    var buttonText2 = getButtonName(index - ((i + 1) * lines));
                    if (buttonText1 == "empty" || buttonText2 == "empty") break;
                    if (buttonText1.Equals(buttonText2)) count++;
                }
                for (int i = 0; i < limit - 3; i++)
                {
                    var buttonText1 = getButtonName(index + (i * lines));
                    var buttonText2 = getButtonName(index + ((i + 1) * lines));
                    if (buttonText1 == "empty" || buttonText2 == "empty") break;
                    if (buttonText1.Equals(buttonText2)) count++;
                }
            }
        }

        if ((position >= limit || position == 0) && row >= limit - 1)
        {
            for (int i = 0; i < limit - 1; i++)
            {
                string buttonText1 = getButtonName(index - (i * (lines + 1)));
                string buttonText2 = getButtonName(index - ((i + 1) * (lines + 1)));
                if (buttonText1 == "empty" || buttonText2 == "empty") break;
                if (string.Equals(buttonText1, buttonText2)) count++;
                else break;
            }
        }
        /*
        if (count != 4)
        {
            count = 0;
            if (position >= limit - 1 && row >= limit - 2)
            {
                string buttonText1 = getButtonName(index);
                string buttonText2 = getButtonName(index - (lines + 1));
                if (buttonText1.Equals(buttonText2)) count++;
                for (int i = 0; i < limit - 2; i++)
                {
                    buttonText1 = getButtonName(index - (i * (lines + 1)));
                    buttonText2 = getButtonName(index - ((i + 1) * (lines + 1)));
                    if (buttonText1 == "empty" || buttonText2 == "empty") break;
                    if (string.Equals(buttonText1, buttonText2)) count++;
                    else break;
                }
            }
        }
        */
        if (count != 4)
        {
            count = 0;
            if (position >= limit - 2 && row >= limit - 3 && row <= lines - 3)
            {
                for (int i = 0; i < limit - 3; i++)
                {
                    var buttonText1 = getButtonName(index - (i * (lines + 1)));
                    var buttonText2 = getButtonName(index - ((i + 1) * (lines + 1)));
                    if (buttonText1 == "empty" || buttonText2 == "empty") break;
                    if (string.Equals(buttonText1, buttonText2)) count++;
                    else break;
                }
                for (int i = 0; i < limit - 3; i++)
                {
                    var buttonText1 = getButtonName(index + (i * (lines + 1)));
                    var buttonText2 = getButtonName(index + ((i + 1) * (lines + 1)));
                    if (buttonText1 == "empty" || buttonText2 == "empty") break;
                    if (string.Equals(buttonText1, buttonText2)) count++;
                    else break;
                }
            }
        }
        



        Debug.Log(count);
        if (count == 4) Debug.Log("Yeet");


    }


    void SetBoardInteractable(bool toggle)
    {
        for (int i = 0; i < buttonList.Length; i++)
        {
            buttonList[i].GetComponentInParent<Button>().interactable = toggle;
        }
    }

    string getButtonName(int index)
    {
        string buttonText = GameObject.Find(index.ToString()).GetComponent<Button>().GetComponentInChildren<Text>().text;
        buttonText = buttonText == "" ? "empty" : buttonText;
        return buttonText;
    }


}
