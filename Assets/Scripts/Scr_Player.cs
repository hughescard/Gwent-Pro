using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scr_Player
{
    public string name;
    public string faction;
    public bool Board_Side = false;//si tiene valor true corresponde la parte de abajo del board y si esta en false la de arriba
    public bool passed = false;    
    public int Lives = 2;

    public Scr_Player(string name, string faction, bool board_Side)
    {
        this.name = name;
        this.faction = faction;
        this.Board_Side = board_Side;
    }

}
