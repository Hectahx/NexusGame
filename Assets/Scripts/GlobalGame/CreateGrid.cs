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


    public void CreateLines()
    {
        int count = 0;
        for (float x = (704); x <= (512 + 704); x += (512 / lines)) //This covers width
        {
            count++;
            if (count == 1 || count == lines + 1) { }
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

    public void CreateButtons()// Done some maths to calculate  
    {
        int buttonLength = 30 * Screen.width / 1920;
        int width = 512 * Screen.width / 1920;
        float length = (float)width / (float)lines;
        float ratio = buttonLength / length; //L + YB Better + Ratio
        int count = 0;

        for (int i = 0; i < lines; i++)
        {
            for (int j = 0; j < lines; j++)
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


/*
        int count = 0;
        for (float x = (704 * widthRatio) ; x <= (512 * widthRatio + 704 * widthRatio) ; x += (512 / lines) * widthRatio ) //This covers width
        {
            count++;
            if (count == 1 || count == lines + 1) { }
            else
            {
                GameObject line = Instantiate(VertLine, new Vector3(x + (2 * widthRatio), 540 * heightRatio, 0), Quaternion.identity, GameObject.FindGameObjectWithTag("ButtonHolder").transform);
            }
        }
        count = 0;
        for (float y = (540 * heightRatio - 256 * heightRatio); y <= 256 * heightRatio + 540 * heightRatio; y += 512 * heightRatio / lines) //This covers height
        {
            count++;
            if (count == 1 || count == lines + 1) { }
            else
            {
                GameObject line = Instantiate(HoriLine, new Vector3(704 * heightRatio + 256 * heightRatio, y + (2 * heightRatio), 0), Quaternion.identity, GameObject.FindGameObjectWithTag("ButtonHolder").transform);
            }
        }
*/