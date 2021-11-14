using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateGrid : MonoBehaviour
{
    public GameObject HoriLine;
    public GameObject VertLine;
    public GameObject buttons;
    int lines = 10;

    public void CreateLines()
    {
        int count = 0;
        for (float x = 704; x <= 512 + 704; x += 512 / lines)
        {
            count++;
            if (count == 1 || count == lines + 1) { }
            else
            {
                GameObject line = Instantiate(VertLine, new Vector3(x + 2, 540, 0), Quaternion.identity, GameObject.FindGameObjectWithTag("Canvas").transform);
            }
        }
        count = 0;
        for (float y = 540 - 256; y <= 256 + 540; y += 512 / lines)
        {
            count++;
            if (count == 1 || count == lines + 1) { }
            else
            {
                GameObject line = Instantiate(HoriLine, new Vector3(704 + 256, y + 2, 0), Quaternion.identity, GameObject.FindGameObjectWithTag("Canvas").transform);
            }
        }

    }

    public void CreateButtons()
    {
        int buttonLength = 30;
        int width = 512;
        float length = (float)width / (float)lines;
        float ratio = buttonLength / length; //L + YB Better + Ratio
        int count = 1;

        for (int i = 0; i < lines; i++)
        {
            for (int j = 0; j < lines; j++)
            {
                GameObject button = Instantiate(buttons, new Vector3(704 + length / 2 + 1 + (length * j), 540 - length / 2 + 256 - (length * i), 0), Quaternion.identity, GameObject.FindGameObjectWithTag("Canvas").transform);
                button.transform.localScale = Vector3.one / (ratio * 1.1f);
                button.gameObject.name = count.ToString();
                count++;
            }
        }
    }

    public int getSize(){
        return lines;
    }
}
