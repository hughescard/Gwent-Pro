using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scr_Card : ScriptableObject
{
    //card properties
    public Sprite image;
    public string name;
    public int real_power;
    public int current_power;
    public string playable_zone;
    public string effect;
    public string card_type;
    public bool player;
    public string unit_type; 

    //card constructor
    public Scr_Card(string name, string card_type, bool player, Sprite image, int power, string playable_zone, string effect, string unit_type)
    {
        this.name = name;
        this.image = image;
        this.real_power = power;
        this.current_power = power;
        this.playable_zone = playable_zone;
        this.effect = effect;
        this.card_type = card_type;
        this.player = player;
        this.unit_type = unit_type;
    }




  
}
