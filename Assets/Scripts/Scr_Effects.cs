using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scr_Effects : MonoBehaviour
{
    public Dictionary<string,Action<Scr_Card>> effects;
    private void Start()
    {
        //aqui se annaden los metodos de los efectos al diccionario
        effects = new Dictionary<string, Action<Scr_Card>>()
        {
            { "Weather" , Weather },
            { "Play_Card" , Play_Card }
        };    
    }


    public void Play_Card(Scr_Card card)
    {
        //verificar si es de oro ya que estas no se afectan por los climas ni por los aumentos
        if (card.unit_type == "G") return;

        //aqui se resta al poder de la carta los efectos de los climas activos que afectan su zona para el jugador 1
        if(card.playable_zone.IndexOf("D") !=-1 && card.player)
        {
            card.current_power = card.real_power - GameObject.Find("Distance_Zone").GetComponent<Scr_DropZone>().weather_effects;
        }
        else if (card.playable_zone.IndexOf("M") != -1 && card.player)
        {
            card.current_power = card.real_power - GameObject.Find("Melee_Zone").GetComponent<Scr_DropZone>().weather_effects;
        }
        else if (card.playable_zone.IndexOf("S") != -1 && card.player)
        {
            card.current_power = card.real_power - GameObject.Find("Siege_Zone").GetComponent<Scr_DropZone>().weather_effects;
        }

        //aqui se resta al poder de la carta los efectos de los climas activos que afectan su zona para el jugador 2
        if (card.playable_zone.IndexOf("D") != -1 && !card.player)
        {
            card.current_power = card.real_power - GameObject.Find("Distance_Zone_Enemy").GetComponent<Scr_DropZone>().weather_effects;
        }
        else if (card.playable_zone.IndexOf("M") != -1 && !card.player)
        {
            card.current_power = card.real_power - GameObject.Find("Melee_Zone_Enemy").GetComponent<Scr_DropZone>().weather_effects;
        }
        else if (card.playable_zone.IndexOf("S") != -1 && !card.player)
        {
            card.current_power = card.real_power - GameObject.Find("Siege_Zone_Enemy").GetComponent<Scr_DropZone>().weather_effects;
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

}
