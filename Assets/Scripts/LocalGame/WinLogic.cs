using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WinLogic
{
    int limit = 5;
    int lines;
    CreateGrid gridCreator;
    public WinLogic(int lines)
    {
        this.lines = lines;
    }
    public int[] getValues(Button button)
    {

        int index = int.Parse(button.name);
        int diagRight = lines + 1;
        int diagLeft = lines - 1;
        int tempIndex = index;

        int position = index % lines;
        int row = index / lines;

        int[] values = new int[] { index, lines, diagRight, diagLeft, position, row };

        return values;
    }
    public int checkRightToLeft(Button button)
    {
        var count = 0;
        int[] values = getValues(button);
        var index = values[0];
        //var lines = values[1];
        var diagRight = values[2];
        var diagLeft = values[3];
        var position = values[4];
        var row = values[5];


        if (position >= limit || position == 0) //If true checks the grid from right to left
        {
            //This checks standard right to left
            for (int i = 0; i < limit - 1; i++)
            {
                string buttonText1 = getButtonText(index - i);
                string buttonText2 = getButtonText(index - i - 1);
                if (buttonText1 == "empty" || buttonText2 == "empty") break;
                if (string.Equals(buttonText1, buttonText2)) count++;
                else break;
            }

            if (count == 4)
            {
                return count;
            }
        }

        //this checks if there is one on the right and 3 on the left
        count = 0;
        if ((lines - position > 0 && position != 10 && index != 100) && index != 1)
        {
            var buttonText1 = getButtonText(index);
            var buttonText2 = getButtonText(index + 1);
            if (buttonText1.Equals(buttonText2)) count++;
            for (int i = 0; i < limit - 2; i++)
            {
                buttonText1 = getButtonText(index - i);
                buttonText2 = getButtonText(index - i - 1);
                if (buttonText1 == "empty" || buttonText2 == "empty")
                    break;
                if (buttonText1.Equals(buttonText2)) count++;
            }
        }


        if (count == 4)
        {
            return count;
        }

        count = 0;

        if ((lines - position > 1 && position != 10 & index != 100) && index != 1)
        {
            for (int i = 0; i < limit - 3; i++)
            {
                var buttonText1 = getButtonText(index + i);
                var buttonText2 = getButtonText(index + i + 1);
                if (buttonText1 == "empty" || buttonText2 == "empty")
                    break;
                if (buttonText1.Equals(buttonText2)) count++;
            }
            for (int i = 0; i < limit - 3; i++)
            {
                var buttonText1 = getButtonText(index - i);
                var buttonText2 = getButtonText(index - i - 1);
                if (buttonText1 == "empty" || buttonText2 == "empty")
                    break;
                if (buttonText1.Equals(buttonText2)) count++;
            }
        }
        return count;
    }
    
    public int checkLeftToRight(Button button)
    {
        var count = 0;
        int[] values = getValues(button);
        var index = values[0];
        var lines = values[1];
        var diagRight = values[2];
        var diagLeft = values[3];
        var position = values[4];
        var row = values[5];

        if (lines - position >= limit - 1 && index != lines * lines) //If true checks the grid from left to right
        {
            for (int i = 0; i < limit - 1; i++)
            {
                string buttonText1 = getButtonText(index + i);
                string buttonText2 = getButtonText(index + i + 1);
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

        if (count == 4)
        {
            return count;
        }

        //this checks if theres one on the left and 3 on the right
        count = 0;
        if (position > 1)
        {
            var buttonText1 = getButtonText(index);
            var buttonText2 = getButtonText(index - 1);
            if (buttonText1.Equals(buttonText2)) count++;
            for (int i = 0; i < limit - 2; i++)
            {
                buttonText1 = getButtonText(index + i);
                buttonText2 = getButtonText(index + i + 1);
                if (buttonText1 == "empty" || buttonText2 == "empty")
                    break;
                if (buttonText1.Equals(buttonText2)) count++;
            }
        }

        if (count == 4)
        {
            return count;
        }

        //this checks if there is 2 on the left and 2 on the right
        count = 0;
        if (position > 2)
        {
            for (int i = 0; i < limit - 3; i++)
            {
                var buttonText1 = getButtonText(index + i);
                var buttonText2 = getButtonText(index + i + 1);
                if (buttonText1 == "empty" || buttonText2 == "empty")
                    break;
                if (buttonText1.Equals(buttonText2)) count++;
            }
            for (int i = 0; i < limit - 3; i++)
            {
                var buttonText1 = getButtonText(index - i);
                var buttonText2 = getButtonText(index - i - 1);
                if (buttonText1 == "empty" || buttonText2 == "empty")
                    break;
                if (buttonText1.Equals(buttonText2)) count++;
            }
        }

        return count;

    }
    
    public int checkDownUp(Button button)
    {
        var count = 0;
        int[] values = getValues(button);
        var index = values[0];
        var lines = values[1];
        var diagRight = values[2];
        var diagLeft = values[3];
        var position = values[4];
        var row = values[5];

        if (row + 1 >= limit && index != lines * 4) //this checks down up
        {
            for (int i = 0; i < limit - 1; i++)
            {
                string buttonText1 = getButtonText(index - (i * lines));
                string buttonText2 = getButtonText(index - ((i + 1) * lines));
                if (buttonText1 == "empty" || buttonText2 == "empty") break;
                if (string.Equals(buttonText1, buttonText2)) count++;
                else break;
            }
        }
        if (count == 4) return count;

        count = 0;
        if (row >= 3 && row <= lines - 2 && index != lines * 3) // this checks if there is one below and 3 above
        {
            string buttonText1 = getButtonText(index);
            string buttonText2 = getButtonText(index + lines);
            if (buttonText1.Equals(buttonText2)) count++;
            for (int i = 0; i < limit - 2; i++)
            {
                buttonText1 = getButtonText(index - (i * lines));
                buttonText2 = getButtonText(index - ((i + 1) * lines));
                if (buttonText1 == "empty" || buttonText2 == "empty") break;
                if (buttonText1.Equals(buttonText2)) count++;
            }
        }


        if (count == 4) return count;
        count = 0;
        if (row >= 2 && row <= lines - 3 && index != lines * 2)// this checks if there is 2 below and 2 above
        {
            for (int i = 0; i < limit - 3; i++)
            {
                var buttonText1 = getButtonText(index - (i * lines));
                var buttonText2 = getButtonText(index - ((i + 1) * lines));
                if (buttonText1 == "empty" || buttonText2 == "empty") break;
                if (buttonText1.Equals(buttonText2)) count++;
            }
            for (int i = 0; i < limit - 3; i++)
            {
                var buttonText1 = getButtonText(index + (i * lines));
                var buttonText2 = getButtonText(index + ((i + 1) * lines));
                if (buttonText1 == "empty" || buttonText2 == "empty") break;
                if (buttonText1.Equals(buttonText2)) count++;
            }
        }

        return count;

    }

    public int checkUpDown(Button button)
    {
        var count = 0;
        int[] values = getValues(button);
        var index = values[0];
        var lines = values[1];
        var diagRight = values[2];
        var diagLeft = values[3];
        var position = values[4];
        var row = values[5];

        if (row <= limit)
        {
            for (int i = 0; i < limit - 1; i++)
            {
                string buttonText1 = getButtonText(index + (i * lines));
                string buttonText2 = getButtonText(index + ((i + 1) * lines));
                if (buttonText1 == "empty" || buttonText2 == "empty") break;
                if (string.Equals(buttonText1, buttonText2)) count++;
                else break;
            }
        }

        if (count == 4)
        {
            return count;
        }

        count = 0;
        if (row <= limit + 1 && row >= 1 && index != lines)
        {
            string buttonText1 = getButtonText(index);
            string buttonText2 = getButtonText(index - lines);
            if (string.Equals(buttonText1, buttonText2)) count++;
            for (int i = 0; i < limit - 2; i++)
            {
                buttonText1 = getButtonText(index + (i * lines));
                buttonText2 = getButtonText(index + ((i + 1) * lines));
                if (buttonText1 == "empty" || buttonText2 == "empty") break;
                if (string.Equals(buttonText1, buttonText2)) count++;
                else break;
            }
        }
        if (count == 4)
        {
            return count;
        }

        return count;
    }
    
    public int checkDiagRightToLeftUp(Button button)
    {
        var count = 0;
        int[] values = getValues(button);
        var index = values[0];
        var lines = values[1];
        var diagRight = values[2];
        var diagLeft = values[3];
        var position = values[4];
        var row = values[5];

        if ((position >= limit || position == 0 /*this is if its on the edge*/) && row >= limit - 1) //this checks diagonally if theres 4 above
        {
            for (int i = 0; i < limit - 1; i++)
            {
                string buttonText1 = getButtonText(index - (i * (lines + 1)));
                string buttonText2 =
                    getButtonText(index - ((i + 1) * (lines + 1)));
                if (buttonText1 == "empty" || buttonText2 == "empty") break;
                if (string.Equals(buttonText1, buttonText2))
                    count++;
                else
                    break;
            }
        }

        if (count == 4) return count;

        count = 0;
        if (position >= limit - 1 && row >= limit - 2 && row <= lines - 2) //this checks diagonally if theres one below and 3 above
        {
            string buttonText1 = getButtonText(index);
            string buttonText2 = getButtonText(index + (lines + 1));
            if (buttonText1.Equals(buttonText2)) count++;
            for (int i = 0; i < limit - 2; i++)
            {
                buttonText1 = getButtonText(index - (i * (lines + 1)));
                buttonText2 =
                    getButtonText(index - ((i + 1) * (lines + 1)));
                if (buttonText1 == "empty" || buttonText2 == "empty") break;
                if (string.Equals(buttonText1, buttonText2))
                    count++;
                else
                    break;
            }
        }



        if (count == 4) return count;

        count = 0;
        if (position >= limit - 2 && row >= limit - 3 && row <= lines - 3)
        {
            for (int i = 0; i < limit - 3; i++)
            {
                var buttonText1 = getButtonText(index - (i * (lines + 1)));
                var buttonText2 =
                    getButtonText(index - ((i + 1) * (lines + 1)));
                if (buttonText1 == "empty" || buttonText2 == "empty") break;
                if (string.Equals(buttonText1, buttonText2))
                    count++;
                else
                    break;
            }
            for (int i = 0; i < limit - 3; i++)
            {
                var buttonText1 = getButtonText(index + (i * (lines + 1)));
                var buttonText2 =
                    getButtonText(index + ((i + 1) * (lines + 1)));
                if (buttonText1 == "empty" || buttonText2 == "empty") break;
                if (string.Equals(buttonText1, buttonText2))
                    count++;
                else
                    break;
            }
        }

        if (count == 4) return count;


        if (position <= limit + 1 && row <= limit)
        {
            count = 0;
            for (int i = 0; i < limit - 1; i++)
            {
                var buttonText1 = getButtonText(index + (i * (lines + 1)));
                var buttonText2 = getButtonText(index + ((i + 1) * (lines + 1)));
                if (buttonText1 == "empty" || buttonText2 == "empty") break;
                if (string.Equals(buttonText1, buttonText2))
                    count++;
                else
                    break;
            }
        }

        return count;
    }

    public int checkDiagLeftToRightDown(Button button)
    {
        var count = 0;
        int[] values = getValues(button);
        var index = values[0];
        //var lines = values[1];
        var diagRight = values[2];
        var diagLeft = values[3];
        var position = values[4];
        var row = values[5];

        if (position <= limit + 1 && position != 0 && row <= limit)
        {
            for (int i = 0; i < limit - 1; i++)
            {
                string buttonText1 = getButtonText(index + (i * (lines + 1)));
                string buttonText2 = getButtonText(index + ((i + 1) * (lines + 1)));
                if (buttonText1 == "empty" || buttonText2 == "empty") break;
                if (string.Equals(buttonText1, buttonText2))
                    count++;
                else
                    break;
            }
        }

        if (count == 4) return count;
        count = 0;

        if (position <= limit + 2 && position != 1 && row <= limit + 1 && row != 0 && index != lines)
        {
            string buttonText1 = getButtonText(index);
            string buttonText2 = getButtonText(index - (lines + 1));
            if (buttonText1.Equals(buttonText2)) count++;
            for (int i = 0; i < limit - 2; i++)
            {
                buttonText1 = getButtonText(index + (i * (lines + 1)));
                buttonText2 = getButtonText(index + ((i + 1) * (lines + 1)));
                if (buttonText1 == "empty" || buttonText2 == "empty") break;
                if (string.Equals(buttonText1, buttonText2))
                    count++;
                else
                    break;
            }
        }
        if (count == 4) return count;

        count = 0;

        //if(position)



        if (count == 4) return count;

        count = 0;



        return count;
    }

    public int checkDiagRightToLeftDown(Button button)
    {
        var count = 0;
        int[] values = getValues(button);
        var index = values[0];
        //var lines = values[1];
        var diagRight = values[2];
        var diagLeft = values[3];
        var position = values[4];
        var row = values[5];

        if (row < 5 && (position >= limit || position == 0))
        {
            for (int i = 0; i < limit - 1; i++)
            {
                string buttonText1 = getButtonText(index + (i * (lines - 1)));
                string buttonText2 = getButtonText(index + ((i + 1) * (lines - 1)));
                if (buttonText1 == "empty" || buttonText2 == "empty") break;
                if (string.Equals(buttonText1, buttonText2))
                    count++;
                else
                    break;
            }
        }

        if (count == 4) return count;
        count = 0;

        if (row != 0 && row < limit + 1 && position != 0 && position > 3)
        {
            var buttonText1 = getButtonText(index);
            var buttonText2 = getButtonText(index - (lines - 1));
            if (string.Equals(buttonText1, buttonText2)) count++;
            for (int i = 0; i < limit - 2; i++)
            {
                buttonText1 = getButtonText(index + (i * (lines - 1)));
                buttonText2 = getButtonText(index + ((i + 1) * (lines - 1)));
                if (buttonText1 == "empty" || buttonText2 == "empty") break;
                if (string.Equals(buttonText1, buttonText2))
                    count++;
                else
                    break;
            }
        }

        if (count == 4) return count;
        count = 0;

        if (position > 2 && position <= lines - 2 && row >= 2 && row < lines - 2 )
        {
            for (int i = 0; i < limit - 3; i++)
            {
                var buttonText1 = getButtonText(index + (i * (lines - 1)));
                var buttonText2 = getButtonText(index + ((i + 1) * (lines - 1)));
                if (buttonText1 == "empty" || buttonText2 == "empty") break;
                if (string.Equals(buttonText1, buttonText2)) count++;
                else break;
            }
            for (int i = 0; i < limit - 3; i++)
            {
                var buttonText1 = getButtonText(index - (i * (lines - 1)));
                var buttonText2 = getButtonText(index - ((i + 1) * (lines - 1)));
                if (buttonText1 == "empty" || buttonText2 == "empty") break;
                if (string.Equals(buttonText1, buttonText2))
                    count++;
                else
                    break;
            }
        }

        //Debug.Log(count);


        return count;
    }

    public int checkDiagLeftToRightUp(Button button)
    {
        var count = 0;
        int[] values = getValues(button);
        var index = values[0];
        //var lines = values[1];
        var diagRight = values[2];
        var diagLeft = values[3];
        var position = values[4];
        var row = values[5];

        if (position != 0 && position <= limit + 1 && row >= limit - 1)
        {
            for (int i = 0; i < limit - 1; i++)
            {
                string buttonText1 = getButtonText(index - (i * (lines - 1)));
                string buttonText2 = getButtonText(index - ((i + 1) * (lines - 1)));
                if (buttonText1 == "empty" || buttonText2 == "empty") break;
                if (string.Equals(buttonText1, buttonText2))
                    count++;
                else
                    break;
            }
        }

        if (count == 4) return count;
        count = 0;

        if (position > 1 && row < lines - 1 && row > 2)
        {
            string buttonText1 = getButtonText(index);
            string buttonText2 = getButtonText(index + (lines - 1));
            if (string.Equals(buttonText1, buttonText2)) count++;
            for (int i = 0; i < limit - 2; i++)
            {
                buttonText1 = getButtonText(index - (i * (lines - 1)));
                buttonText2 = getButtonText(index - ((i + 1) * (lines - 1)));
                if (buttonText1 == "empty" || buttonText2 == "empty") break;
                if (string.Equals(buttonText1, buttonText2))
                    count++;
                else
                    break;
            }

        }


        return count;
    }

    public string getButtonText(int index)
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

}