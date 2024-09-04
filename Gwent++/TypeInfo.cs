using System.Collections.Generic;

using interpreter ;
    public abstract class TypeInfo
    {
        public string? Name{get;set;}
        public Dictionary<string,string>? Properties;

    }
    public class CardInfo: TypeInfo
    {
        public CardInfo(string Name)
        {
            this.Name = Name;
            Properties = new Dictionary<string, string>();
            Properties["Type"] = "string";
            Properties["Name"] = "string";
            Properties["Faction"] = "string";
            Properties["Power"] = "double";
            Properties["Range"] = "List<string>";
            Properties["Owner"] = "player";
        }
    }
    public class ContextInfo: TypeInfo
    {
        public ContextInfo(string Name)
        {
            this.Name = Name;
            Properties = new Dictionary<string, string>();
            Properties["TriggerPlayer"] = "Player";
            Properties["Board"] = "List<card>";
            Properties["HandOfPlayer"] = "List<card>";
            Properties["FieldOfPlayer"] = "List<card>";
            Properties["GraveyardOfPlayer"] = "List<card>";
            Properties["DeckOfPlayer"] = "List<card>";
            Properties["Hand"] = "List<card>";
            Properties["Field"] = "List<card>";
            Properties["Graveyard"] = "List<card>";
            Properties["Deck"] = "List<card>";
            Properties["OtherDeck"] = "List<card>";
            Properties["OtherField"] = "List<card>";
            Properties["OtherGraveyard"] ="List<card>";
            Properties["OtherHand"] ="List<card>";
        }
    }