using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class Scr_Effects : MonoBehaviour
{
    public Dictionary<string,Action<Scr_Card>> effects;
    public List<GameObject> Main_Objects;
    private void Start()
    {
        //aqui se annaden los metodos de los efectos al diccionario
        effects = new Dictionary<string, Action<Scr_Card>>()
        {
            { "Weather" , Weather },
            { "Play_Card" , Play_Card },
            { "Clear" , Clear },
            { "Increase" , Increase },
        };    
    }


    public void Play_Card(Scr_Card card)
    {
        //verificar si es de oro ya que estas no se afectan por los climas ni por los aumentos
        if (card.unit_type == "G") return;

        //aqui se resta al poder de la carta los efectos de los climas activos que afectan su zona para el jugador 1
        if(card.playable_zone.IndexOf("D") !=-1 && card.player)
        {
            card.current_power = card.real_power - GameObject.Find("Distance_Zone").GetComponent<Scr_DropZone>().weather_effects + GameObject.Find("Distance_Zone").GetComponent<Scr_DropZone>().raise_effects;
        }
        else if (card.playable_zone.IndexOf("M") != -1 && card.player)
        {
            card.current_power = card.real_power - GameObject.Find("Melee_Zone").GetComponent<Scr_DropZone>().weather_effects + GameObject.Find("Melee_Zone").GetComponent<Scr_DropZone>().raise_effects;
        }
        else if (card.playable_zone.IndexOf("S") != -1 && card.player)
        {
            card.current_power = card.real_power - GameObject.Find("Siege_Zone").GetComponent<Scr_DropZone>().weather_effects + GameObject.Find("Siege_Zone").GetComponent<Scr_DropZone>().raise_effects;
        }

        //aqui se resta al poder de la carta los efectos de los climas activos que afectan su zona para el jugador 2
        else if (card.playable_zone.IndexOf("D") != -1 && !card.player)
        {
            card.current_power = card.real_power - GameObject.Find("Distance_Zone_Enemy").GetComponent<Scr_DropZone>().weather_effects + GameObject.Find("Distance_Zone_Enemy").GetComponent<Scr_DropZone>().raise_effects;
        }
        else if (card.playable_zone.IndexOf("M") != -1 && !card.player)
        {
            card.current_power = card.real_power - GameObject.Find("Melee_Zone_Enemy").GetComponent<Scr_DropZone>().weather_effects + GameObject.Find("Melee_Zone_Enemy").GetComponent<Scr_DropZone>().raise_effects;
        }
        else if (card.playable_zone.IndexOf("S") != -1 && !card.player)
        {
            card.current_power = card.real_power - GameObject.Find("Siege_Zone_Enemy").GetComponent<Scr_DropZone>().weather_effects + GameObject.Find("Siege_Zone_Enemy").GetComponent<Scr_DropZone>().raise_effects;
        }
    }
    public void Weather(Scr_Card card)
    {
        Scr_Card Affecting_card = card;
        Scr_Game_Manager Game_Manager = GameObject.Find("Game_Manager").GetComponent<Scr_Game_Manager>();
        Scr_DropZone Affected_DropZone1;
        Scr_DropZone Affected_DropZone2;

        if (Affecting_card.card_type == "WM")
        {
            Affected_DropZone1 = GameObject.Find("Melee_Zone").GetComponent<Scr_DropZone>();
            Affected_DropZone2 = GameObject.Find("Melee_Zone_Enemy").GetComponent<Scr_DropZone>();
            Affected_DropZone1.weather_effects += 1;
            Affected_DropZone2.weather_effects += 1;
        }
        else if (Affecting_card.card_type == "WD")
        {
            Affected_DropZone1 = GameObject.Find("Distance_Zone").GetComponent<Scr_DropZone>();
            Affected_DropZone2 = GameObject.Find("Distance_Zone_Enemy").GetComponent<Scr_DropZone>();
            Affected_DropZone1.weather_effects += 1;
            Affected_DropZone2.weather_effects += 1;
        }
        else if (Affecting_card.card_type == "WS")
        {
            Affected_DropZone1 = GameObject.Find("Siege_Zone").GetComponent<Scr_DropZone>();
            Affected_DropZone2 = GameObject.Find("Siege_Zone_Enemy").GetComponent<Scr_DropZone>();
            Affected_DropZone1.weather_effects += 1;
            Affected_DropZone2.weather_effects += 1;
        }

 

        if (Affecting_card.card_type == "WM") 
        { 
            foreach(Transform obj in GameObject.Find("Melee_Zone").transform)
            {
                Display_Card obj_display_card = obj.GetComponent<Display_Card>();
                if (obj_display_card.Card.unit_type == "G") continue;
                obj_display_card.Card.current_power -= 1;
            }
            foreach (Transform obj in GameObject.Find("Melee_Zone_Enemy").transform)
            {
                Display_Card obj_display_card = obj.GetComponent<Display_Card>();
                if (obj_display_card.Card.unit_type == "G") continue;
                obj_display_card.Card.current_power -= 1;
            }

        }
        else if (Affecting_card.card_type == "WD")
        {
            foreach (Transform obj in GameObject.Find("Distance_Zone").transform)
            {
                Display_Card obj_display_card = obj.GetComponent<Display_Card>();
                if (obj_display_card.Card.unit_type == "G") continue;
                obj_display_card.Card.current_power -= 1;
            }
            foreach (Transform obj in GameObject.Find("Distance_Zone_Enemy").transform)
            {
                Display_Card obj_display_card = obj.GetComponent<Display_Card>();
                if (obj_display_card.Card.unit_type == "G") continue;
                obj_display_card.Card.current_power -= 1;
            }

        }
        else if (Affecting_card.card_type == "WS")
        {
            foreach (Transform obj in GameObject.Find("Siege_Zone").transform)
            {
                Display_Card obj_display_card = obj.GetComponent<Display_Card>();
                if (obj_display_card.Card.unit_type == "G") continue;
                obj_display_card.Card.current_power -= 1;
            }
            foreach (Transform obj in GameObject.Find("Siege_Zone_Enemy").transform)
            {
                Display_Card obj_display_card = obj.GetComponent<Display_Card>();
                if (obj_display_card.Card.unit_type == "G") continue;
                obj_display_card.Card.current_power -= 1;
            }

        }

    }
    public void Clear(Scr_Card card)
    {
        GameObject weather_zone = GameObject.Find("Weather_Zone");


        foreach(Transform obj in weather_zone.transform)
        {
            Display_Card obj_display_card = obj.GetComponent<Display_Card>();

            if (obj_display_card.Card.card_type == "WM")
            {
                if (Main_Objects[1].GetComponent<Scr_DropZone>().weather_effects == 0) continue;
                    Main_Objects[1].GetComponent<Scr_DropZone>().weather_effects--;
                if (Main_Objects[4].GetComponent<Scr_DropZone>().weather_effects == 0) continue;
                    Main_Objects[4].GetComponent<Scr_DropZone>().weather_effects--;

            }
            else if (obj_display_card.Card.card_type == "WD")
            {
                if (Main_Objects[2].GetComponent<Scr_DropZone>().weather_effects == 0) continue;
                    Main_Objects[2].GetComponent<Scr_DropZone>().weather_effects--;
                if (Main_Objects[5].GetComponent<Scr_DropZone>().weather_effects == 0) continue;
                    Main_Objects[5].GetComponent<Scr_DropZone>().weather_effects--;

            }
            else if (obj_display_card.Card.card_type == "WS")
            {
                if (Main_Objects[3].GetComponent<Scr_DropZone>().weather_effects == 0) continue;
                    Main_Objects[3].GetComponent<Scr_DropZone>().weather_effects--;
                if (Main_Objects[6].GetComponent<Scr_DropZone>().weather_effects == 0) continue;
                    Main_Objects[6].GetComponent<Scr_DropZone>().weather_effects--;

            }

            if (obj_display_card.Card.player)
            {
                Main_Objects[0].GetComponent<Scr_Game_Manager>().Deck1.Grave.Add(obj_display_card.Card);
                Main_Objects[0].GetComponent<Scr_Game_Manager>().Deck1.Grave.Add(card);

            }

            else
            {
                Main_Objects[0].GetComponent<Scr_Game_Manager>().Deck2.Grave.Add(obj_display_card.Card);
                Main_Objects[0].GetComponent<Scr_Game_Manager>().Deck2.Grave.Add(card);

            }

            Destroy(obj.gameObject);

            for(int i=1;i<=6;i++)
            {
                if(Main_Objects[i].GetComponent<Scr_DropZone>()!=null)
                    Main_Objects[i].GetComponent<Scr_DropZone>().Power_Modifier();
            }

            //creando la zona del cementerio en caso de que los climass que esta eliminando sean la primera carta en morir 
            if (Main_Objects[0].GetComponent<Scr_Game_Manager>().Player1.faction == "CM" && Main_Objects[7].transform.childCount == 0 && Main_Objects[0].GetComponent<Scr_Game_Manager>().Deck1.GetComponent<Scr_Deck>().Grave.Count!=0)
                Instantiate(Main_Objects[9], Main_Objects[7].transform);

            else if (Main_Objects[0].GetComponent<Scr_Game_Manager>().Player1.faction != "CM" && Main_Objects[7].transform.childCount == 0 && Main_Objects[0].GetComponent<Scr_Game_Manager>().Deck1.GetComponent<Scr_Deck>().Grave.Count != 0)
                Instantiate(Main_Objects[10], Main_Objects[7].transform);

            if (Main_Objects[0].GetComponent<Scr_Game_Manager>().Player2.faction == "CM" && Main_Objects[8].transform.childCount == 0 && Main_Objects[0].GetComponent<Scr_Game_Manager>().Deck2.GetComponent<Scr_Deck>().Grave.Count != 0)
                Instantiate(Main_Objects[9], Main_Objects[8].transform);

            else if (Main_Objects[0].GetComponent<Scr_Game_Manager>().Player2.faction != "CM" && Main_Objects[8].transform.childCount == 0 && Main_Objects[0].GetComponent<Scr_Game_Manager>().Deck2.GetComponent<Scr_Deck>().Grave.Count != 0)
                Instantiate(Main_Objects[10], Main_Objects[8].transform);


            //creando la zona del cementerio en caso de que sea la primera carta en morir 
            if (Main_Objects[0].GetComponent<Scr_Game_Manager>().Player1.faction == "CM" && Main_Objects[7].transform.childCount == 0 && Main_Objects[0].GetComponent<Scr_Game_Manager>().Deck1.GetComponent<Scr_Deck>().Grave.Count == 0 && card.player)
                Instantiate(Main_Objects[9], Main_Objects[7].transform);

            else if (Main_Objects[0].GetComponent<Scr_Game_Manager>().Player1.faction != "CM" && Main_Objects[7].transform.childCount == 0 && Main_Objects[0].GetComponent<Scr_Game_Manager>().Deck1.GetComponent<Scr_Deck>().Grave.Count == 0 && card.player)
                Instantiate(Main_Objects[10], Main_Objects[7].transform);

            if (Main_Objects[0].GetComponent<Scr_Game_Manager>().Player2.faction == "CM" && Main_Objects[8].transform.childCount == 0 && Main_Objects[0].GetComponent<Scr_Game_Manager>().Deck2.GetComponent<Scr_Deck>().Grave.Count == 0 && !card.player)
                Instantiate(Main_Objects[9], Main_Objects[8].transform);

            else if (Main_Objects[0].GetComponent<Scr_Game_Manager>().Player2.faction != "CM" && Main_Objects[8].transform.childCount == 0 && Main_Objects[0].GetComponent<Scr_Game_Manager>().Deck2.GetComponent<Scr_Deck>().Grave.Count == 0 && !card.player)
                Instantiate(Main_Objects[10], Main_Objects[8].transform);
        }

        Main_Objects[0].GetComponent<Scr_Game_Manager>().Power_Update();

    }
    public void Increase(Scr_Card card)
    {
        Scr_Card Affecting_card = card;
        Scr_Game_Manager Game_Manager = GameObject.Find("Game_Manager").GetComponent<Scr_Game_Manager>();
        Scr_DropZone Affected_DropZone1;
        Scr_DropZone Affected_DropZone2;

        if (Affecting_card.card_type == "Rm")
        {
            if (Affecting_card.player)
            {
                Affected_DropZone1 = GameObject.Find("Melee_Zone").GetComponent<Scr_DropZone>();
                Affected_DropZone1.raise_effects += 1;
            }
            else
            {
                Affected_DropZone2 = GameObject.Find("Melee_Zone_Enemy").GetComponent<Scr_DropZone>();
                Affected_DropZone2.raise_effects += 1;
            }
        }
        else if (Affecting_card.card_type == "Rd")
        {
            if (Affecting_card.player)
            {
                Affected_DropZone1 = GameObject.Find("Distance_Zone").GetComponent<Scr_DropZone>();
                Affected_DropZone1.raise_effects += 1;
            }
            else
            {
                Affected_DropZone2 = GameObject.Find("Distance_Zone_Enemy").GetComponent<Scr_DropZone>();
                Affected_DropZone2.raise_effects += 1;
            } 
        }
        else if (Affecting_card.card_type == "Rs")
        {
            if (Affecting_card.player)
            {
                Affected_DropZone1 = GameObject.Find("Siege_Zone").GetComponent<Scr_DropZone>();
                Affected_DropZone1.raise_effects += 1;
            }
            else
            {
                Affected_DropZone2 = GameObject.Find("Siege_Zone_Enemy").GetComponent<Scr_DropZone>();
                Affected_DropZone2.raise_effects += 1;
            } 
        }



        if (Affecting_card.card_type == "Rm")
        {
            if(Affecting_card.player)
            {  
                foreach (Transform obj in GameObject.Find("Melee_Zone").transform)
                {
                    Display_Card obj_display_card = obj.GetComponent<Display_Card>();
                    if (obj_display_card.Card.unit_type == "G") continue;
                    obj_display_card.Card.current_power += 1;
                }
            }
            else 
            {
                foreach (Transform obj in GameObject.Find("Melee_Zone_Enemy").transform)
                {
                    Display_Card obj_display_card = obj.GetComponent<Display_Card>();
                    if (obj_display_card.Card.unit_type == "G") continue;
                    obj_display_card.Card.current_power += 1;
                }
            }

        }
        else if (Affecting_card.card_type == "Rd")
        {
            if(Affecting_card.player)
            { 
                foreach (Transform obj in GameObject.Find("Distance_Zone").transform)
                {
                    Display_Card obj_display_card = obj.GetComponent<Display_Card>();
                    if (obj_display_card.Card.unit_type == "G") continue;
                    obj_display_card.Card.current_power += 1;
                }
            }
            else 
            {
                foreach (Transform obj in GameObject.Find("Distance_Zone_Enemy").transform)
                {
                    Display_Card obj_display_card = obj.GetComponent<Display_Card>();
                    if (obj_display_card.Card.unit_type == "G") continue;
                    obj_display_card.Card.current_power += 1;
                }
            }

        }
        else if (Affecting_card.card_type == "Rs")
        {
            if(Affecting_card.player)
            { 
                foreach (Transform obj in GameObject.Find("Siege_Zone").transform)
                {
                    Display_Card obj_display_card = obj.GetComponent<Display_Card>();
                    if (obj_display_card.Card.unit_type == "G") continue;
                    obj_display_card.Card.current_power += 1;
                }
            }
            else 
            {
                foreach (Transform obj in GameObject.Find("Siege_Zone_Enemy").transform)
                {
                    Display_Card obj_display_card = obj.GetComponent<Display_Card>();
                    if (obj_display_card.Card.unit_type == "G") continue;
                    obj_display_card.Card.current_power += 1;
                }
            }

        }
    }
    public void Decoy(Scr_Card card , Display_Card card_display)
    {
        if(card.playable_zone == "W")
        {
            if (card_display.Card.card_type == "WM")
            {
                if (Main_Objects[1].GetComponent<Scr_DropZone>().weather_effects != 0)
                Main_Objects[1].GetComponent<Scr_DropZone>().weather_effects--;
                if (Main_Objects[4].GetComponent<Scr_DropZone>().weather_effects != 0)
                Main_Objects[4].GetComponent<Scr_DropZone>().weather_effects--;

            }
            else if (card_display.Card.card_type == "WD")
            {
                if (Main_Objects[2].GetComponent<Scr_DropZone>().weather_effects != 0) 
                Main_Objects[2].GetComponent<Scr_DropZone>().weather_effects--;
                if (Main_Objects[5].GetComponent<Scr_DropZone>().weather_effects != 0) 
                Main_Objects[5].GetComponent<Scr_DropZone>().weather_effects--;

            }
            else if (card_display.Card.card_type == "WS")
            {
                if (Main_Objects[3].GetComponent<Scr_DropZone>().weather_effects != 0) 
                Main_Objects[3].GetComponent<Scr_DropZone>().weather_effects--;
                if (Main_Objects[6].GetComponent<Scr_DropZone>().weather_effects != 0) 
                Main_Objects[6].GetComponent<Scr_DropZone>().weather_effects--;

            }

            for (int i = 1; i <= 6; i++)
            {
                if (Main_Objects[i].GetComponent<Scr_DropZone>() != null)
                    Main_Objects[i].GetComponent<Scr_DropZone>().Power_Modifier();
            }

            Main_Objects[0].GetComponent<Scr_Game_Manager>().Power_Update();
        }

        else if (card.playable_zone == "Rm" || card.playable_zone == "Rd" || card.playable_zone == "Rs")
        {
            if(card.playable_zone == "Rm") 
            {
                if(card.player)
                {
                    Main_Objects[1].GetComponent<Scr_DropZone>().raise_effects--;
                }
                else
                {
                    Main_Objects[4].GetComponent<Scr_DropZone>().raise_effects--;
                }
            }

            else if (card.playable_zone == "Rd")
            {
                if (card.player)
                {
                    Main_Objects[2].GetComponent<Scr_DropZone>().raise_effects--;
                }
                else
                {
                    Main_Objects[5].GetComponent<Scr_DropZone>().raise_effects--;
                }
            }
            
            else if (card.playable_zone == "Rs")
            {
                if (card.player)
                {
                    Main_Objects[3].GetComponent<Scr_DropZone>().raise_effects--;
                }
                else
                {
                    Main_Objects[6].GetComponent<Scr_DropZone>().raise_effects--;
                }
            }

            for (int i = 1; i <= 6; i++)
            {
                if (Main_Objects[i].GetComponent<Scr_DropZone>() != null)
                    Main_Objects[i].GetComponent<Scr_DropZone>().Power_Modifier();
            }

            Main_Objects[0].GetComponent<Scr_Game_Manager>().Power_Update();
        }
    }
}
