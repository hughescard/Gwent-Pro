using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.XR;
//using static UnityEditor.Experimental.GraphView.GraphView;


//revisar poderes de eliminacionde cartas;
public class Scr_Effects : MonoBehaviour
{
    public Dictionary<string,Action<Scr_Card,GameObject>> effects;
    public List<GameObject> Main_Objects;
    private void Start()
    {
        //aqui se annaden los metodos de los efectos al diccionario
        effects = new Dictionary<string, Action<Scr_Card , GameObject>>()
        {
            { "Weather" , Weather },
            { "Play_Card" , Play_Card },
            { "Clear" , Clear },
            { "Increase" , Increase },
            { "Duplicate_Power" , Duplicate_Power },
            { "Beehive" , Beehive },
            { "Destroy_Best_Card" , Destroy_Best_Card },
            { "Destroy_Worst_Card" , Destroy_Worst_Card},
            { "Destroy_Worst_Enemy_Card" , Destroy_Worst_Enemy_Card},
            { "Get_Card" , Get_Card },
            { "Destroy_Raw" , Destroy_Raw },
            { "Weather_Invoque" , Weather_Invoque },
            { "Average_Power" , Average_Power},
        };    
    }


    public void Play_Card(Scr_Card card , GameObject Current_Zone)
    {
        //verificar si es de oro ya que estas no se afectan por los climas ni por los aumentos
        if (card.unit_type == "G") return;

        //aqui se resta y se suma al poder de la carta los efectos de los climas y los aumentos activos que afectan su zona para el jugador 1
        //si la carta se puede jugar en dos rangos y es del jugador 1 
        if (card.playable_zone.IndexOf("D") != -1 && card.playable_zone.IndexOf("M") != -1 && card.player)//si laa carta se puede jugar en dos rangoos (DM)
        {
            if (Current_Zone == GameObject.Find("Distance_Zone"))
                card.current_power = card.current_power - GameObject.Find("Distance_Zone").GetComponent<Scr_DropZone>().weather_effects + GameObject.Find("Distance_Zone").GetComponent<Scr_DropZone>().raise_effects;
            else
                card.current_power = card.current_power - GameObject.Find("Melee_Zone").GetComponent<Scr_DropZone>().weather_effects + GameObject.Find("Melee_Zone").GetComponent<Scr_DropZone>().raise_effects;
        }
        else if (card.playable_zone.IndexOf("D") != -1 && card.playable_zone.IndexOf("S") != -1 && card.player)//si la carta se puede jugar en dos rangos (DS) 
        {
            if (Current_Zone == GameObject.Find("Distance_Zone"))
                card.current_power = card.current_power - GameObject.Find("Distance_Zone").GetComponent<Scr_DropZone>().weather_effects + GameObject.Find("Distance_Zone").GetComponent<Scr_DropZone>().raise_effects;
            else
                card.current_power = card.current_power - GameObject.Find("Siege_Zone").GetComponent<Scr_DropZone>().weather_effects + GameObject.Find("Siege_Zone").GetComponent<Scr_DropZone>().raise_effects;
        }
        else if (card.playable_zone.IndexOf("S") != -1 && card.playable_zone.IndexOf("M") != -1 && card.player)//si la carta se puede jugar en dos rangos (MS)
        {
            if (Current_Zone == GameObject.Find("Siege_Zone"))
                card.current_power = card.current_power - GameObject.Find("Siege_Zone").GetComponent<Scr_DropZone>().weather_effects + GameObject.Find("Siege_Zone").GetComponent<Scr_DropZone>().raise_effects;
            else
                card.current_power = card.current_power - GameObject.Find("Melee_Zone").GetComponent<Scr_DropZone>().weather_effects + GameObject.Find("Melee_Zone").GetComponent<Scr_DropZone>().raise_effects;
        }

        //si la carta se puede jugar en un solo rango y es del jugador 1 
        else if (card.playable_zone.IndexOf("D") !=-1 && card.player)
        {
            card.current_power = card.current_power - GameObject.Find("Distance_Zone").GetComponent<Scr_DropZone>().weather_effects + GameObject.Find("Distance_Zone").GetComponent<Scr_DropZone>().raise_effects;
        }
        else if (card.playable_zone.IndexOf("M") != -1 && card.player)
        {
            card.current_power = card.current_power - GameObject.Find("Melee_Zone").GetComponent<Scr_DropZone>().weather_effects + GameObject.Find("Melee_Zone").GetComponent<Scr_DropZone>().raise_effects;
        }
        else if (card.playable_zone.IndexOf("S") != -1 && card.player)
        {
            card.current_power = card.current_power - GameObject.Find("Siege_Zone").GetComponent<Scr_DropZone>().weather_effects + GameObject.Find("Siege_Zone").GetComponent<Scr_DropZone>().raise_effects;
        }


        //aqui se resta y se suma al poder de la carta los efectos de los climas y los aumentos activos que afectan su zona para el jugador 2
        //si la carta se puede jugar en dos rangos y es del jugador 2
        else if (card.playable_zone.IndexOf("D") != -1 && card.playable_zone.IndexOf("M") != -1 && !card.player)//si laa carta se puede jugar en dos rangoos (DM)
        {
            if (Current_Zone == GameObject.Find("Distance_Zone_Enemy"))
                card.current_power = card.current_power - GameObject.Find("Distance_Zone_Enemy").GetComponent<Scr_DropZone>().weather_effects + GameObject.Find("Distance_Zone_Enemy").GetComponent<Scr_DropZone>().raise_effects;
            else
                card.current_power = card.current_power - GameObject.Find("Melee_Zone_Enemy").GetComponent<Scr_DropZone>().weather_effects + GameObject.Find("Melee_Zone_Enemy").GetComponent<Scr_DropZone>().raise_effects;
        }
        else if (card.playable_zone.IndexOf("D") != -1 && card.playable_zone.IndexOf("S") != -1 && !card.player)//si la carta se puede jugar en dos rangos (DS) 
        {
            if (Current_Zone == GameObject.Find("Distance_Zone_Enemy"))
                card.current_power = card.current_power - GameObject.Find("Distance_Zone_Enemy").GetComponent<Scr_DropZone>().weather_effects + GameObject.Find("Distance_Zone_Enemy").GetComponent<Scr_DropZone>().raise_effects;
            else
                card.current_power = card.current_power - GameObject.Find("Siege_Zone_Enemy").GetComponent<Scr_DropZone>().weather_effects + GameObject.Find("Siege_Zone_Enemy").GetComponent<Scr_DropZone>().raise_effects;
        }
        else if (card.playable_zone.IndexOf("S") != -1 && card.playable_zone.IndexOf("M") != -1 && !card.player)//si la carta se puede jugar en dos rangos (MS)
        {
            if (Current_Zone == GameObject.Find("Siege_Zone_Enemy"))
                card.current_power = card.current_power - GameObject.Find("Siege_Zone_Enemy").GetComponent<Scr_DropZone>().weather_effects + GameObject.Find("Siege_Zone_Enemy").GetComponent<Scr_DropZone>().raise_effects;
            else
                card.current_power = card.current_power - GameObject.Find("Melee_Zone_Enemy").GetComponent<Scr_DropZone>().weather_effects + GameObject.Find("Melee_Zone_Enemy").GetComponent<Scr_DropZone>().raise_effects;
        }

        // si la carta es de un solo rango y es del jugador 2
        else if (card.playable_zone.IndexOf("D") != -1 && !card.player)
        {
            card.current_power = card.current_power - GameObject.Find("Distance_Zone_Enemy").GetComponent<Scr_DropZone>().weather_effects + GameObject.Find("Distance_Zone_Enemy").GetComponent<Scr_DropZone>().raise_effects;
        }
        else if (card.playable_zone.IndexOf("M") != -1 && !card.player)
        {
            card.current_power = card.current_power - GameObject.Find("Melee_Zone_Enemy").GetComponent<Scr_DropZone>().weather_effects + GameObject.Find("Melee_Zone_Enemy").GetComponent<Scr_DropZone>().raise_effects;
        }
        else if (card.playable_zone.IndexOf("S") != -1 && !card.player)
        {
            card.current_power = card.current_power - GameObject.Find("Siege_Zone_Enemy").GetComponent<Scr_DropZone>().weather_effects + GameObject.Find("Siege_Zone_Enemy").GetComponent<Scr_DropZone>().raise_effects;
        }
    }
    public void Weather(Scr_Card card, GameObject Current_Zone)
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
    public void Clear(Scr_Card card , GameObject Current_Zone)
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

