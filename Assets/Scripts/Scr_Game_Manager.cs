using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEditor.Experimental.GraphView;


/// <summary>
/// arreglar en el decoy que se esta cambiando por cartas del enemigo 
/// 
/// </summary>






public class Scr_Game_Manager : MonoBehaviour
{
    public Scr_Player Player1;
    public Scr_Player Player2;
    public bool turn;
    public List<GameObject> Principal_Objects;
    public int round = 1;
    public int Continuous_Passes = 0;
    public Scr_Deck Deck1;
    public Scr_Deck Deck2;
    public int total_power_p1;
    public int total_power_p2;
    public TextMeshProUGUI total_power_p1_t;
    public TextMeshProUGUI total_power_p2_t;
    public bool Change_round;
    bool Change_round_2;
    private bool Winp1 = false;
    private bool Winp2 = false;
    public bool leader_effect_used = true;
    public bool leader_effect_used2 = true;
    public List<Scr_DropZone> Drop_Zones;
    public int card_keeped_power;

    void Start()
    {
        leader_effect_used = false;
        leader_effect_used2 = false;
        turn = true;
        Player1 = new Scr_Player("Guille", "CM", true);
        Player2 = new Scr_Player("Jean", "C_and_R", false);

        if(Player1.faction == "CM")
        {
            GameObject.Find("Deck_Zone").GetComponent<Image>().sprite = Resources.Load<Sprite>("Spr_Dorso_CM");
            Instantiate(Principal_Objects[12], Principal_Objects[10].transform);
            Instantiate(Principal_Objects[12], Principal_Objects[10].transform);
            if (Principal_Objects[10].transform.childCount != 0)
                foreach (Transform obj in Principal_Objects[10].transform)
                    obj.GetComponent<Image>().sprite = Resources.Load<Sprite>("Spr_Lives_CM");
   
        }

        else if (Player1.faction != "CM")
        {
            GameObject.Find("Deck_Zone").GetComponent<Image>().sprite = Resources.Load<Sprite>("Spr_Dorso_C_and_R");
            Instantiate(Principal_Objects[12], Principal_Objects[10].transform);
            Instantiate(Principal_Objects[12], Principal_Objects[10].transform);
            if(Principal_Objects[10].transform.childCount != 0)
                foreach (Transform obj in Principal_Objects[10].transform)
                    obj.GetComponent<Image>().sprite = Resources.Load<Sprite>("Spr_Lives_CR");
        }


        if (Player2.faction == "CM")
        {
            GameObject.Find("Deck_Zone_Enemy").GetComponent<Image>().sprite = Resources.Load<Sprite>("Spr_Dorso_CM");
            Instantiate(Principal_Objects[12], Principal_Objects[11].transform);
            Instantiate(Principal_Objects[12], Principal_Objects[11].transform);
            if (Principal_Objects[11].transform.childCount != 0)
                foreach (Transform obj in Principal_Objects[11].transform)
                    obj.GetComponent<Image>().sprite = Resources.Load<Sprite>("Spr_Lives_CM");
        }


        else if(Player2.faction == "C_and_R")
        {
            GameObject.Find("Deck_Zone_Enemy").GetComponent<Image>().sprite = Resources.Load<Sprite>("Spr_Dorso_C_and_R");
            Instantiate(Principal_Objects[12], Principal_Objects[11].transform);
            Instantiate(Principal_Objects[12], Principal_Objects[11].transform);
            if (Principal_Objects[11].transform.childCount != 0)
                    foreach (Transform obj in Principal_Objects[11].transform)
                        obj.GetComponent<Image>().sprite = Resources.Load<Sprite>("Spr_Lives_CR");
        }


        Deck1 = GameObject.Find("Deck_Zone").GetComponent<Scr_Deck>();
        Deck1.Deck = Scr_Card_DataBase.Create_Deck(Player1.Board_Side, Player1.faction);
        Deck1.Deck_Order_Randomizer();

        Deck2 = GameObject.Find("Deck_Zone_Enemy").GetComponent<Scr_Deck>();
        Deck2.Deck = Scr_Card_DataBase.Create_Deck(Player2.Board_Side, Player2.faction);
        Deck2.Deck_Order_Randomizer();

        Power_Update();
      
    }

