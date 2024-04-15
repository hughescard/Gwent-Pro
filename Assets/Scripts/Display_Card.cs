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
        
        if (Card != null )
        {
            Image.sprite = Card.image;
            if (Card.current_power != 0)
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
