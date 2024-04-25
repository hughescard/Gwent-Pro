using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scr_DropZone : MonoBehaviour
{
    public bool board_side;
    public int weather_effects;
    public int raise_effects;

    public void Power_Modifier()
    {
        foreach(Transform obj in this.transform)
        {
            if(obj.GetComponent<Display_Card>().Card.card_type != "D")
            {
                Display_Card obj_display_card = obj.GetComponent<Display_Card>();
                obj_display_card.Card.current_power = obj_display_card.Card.current_power - this.weather_effects + this.raise_effects;
            }
        }
    }


}
