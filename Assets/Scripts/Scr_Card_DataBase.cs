using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scr_Card_DataBase : MonoBehaviour
{
    public static List<Scr_Card> Create_Deck(bool jugador,string faction)
    {
        if(faction == "C_and_R") 
        {
            List<Scr_Card> C_and_R_Deck = new List<Scr_Card>();
            C_and_R_Deck.Add(new Scr_Card("Comandante Cerebro Central", "L", jugador, Resources.Load<Sprite>("Spr_Comandante_Cerebro_Central"), 0, "L", "", "U"));
            C_and_R_Deck.Add(new Scr_Card("Tormenta de Plasma", "WM", jugador, Resources.Load<Sprite>("Spr_Tormenta_de_Plasma"), 0, "W", "Weather", "U"));
            C_and_R_Deck.Add(new Scr_Card("Neblina de Batalla", "WM", jugador, Resources.Load<Sprite>("Spr_Neblina_de_Batalla"), 0, "W", "Weather", "U"));
            C_and_R_Deck.Add(new Scr_Card("Viento Ácido", "WD", jugador, Resources.Load<Sprite>("Spr_Viento_Acido"), 0, "W", "Weather", "U"));
            C_and_R_Deck.Add(new Scr_Card("Lluvia de Misiles", "WS", jugador, Resources.Load<Sprite>("Spr_Lluvia_de_Misiles"), 0, "W", "Weather", "U"));
            C_and_R_Deck.Add(new Scr_Card("Dispersión de Niebla", "C", jugador, Resources.Load<Sprite>("Spr_Dispersion_de_Niebla"), 0, "W", "Clear", "U"));
            C_and_R_Deck.Add(new Scr_Card("Sobrecarga Electromagnética", "C", jugador, Resources.Load<Sprite>("Spr_Sobrecarga_Electromagnetica"), 0, "W", "Clear", "U"));
            C_and_R_Deck.Add(new Scr_Card("Contramedidas de Alta Tecnología", "C", jugador, Resources.Load<Sprite>("Spr_Contramedidas_de_Alta_Tecnologia"), 0, "W", "Clear", "U"));
            C_and_R_Deck.Add(new Scr_Card("Mejoras de Combate", "Rm", jugador, Resources.Load<Sprite>("Spr_Mejoras_de_Combate"), 0, "Rm", "Increase", "U"));
            C_and_R_Deck.Add(new Scr_Card("Potenciador de Largo Alcance", "Rd", jugador, Resources.Load<Sprite>("Spr_Potenciador_de_Largo_Alcance"), 0, "Rd", "Increase", "U"));
            C_and_R_Deck.Add(new Scr_Card("Escudo Energético Reforzado", "Rs", jugador, Resources.Load<Sprite>("Spr_Escudo_Energetico_Reforzado"), 0, "Rs", "Increase", "U"));
            C_and_R_Deck.Add(new Scr_Card("Señuelo Táctico", "D", jugador, Resources.Load<Sprite>("Spr_Senuelo_Tactico"), 0, "Card", "Decoy", "U"));
            C_and_R_Deck.Add(new Scr_Card("Señuelo de Despliegue", "D", jugador, Resources.Load<Sprite>("Spr_Senuelo_de_Despliegue"), 0, "Card", "Decoy", "U"));
            C_and_R_Deck.Add(new Scr_Card("Señuelo de Engaño", "D", jugador, Resources.Load<Sprite>("Spr_Senuelo_de_Engano"), 0, "Card", "Decoy", "U"));
            C_and_R_Deck.Add(new Scr_Card("Soldado de Asalto Cibernético", "U", jugador, Resources.Load<Sprite>("Spr_Soldado_de_Asalto_Cibernetico"), 3, "M", "", "U"));
            C_and_R_Deck.Add(new Scr_Card("Francotirador de Precisión Óptica", "U", jugador, Resources.Load<Sprite>("Spr_Francotirador_de_Precision_Optica"), 4, "D", "", "U"));
            C_and_R_Deck.Add(new Scr_Card("Tanque de Asedio Blindado", "U", jugador, Resources.Load<Sprite>("Spr_Tanque_de_Asedio_Blindado"), 5, "S", "", "U"));
            C_and_R_Deck.Add(new Scr_Card("Escuadrón de Infiltración Nanotecnológica", "U", jugador, Resources.Load<Sprite>("Spr_Escuadron_de_Infiltracion_Nanotecnologica"), 2, "M", "", "U"));
            C_and_R_Deck.Add(new Scr_Card("Artillero de Plasma Pesado", "U", jugador, Resources.Load<Sprite>("Spr_Artillero_de_Plasma_Pesado"), 4, "D", "", "U"));
            C_and_R_Deck.Add(new Scr_Card("Dron de Ataque Aéreo", "U", jugador, Resources.Load<Sprite>("Spr_Dron_de_Ataque_Aereo"), 3, "D", "", "U"));
            C_and_R_Deck.Add(new Scr_Card("Unidad de Defensa Cibernética", "U", jugador, Resources.Load<Sprite>("Spr_Unidad_de_Defensa_Cibernetica"), 3, "S", "", "U"));
            C_and_R_Deck.Add(new Scr_Card("Titán de Batalla Mejorado", "U", jugador, Resources.Load<Sprite>("Spr_Titan_de_Batalla_Mejorado"), 10, "M", "Duplicate_Power", "G"));
            C_and_R_Deck.Add(new Scr_Card("Centinela de Vanguardia", "U", jugador, Resources.Load<Sprite>("Spr_Centinela_de_Vanguardia"), 7, "S", "Get_Card", "S"));
            C_and_R_Deck.Add(new Scr_Card("Vanguardia Táctica", "U", jugador, Resources.Load<Sprite>("Spr_Vanguardia_Tactica"), 6, "D", "Destroy_Worst_Enemy_Card", "S"));
            C_and_R_Deck.Add(new Scr_Card("Vanguardia Táctica", "U", jugador, Resources.Load<Sprite>("Spr_Vanguardia_Tactica"), 6, "D", "Destroy_Worst_Enemy_Card", "S"));
            C_and_R_Deck.Add(new Scr_Card("Cazador de Objetivos", "U", jugador, Resources.Load<Sprite>("Spr_Cazador_de_Objetivos"), 8, "D", "Destroy_Worst_Card", "S"));
            C_and_R_Deck.Add(new Scr_Card("Cazador de Objetivos", "U", jugador, Resources.Load<Sprite>("Spr_Cazador_de_Objetivos"), 8, "D", "Destroy_Worst_Card", "S"));


            return C_and_R_Deck;
        }
        
        else
        {
                List<Scr_Card> CM_Deck = new List<Scr_Card>();
                CM_Deck.Add(new Scr_Card("Rey Caballero Arthur IV", "L", jugador, Resources.Load<Sprite>("Spr_Rey_Caballero_Arthur_IV"), 0, "L", "", "U"));
                CM_Deck.Add(new Scr_Card("Niebla Espesa", "WM", jugador, Resources.Load<Sprite>("Spr_Niebla_Espesa"), 0, "W", "Weather", "U"));
                CM_Deck.Add(new Scr_Card("Lluvia de Flechas", "WM", jugador, Resources.Load<Sprite>("Spr_Lluvia_de_Flechas"), 0, "W", "Weather", "U"));
                CM_Deck.Add(new Scr_Card("ToRmenta de Truenos", "WS", jugador, Resources.Load<Sprite>("Spr_Tormenta_de_Truenos"), 0, "W", "Weather", "U"));
                CM_Deck.Add(new Scr_Card("Bruma Matinal", "WS", jugador, Resources.Load<Sprite>("Spr_Bruma_Matinal"), 0, "W", "Weather", "U"));
                CM_Deck.Add(new Scr_Card("Emboscada Relámpago", "C", jugador, Resources.Load<Sprite>("Spr_Emboscada_Relampago"), 0, "W", "Clear", "U"));
                CM_Deck.Add(new Scr_Card("Avance Estratégico", "C", jugador, Resources.Load<Sprite>("Spr_Avance_Estrategico"), 0, "W", "Clear", "U"));
                CM_Deck.Add(new Scr_Card("Asalto Certero", "C", jugador, Resources.Load<Sprite>("Spr_Asalto_Certero"), 0, "W", "Clear", "U"));
                CM_Deck.Add(new Scr_Card("Grito de Batalla", "Rm", jugador, Resources.Load<Sprite>("Spr_Grito_de_Batalla"), 0, "Rm", "Increase", "U"));
                CM_Deck.Add(new Scr_Card("Bendición_del_Clero", "Rd", jugador, Resources.Load<Sprite>("Spr_Bendicion_del_Clero"), 0, "Rd", "Increase", "U"));
                CM_Deck.Add(new Scr_Card("Estrategia de Vanguardia", "Rs", jugador, Resources.Load<Sprite>("Spr_Estrategia_de_Vanguardia"), 0, "Rs", "Increase", "U"));
                CM_Deck.Add(new Scr_Card("Caballero_Fantasma", "D", jugador, Resources.Load<Sprite>("Spr_Caballero_Fantasma"), 0, "Card", "Decoy", "U"));
                CM_Deck.Add(new Scr_Card("Arquero Emboscado", "D", jugador, Resources.Load<Sprite>("Spr_Arquero_Emboscado"), 0, "Card", "Decoy", "U"));
                CM_Deck.Add(new Scr_Card("Emboscada de la Vanguardia", "D", jugador, Resources.Load<Sprite>("Spr_Emboscada_de_la_Vanguardia"), 0, "Card", "Decoy", "U"));
                CM_Deck.Add(new Scr_Card("Caballero de la Mesa Redonda", "U", jugador, Resources.Load<Sprite>("Spr_Caballero_de_la_Mesa_Redonda"), 4, "M", "", "U"));
                CM_Deck.Add(new Scr_Card("Arquero de Sherwood", "U", jugador, Resources.Load<Sprite>("Spr_Arquero_de_Sherwood"), 3, "D", "", "U"));
                CM_Deck.Add(new Scr_Card("Catapulta de Asedio", "U", jugador, Resources.Load<Sprite>("Spr_Catapulta_de_Asedio"), 5, "S", "", "U"));
                CM_Deck.Add(new Scr_Card("Sacerdote Guerrero", "U", jugador, Resources.Load<Sprite>("Spr_Sacerdote_Guerrero"), 3, "M", "", "U"));
                CM_Deck.Add(new Scr_Card("Lancero de la Guardia Real", "U", jugador, Resources.Load<Sprite>("Spr_Lancero_de_la_Guardia_Real"), 4, "M", "", "U"));
                CM_Deck.Add(new Scr_Card("Caballería de Choque", "U", jugador, Resources.Load<Sprite>("Spr_Caballeria_de_Choque"), 5, "M", "", "U"));
                CM_Deck.Add(new Scr_Card("Ballestero Real", "U", jugador, Resources.Load<Sprite>("Spr_Ballestero_Real"), 4, "D", "", "U"));
                CM_Deck.Add(new Scr_Card("Rey Arturo", "U", jugador, Resources.Load<Sprite>("Spr_Rey_Arturo"), 9, "M", "Duplicate_Power", "G"));
                CM_Deck.Add(new Scr_Card("Caballero de la Guardia Real", "U", jugador, Resources.Load<Sprite>("Spr_Caballero_de_la_Guardia_Real"), 5, "M", "Beehive", "S"));
                CM_Deck.Add(new Scr_Card("Caballero de la Guardia Real", "U", jugador, Resources.Load<Sprite>("Spr_Caballero_de_la_Guardia_Real"), 5, "M", "Beehive", "S"));
                CM_Deck.Add(new Scr_Card("Caballero de la Guardia Real", "U", jugador, Resources.Load<Sprite>("Spr_Caballero_de_la_Guardia_Real"), 5, "M", "Beehive", "S"));
                CM_Deck.Add(new Scr_Card("Arquero de Élite", "U", jugador, Resources.Load<Sprite>("Spr_Arquero_de_Elite"), 6, "D", "Destroy_Best_Card", "S"));
                CM_Deck.Add(new Scr_Card("Cruzado Templario", "U", jugador, Resources.Load<Sprite>("Spr_Cruzado_Templario"), 7, "M", "Destroy_Raw", "S"));


                return CM_Deck;
        }

    }

    

}
