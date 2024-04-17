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
            C_and_R_Deck.Add(new Scr_Card("Comandante Cerebro Central", "L", jugador, Resources.Load<Sprite>("Spr_Comandante_Cerebro_Central"), 0, "L", ""));
            C_and_R_Deck.Add(new Scr_Card("Tormenta de Plasma", "W", jugador, Resources.Load<Sprite>("Spr_Tormenta_de_Plasma"), 0, "W", ""));
            C_and_R_Deck.Add(new Scr_Card("Neblina de Batalla", "W", jugador, Resources.Load<Sprite>("Spr_Neblina_de_Batalla"), 0, "W", ""));
            C_and_R_Deck.Add(new Scr_Card("Viento Ácido", "W", jugador, Resources.Load<Sprite>("Spr_Viento_Acido"), 0, "W", ""));
            C_and_R_Deck.Add(new Scr_Card("Lluvia de Misiles", "W", jugador, Resources.Load<Sprite>("Spr_Lluvia_de_Misiles"), 0, "W", ""));
            C_and_R_Deck.Add(new Scr_Card("Dispersión de Niebla", "W", jugador, Resources.Load<Sprite>("Spr_Dispersion_de_Niebla"), 0, "W", ""));
            C_and_R_Deck.Add(new Scr_Card("Sobrecarga Electromagnética", "W", jugador, Resources.Load<Sprite>("Spr_Sobrecarga_Electromagnetica"), 0, "W", ""));
            C_and_R_Deck.Add(new Scr_Card("Contramedidas de Alta Tecnología", "W", jugador, Resources.Load<Sprite>("Spr_Contramedidas_de_Alta_Tecnologia"), 0, "W", ""));
            C_and_R_Deck.Add(new Scr_Card("Mejoras de Combate", "R", jugador, Resources.Load<Sprite>("Spr_Mejoras_de_Combate"), 0, "Rm", ""));
            C_and_R_Deck.Add(new Scr_Card("Potenciador de Largo Alcance", "R", jugador, Resources.Load<Sprite>("Spr_Potenciador_de_Largo_Alcance"), 0, "Rd", ""));
            C_and_R_Deck.Add(new Scr_Card("Escudo Energético Reforzado", "R", jugador, Resources.Load<Sprite>("Spr_Escudo_Energetico_Reforzado"), 0, "Rs", ""));
            C_and_R_Deck.Add(new Scr_Card("Señuelo Táctico", "D", jugador, Resources.Load<Sprite>("Spr_Senuelo_Tactico"), 0, "MDS", ""));
            C_and_R_Deck.Add(new Scr_Card("Señuelo de Despliegue", "D", jugador, Resources.Load<Sprite>("Spr_Senuelo_de_Despliegue"), 0, "MDS", ""));
            C_and_R_Deck.Add(new Scr_Card("Señuelo de Engaño", "D", jugador, Resources.Load<Sprite>("Spr_Senuelo_de_Engano"), 0, "MDS", ""));
            C_and_R_Deck.Add(new Scr_Card("Soldado de Asalto Cibernético", "U", jugador, Resources.Load<Sprite>("Spr_Soldado_de_Asalto_Cibernetico"), 3, "M", ""));
            C_and_R_Deck.Add(new Scr_Card("Francotirador de Precisión Óptica", "U", jugador, Resources.Load<Sprite>("Spr_Francotirador_de_Precision_Optica"), 4, "D", ""));
            C_and_R_Deck.Add(new Scr_Card("Tanque de Asedio Blindado", "U", jugador, Resources.Load<Sprite>("Spr_Tanque_de_Asedio_Blindado"), 5, "S", ""));
            C_and_R_Deck.Add(new Scr_Card("Escuadrón de Infiltración Nanotecnológica", "U", jugador, Resources.Load<Sprite>("Spr_Escuadron_de_Infiltracion_Nanotecnologica"), 2, "M", ""));
            C_and_R_Deck.Add(new Scr_Card("Artillero de Plasma Pesado", "U", jugador, Resources.Load<Sprite>("Spr_Artillero_de_Plasma_Pesado"), 4, "D", ""));
            C_and_R_Deck.Add(new Scr_Card("Dron de Ataque Aéreo", "U", jugador, Resources.Load<Sprite>("Spr_Dron_de_Ataque_Aereo"), 3, "D", ""));
            C_and_R_Deck.Add(new Scr_Card("Unidad de Defensa Cibernética", "U", jugador, Resources.Load<Sprite>("Spr_Unidad_de_Defensa_Cibernetica"), 3, "S", ""));
            C_and_R_Deck.Add(new Scr_Card("Titán de Batalla Mejorado", "U", jugador, Resources.Load<Sprite>("Spr_Titan_de_Batalla_Mejorado"), 10, "M", ""));
            C_and_R_Deck.Add(new Scr_Card("Centinela de Vanguardia", "U", jugador, Resources.Load<Sprite>("Spr_Centinela_de_Vanguardia"), 7, "S", ""));
            C_and_R_Deck.Add(new Scr_Card("Vanguardia Táctica", "U", jugador, Resources.Load<Sprite>("Spr_Vanguardia_Tactica"), 6, "D", ""));
            C_and_R_Deck.Add(new Scr_Card("Cazador de Objetivos", "U", jugador, Resources.Load<Sprite>("Spr_Cazador_de_Objetivos"), 8, "D", ""));


            return C_and_R_Deck;
        }
        
        else
        {
                List<Scr_Card> CM_Deck = new List<Scr_Card>();
                CM_Deck.Add(new Scr_Card("Rey Caballero Arthur IV", "L", jugador, Resources.Load<Sprite>("Spr_Rey_Caballero_Arthur_IV"), 0, "L", ""));
                CM_Deck.Add(new Scr_Card("Niebla Espesa", "W", jugador, Resources.Load<Sprite>("Spr_Niebla_Espesa"), 0, "W", ""));
                CM_Deck.Add(new Scr_Card("Lluvia de Flechas", "W", jugador, Resources.Load<Sprite>("Spr_Lluvia_de_Flechas"), 0, "W", ""));
                CM_Deck.Add(new Scr_Card("ToRmenta de Truenos", "W", jugador, Resources.Load<Sprite>("Spr_Tormenta_de_Truenos"), 0, "W", ""));
                CM_Deck.Add(new Scr_Card("Bruma Matinal", "W", jugador, Resources.Load<Sprite>("Spr_Bruma_Matinal"), 0, "W", ""));
                CM_Deck.Add(new Scr_Card("Emboscada Relámpago", "W", jugador, Resources.Load<Sprite>("Spr_Emboscada_Relampago"), 0, "W", ""));
                CM_Deck.Add(new Scr_Card("Avance Estratégico", "W", jugador, Resources.Load<Sprite>("Spr_Avance_Estrategico"), 0, "W", ""));
                CM_Deck.Add(new Scr_Card("Asalto Certero", "W", jugador, Resources.Load<Sprite>("Spr_Asalto_Certero"), 0, "W", ""));
                CM_Deck.Add(new Scr_Card("Grito de Batalla", "W", jugador, Resources.Load<Sprite>("Spr_Grito_de_Batalla"), 0, "Rm", ""));
                CM_Deck.Add(new Scr_Card("Bendición_del_Clero", "W", jugador, Resources.Load<Sprite>("Spr_Bendicion_del_Clero"), 0, "Rd", ""));
                CM_Deck.Add(new Scr_Card("Estrategia de Vanguardia", "W", jugador, Resources.Load<Sprite>("Spr_Estrategia_de_Vanguardia"), 0, "Rs", ""));
                CM_Deck.Add(new Scr_Card("Caballero_Fantasma", "D", jugador, Resources.Load<Sprite>("Spr_Caballero_Fantasma"), 0, "MDS", ""));
                CM_Deck.Add(new Scr_Card("Arquero Emboscado", "D", jugador, Resources.Load<Sprite>("Spr_Arquero_Emboscado"), 0, "MDS", ""));
                CM_Deck.Add(new Scr_Card("Emboscada de la Vanguardia", "D", jugador, Resources.Load<Sprite>("Spr_Emboscada_de_la_Vanguardia"), 0, "MDS", ""));
                CM_Deck.Add(new Scr_Card("Caballero de la Mesa Redonda", "U", jugador, Resources.Load<Sprite>("Spr_Caballero_de_la_Mesa_Redonda"), 4, "M", ""));
                CM_Deck.Add(new Scr_Card("Arquero de Sherwood", "U", jugador, Resources.Load<Sprite>("Spr_Arquero_de_Sherwood"), 3, "D", ""));
                CM_Deck.Add(new Scr_Card("Catapulta de Asedio", "U", jugador, Resources.Load<Sprite>("Spr_Catapulta_de_Asedio"), 5, "S", ""));
                CM_Deck.Add(new Scr_Card("Sacerdote Guerrero", "U", jugador, Resources.Load<Sprite>("Spr_Sacerdote_Guerrero"), 3, "M", ""));
                CM_Deck.Add(new Scr_Card("Lancero de la Guardia Real", "U", jugador, Resources.Load<Sprite>("Spr_Lancero_de_la_Guardia_Real"), 4, "M", ""));
                CM_Deck.Add(new Scr_Card("Caballería de Choque", "U", jugador, Resources.Load<Sprite>("Spr_Caballeria_de_Choque"), 5, "M", ""));
                CM_Deck.Add(new Scr_Card("Ballestero Real", "U", jugador, Resources.Load<Sprite>("Spr_Ballestero_Real"), 4, "D", ""));
                CM_Deck.Add(new Scr_Card("Rey Arturo", "U", jugador, Resources.Load<Sprite>("Spr_Rey_Arturo"), 9, "M", ""));
                CM_Deck.Add(new Scr_Card("Caballero de la Guardia Real", "U", jugador, Resources.Load<Sprite>("Spr_Caballero_de_la_Guardia_Real"), 7, "M", ""));
                CM_Deck.Add(new Scr_Card("Arquero de Élite", "U", jugador, Resources.Load<Sprite>("Spr_Arquero_de_Elite"), 6, "D", ""));
                CM_Deck.Add(new Scr_Card("Cruzado Templario", "U", jugador, Resources.Load<Sprite>("Spr_Cruzado_Templario"), 7, "M", ""));


                return CM_Deck;
        }

    }

    

}
