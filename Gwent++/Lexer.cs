/*
    preguntar a camilo si el amount que sale en el ejemplo es un identifier o un token propio del dsl
    preguntar q hacer con el int que esta en int i
*/

using System.Text.RegularExpressions;
namespace Interpreter;


    public class Lexer 
    {
        public enum TokenType
        {
            // Keywords
            Effect,Card,Name,Params,Action,Type,Faction,Power,Range,OnActivation,EffectKeyword,Selector,PostAction,
            Source,Single,Predicate,Amount,
            // Data Types
            NumberType,StringType,BooleanType,IntType,
            // Symbols
            OpenParen,CloseParen,OpenBrace,CloseBrace,OpenBracket,CloseBracket,Colon,Dot,Comma,Equals,
            Arrow,LessThan,BiggerThan,LessorEqualThan,BiggerorEqualThan,
            // Identifiers and Literals
            Identifier,Number,String,Boolean,
            // Whitespace
            Whitespace,
            //ContractedOperator
            ContractOperator,
            //Operator
            Operator,
            //Loops
            For,While,
            // Unknown
            Unknown,
            //Comments
            CommentLine,
            MultiLineComment
    
        }
        public Dictionary<TokenType,string> Tokens_RegexD = new Dictionary<TokenType,string>
        {
            //DSL 
            { TokenType.Effect,@"\beffect\b"},
            { TokenType.Card, @"\bcard\b"},
            { TokenType.Name, @"\bName\b"},
            { TokenType.Params, @"\bParams\b" },
            { TokenType.Action, @"\bAction\b"},
            { TokenType.Type, @"\bType\b"},
            { TokenType.Faction,@"\bFaction\b"},
            { TokenType.Power, @"\bPower\b" },
            { TokenType.Range, @"\bRange\b" },
            { TokenType.OnActivation, @"\bOnActivation\b" },
            { TokenType.EffectKeyword, @"\bEffect\b" },
            { TokenType.Selector, @"\bSelector\b"},
            { TokenType.PostAction, @"\bPostAction\b" },
            { TokenType.Source, @"\bSource\b" },
            { TokenType.Single, @"\bSingle\b" },
            { TokenType.Predicate, @"\bPredicate\b" },
            { TokenType.Amount, @"\bAmount\b" },
            // Comments
            { TokenType.MultiLineComment, "/\\*([^*]|[\r\n]|(\\*+([^*/])))*\\*+/" },
            { TokenType.CommentLine, "//.*?\\n" },
            // Numbers
            { TokenType.Number, @"\b\d+(\.\d+)?\b" },
            //Int type
            { TokenType.IntType, @"\bint\b" },
            // Strings (double-quoted)
            { TokenType.String, @"""[^""]*""" },
            // Data Types
            { TokenType.NumberType, @"\bNumber\b" },
            { TokenType.StringType, @"\bString\b" },
            { TokenType.BooleanType,@"\bBoolean\b" },
            // Symbols
            { TokenType.OpenParen, @"\(" },
            { TokenType.CloseParen, @"\)" },
            { TokenType.OpenBrace, @"\{" },
            { TokenType.CloseBrace, @"\}" },
            { TokenType.OpenBracket, @"\[" },
            { TokenType.CloseBracket, @"\]" },
            { TokenType.Arrow, @"=>"},
            { TokenType.Colon, @":" },
            { TokenType.Comma, @"," },
            { TokenType.Dot, @"\." },
            { TokenType.Equals, @"=" },
            { TokenType.LessThan, "<" },
            { TokenType.BiggerThan, ">" },
            { TokenType.LessorEqualThan, "<=" },
            { TokenType.BiggerorEqualThan, ">=" },   
            //Loops
            { TokenType.For, @"\bfor\b"},
            { TokenType.While, @"\bwhile\b"},
            // Booleans
            { TokenType.Boolean, @"\b(true|false)\b" },
            // Whitespace
            { TokenType.Whitespace, @"\s+"},
            //Contract Operator 
            { TokenType.ContractOperator, @"\+\+|--|\+=|-=|/=|\*=" },
            //Operator
            {TokenType.Operator, @"[+\-*/]" },
            // Identifiers
            { TokenType.Identifier, @"[a-zA-Z_][\w]*"},
            //Unknown
            { TokenType.Unknown, "[^ \\t\\r\\n]*" }
        };
        private string input_string;
        Regex Tokens_Regex;
        public Lexer (string input_string)
        {
            //create a regular expression for every pattern
            string pattern = string.Join("|", Tokens_RegexD.Select(kv => $"(?<{kv.Key}>{kv.Value})"));//grouping all the strings(separeting them by |) to get the regular expressions  
            Tokens_Regex = new Regex(pattern, RegexOptions.Compiled | RegexOptions.Singleline);//creating the regular expression
            this.input_string = input_string;
        }

        public List<Token> Tokenize()
        {
            
            //create the list to store the tokens  
            List<Token> tokens = new List<Token>();
            var matches = Tokens_Regex.Matches(input_string); 
            
            int row = 1;
            int column = 1;

            foreach (Match match in matches)
            {
                foreach (var groupName in Tokens_RegexD.Keys.Select(x => x.ToString()))
                {
                    if (match.Groups[groupName].Success)
                    {
                        TokenType tokenType = (TokenType)Enum.Parse(typeof(TokenType), groupName);//here we do some cast to get the tokentype found
                        string value = match.Value;

                        // Calculate the token position
                        int tokenRow = row;
                        int tokenColumn = column;

                        // Update the current row and column after matching the last token
                        foreach (char c in value)
                        {
                            if (c == '\n')
                            {
                                row++;
                                column = 1;
                            }
                            else
                                column++;
                        }
                        if(tokenType != TokenType.Whitespace)
                            tokens.Add(new Token(tokenType, value, tokenRow, tokenColumn));
                        break;
                    }
                }
            }  
            return tokens;
        }
    

    }
    public class Token
    {
        public Lexer.TokenType Type{get;set;}
        public string Value{get;set;}
        public int Row;
        public int Column;
        public Token(Lexer.TokenType type, string value, int row, int column)//Constructor
        {
            this.Type = type;
            this.Value = value;
            this.Column = column;
            this.Row = row; 
        }
        public override string ToString()//this is an override of ToString method to be able to show in Console all the information of a token and then test the lexer
        {
             return $"{Type}: {Value} (Row: {Row}, Column: {Column})";
        }
    }

   




