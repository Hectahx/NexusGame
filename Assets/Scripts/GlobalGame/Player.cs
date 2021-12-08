using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Player
{
    public string side;
    public bool isReversed { get; set; }

    public bool isEnabled { get; set; }

    public Player(string side)
    {
        this.side = side;
    }

}