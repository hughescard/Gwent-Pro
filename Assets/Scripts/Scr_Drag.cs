using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scr_Drag : MonoBehaviour
{
    //private bool Is_Over_Zone = false;
    private bool Dragged = false;
    public bool Played = false;
    private Vector2 Initial_Position;
    private GameObject Colliding_Zone;
    private List<GameObject> Colliding_Zones = new List<GameObject>();
    Display_Card Current_Card;
    bool dragged_trial = false;
    public void Begin_Drag()
    {
        Scr_Game_Manager Prov_GM = GameObject.Find("Game_Manager").GetComponent<Scr_Game_Manager>();
        Current_Card = gameObject.GetComponent<Display_Card>();
        if (Current_Card.Card.player == Prov_GM.turn && !Played)
        {
            dragged_trial = true;
            Initial_Position = transform.position;
            Dragged = true;
        }
        else
            dragged_trial = false;
    }

    public void End_Drag()
    {
        if(dragged_trial)
        {
            Colliding_Zone = Is_Correct_Zone(Current_Card);
            if (Colliding_Zone != null)
            {
                Dragged = false;
                transform.SetParent(Colliding_Zone.transform, false);
                Played = true;
                dragged_trial = false;
            }

            else
            {
                Dragged = false;
                transform.position = Initial_Position;
                Played = false;
                dragged_trial = false ;
            }
        }
        

    }

    private GameObject Is_Correct_Zone(Display_Card Current_Card) 
    {
        foreach( GameObject droppable_zone in Colliding_Zones ) 
        {
            if(droppable_zone.tag == "W" )//si es una dropzone de clima
            {
                if (Current_Card.Card.playable_zone.IndexOf(droppable_zone.tag) != -1 && droppable_zone.transform.childCount < 3)//verificar si el rango de la carta es parte de la zona en que esta colisionando y si no esta llena ya la zona 
                {
                    return droppable_zone;
                }
            }

            else if(droppable_zone.tag == "RD" && Current_Card.Card.player == droppable_zone.GetComponent<Scr_DropZone>().board_side)//si es una dropzone de aumento de distancia
            {
                if (Current_Card.Card.playable_zone.IndexOf(droppable_zone.tag) != -1 && droppable_zone.transform.childCount < 1)//verificar si el rango de la carta es parte de la zona en que esta colisionando y si no esta llena ya la zona 
                {
                    return droppable_zone;
                }
            }

            else if (droppable_zone.tag == "RS" && Current_Card.Card.player == droppable_zone.GetComponent<Scr_DropZone>().board_side)//si es una dropzone de aumento de asedio
            {
                if (Current_Card.Card.playable_zone.IndexOf(droppable_zone.tag) != -1 && droppable_zone.transform.childCount < 1)//verificar si el rango de la carta es parte de la zona en que esta colisionando y si no esta llena ya la zona 
                {
                    return droppable_zone;
                }
            }

            else if (droppable_zone.tag == "RM" && Current_Card.Card.player == droppable_zone.GetComponent<Scr_DropZone>().board_side)//si es una dropzone de aumento de melee
            {
                if (Current_Card.Card.playable_zone.IndexOf(droppable_zone.tag) != -1 && droppable_zone.transform.childCount < 1)//verificar si el rango de la carta es parte de la zona en que esta colisionando y si no esta llena ya la zona 
                {
                    return droppable_zone;
                }
            }
            else if((droppable_zone.tag == "D" || droppable_zone.tag == "M" || droppable_zone.tag == "S") && Current_Card.Card.player == droppable_zone.GetComponent<Scr_DropZone>().board_side)
            {
                if (Current_Card.Card.playable_zone.IndexOf(droppable_zone.tag) != -1 && droppable_zone.transform.childCount < 6)//verificar si el rango de la carta es parte de la zona en que esta colisionando y si no esta llena ya la zona 
                {
                    return droppable_zone;
                }
            }
            
        }

        return null;//las zonas con las q estaba colisionando no son validas
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        //Is_Over_Zone = true;
        //Colliding_Zone = collision.gameObject;
        Colliding_Zones.Insert(0, collision.gameObject); 
    }

    public void OnCollisionExit2D(Collision2D collision)
    {
        Colliding_Zones.Remove(collision.gameObject);
    }


    void Update()
    {
        if(Dragged)
        {
            transform.position = new Vector2(Input.mousePosition.x,Input.mousePosition.y);
        }
    }
}
