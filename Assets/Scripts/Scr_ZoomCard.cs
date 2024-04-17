using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;

public class Scr_ZoomCard : MonoBehaviour
{
    public GameObject Card_prefab;
    GameObject Zoomed_Card;
    public void Pointer_on_Card()
    {
        GameObject Zoom_Zone = GameObject.Find("Zoom_Card_Zone");
        Zoomed_Card = Instantiate(Card_prefab, Zoom_Zone.transform);
        Zoomed_Card.transform.position = new Vector3(Zoom_Zone.transform.position.x,Zoom_Zone.transform.position.y,Zoom_Zone.transform.position.z);
        Zoomed_Card.transform.localScale = new Vector2(2.5f,2.6f);
        Display_Card disp = Zoomed_Card.GetComponent<Display_Card>();
        disp.Card = gameObject.GetComponent<Display_Card>().Card;
        disp.Image = gameObject.GetComponent<Display_Card>().Image;
        disp.Current_Power = gameObject.GetComponent<Display_Card>().Current_Power;

    }

    public void Pointer_out_Card()
    {
        Destroy(Zoomed_Card);
    }

}
