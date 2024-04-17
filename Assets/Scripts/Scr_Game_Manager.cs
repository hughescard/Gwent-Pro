using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Scr_Game_Manager : MonoBehaviour
{
    public Scr_Player Player1;
    public Scr_Player Player2;
    public bool turn;
    public List<GameObject> Principal_Objects;
    public int round = 0;
    public int Continuous_Passes = 0;
    Scr_Deck Deck1;
    Scr_Deck Deck2;
    int total_power_p1;
    int total_power_p2;
    public TextMeshProUGUI total_power_p1_t;
    public TextMeshProUGUI total_power_p2_t;
    bool Change_round;


    void Start()
    {
        turn = true;
        Player1 = new Scr_Player("Guille", "CM", true);
        Player2 = new Scr_Player("Jean", "C_and_R", false);

        if(Player1.faction == "CM")
            GameObject.Find("Deck_Zone").GetComponent<Image>().sprite = Resources.Load<Sprite>("Spr_Dorso_CM");
        else if(Player1.faction != "CM")
            GameObject.Find("Deck_Zone").GetComponent<Image>().sprite = Resources.Load<Sprite>("Spr_Dorso_C_and_R");

        if (Player2.faction == "CM")
            GameObject.Find("Deck_Zone_Enemy").GetComponent<Image>().sprite = Resources.Load<Sprite>("Spr_Dorso_CM");
        else if(Player2.faction == "C_and_R")
            GameObject.Find("Deck_Zone_Enemy").GetComponent<Image>().sprite = Resources.Load<Sprite>("Spr_Dorso_C_and_R");

        Deck1 = GameObject.Find("Deck_Zone").GetComponent<Scr_Deck>();
        Deck1.Deck = Scr_Card_DataBase.Create_Deck(Player1.Board_Side, Player1.faction);
        Deck1.Deck_Order_Randomizer();

        Deck2 = GameObject.Find("Deck_Zone_Enemy").GetComponent<Scr_Deck>();
        Deck2.Deck = Scr_Card_DataBase.Create_Deck(Player2.Board_Side, Player2.faction);
        Deck2.Deck_Order_Randomizer();

        Power_Update();
      
    }

    public void Change_Turn()
    {
        turn = !turn;
        foreach (GameObject obj in Principal_Objects)
        {
            if (obj.name == "Grave_Image_CM" || obj.name == "Grave_Image_CR" || obj.name == "Grave_Zone" || obj.name == "Grave_Zone_Enemy") continue;

            Rotate_Object(obj);
        }
    }

    public void Rotate_Object(GameObject obj)
    { 
        obj.transform.Rotate(0, 0, 180);
    }

    public void Passed()
    {
        if(Player1.Board_Side == this.turn)
        {
            Player1.passed = true;
        }
        else
        {
            Player2.passed = true;
        }

        this.Continuous_Passes++;

        Change_Round();
        Change_Turn();
    }

    public void Change_Round()
    {
        if(this.Continuous_Passes==2)
        {
            //primero debe verificarse si con esta ronda se acaba el juego 
            if(total_power_p1>total_power_p2)//gana jugador 1
            {
                Player2.Lives--;
                if(Player2.Lives==0)
                {
                    Debug.Log("se acabo el juego");
                }
            }

            else if (total_power_p2 > total_power_p1)//gana jugador 2
            {
                Player1.Lives--;
                if (Player1.Lives == 0)
                {
                    Debug.Log("se acabo el juego");
                }
            }

            else
            {
                Player1.Lives--;
                Player2.Lives--;
                if (Player1.Lives == 0 || Player2.Lives == 0)
                {
                    Debug.Log("se acabo el juego");
                }
            }

            round++;
            foreach (GameObject obj in FindObjectsOfType<GameObject>())//destruir todas las cartas jugadas 
            {
                if (obj.name.Contains("CardPrefab") && obj.transform.parent.gameObject != null
                    && obj.transform.parent.gameObject != GameObject.Find("Hand_Zone")
                    && obj.transform.parent.gameObject != GameObject.Find("Hand_Zone_Enemy"))
                {

                    if (obj.GetComponent<Display_Card>().Card.player)
                    { 
                        Deck1.Grave.Insert(0,obj.GetComponent<Display_Card>().Card);

                        if (Player1.faction == "CM" && Principal_Objects[4].transform.childCount == 0)
                            Instantiate(Principal_Objects[6], Principal_Objects[4].transform);
                        
                        else if (Player1.faction != "CM" && Principal_Objects[4].transform.childCount == 0)
                            Instantiate(Principal_Objects[7], Principal_Objects[4].transform); }

                    else
                    { 
                        Deck2.Grave.Insert(0, obj.GetComponent<Display_Card>().Card);

                        if (Player2.faction == "CM" && Principal_Objects[5].transform.childCount==0)
                            Instantiate(Principal_Objects[6], Principal_Objects[5].transform);
                        
                        else if (Player2.faction != "CM" && Principal_Objects[5].transform.childCount == 0)
                            Instantiate(Principal_Objects[7], Principal_Objects[5].transform);
                    }

                    Destroy(obj);
                }
            }
            Change_round = true;
            Power_Update();

            Continuous_Passes = 0;

            //annadiendo las dos q tocan por ronda
            Deck1.Instantiate_Card(2);
            Deck2.Instantiate_Card(2);

        }
    }

    public void Power_Update()
    {
        if(Change_round)
        {
            total_power_p1 = 0;
            total_power_p2 = 0;
            Change_round = false;
            total_power_p1_t.text = total_power_p1.ToString();
            total_power_p2_t.text = total_power_p2.ToString();
            return;
        }

        //aqui se ejecuta la actualizacion del poder acumulado por el jugador 1 
        #region //Jugador 1
        foreach (Transform obj in GameObject.Find("Melee_Zone").transform)
            total_power_p1 += obj.GetComponent<Display_Card>().Card.current_power;
        foreach (Transform obj in GameObject.Find("Distance_Zone").transform)
            total_power_p1 += obj.GetComponent<Display_Card>().Card.current_power;
        foreach (Transform obj in GameObject.Find("Siege_Zone").transform)
            total_power_p1 += obj.GetComponent<Display_Card>().Card.current_power;

        #endregion

        //aqui se ejecuta la actualizacion de poder acumulado por el jugador 2
        #region//Jugador 2
      foreach (Transform obj in GameObject.Find("Melee_Zone_Enemy").transform)
            total_power_p2 += obj.GetComponent<Display_Card>().Card.current_power;
        foreach (Transform obj in GameObject.Find("Distance_Zone_Enemy").transform)
            total_power_p2 += obj.GetComponent<Display_Card>().Card.current_power;
        foreach (Transform obj in GameObject.Find("Siege_Zone_Enemy").transform)
            total_power_p2 += obj.GetComponent<Display_Card>().Card.current_power;

        #endregion
    
        //aqui se actualiza el texto
        total_power_p1_t.text = total_power_p1.ToString();
        total_power_p2_t.text = total_power_p2.ToString();


    }

}