            if(obj.gameObject!=null)
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
    public void Increase(Scr_Card card, GameObject Current_Zone)
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

        else if(card.unit_type == "G")
        {
            if (card.player)
                foreach (Transform obj in Main_Objects[1].transform)
                {
                    obj.GetComponent<Display_Card>().Card.current_power = obj.GetComponent<Display_Card>().Card.real_power - Main_Objects[1].GetComponent<Scr_DropZone>().weather_effects + Main_Objects[1].GetComponent<Scr_DropZone>().raise_effects;
                }
            else
                foreach (Transform obj in Main_Objects[4].transform)
                {
                    obj.GetComponent<Display_Card>().Card.current_power = obj.GetComponent<Display_Card>().Card.real_power - Main_Objects[1].GetComponent<Scr_DropZone>().weather_effects + Main_Objects[1].GetComponent<Scr_DropZone>().raise_effects;
                }
        }
    }
    public void Duplicate_Power(Scr_Card card , GameObject Current_Zone)
    {
        if(card.player) {
            foreach(Transform obj in Main_Objects[1].transform)
            {
                if(obj.GetComponent<Display_Card>().Card.unit_type!="G" && obj.GetComponent<Display_Card>().Card.card_type != "D")
                obj.GetComponent<Display_Card>().Card.current_power *= 2;
            }
        }
        else if(!card.player) { 
            foreach(Transform obj in Main_Objects[4].transform)
            {
                if(obj.GetComponent<Display_Card>().Card.unit_type !="G" && obj.GetComponent<Display_Card>().Card.card_type != "D")
                    obj.GetComponent<Display_Card>().Card.current_power *= 2;
            }
        } 
        
    }
    public void Beehive(Scr_Card card , GameObject Current_Zone)
    {
        int cont=1;
        if (card.player)
        {
            foreach (Transform obj in Main_Objects[1].transform)
            {
                if (obj.GetComponent<Display_Card>().Card.name == card.name)
                    cont++;
            }
            foreach (Transform obj in Main_Objects[2].transform)
            {
                if (obj.GetComponent<Display_Card>().Card.name == card.name)
                    cont++;
            }
            foreach (Transform obj in Main_Objects[3].transform)
            {
                if (obj.GetComponent<Display_Card>().Card.name == card.name)
                    cont++;
            }
        }
        else if (!card.player)
        {
            foreach (Transform obj in Main_Objects[4].transform)
            {
                if (obj.GetComponent<Display_Card>().Card.name == card.name)
                    cont++;
            }
            foreach (Transform obj in Main_Objects[5].transform)
            {
                if (obj.GetComponent<Display_Card>().Card.name == card.name)
                    cont++;
            }
            foreach (Transform obj in Main_Objects[6].transform)
            {
                if (obj.GetComponent<Display_Card>().Card.name == card.name)
                    cont++;
            }
        }
        card.current_power *= cont;
        Play_Card(card , Current_Zone);
    } 
    public void Destroy_Best_Card(Scr_Card card , GameObject Current_Zone)
    {
        (int, GameObject) Best_Card = (-1, null);
        #region//getting the best_card
        foreach (Transform obj in Main_Objects[1].transform)
        {
            if (obj.GetComponent<Display_Card>().Card.current_power > Best_Card.Item1 && obj.GetComponent<Display_Card>().Card.unit_type!="G")
                Best_Card = (obj.GetComponent<Display_Card>().Card.current_power, obj.gameObject);
                
        }
        foreach (Transform obj in Main_Objects[2].transform )
        {
            if (obj.GetComponent<Display_Card>().Card.current_power > Best_Card.Item1 && obj.GetComponent<Display_Card>().Card.unit_type != "G")
                Best_Card = (obj.GetComponent<Display_Card>().Card.current_power, obj.gameObject);

        }
        foreach (Transform obj in Main_Objects[3].transform)
        {
            if (obj.GetComponent<Display_Card>().Card.current_power > Best_Card.Item1 && obj.GetComponent<Display_Card>().Card.unit_type != "G")
                Best_Card = (obj.GetComponent<Display_Card>().Card.current_power, obj.gameObject);

        }
        foreach (Transform obj in Main_Objects[4].transform)
        {
            if (obj.GetComponent<Display_Card>().Card.current_power > Best_Card.Item1 && obj.GetComponent<Display_Card>().Card.unit_type != "G")
                Best_Card = (obj.GetComponent<Display_Card>().Card.current_power, obj.gameObject);

        }
        foreach (Transform obj in Main_Objects[5].transform)
        {
            if (obj.GetComponent<Display_Card>().Card.current_power > Best_Card.Item1 && obj.GetComponent<Display_Card>().Card.unit_type != "G")
                Best_Card = (obj.GetComponent<Display_Card>().Card.current_power, obj.gameObject);

        }
        foreach (Transform obj in Main_Objects[6].transform)
        {
            if (obj.GetComponent<Display_Card>().Card.current_power > Best_Card.Item1 && obj.GetComponent<Display_Card>().Card.unit_type != "G")
                Best_Card = (obj.GetComponent<Display_Card>().Card.current_power, obj.gameObject);

        }
        #endregion

        Play_Card(card , Current_Zone);//actualizar poder de la ccarta q activo el efcto antes de actualizar el poder del juego en general 

        #region //deleting it

        if (Best_Card.Item2 == null) //no hay mejor carta
        {
            GameObject Prov_ = GameObject.Find("Game_Manager");
            if (card.player)
            {
                Prov_.GetComponent<Scr_Game_Manager>().total_power_p1 = Prov_.GetComponent<Scr_Game_Manager>().total_power_p1 + card.current_power;
                Prov_.GetComponent<Scr_Game_Manager>().total_power_p1_t.text = Prov_.GetComponent<Scr_Game_Manager>().total_power_p1.ToString();
            }
            else
            {
                Prov_.GetComponent<Scr_Game_Manager>().total_power_p2 = Prov_.GetComponent<Scr_Game_Manager>().total_power_p2 + card.current_power;
                Prov_.GetComponent<Scr_Game_Manager>().total_power_p2_t.text = Prov_.GetComponent<Scr_Game_Manager>().total_power_p2.ToString();
            }

            Prov_.GetComponent<Scr_Game_Manager>().turn = !Prov_.GetComponent<Scr_Game_Manager>().turn;
            foreach (GameObject obj in Prov_.GetComponent<Scr_Game_Manager>().Principal_Objects)
            {
                if (obj.name == "Grave_Image_CM" || obj.name == "Grave_Image_CR" || obj.name == "Grave_Zone" || obj.name == "Grave_Zone_Enemy" || obj.name == "Lives" || obj.name == "Lives_Zone" || obj.name == "Lives_Zone_Enemy") continue;

                Prov_.GetComponent<Scr_Game_Manager>().Rotate_Object(obj);
            }
        }//no hay mejor carta

        else if (Best_Card.Item2.GetComponent<Display_Card>().Card.player)//si la mejor carta es del jugador 1
        {
            GameObject Prov_ = GameObject.Find("Game_Manager");
            if (Prov_ != null)
            {
                Destroy(Best_Card.Item2);
                Prov_.GetComponent<Scr_Game_Manager>().Grave_Image_Create(Best_Card.Item2);
                Prov_.GetComponent<Scr_Game_Manager>().Deck1.Grave.Add(Best_Card.Item2.GetComponent<Display_Card>().Card);
                if(card.player)//si quien activo la carta de efecto es el jugador 1 
                {
                    //actualizar poder jugador 1
                    Prov_.GetComponent<Scr_Game_Manager>().total_power_p1 = Prov_.GetComponent<Scr_Game_Manager>().total_power_p1 - Best_Card.Item1 + card.current_power;
                    Prov_.GetComponent<Scr_Game_Manager>().total_power_p1_t.text = Prov_.GetComponent<Scr_Game_Manager>().total_power_p1.ToString();
                    //actualizar poder jugador 2
                    Prov_.GetComponent<Scr_Game_Manager>().total_power_p2 = Prov_.GetComponent<Scr_Game_Manager>().total_power_p2;
                    Prov_.GetComponent<Scr_Game_Manager>().total_power_p2_t.text = Prov_.GetComponent<Scr_Game_Manager>().total_power_p2.ToString();
                }

                else//si quien activo la carta de efecto es el jugador 2 
                {
                    //actualizar poder jugador 1
                    Prov_.GetComponent<Scr_Game_Manager>().total_power_p1 = Prov_.GetComponent<Scr_Game_Manager>().total_power_p1 - Best_Card.Item1;
                    Prov_.GetComponent<Scr_Game_Manager>().total_power_p1_t.text = Prov_.GetComponent<Scr_Game_Manager>().total_power_p1.ToString();
                    //actualizar poder jugador 2 
                    Prov_.GetComponent<Scr_Game_Manager>().total_power_p2 = Prov_.GetComponent<Scr_Game_Manager>().total_power_p2 + card.current_power;
                    Prov_.GetComponent<Scr_Game_Manager>().total_power_p2_t.text = Prov_.GetComponent<Scr_Game_Manager>().total_power_p2.ToString();
                }


                Prov_.GetComponent<Scr_Game_Manager>().turn = !Prov_.GetComponent<Scr_Game_Manager>().turn;
                foreach (GameObject obj in Prov_.GetComponent<Scr_Game_Manager>().Principal_Objects)
                {
                    if (obj.name == "Grave_Image_CM" || obj.name == "Grave_Image_CR" || obj.name == "Grave_Zone" || obj.name == "Grave_Zone_Enemy" || obj.name == "Lives" || obj.name == "Lives_Zone" || obj.name == "Lives_Zone_Enemy") continue;

                    Prov_.GetComponent<Scr_Game_Manager>().Rotate_Object(obj);
                }
            }
        }
        
        else if (!Best_Card.Item2.GetComponent<Display_Card>().Card.player)//si la mejor carta es del jugador 2 
        {
            GameObject Prov_ = GameObject.Find("Game_Manager");
            if (Prov_ != null)
            {
                Destroy(Best_Card.Item2);
                Prov_.GetComponent<Scr_Game_Manager>().Grave_Image_Create(Best_Card.Item2);
                Prov_.GetComponent<Scr_Game_Manager>().Deck2.Grave.Add(Best_Card.Item2.GetComponent<Display_Card>().Card);
                if(card.player)//si la activo el jugador 1 
                {
                    //actualizar poder jugador 1
                    Prov_.GetComponent<Scr_Game_Manager>().total_power_p1 = Prov_.GetComponent<Scr_Game_Manager>().total_power_p1 + card.current_power;
                    Prov_.GetComponent<Scr_Game_Manager>().total_power_p1_t.text = Prov_.GetComponent<Scr_Game_Manager>().total_power_p1.ToString();
                    //actualizar poder jugador 2
                    Prov_.GetComponent<Scr_Game_Manager>().total_power_p2 = Prov_.GetComponent<Scr_Game_Manager>().total_power_p2 - Best_Card.Item1;
                    Prov_.GetComponent<Scr_Game_Manager>().total_power_p2_t.text = Prov_.GetComponent<Scr_Game_Manager>().total_power_p2.ToString();
                }

                else//si la activo el jugador 2
                {
                    //actualizar poder jugador 1
                    Prov_.GetComponent<Scr_Game_Manager>().total_power_p1 = Prov_.GetComponent<Scr_Game_Manager>().total_power_p1;
                    Prov_.GetComponent<Scr_Game_Manager>().total_power_p1_t.text = Prov_.GetComponent<Scr_Game_Manager>().total_power_p1.ToString();
                    //actualizar poder jugador 2
                    Prov_.GetComponent<Scr_Game_Manager>().total_power_p2 = Prov_.GetComponent<Scr_Game_Manager>().total_power_p2 - Best_Card.Item1 +card.current_power;
                    Prov_.GetComponent<Scr_Game_Manager>().total_power_p2_t.text = Prov_.GetComponent<Scr_Game_Manager>().total_power_p2.ToString();
                }

                Prov_.GetComponent<Scr_Game_Manager>().turn = !Prov_.GetComponent<Scr_Game_Manager>().turn;
                foreach (GameObject obj in Prov_.GetComponent<Scr_Game_Manager>().Principal_Objects)
                {
                    if (obj.name == "Grave_Image_CM" || obj.name == "Grave_Image_CR" || obj.name == "Grave_Zone" || obj.name == "Grave_Zone_Enemy" || obj.name == "Lives" || obj.name == "Lives_Zone" || obj.name == "Lives_Zone_Enemy") continue;

                    Prov_.GetComponent<Scr_Game_Manager>().Rotate_Object(obj);
                }
            }
        }

        
        #endregion
    }

    public void Destroy_Worst_Card(Scr_Card card , GameObject Current_Zone)
    {
        (int, GameObject) Worst_Card = (100, null);
        #region//getting the worst_card
        foreach (Transform obj in Main_Objects[1].transform)
        {
            if (obj.GetComponent<Display_Card>().Card.current_power < Worst_Card.Item1 && obj.GetComponent<Display_Card>().Card.unit_type != "G")
                Worst_Card = (obj.GetComponent<Display_Card>().Card.current_power, obj.gameObject);

        }
        foreach (Transform obj in Main_Objects[2].transform)
        {
            if (obj.GetComponent<Display_Card>().Card.current_power < Worst_Card.Item1 && obj.GetComponent<Display_Card>().Card.unit_type != "G")
                Worst_Card = (obj.GetComponent<Display_Card>().Card.current_power, obj.gameObject);

        }
        foreach (Transform obj in Main_Objects[3].transform)
        {
            if (obj.GetComponent<Display_Card>().Card.current_power < Worst_Card.Item1 && obj.GetComponent<Display_Card>().Card.unit_type != "G")
                Worst_Card = (obj.GetComponent<Display_Card>().Card.current_power, obj.gameObject);

        }
        foreach (Transform obj in Main_Objects[4].transform)
        {
            if (obj.GetComponent<Display_Card>().Card.current_power < Worst_Card.Item1 && obj.GetComponent<Display_Card>().Card.unit_type != "G")
                Worst_Card = (obj.GetComponent<Display_Card>().Card.current_power, obj.gameObject);

        }
        foreach (Transform obj in Main_Objects[5].transform)
        {
            if (obj.GetComponent<Display_Card>().Card.current_power < Worst_Card.Item1 && obj.GetComponent<Display_Card>().Card.unit_type != "G")
                Worst_Card = (obj.GetComponent<Display_Card>().Card.current_power, obj.gameObject);

        }
        foreach (Transform obj in Main_Objects[6].transform)
        {
            if (obj.GetComponent<Display_Card>().Card.current_power < Worst_Card.Item1 && obj.GetComponent<Display_Card>().Card.unit_type != "G")
                Worst_Card = (obj.GetComponent<Display_Card>().Card.current_power, obj.gameObject);

        }
        #endregion

        Play_Card(card, Current_Zone);

        #region //deleting it
        if (Worst_Card.Item2 == null) //no hay mejor carta
        {
            GameObject Prov_ = GameObject.Find("Game_Manager");
            if (card.player)
            {
                Prov_.GetComponent<Scr_Game_Manager>().total_power_p1 = Prov_.GetComponent<Scr_Game_Manager>().total_power_p1 + card.current_power;
                Prov_.GetComponent<Scr_Game_Manager>().total_power_p1_t.text = Prov_.GetComponent<Scr_Game_Manager>().total_power_p1.ToString();
            }
            else
            {
                Prov_.GetComponent<Scr_Game_Manager>().total_power_p2 = Prov_.GetComponent<Scr_Game_Manager>().total_power_p2 + card.current_power;
                Prov_.GetComponent<Scr_Game_Manager>().total_power_p2_t.text = Prov_.GetComponent<Scr_Game_Manager>().total_power_p2.ToString();
            }

            Prov_.GetComponent<Scr_Game_Manager>().turn = !Prov_.GetComponent<Scr_Game_Manager>().turn;
            foreach (GameObject obj in Prov_.GetComponent<Scr_Game_Manager>().Principal_Objects)
            {
                if (obj.name == "Grave_Image_CM" || obj.name == "Grave_Image_CR" || obj.name == "Grave_Zone" || obj.name == "Grave_Zone_Enemy" || obj.name == "Lives" || obj.name == "Lives_Zone" || obj.name == "Lives_Zone_Enemy") continue;

                Prov_.GetComponent<Scr_Game_Manager>().Rotate_Object(obj);
            }
        }//no hay mejor carta
        else if (Worst_Card.Item2!=null && Worst_Card.Item2.GetComponent<Display_Card>().Card.player)//si la peor carta es del jugador 1
        {
            GameObject Prov_ = GameObject.Find("Game_Manager");
            if (Prov_ != null)
            { 
                Destroy(Worst_Card.Item2);
                Prov_.GetComponent<Scr_Game_Manager>().Grave_Image_Create(Worst_Card.Item2);
                Prov_.GetComponent<Scr_Game_Manager>().Deck1.Grave.Add(Worst_Card.Item2.GetComponent<Display_Card>().Card);
                if(card.player)//si el efecto lo activo el jugador 1
                {  //actualizar poder jugador 1
                    Prov_.GetComponent<Scr_Game_Manager>().total_power_p1 = Prov_.GetComponent<Scr_Game_Manager>().total_power_p1 - Worst_Card.Item1 + card.current_power;
                    Prov_.GetComponent<Scr_Game_Manager>().total_power_p1_t.text = Prov_.GetComponent<Scr_Game_Manager>().total_power_p1.ToString();
                    //el poder del jugador 2 no se actualiza porque se queda igual
                }
                else//si el efecto lo activo el jugador 2
                {
                    //actualizar poder jugador 1 
                    Prov_.GetComponent<Scr_Game_Manager>().total_power_p1 = Prov_.GetComponent<Scr_Game_Manager>().total_power_p1 - Worst_Card.Item1;
                    Prov_.GetComponent<Scr_Game_Manager>().total_power_p1_t.text = Prov_.GetComponent<Scr_Game_Manager>().total_power_p1.ToString();
                    //actualizar poder jugador 2
                    Prov_.GetComponent<Scr_Game_Manager>().total_power_p2 = Prov_.GetComponent<Scr_Game_Manager>().total_power_p2 + card.current_power;
                    Prov_.GetComponent<Scr_Game_Manager>().total_power_p2_t.text = Prov_.GetComponent<Scr_Game_Manager>().total_power_p2.ToString();
                }
                Prov_.GetComponent<Scr_Game_Manager>().turn = !Prov_.GetComponent<Scr_Game_Manager>().turn;
                foreach (GameObject obj in Prov_.GetComponent<Scr_Game_Manager>().Principal_Objects)
                {
                    if (obj.name == "Grave_Image_CM" || obj.name == "Grave_Image_CR" || obj.name == "Grave_Zone" || obj.name == "Grave_Zone_Enemy" || obj.name == "Lives" || obj.name == "Lives_Zone" || obj.name == "Lives_Zone_Enemy") continue;

                    Prov_.GetComponent<Scr_Game_Manager>().Rotate_Object(obj);
                }
            }
        }
        else if (Worst_Card.Item2!=null && !Worst_Card.Item2.GetComponent<Display_Card>().Card.player)//si la peor carta es del jugador 2
        {
            GameObject Prov_ = GameObject.Find("Game_Manager");
            if (Prov_ != null)
            {
                Destroy(Worst_Card.Item2);
                Prov_.GetComponent<Scr_Game_Manager>().Grave_Image_Create(Worst_Card.Item2);
                Prov_.GetComponent<Scr_Game_Manager>().Deck2.Grave.Add(Worst_Card.Item2.GetComponent<Display_Card>().Card);
                if(card.player)//si activo el efecto el jugador 1
                {
                    //actualizar poder jugador 1 
                    Prov_.GetComponent<Scr_Game_Manager>().total_power_p1 = Prov_.GetComponent<Scr_Game_Manager>().total_power_p1 + card.current_power;
                    Prov_.GetComponent<Scr_Game_Manager>().total_power_p1_t.text = Prov_.GetComponent<Scr_Game_Manager>().total_power_p1.ToString();
                    //actualizar poder jugador 2 
                    Prov_.GetComponent<Scr_Game_Manager>().total_power_p2 = Prov_.GetComponent<Scr_Game_Manager>().total_power_p2 - Worst_Card.Item1;
                    Prov_.GetComponent<Scr_Game_Manager>().total_power_p2_t.text = Prov_.GetComponent<Scr_Game_Manager>().total_power_p2.ToString();
                }
                else//si activo el efecto el jugador 2 
                {
                    //no hay q actuaalizar el poder del 1 porque no se afecto

                    //actualizar poder jugador 2 
                    Prov_.GetComponent<Scr_Game_Manager>().total_power_p2 = Prov_.GetComponent<Scr_Game_Manager>().total_power_p2 - Worst_Card.Item1 + card.current_power;
                    Prov_.GetComponent<Scr_Game_Manager>().total_power_p2_t.text = Prov_.GetComponent<Scr_Game_Manager>().total_power_p2.ToString();
                }

                Prov_.GetComponent<Scr_Game_Manager>().turn = !Prov_.GetComponent<Scr_Game_Manager>().turn;
                foreach (GameObject obj in Prov_.GetComponent<Scr_Game_Manager>().Principal_Objects)
                {
                    if (obj.name == "Grave_Image_CM" || obj.name == "Grave_Image_CR" || obj.name == "Grave_Zone" || obj.name == "Grave_Zone_Enemy" || obj.name == "Lives" || obj.name == "Lives_Zone" || obj.name == "Lives_Zone_Enemy") continue;

                    Prov_.GetComponent<Scr_Game_Manager>().Rotate_Object(obj);
                }
            }
        }
        #endregion
    }
    public void Destroy_Worst_Enemy_Card(Scr_Card card , GameObject Current_Zone)
    {
        (int, GameObject) Worst_Enemy_Card = (100, null);
        
        if(!card.player)//activo el efecto el jugador 2 
        {
            #region getting worst enemy card
            foreach (Transform obj in Main_Objects[1].transform)
            {
                if (obj.GetComponent<Display_Card>().Card.current_power < Worst_Enemy_Card.Item1 && obj.GetComponent<Display_Card>().Card.unit_type != "G")
                    Worst_Enemy_Card = (obj.GetComponent<Display_Card>().Card.current_power, obj.gameObject);

            }
            foreach (Transform obj in Main_Objects[2].transform)
            {
                if (obj.GetComponent<Display_Card>().Card.current_power < Worst_Enemy_Card.Item1 && obj.GetComponent<Display_Card>().Card.unit_type != "G")
                    Worst_Enemy_Card = (obj.GetComponent<Display_Card>().Card.current_power, obj.gameObject);

            }
            foreach (Transform obj in Main_Objects[3].transform)
            {
                if (obj.GetComponent<Display_Card>().Card.current_power < Worst_Enemy_Card.Item1 && obj.GetComponent<Display_Card>().Card.unit_type != "G")
                    Worst_Enemy_Card = (obj.GetComponent<Display_Card>().Card.current_power, obj.gameObject);

            }
            #endregion

            //actualizar el poder de la carta que activo el efecto antes de aactualizar el poder general 
            Play_Card(card, Current_Zone);

            //eliminando la peor carta del rival 
            GameObject Prov_ = GameObject.Find("Game_Manager");

            if (Worst_Enemy_Card.Item2 == null) //no hay peor carta en el rival
            {    
                ///se actualiza el poder del jugador 2 solamente 
                Prov_.GetComponent<Scr_Game_Manager>().total_power_p2 = Prov_.GetComponent<Scr_Game_Manager>().total_power_p2 + card.current_power;
                Prov_.GetComponent<Scr_Game_Manager>().total_power_p2_t.text = Prov_.GetComponent<Scr_Game_Manager>().total_power_p2.ToString();
               
                Prov_.GetComponent<Scr_Game_Manager>().turn = !Prov_.GetComponent<Scr_Game_Manager>().turn;
                foreach (GameObject obj in Prov_.GetComponent<Scr_Game_Manager>().Principal_Objects)
                {
                    if (obj.name == "Grave_Image_CM" || obj.name == "Grave_Image_CR" || obj.name == "Grave_Zone" || obj.name == "Grave_Zone_Enemy" || obj.name == "Lives" || obj.name == "Lives_Zone" || obj.name == "Lives_Zone_Enemy") continue;

                    Prov_.GetComponent<Scr_Game_Manager>().Rotate_Object(obj);
                }
            }
            
            else if (Prov_ != null)//hay peor carta en el campo rival 
            {
                Destroy(Worst_Enemy_Card.Item2);
                Prov_.GetComponent<Scr_Game_Manager>().Grave_Image_Create(Worst_Enemy_Card.Item2);
                Prov_.GetComponent<Scr_Game_Manager>().Deck1.Grave.Add(Worst_Enemy_Card.Item2.GetComponent<Display_Card>().Card);

                //actualizar poder jugador 2 y 1 
                Prov_.GetComponent<Scr_Game_Manager>().total_power_p1 = Prov_.GetComponent<Scr_Game_Manager>().total_power_p1 - Worst_Enemy_Card.Item1;
                Prov_.GetComponent<Scr_Game_Manager>().total_power_p1_t.text = Prov_.GetComponent<Scr_Game_Manager>().total_power_p1.ToString();

                Prov_.GetComponent<Scr_Game_Manager>().total_power_p2 = Prov_.GetComponent<Scr_Game_Manager>().total_power_p2 + card.current_power;
                Prov_.GetComponent<Scr_Game_Manager>().total_power_p2_t.text = Prov_.GetComponent<Scr_Game_Manager>().total_power_p2.ToString();

                Prov_.GetComponent<Scr_Game_Manager>().turn = !Prov_.GetComponent<Scr_Game_Manager>().turn;
                foreach (GameObject obj in Prov_.GetComponent<Scr_Game_Manager>().Principal_Objects)
                {
                    if (obj.name == "Grave_Image_CM" || obj.name == "Grave_Image_CR" || obj.name == "Grave_Zone" || obj.name == "Grave_Zone_Enemy" || obj.name == "Lives" || obj.name == "Lives_Zone" || obj.name == "Lives_Zone_Enemy") continue;

                    Prov_.GetComponent<Scr_Game_Manager>().Rotate_Object(obj);
                }
            }
        }
        
        else if(card.player)//activo el efecto el jugador 1
        {
            #region getting worst_enemy_card
            foreach (Transform obj in Main_Objects[4].transform)
            {
                if (obj.GetComponent<Display_Card>().Card.current_power < Worst_Enemy_Card.Item1 && obj.GetComponent<Display_Card>().Card.unit_type != "G")
                    Worst_Enemy_Card = (obj.GetComponent<Display_Card>().Card.current_power, obj.gameObject);

            }
            foreach (Transform obj in Main_Objects[5].transform)
            {
                if (obj.GetComponent<Display_Card>().Card.current_power < Worst_Enemy_Card.Item1 && obj.GetComponent<Display_Card>().Card.unit_type != "G")
                    Worst_Enemy_Card = (obj.GetComponent<Display_Card>().Card.current_power, obj.gameObject);

            }
            foreach (Transform obj in Main_Objects[6].transform)
            {
                if (obj.GetComponent<Display_Card>().Card.current_power < Worst_Enemy_Card.Item1 && obj.GetComponent<Display_Card>().Card.unit_type != "G")
                    Worst_Enemy_Card = (obj.GetComponent<Display_Card>().Card.current_power, obj.gameObject);

            }
            #endregion

            Play_Card(card, Current_Zone);///actualizar el poder de la carta que activo el efecto antes de actualizar el poder general 

            GameObject Prov_ = GameObject.Find("Game_Manager");
            if (Worst_Enemy_Card.Item2 == null) //no hay peor carta en el rival
            {
                ///se actualiza el poder del jugador 1 solamente 
                Prov_.GetComponent<Scr_Game_Manager>().total_power_p1 = Prov_.GetComponent<Scr_Game_Manager>().total_power_p1 + card.current_power;
                Prov_.GetComponent<Scr_Game_Manager>().total_power_p1_t.text = Prov_.GetComponent<Scr_Game_Manager>().total_power_p1.ToString();

                Prov_.GetComponent<Scr_Game_Manager>().turn = !Prov_.GetComponent<Scr_Game_Manager>().turn;
                foreach (GameObject obj in Prov_.GetComponent<Scr_Game_Manager>().Principal_Objects)
                {
                    if (obj.name == "Grave_Image_CM" || obj.name == "Grave_Image_CR" || obj.name == "Grave_Zone" || obj.name == "Grave_Zone_Enemy" || obj.name == "Lives" || obj.name == "Lives_Zone" || obj.name == "Lives_Zone_Enemy") continue;

                    Prov_.GetComponent<Scr_Game_Manager>().Rotate_Object(obj);
                }
            }

            else if (Prov_ != null)//si hay peor carta en el campo enemigo
            {
                Destroy(Worst_Enemy_Card.Item2);
                Prov_.GetComponent<Scr_Game_Manager>().Grave_Image_Create(Worst_Enemy_Card.Item2);
                Prov_.GetComponent<Scr_Game_Manager>().Deck2.Grave.Add(Worst_Enemy_Card.Item2.GetComponent<Display_Card>().Card);
                //actualizar el poder del jugador 1 y 2
                Prov_.GetComponent<Scr_Game_Manager>().total_power_p2 = Prov_.GetComponent<Scr_Game_Manager>().total_power_p2 - Worst_Enemy_Card.Item1;
                Prov_.GetComponent<Scr_Game_Manager>().total_power_p2_t.text = Prov_.GetComponent<Scr_Game_Manager>().total_power_p2.ToString();

                Prov_.GetComponent<Scr_Game_Manager>().total_power_p1 = Prov_.GetComponent<Scr_Game_Manager>().total_power_p1 + card.current_power;
                Prov_.GetComponent<Scr_Game_Manager>().total_power_p1_t.text = Prov_.GetComponent<Scr_Game_Manager>().total_power_p1.ToString();

                Prov_.GetComponent<Scr_Game_Manager>().turn = !Prov_.GetComponent<Scr_Game_Manager>().turn;
                foreach (GameObject obj in Prov_.GetComponent<Scr_Game_Manager>().Principal_Objects)
                {
                    if (obj.name == "Grave_Image_CM" || obj.name == "Grave_Image_CR" || obj.name == "Grave_Zone" || obj.name == "Grave_Zone_Enemy" || obj.name == "Lives" || obj.name == "Lives_Zone" || obj.name == "Lives_Zone_Enemy") continue;

                    Prov_.GetComponent<Scr_Game_Manager>().Rotate_Object(obj);
                }
            }
        }
        
    }

    public void Get_Card(Scr_Card card , GameObject Current_Zone)
    {
        GameObject hand;
        if (card.player)
        {
            hand = GameObject.Find("Hand_Zone");
        }
        else
        {
            hand = GameObject.Find("Hand_Zone_Enemy");
        }
        Scr_Game_Manager Prov_ = GameObject.Find("Game_Manager").GetComponent<Scr_Game_Manager>();
        if(card.player)
        {
            Prov_.Deck1.Instantiate_Card(1);
        }
        else
        {
            Prov_.Deck2.Instantiate_Card(1);
        }
    }    
    public void Destroy_Raw(Scr_Card card , GameObject Current_Zone)
    {
        (int,GameObject) Smallest_Raw = (10,null);
        for(int i = 1 ; i<=6 ; i++)
        {
            if(Smallest_Raw.Item1 > Main_Objects[i].transform.childCount)
            {
                if (Main_Objects[i].transform.childCount == 0) continue;
                Smallest_Raw.Item1 = Math.Min(Smallest_Raw.Item1, Main_Objects[i].transform.childCount);
                Smallest_Raw.Item2 = Main_Objects[i];
            }
            
        }

        if(Smallest_Raw.Item2 == null)//no hay cartas jugadas en ninguna fila
        {
            GameObject Prov_ = GameObject.Find("Game_Manager");

            if(card.player)//el eefcto fue activado por el jugador 1 
            {
                Prov_.GetComponent<Scr_Game_Manager>().total_power_p1 = Prov_.GetComponent<Scr_Game_Manager>().total_power_p1 + card.real_power;
                Prov_.GetComponent<Scr_Game_Manager>().total_power_p1_t.text = Prov_.GetComponent<Scr_Game_Manager>().total_power_p1.ToString();

                Prov_.GetComponent<Scr_Game_Manager>().turn = !Prov_.GetComponent<Scr_Game_Manager>().turn;
                foreach (GameObject obj in Prov_.GetComponent<Scr_Game_Manager>().Principal_Objects)
                {
                    if (obj.name == "Grave_Image_CM" || obj.name == "Grave_Image_CR" || obj.name == "Grave_Zone" || obj.name == "Grave_Zone_Enemy" || obj.name == "Lives" || obj.name == "Lives_Zone" || obj.name == "Lives_Zone_Enemy") continue;

                    Prov_.GetComponent<Scr_Game_Manager>().Rotate_Object(obj);
                }
            }
            else //el efecto lo activo el jugador 2
            {
                Prov_.GetComponent<Scr_Game_Manager>().total_power_p2 = Prov_.GetComponent<Scr_Game_Manager>().total_power_p2 + card.real_power;
                Prov_.GetComponent<Scr_Game_Manager>().total_power_p2_t.text = Prov_.GetComponent<Scr_Game_Manager>().total_power_p2.ToString();

                Prov_.GetComponent<Scr_Game_Manager>().turn = !Prov_.GetComponent<Scr_Game_Manager>().turn;
                foreach (GameObject obj in Prov_.GetComponent<Scr_Game_Manager>().Principal_Objects)
                {
                    if (obj.name == "Grave_Image_CM" || obj.name == "Grave_Image_CR" || obj.name == "Grave_Zone" || obj.name == "Grave_Zone_Enemy" || obj.name == "Lives" || obj.name == "Lives_Zone" || obj.name == "Lives_Zone_Enemy") continue;

                    Prov_.GetComponent<Scr_Game_Manager>().Rotate_Object(obj);
                }
            }
                
            return;
        }

        else //se encontro la fila con menos cartas 
        {
            int power_to_dif = 0;
            bool player_affected = Smallest_Raw.Item2.transform.GetChild(0).GetComponent<Display_Card>().Card.player;

            for (int i = 0; i < Smallest_Raw.Item1; i++)
            {
                power_to_dif += Smallest_Raw.Item2.transform.GetChild(i).GetComponent<Display_Card>().Card.current_power;
                GameObject.Find("Game_Manager").GetComponent<Scr_Game_Manager>().Grave_Image_Create(Smallest_Raw.Item2.transform.GetChild(i).gameObject);
                GameObject.Find("Game_Manager").GetComponent<Scr_Game_Manager>().Grave_Add(Smallest_Raw.Item2.transform.GetChild(i).gameObject);
                Destroy(Smallest_Raw.Item2.transform.GetChild(i).gameObject);
            }

            GameObject Prov_ = GameObject.Find("Game_Manager");
            Play_Card(card, Current_Zone);//actualizar el poder de la carta que activo el efecto antes de actualizar el poder general 
            
            if(player_affected && card.player)//el efecto fue activado por el jugador 1 y afecto al jugador 1
            {
                Prov_.GetComponent<Scr_Game_Manager>().total_power_p1 = Prov_.GetComponent<Scr_Game_Manager>().total_power_p1 + card.current_power - power_to_dif;
                Prov_.GetComponent<Scr_Game_Manager>().total_power_p1_t.text = Prov_.GetComponent<Scr_Game_Manager>().total_power_p1.ToString();
            }

            else if (!player_affected && !card.player)// efecto lo activo el jugador 2 y afecto al juagador 2
            {
                Prov_.GetComponent<Scr_Game_Manager>().total_power_p2 = Prov_.GetComponent<Scr_Game_Manager>().total_power_p2 + card.current_power - power_to_dif;
                Prov_.GetComponent<Scr_Game_Manager>().total_power_p2_t.text = Prov_.GetComponent<Scr_Game_Manager>().total_power_p2.ToString();
            }

            else if(!player_affected && card.player)//el efecto lo activo el jugador 1 y afecto al jugador 2
            {
                //actualizar el poder de ambos jugadores
                Prov_.GetComponent<Scr_Game_Manager>().total_power_p1 = Prov_.GetComponent<Scr_Game_Manager>().total_power_p1 + card.current_power ;
                Prov_.GetComponent<Scr_Game_Manager>().total_power_p1_t.text = Prov_.GetComponent<Scr_Game_Manager>().total_power_p1.ToString();

                Prov_.GetComponent<Scr_Game_Manager>().total_power_p2 = Prov_.GetComponent<Scr_Game_Manager>().total_power_p2 - power_to_dif;
                Prov_.GetComponent<Scr_Game_Manager>().total_power_p2_t.text = Prov_.GetComponent<Scr_Game_Manager>().total_power_p2.ToString();
            }

            else if (player_affected && !card.player)//el efecto lo activo el jugador 2 y afecto al jugador 1
            {
                //actualizar el poder de ambos jugadores
                Prov_.GetComponent<Scr_Game_Manager>().total_power_p2 = Prov_.GetComponent<Scr_Game_Manager>().total_power_p2 + card.current_power;
                Prov_.GetComponent<Scr_Game_Manager>().total_power_p2_t.text = Prov_.GetComponent<Scr_Game_Manager>().total_power_p2.ToString();

                Prov_.GetComponent<Scr_Game_Manager>().total_power_p1 = Prov_.GetComponent<Scr_Game_Manager>().total_power_p1 - power_to_dif;
                Prov_.GetComponent<Scr_Game_Manager>().total_power_p1_t.text = Prov_.GetComponent<Scr_Game_Manager>().total_power_p1.ToString();
            }


            Prov_.GetComponent<Scr_Game_Manager>().turn = !Prov_.GetComponent<Scr_Game_Manager>().turn;
            foreach (GameObject obj in Prov_.GetComponent<Scr_Game_Manager>().Principal_Objects)
            {
                if (obj.name == "Grave_Image_CM" || obj.name == "Grave_Image_CR" || obj.name == "Grave_Zone" || obj.name == "Grave_Zone_Enemy" || obj.name == "Lives" || obj.name == "Lives_Zone" || obj.name == "Lives_Zone_Enemy") continue;

                Prov_.GetComponent<Scr_Game_Manager>().Rotate_Object(obj);
            }

        }
    }
    
    public void Weather_Invoque(Scr_Card card, GameObject Current_Zone)
    {
        if(card.player)//activo el efecto el jugador 1
        {
            GameObject hand = GameObject.Find("Hand_Zone");
            foreach(Transform obj in hand.transform)
            {
                if(obj.GetComponent<Display_Card>().Card.effect == "Weather")
                {
                    obj.SetParent(GameObject.Find("Weather_Zone").transform);
                    Weather(obj.GetComponent<Display_Card>().Card , Current_Zone);
                    break;
                }
            }
        }

        if (!card.player)//activo el efecto el jugador 2
        {
            GameObject hand = GameObject.Find("Hand_Zone_Enemy");
            foreach (Transform obj in hand.transform)
            {
                if (obj.GetComponent<Display_Card>().Card.effect == "Weather")
                {
                    obj.GetComponent<Scr_Drag>().Played = true;
                    obj.SetParent(GameObject.Find("Weather_Zone").transform);
                    Weather(obj.GetComponent<Display_Card>().Card,Current_Zone);
                    break;
                }
            }
        }
    }

    public void Average_Power(Scr_Card card, GameObject Current_Zone)
    {
        int total_power=0;
        int total_cards=0;

        for(int i=1;i<=6;i++)
        {
            foreach(Transform obj in Main_Objects[i].transform)
            {
                if (obj.GetComponent<Display_Card>().Card.unit_type == "G" || obj.GetComponent<Display_Card>().Card.card_type == "D") continue;
                total_cards++;
                total_power += int.Parse(obj.GetComponent<Display_Card>().Current_Power.text);
            }
        }

        int average = total_power /= total_cards;

        for (int i = 1; i <= 6; i++)
        {
            foreach (Transform obj in Main_Objects[i].transform)
            {
                if (obj.GetComponent<Display_Card>().Card.unit_type == "G" || obj.GetComponent<Display_Card>().Card.card_type == "D") continue;
                obj.GetComponent<Display_Card>().Card.current_power = average;
            }
        }

    }

   
}
