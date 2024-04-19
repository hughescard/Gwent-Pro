using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scr_Drag : MonoBehaviour
{
    //private bool Is_Over_Zone = false;
    public bool Dragged = false;
    public bool Played = false;
    private Vector2 Initial_Position;
    private GameObject Colliding_Zone;
    private List<GameObject> Colliding_Zones = new List<GameObject>();
    Display_Card Current_Card;
    bool dragged_trial = false;
    private Scr_Effects effects;
    
    public void Awake()
    {
        effects = GameObject.Find("Game_Manager").GetComponent<Scr_Effects>();
        Current_Card = gameObject.GetComponent<Display_Card>();
    }
    public void Begin_Drag()
    {
        Scr_Game_Manager Prov_GM = GameObject.Find("Game_Manager").GetComponent<Scr_Game_Manager>();
        if (Current_Card.Card.player == Prov_GM.turn && !Played)
        {
            Transform Prov_ = GameObject.Find("Zoom_Card_Zone").transform;//accede aal transform del panel del zoom_card
            if(Prov_.childCount != 0)
                Destroy(Prov_.GetChild(0).gameObject);//destruir el zoom card cuando se comience el drag
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
                if(Current_Card.Card.card_type == "D")
                {
                   
                    Display_Card Exchange = Colliding_Zone.GetComponent<Display_Card>();
                    Transform drop = Colliding_Zone.transform.parent;
                    transform.SetParent(drop, false);
                    Exchange.Card.current_power = Exchange.Card.real_power;
                    Exchange.gameObject.GetComponent<Scr_Drag>().Played = false;
                    if(Exchange.Card.player)
                        Colliding_Zone.transform.SetParent(GameObject.Find("Hand_Zone").transform, false);
                    else if(!Exchange.Card.player)
                        Colliding_Zone.transform.SetParent(GameObject.Find("Hand_Zone_Enemy").transform, false);

                    effects.Decoy(Exchange.Card,Exchange);

                    Dragged = false;
                    dragged_trial = false;
                    Played = true;

                    GameObject Prov_GM = GameObject.Find("Game_Manager");

                    Prov_GM.GetComponent<Scr_Game_Manager>().Continuous_Passes = 0;
                    Prov_GM.GetComponent<Scr_Game_Manager>().Power_Update();
                    Prov_GM.GetComponent<Scr_Game_Manager>().Change_Turn();

                    return;
                }

                if(Current_Card.Card.effect != "")
                    effects.effects[Current_Card.Card.effect](Current_Card.Card);

                else if(Current_Card.Card.card_type == "U")
                    effects.effects["Play_Card"](Current_Card.Card);

                Dragged = false;
                transform.SetParent(Colliding_Zone.transform, false);
                dragged_trial = false;
                Played = true;

                GameObject Prov_Gm = GameObject.Find("Game_Manager");

                Prov_Gm.GetComponent<Scr_Game_Manager>().Continuous_Passes=0;

                if (Current_Card.Card.effect == "Destroy_Best_Card")
                    return;
                else if (Current_Card.Card.effect == "Destroy_Worst_Card")
                    return;
                else if (Current_Card.Card.effect == "Destroy_Worst_Enemy_Card")
                    return;
                else if (Current_Card.Card.effect == "Destroy_Raw")
                    return;


                Prov_Gm.GetComponent<Scr_Game_Manager>().Power_Update();
                Prov_Gm.GetComponent<Scr_Game_Manager>().Change_Turn();

                if (Current_Card.Card.card_type == "C")
                    Destroy(gameObject);
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
                if(Current_Card.Card.card_type == "C" && Current_Card.Card.playable_zone.IndexOf(droppable_zone.tag) != -1)
                {
                    return droppable_zone;
                }
                else if (Current_Card.Card.playable_zone.IndexOf(droppable_zone.tag) != -1 && droppable_zone.transform.childCount < 3)//verificar si el rango de la carta es parte de la zona en que esta colisionando y si no esta llena ya la zona 
                {
                    return droppable_zone;
                }
            }

            else if(droppable_zone.tag == "Rd" && Current_Card.Card.player == droppable_zone.GetComponent<Scr_DropZone>().board_side)//si es una dropzone de aumento de distancia
            {
                if (Current_Card.Card.playable_zone.IndexOf(droppable_zone.tag) != -1 && droppable_zone.transform.childCount < 1)//verificar si el rango de la carta es parte de la zona en que esta colisionando y si no esta llena ya la zona 
                {
                    return droppable_zone;
                }
            }

            else if (droppable_zone.tag == "Rs" && Current_Card.Card.player == droppable_zone.GetComponent<Scr_DropZone>().board_side)//si es una dropzone de aumento de asedio
            {
                if (Current_Card.Card.playable_zone.IndexOf(droppable_zone.tag) != -1 && droppable_zone.transform.childCount < 1)//verificar si el rango de la carta es parte de la zona en que esta colisionando y si no esta llena ya la zona 
                {
                    return droppable_zone;
                }
            }

            else if (droppable_zone.tag == "Rm" && Current_Card.Card.player == droppable_zone.GetComponent<Scr_DropZone>().board_side)//si es una dropzone de aumento de melee
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
            else if(droppable_zone.tag == "Card")
            {
                if (Current_Card.Card.playable_zone == droppable_zone.tag && droppable_zone.GetComponent<Display_Card>().Card.player == Current_Card.Card.player)//verificar si el rango de la carta es parte de la zona en que esta colisionando y si no esta llena ya la zona 
                {
                    return droppable_zone;
                }
            }


        }

        return null;//las zonas con las q estaba colisionando no son validas
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if(Current_Card.Card.card_type == "D")
        {
            if (collision.gameObject!=null && collision.gameObject.tag == "Card")
                Colliding_Zones.Insert(0, collision.gameObject);
        }
       
        else
            Colliding_Zones.Insert(0, collision.gameObject); 
    }

    public void OnCollisionExit2D(Collision2D collision)
    {
        Colliding_Zones.Remove(collision.gameObject);
    }

    public void Activate_Leader_Effect()
    {
        return;//comentar esta linea en caso de annadir nuevos efctos de lideres 
        Scr_Game_Manager Prov_Gm = GameObject.Find("Game_Manager").GetComponent<Scr_Game_Manager>();
        if (Current_Card.Card.player != Prov_Gm.turn || Prov_Gm.leader_effect_used) return;

        effects.effects[Current_Card.Card.effect](Current_Card.Card);
        Prov_Gm.leader_effect_used = true;
        Prov_Gm.Continuous_Passes = 0;
        Prov_Gm.Power_Update();
        Prov_Gm.Change_Turn();
    }

    void Update()
    {
        if(Dragged)
        {
            Transform Prov_ = GameObject.Find("Zoom_Card_Zone").transform;//accede aal transform del panel del zoom_card
            if (Prov_.childCount != 0)
                Destroy(Prov_.GetChild(0).gameObject);//destruir el zoom card cuando se comience el drag
            transform.position = new Vector2(Input.mousePosition.x,Input.mousePosition.y);
        }
    }
}
