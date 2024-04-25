using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Display_Card : MonoBehaviour
{
    public Image Image;
    public TextMeshProUGUI Current_Power;
    public Scr_Card Card;

    // Update is called once per frame
    void Update()
    {
        /*if(gameObject.transform.parent == GameObject.Find("Melee_Zone") ||
            gameObject.transform.parent == GameObject.Find("Distance_Zone") ||
            gameObject.transform.parent == GameObject.Find("Siege_Zone") ||
            gameObject.transform.parent == GameObject.Find("Melee_Zone_Enemy") ||
            gameObject.transform.parent == GameObject.Find("Distance_Zone_Enemy") ||
            gameObject.transform.parent == GameObject.Find("Siege_Zone_Enemy"))
        gameObject.transform.parent.GetComponent<Scr_DropZone>().Power_Modifier();*/

        if (Card != null)
        {
            Image.sprite = Card.image;
            if (Card.card_type == "D")
            {
                Current_Power.text = "0";
            }
            else if (Card.current_power > 0)
            {
                Current_Power.text = Card.current_power.ToString();
            }
            
            else
            {
                Current_Power.text = "";
            }


        }


    } 
}
