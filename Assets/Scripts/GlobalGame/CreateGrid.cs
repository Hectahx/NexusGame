using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateGrid : MonoBehaviour
{
    public GameObject HoriLine;
    public GameObject VertLine;
    public GameObject buttons;
    int lines = WsClient.size;
    int height = Screen.height;
    int width = Screen.width;
    float heightRatio = Screen.height / 1080;
    float widthRatio = Screen.width / 1920;


    public void CreateLines() //This instantiates all the lines. This is done to later give the user the option as to how big they want the board to be. 
    {
        int count = 0;//a count of how many lines were made
        for (float x = (704); x <= (512 + 704); x += (512 / lines)) //This covers width
        {
            count++;
            if (count == 1 || count == lines + 1) { } //This if statement is so a the first and last line isnt made on the board as they are already outlines
            else
            {
                GameObject line = Instantiate(VertLine, new Vector3((x * Screen.width / 1920) + (2 * Screen.width / 1920), 540 * Screen.height / 1080, 0), 
                    Quaternion.identity, GameObject.FindGameObjectWithTag("ButtonHolder").transform);
                //Adjusted for different screen resolutions
            }
        }
        count = 0;
        for (float y = (540 - 256); y <= 256 + 540; y += 512 / lines) //This covers height
        {
            count++;
            if (count == 1 || count == lines + 1) { }
            else
            {
                GameObject line = Instantiate(HoriLine, new Vector3((704 * Screen.width / 1920) + (256 * Screen.width / 1920), (y * Screen.width / 1920) + (2 * Screen.width / 1920), 0), 
                    Quaternion.identity, GameObject.FindGameObjectWithTag("ButtonHolder").transform);
            }
        }

    }

    public void CreateButtons()//This instantiates all the buttons. This uses size of the board to calculate how many buttons there should be
    {
        int buttonLength = 30 * Screen.width / 1920;
        int width = 512 * Screen.width / 1920;
        float length = (float)width / (float)lines;
        float ratio = buttonLength / length; //L + YB Better + Ratio
        int count = 0;

        for (int i = 0; i < lines; i++)
        {
            for (int j = 0; j < lines; j++) //nested for loop to do both horizontal and vertical buttonws 
            {
                GameObject button = Instantiate(buttons, new Vector3(704 * Screen.width / 1920 + length / 2 + 1 + (length * j) , 
                (540  * Screen.height / 1080 - length / 2 + 256 * Screen.height / 1080 - (length * i) ), 0), Quaternion.identity, GameObject.FindGameObjectWithTag("ButtonHolder").transform);
                
                button.transform.localScale = Vector3.one / (ratio * 1.1f);
                button.gameObject.name = count.ToString();
                count++;
            }
        }
    }

    public int getSize()
    {
        return lines;
    }
}