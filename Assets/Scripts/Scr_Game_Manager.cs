using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Scr_Game_Manager : MonoBehaviour
{
    public Scr_Player Player1;
    public Scr_Player Player2;
    
    void Start()
    {
        Player1 = new Scr_Player("Guille", "CM", true);
        Player2 = new Scr_Player("Jean", "CM", false);

        if(Player1.faction == "CM")
            GameObject.Find("Deck_Zone").GetComponent<Image>().sprite = Resources.Load<Sprite>("Spr_Dorso_CM");
        else if(Player1.faction != "CM")
            GameObject.Find("Deck_Zone").GetComponent<Image>().sprite = Resources.Load<Sprite>("Spr_Dorso_C_and_R");

        if (Player2.faction == "CM")
            GameObject.Find("Deck_Zone_Enemy").GetComponent<Image>().sprite = Resources.Load<Sprite>("Spr_Dorso_CM");
        else if(Player2.faction == "C_and_R")
            GameObject.Find("Deck_Zone_Enemy").GetComponent<Image>().sprite = Resources.Load<Sprite>("Spr_Dorso_C_and_R");

        Scr_Deck Deck = GameObject.Find("Deck_Zone").GetComponent<Scr_Deck>();
        Deck.Deck = Scr_Card_DataBase.Create_Deck(Player1.faction);
        Deck.Deck_Order_Randomizer();

        Deck = GameObject.Find("Deck_Zone_Enemy").GetComponent<Scr_Deck>();
        Deck.Deck = Scr_Card_DataBase.Create_Deck(Player2.faction);
        Deck.Deck_Order_Randomizer();
    }

}
