using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Scr_Deck: MonoBehaviour
{
    public Sprite Deck_Image;
    public Sprite Grave_Image;
    public List<Scr_Card> Deck;
    public List<Scr_Card> Grave;
    public Transform Leader_Zone;
    public Transform Hand_Zone;


    public GameObject Prefab_Card;
    public GameObject Prefab_Leader;


    private void Awake()
    {
        Grave = new List<Scr_Card>();   
    }

    void Instantiate_Leader()//Leader Card Creation and Display 
    {
        GameObject Leader_Instance = Instantiate(Prefab_Leader, Leader_Zone);
        Display_Card Leader_Display = Leader_Instance.GetComponent<Display_Card>();
        Leader_Display.Card = Deck[0];
        Leader_Display.Image = Leader_Instance.transform.GetChild(0).GetComponent<Image>();
        Leader_Display.Current_Power = Leader_Instance.transform.GetChild(1).GetComponent<TextMeshProUGUI>();

        Deck.RemoveAt(0);
    }

    public void Instantiate_Card(int n)//Card Creation and Display 
    {
        if (n == 0) return;

        if(Deck.Count > 0 && Hand_Zone.childCount<=9) {

            GameObject Card_Instance = Instantiate(Prefab_Card, Hand_Zone);
            Display_Card Card_Display = Card_Instance.GetComponent<Display_Card>();
            Card_Display.Card = Deck[Deck.Count-1];
            Card_Display.Image = Card_Instance.transform.GetChild(0).GetComponent<Image>();
            Card_Display.Current_Power = Card_Instance.transform.GetChild(1).GetComponent<TextMeshProUGUI>();

            Deck.RemoveAt(Deck.Count - 1);
        }
        else if(Hand_Zone.childCount>9)//enviar al cementerio 
        {
            Grave.Add(Deck[Deck.Count - 1]);
            Deck.RemoveAt(Deck.Count - 1);
        }

        n--;
        Instantiate_Card(n);
        
    }

    public void Deck_Order_Randomizer()
    {
        Instantiate_Leader();
        System.Random rand = new System.Random();

        for(int i=0; i<Deck.Count; i++)
        {
            int k = rand.Next(Deck.Count-1);
            Scr_Card temp = Deck[k];
            Deck[k] = Deck[i];
            Deck[i] = temp;
;       }
        
        Instantiate_Card(10);

    }


}