    public void Rotate_Object(GameObject obj)
    {
        obj.transform.Rotate(0, 0, 180);
    }
    public void Change_Turn()
    {
        if(Change_round_2)
        {
            if (Winp1)
            {
                Winp1 = false;
                if (turn == false)
                {
                    foreach (GameObject obj in Principal_Objects)
                    {
                        if (obj.name == "Grave_Image_CM" || obj.name == "Grave_Image_CR" || obj.name == "Grave_Zone" || obj.name == "Grave_Zone_Enemy" || obj.name == "Lives" || obj.name == "Lives_Zone" || obj.name == "Lives_Zone_Enemy") continue;

                        Rotate_Object(obj);
                    }
                    turn = true;
                }
                Change_round_2 = false;
                if (Player1.faction == "C_and_R")
                {
                    total_power_p1 = card_keeped_power;
                    total_power_p2 = 0;
                }
                else if (Player2.faction == "C_and_R")
                {
                    total_power_p2 = card_keeped_power;
                    total_power_p1 = 0;
                }
                total_power_p1_t.text = total_power_p1.ToString();
                total_power_p2_t.text = total_power_p2.ToString();
                return;
            }

            else if (Winp2)
            {
                Winp2 = false;
                if (turn==true)
                {
                    foreach (GameObject obj in Principal_Objects)
                    {
                        if (obj.name == "Grave_Image_CM" || obj.name == "Grave_Image_CR" || obj.name == "Grave_Zone" || obj.name == "Grave_Zone_Enemy" || obj.name == "Lives" || obj.name == "Lives_Zone" || obj.name == "Lives_Zone_Enemy") continue;

                        Rotate_Object(obj);
                    }
                    turn = false;
                }
                Change_round_2 = false;
                if (Player1.faction == "C_and_R")
                {
                    total_power_p1 = card_keeped_power;
                    total_power_p2 = 0;
                }
                else if (Player2.faction == "C_and_R")
                {
                    total_power_p2 = card_keeped_power;
                    total_power_p1 = 0;
                }
                total_power_p1_t.text = total_power_p1.ToString();
                total_power_p2_t.text = total_power_p2.ToString();
                return;
            }

            //Power_Update();
            //Change_round_2 = false;
            //return;
        }

        turn = !turn;     
        foreach (GameObject obj in Principal_Objects)
        {
            if (obj.name == "Grave_Image_CM" || obj.name == "Grave_Image_CR" || obj.name == "Grave_Zone" || obj.name == "Grave_Zone_Enemy" || obj.name == "Lives" || obj.name == "Lives_Zone" || obj.name == "Lives_Zone_Enemy") continue;

            Rotate_Object(obj);
        }

        Power_Update();
    }
    public void Change_Round()
    {
        if (this.Continuous_Passes == 2)
        {
            //primero debe verificarse si con esta ronda se acaba el juego 
            //gana jugador 1
            if (total_power_p1 > total_power_p2)
            {
                Player2.Lives--;
                Destroy(Principal_Objects[11].transform.GetChild(0).gameObject);
                if (Player2.Lives == 0)
                {
                    Debug.Log("se acabo el juego, gana Player1");
                    string sceneName = SceneManager.GetActiveScene().name;
                    SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
                }
            }

            //gana jugador 2
            else if (total_power_p2 > total_power_p1)
            {
                Player1.Lives--;
                Destroy(Principal_Objects[10].transform.GetChild(0).gameObject);
                if (Player1.Lives == 0)
                {
                    Debug.Log("se acabo el juego, gana Player2");
                    string sceneName = SceneManager.GetActiveScene().name;
                    SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
                }
            }

            //empate
            else
            {
                ////descomentar el codigo siguiente en caso de querer usar efecto de lider de ganar en caso de empate 
                //if(leader_effect_used ==false)
                //{
                //    if (Player1.faction=="faccion del lider cuyo efecto es este")//el lider es de jugador 1
                //    {
                //        Player2.Lives--;
                //        Destroy(Principal_Objects[11].transform.GetChild(0).gameObject);
                //        leader_effect_used2 = true;
                //        if (Player2.Lives == 0)
                //        {
                //            Debug.Log("se acabo el juego, gana Player1");
                //            string sceneName = SceneManager.GetActiveScene().name;
                //            SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
                //        }
                //    }
                //    else if (Player2.faction=="faccion del lider cuyo efecto es este")//el lider es del jugador 2 
                //    {
                //        Player1.Lives--;
                //        Destroy(Principal_Objects[10].transform.GetChild(0).gameObject);
                //        if (Player1.Lives == 0)
                //        {
                //            Debug.Log("se acabo el juego, gana Player2");
                //            string sceneName = SceneManager.GetActiveScene().name;
                //            SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
                //        }
                //    }
                //}

                if(true)//cambiar if(true) por else en caso de querer usar el poder del lider cuyo efecto es ganar una ronda en empate 
                {
                    Player1.Lives--;
                    Player2.Lives--;
                    Destroy(Principal_Objects[11].transform.GetChild(0).gameObject);
                    Destroy(Principal_Objects[10].transform.GetChild(0).gameObject);
                    if (Player1.Lives == 0 || Player2.Lives == 0)
                    {
                        Debug.Log("se acabo el juego, quedo en empate");
                        string sceneName = SceneManager.GetActiveScene().name;
                        SceneManager.LoadScene(sceneName, LoadSceneMode.Single);

                    }
                }
            }

            round++;

            foreach (GameObject obj in FindObjectsOfType<GameObject>())//destruir todas las cartas jugadas 
            {
                if (obj.name.Contains("CardPrefab") && obj.transform.parent.gameObject != null
                    && obj.transform.parent.gameObject != GameObject.Find("Hand_Zone")
                    && obj.transform.parent.gameObject != GameObject.Find("Hand_Zone_Enemy"))
                {
                    if(obj.GetComponent<Display_Card>().Card.faction=="CR" && !leader_effect_used)
                    {
                        leader_effect_used = true;
                        card_keeped_power = obj.GetComponent<Display_Card>().Card.real_power;
                        continue;
                    }

                    if (obj.GetComponent<Display_Card>().Card.player)
                    {
                        Deck1.Grave.Insert(0, obj.GetComponent<Display_Card>().Card);

                        if (Player1.faction == "CM" && Principal_Objects[4].transform.childCount == 0)
                            Instantiate(Principal_Objects[6], Principal_Objects[4].transform);

                        else if (Player1.faction != "CM" && Principal_Objects[4].transform.childCount == 0)
                            Instantiate(Principal_Objects[7], Principal_Objects[4].transform);
                    }

                    else
                    {
                        Deck2.Grave.Insert(0, obj.GetComponent<Display_Card>().Card);

                        if (Player2.faction == "CM" && Principal_Objects[5].transform.childCount == 0)
                            Instantiate(Principal_Objects[6], Principal_Objects[5].transform);

                        else if (Player2.faction != "CM" && Principal_Objects[5].transform.childCount == 0)
                            Instantiate(Principal_Objects[7], Principal_Objects[5].transform);
                    }

                    Destroy(obj);
                }
            }

            foreach(Scr_DropZone dropzone in Drop_Zones)
            {
                dropzone.weather_effects = 0;
                dropzone.raise_effects = 0;
            }

            Change_round = true;
            Change_round_2 = true;
            leader_effect_used = false;

            //descomentar el siguiente codigo para usar efecto de lider de en caso de eprder una ronda 

            if (total_power_p1 >= total_power_p2) Winp1 = true;
            else if(total_power_p2 > total_power_p1)Winp2 = true;

            Power_Update();

            //reseteando la cantidad de pases
            Continuous_Passes = 0;

            //annadiendo las dos q tocan por ronda
            Deck1.Instantiate_Card(2);
            Deck2.Instantiate_Card(2);

            //annadiendo la carta extra que da el lider de CM en la segunda ronda 
            if (Player1.faction == "CM" && round==2)
            {
                Deck1.Instantiate_Card(1);
            }
            else if (Player2.faction == "CM" && round==2)
            {
                Deck2.Instantiate_Card(1);
            }

        }
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
    public void Power_Update()
    {
        total_power_p1 = 0;
        total_power_p2 = 0;
        
        if(Change_round)
        {
            if (Player1.faction == "C_and_R")
            { 
                total_power_p1 = card_keeped_power;
                total_power_p2 = 0;
            } 
            else if (Player2.faction == "C_and_R")
            {
                total_power_p2 = card_keeped_power;
                total_power_p1 = 0;
            }

            Change_round = false;
            total_power_p1_t.text = total_power_p1.ToString();
            total_power_p2_t.text = total_power_p2.ToString();
            return;
        }

        //aqui se ejecuta la actualizacion del poder acumulado por el jugador 1 
        #region //Jugador 1
        foreach (Transform obj in Drop_Zones[2].transform)
            if (obj.GetComponent<Display_Card>().Card.card_type == "D") continue;
            else 
            total_power_p1 += obj.GetComponent<Display_Card>().Card.current_power;


        foreach (Transform obj in Drop_Zones[1].transform)
            if (obj.GetComponent<Display_Card>().Card.card_type == "D") continue;
            else    
            total_power_p1 += obj.GetComponent<Display_Card>().Card.current_power;
        
        foreach (Transform obj in Drop_Zones[0].transform)
            if (obj.GetComponent<Display_Card>().Card.card_type == "D") continue;
            else    
            total_power_p1 += obj.GetComponent<Display_Card>().Card.current_power;

        #endregion

        //aqui se ejecuta la actualizacion de poder acumulado por el jugador 2
        #region//Jugador 2
        foreach (Transform obj in Drop_Zones[3].transform)
            if (obj.GetComponent<Display_Card>().Card.card_type == "D") continue;
            else
                total_power_p2 += obj.GetComponent<Display_Card>().Card.current_power;
        
        
        foreach (Transform obj in Drop_Zones[5].transform)
            if (obj.GetComponent<Display_Card>().Card.card_type == "D") continue;
            else
                total_power_p2 += obj.GetComponent<Display_Card>().Card.current_power;
        
        
        foreach (Transform obj in Drop_Zones[4].transform)
            if (obj.GetComponent<Display_Card>().Card.card_type == "D") continue;
            else
                total_power_p2 += obj.GetComponent<Display_Card>().Card.current_power;

        #endregion
    
        //aqui se actualiza el texto
        total_power_p1_t.text = total_power_p1.ToString();
        total_power_p2_t.text = total_power_p2.ToString();


    }
    public void Grave_Image_Create(GameObject obj)
    {
        if (obj.GetComponent<Display_Card>().Card.player)
        { 

            if (Player1.faction == "CM" && Principal_Objects[4].transform.childCount == 0)
                Instantiate(Principal_Objects[6], Principal_Objects[4].transform);

            else if (Player1.faction != "CM" && Principal_Objects[4].transform.childCount == 0)
                Instantiate(Principal_Objects[7], Principal_Objects[4].transform);
        }
        else
        {

            if (Player2.faction == "CM" && Principal_Objects[5].transform.childCount == 0)
                Instantiate(Principal_Objects[6], Principal_Objects[5].transform);

            else if (Player2.faction != "CM" && Principal_Objects[5].transform.childCount == 0)
                Instantiate(Principal_Objects[7], Principal_Objects[5].transform);
        }
    }
    public void Grave_Add(GameObject obj)
    {
        if(obj.GetComponent<Display_Card>().Card.player)
        {
            Deck1.Grave.Add(obj.GetComponent<Display_Card>().Card);
        }
        else
        {
            Deck2.Grave.Add(obj.GetComponent<Display_Card>().Card);
        }
    }
}
