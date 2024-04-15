using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scr_Card_DataBase : MonoBehaviour
{
    public static List<Scr_Card> Create_Deck(string faction)
    {
        if(faction == "C_and_R") 
        {
            List<Scr_Card> C_and_R_Deck = new List<Scr_Card>();
            C_and_R_Deck.Add(new Scr_Card("Comandante Cerebro Central", "L", Resources.Load<Sprite>("Spr_Comandante_Cerebro_Central"), 0, "", ""));
            C_and_R_Deck.Add(new Scr_Card("Tormenta de Plasma", "W", Resources.Load<Sprite>("Spr_Tormenta_de_Plasma"), 0, "Weather_Zone", ""));
            C_and_R_Deck.Add(new Scr_Card("Neblina de Batalla", "W", Resources.Load<Sprite>("Spr_Neblina_de_Batalla"), 0, "Weather_Zone", ""));
            C_and_R_Deck.Add(new Scr_Card("Viento Ácido", "W", Resources.Load<Sprite>("Spr_Viento_Acido"), 0, "Weather_Zone", ""));
            C_and_R_Deck.Add(new Scr_Card("Lluvia de Misiles", "W", Resources.Load<Sprite>("Spr_Lluvia_de_Misiles"), 0, "Weather_Zone", ""));
            C_and_R_Deck.Add(new Scr_Card("Dispersión de Niebla", "W", Resources.Load<Sprite>("Spr_Dispersion_de_Niebla"), 0, "Weather_Zone", ""));
            C_and_R_Deck.Add(new Scr_Card("Sobrecarga Electromagnética", "W", Resources.Load<Sprite>("Spr_Sobrecarga_Electromagnetica"), 0, "Weather_Zone", ""));
            C_and_R_Deck.Add(new Scr_Card("Contramedidas de Alta Tecnología", "W", Resources.Load<Sprite>("Spr_Contramedidas_de_Alta_Tecnologia"), 0, "Weather_Zone", ""));
            C_and_R_Deck.Add(new Scr_Card("Mejoras de Combate", "W", Resources.Load<Sprite>("Spr_Mejoras_de_Combate"), 0, "Raise_Zone_1", ""));
            C_and_R_Deck.Add(new Scr_Card("Potenciador de Largo Alcance", "W", Resources.Load<Sprite>("Spr_Potenciador_de_Largo_Alcance"), 0, "Raise_Zone_2", ""));
            C_and_R_Deck.Add(new Scr_Card("Escudo Energético Reforzado", "W", Resources.Load<Sprite>("Spr_Escudo_Energetico_Reforzado"), 0, "Raise_Zone_3", ""));
            C_and_R_Deck.Add(new Scr_Card("Señuelo Táctico", "W", Resources.Load<Sprite>("Spr_Senuelo_Tactico"), 0, "", ""));
            C_and_R_Deck.Add(new Scr_Card("Señuelo de Despliegue", "D", Resources.Load<Sprite>("Spr_Senuelo_de_Despliegue"), 0, "", ""));
            C_and_R_Deck.Add(new Scr_Card("Señuelo de Engaño", "D", Resources.Load<Sprite>("Spr_Senuelo_de_Engano"), 0, "", ""));
            C_and_R_Deck.Add(new Scr_Card("Soldado de Asalto Cibernético", "U", Resources.Load<Sprite>("Spr_Soldado_de_Asalto_Cibernetico"), 3, "Melee_Zone", ""));
            C_and_R_Deck.Add(new Scr_Card("Francotirador de Precisión Óptica", "U", Resources.Load<Sprite>("Spr_Francotirador_de_Precision_Optica"), 4, "Distance_Zone", ""));
            C_and_R_Deck.Add(new Scr_Card("Tanque de Asedio Blindado", "U", Resources.Load<Sprite>("Spr_Tanque_de_Asedio_Blindado"), 5, "Siege_Zone", ""));
            C_and_R_Deck.Add(new Scr_Card("Escuadrón de Infiltración Nanotecnológica", "U", Resources.Load<Sprite>("Spr_Escuadron_de_Infiltracion_Nanotecnologica"), 2, "Melee_Zone", ""));
            C_and_R_Deck.Add(new Scr_Card("Artillero de Plasma Pesado", "U", Resources.Load<Sprite>("Spr_Artillero_de_Plasma_Pesado"), 4, "Distance_Zone", ""));
            C_and_R_Deck.Add(new Scr_Card("Dron de Ataque Aéreo", "U", Resources.Load<Sprite>("Spr_Dron_de_Ataque_Aereo"), 3, "Distance_Zone", ""));
            C_and_R_Deck.Add(new Scr_Card("Unidad de Defensa Cibernética", "U", Resources.Load<Sprite>("Spr_Unidad_de_Defensa_Cibernetica"), 3, "Siege_Zone", ""));
            C_and_R_Deck.Add(new Scr_Card("Titán de Batalla Mejorado", "U", Resources.Load<Sprite>("Spr_Titan_de_Batalla_Mejorado"), 10, "Melee_Zone", ""));
            C_and_R_Deck.Add(new Scr_Card("Centinela de Vanguardia", "U", Resources.Load<Sprite>("Spr_Centinela_de_Vanguardia"), 7, "Siege_Zone", ""));
            C_and_R_Deck.Add(new Scr_Card("Vanguardia Táctica", "U", Resources.Load<Sprite>("Spr_Vanguardia_Tactica"), 6, "Distance_Zone", ""));
            C_and_R_Deck.Add(new Scr_Card("Cazador de Objetivos", "U", Resources.Load<Sprite>("Spr_Cazador_de_Objetivos"), 8, "Distance_Zone", ""));


            return C_and_R_Deck;
        }
        
        else
        {
                List<Scr_Card> CM_Deck = new List<Scr_Card>();
                CM_Deck.Add(new Scr_Card("Rey Caballero Arthur IV", "L", Resources.Load<Sprite>("Spr_Rey_Caballero_Arthur_IV"), 0, "", ""));
                CM_Deck.Add(new Scr_Card("Niebla Espesa", "W", Resources.Load<Sprite>("Spr_Niebla_Espesa"), 0, "Weather_Zone", ""));
                CM_Deck.Add(new Scr_Card("Lluvia de Flechas", "W", Resources.Load<Sprite>("Spr_Lluvia_de_Flechas"), 0, "Weather_Zone", ""));
                CM_Deck.Add(new Scr_Card("Tormenta de Truenos", "W", Resources.Load<Sprite>("Spr_Tormenta_de_Truenos"), 0, "Weather_Zone", ""));
                CM_Deck.Add(new Scr_Card("Bruma Matinal", "W", Resources.Load<Sprite>("Spr_Bruma_Matinal"), 0, "Weather_Zone", ""));
                CM_Deck.Add(new Scr_Card("Emboscada Relámpago", "W", Resources.Load<Sprite>("Spr_Emboscada_Relampago"), 0, "Weather_Zone", ""));
                CM_Deck.Add(new Scr_Card("Avance Estratégico", "W", Resources.Load<Sprite>("Spr_Avance_Estrategico"), 0, "Weather_Zone", ""));
                CM_Deck.Add(new Scr_Card("Asalto Certero", "W", Resources.Load<Sprite>("Spr_Asalto_Certero"), 0, "Weather_Zone", ""));
                CM_Deck.Add(new Scr_Card("Grito de Batalla", "W", Resources.Load<Sprite>("Spr_Grito_de_Batalla"), 0, "Raise_Zone_1", ""));
                CM_Deck.Add(new Scr_Card("Bendición_del_Clero", "W", Resources.Load<Sprite>("Spr_Bendicion_del_Clero"), 0, "Raise_Zone_2", ""));
                CM_Deck.Add(new Scr_Card("Estrategia de Vanguardia", "W", Resources.Load<Sprite>("Spr_Estrategia_de_Vanguardia"), 0, "Raise_Zone_3", ""));
                CM_Deck.Add(new Scr_Card("Caballero_Fantasma", "D", Resources.Load<Sprite>("Spr_Caballero_Fantasma"), 0, "", ""));
                CM_Deck.Add(new Scr_Card("Arquero Emboscado", "D", Resources.Load<Sprite>("Spr_Arquero_Emboscado"), 0, "", ""));
                CM_Deck.Add(new Scr_Card("Emboscada de la Vanguardia", "D", Resources.Load<Sprite>("Spr_Emboscada_de_la_Vanguardia"), 0, "", ""));
                CM_Deck.Add(new Scr_Card("Caballero de la Mesa Redonda", "U", Resources.Load<Sprite>("Spr_Caballero_de_la_Mesa_Redonda"), 4, "Melee_Zone", ""));
                CM_Deck.Add(new Scr_Card("Arquero de Sherwood", "U", Resources.Load<Sprite>("Spr_Arquero_de_Sherwood"), 3, "Distance_Zone", ""));
                CM_Deck.Add(new Scr_Card("Catapulta de Asedio", "U", Resources.Load<Sprite>("Spr_Catapulta_de_Asedio"), 5, "Siege_Zone", ""));
                CM_Deck.Add(new Scr_Card("Sacerdote Guerrero", "U", Resources.Load<Sprite>("Spr_Sacerdote_Guerrero"), 3, "Melee_Zone", ""));
                CM_Deck.Add(new Scr_Card("Lancero de la Guardia Real", "U", Resources.Load<Sprite>("Spr_Lancero_de_la_Guardia_Real"), 4, "Melee_Zone", ""));
                CM_Deck.Add(new Scr_Card("Caballería de Choque", "U", Resources.Load<Sprite>("Spr_Caballeria_de_Choque"), 5, "Melee_Zone", ""));
                CM_Deck.Add(new Scr_Card("Ballestero Real", "U", Resources.Load<Sprite>("Spr_Ballestero_Real"), 4, "Distance_Zone", ""));
                CM_Deck.Add(new Scr_Card("Rey Arturo", "U", Resources.Load<Sprite>("Spr_Rey_Arturo"), 9, "Melee_Zone", ""));
                CM_Deck.Add(new Scr_Card("Caballero de la Guardia Real", "U", Resources.Load<Sprite>("Spr_Caballero_de_la_Guardia_Real"), 7, "Melee_Zone", ""));
                CM_Deck.Add(new Scr_Card("Arquero de Élite", "U", Resources.Load<Sprite>("Spr_Arquero_de_Elite"), 6, "Distance_Zone", ""));
                CM_Deck.Add(new Scr_Card("Cruzado Templario", "U", Resources.Load<Sprite>("Spr_Cruzado_Templario"), 7, "Melee_Zone", ""));


                return CM_Deck;
        }

    }

    

}
