using System.Collections;
using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks.Dataflow;

namespace Interpreter;

public class Parser
{
    public List<Token> Tokens{get;}
    public int position{set; get;}
    public Token CurrentToken {
        get {
            return position<Tokens.Count ? Tokens[position] : null!;
        }
        set {}
        }
    
    //constructor
    public Parser(List<Token> tokens)
    {
        this.Tokens = tokens;
        this.position = 0;
    }

    #region Metodos de la clase Parser
    
    public Token GetNextToken(int amount=1)
    {
        return position+amount < this.Tokens.Count ? this.Tokens[position+amount] : null;
    }
    public bool Match(Token token ,Lexer.TokenType expected_type)
    {
        if(token.Type == expected_type) return true;
        
        return false;
    }
    public bool Match(Token token, List<Lexer.TokenType> expected_types)
    {
        foreach(var type in expected_types) 
        {
            if(token.Type == type)return true;
        } 

        return false;
    }
    public Token Consume(Lexer.TokenType type)
    {
        if(CurrentToken == null )
            throw new Exception("No Token found");
         if(CurrentToken.Type == type)
        {
            position ++;
            return CurrentToken;
        }
        else 
            throw new Exception($"Invalid Token: {CurrentToken}. Expected {type}");
    
    }
    public bool IsBinary_Expr_Operator(Lexer.TokenType type)
    {
        return BinaryExpressionNode.Levels.ContainsKey(type);
    }
    public bool IsAssignment_Operator(Lexer.TokenType type)
    {
        if(type == Lexer.TokenType.Colon || type == Lexer.TokenType.Equals || type == Lexer.TokenType.MinusEqual || type == Lexer.TokenType.PlusEqual || type == Lexer.TokenType.MultiplyEqual || type == Lexer.TokenType.DivideEqual)
            return true;

        return false;
    }
    public bool IsBoolean_Operator(Lexer.TokenType type)
    {
        if(type == Lexer.TokenType.And || type == Lexer.TokenType.Or || type == Lexer.TokenType.LessThan || type == Lexer.TokenType.LessOrEqualThan
           || type == Lexer.TokenType.BiggerThan || type == Lexer.TokenType.BiggerOrEqualThan || type == Lexer.TokenType.EqualValue || type == Lexer.TokenType.NotEqualValue)
        {
            return true;
        } 
        return false;
    }
    public bool IsConcat_Operator(Lexer.TokenType type)
    {
        if(type == Lexer.TokenType.CompConcat || type == Lexer.TokenType.SimpleConcat)
            return true;
        return false;
    }
    public Lexer.TokenType GetSimpleOperator(Lexer.TokenType type)
    {
        switch (type)
        {
            case Lexer.TokenType.PlusEqual:
                return Lexer.TokenType.Plus;
            case Lexer.TokenType.MinusEqual:
                return Lexer.TokenType.Minus;
            case Lexer.TokenType.MultiplyEqual:
                return Lexer.TokenType.Multiply;
            case Lexer.TokenType.DivideEqual:
                return Lexer.TokenType.Divide;
            default:throw new Exception($"El token:{type} no es un contract_operator");

        }
    }


    #region Parsing Program
    public ProgramNode Parse_ProgramNode()
    {
        ProgramNode program = new ProgramNode();//aqui se crea el nodo programa que se va a rellenar durante el parseo 

        while(position < Tokens.Count)
        { 
            Token section = new Token(CurrentToken.Type,CurrentToken.Value,CurrentToken.Row,CurrentToken.Column);
            //Manejo de errores(los tokens correctos son Effect y/o Card)
            if(!Match(CurrentToken ,Lexer.TokenType.Effect) && !Match(CurrentToken ,Lexer.TokenType.Card))
            {
                throw new Exception ($"Invalid Token: {CurrentToken}. Expected Effect or Card ");
            }

            //instrucciones en casos de que haya recibido un effect o card
            else
            {
                if(section.Type == Lexer.TokenType.Effect)
                    program.Sections.Add(Parse_EffectNode());
                else if(section.Type == Lexer.TokenType.Card)
                    program.Sections.Add(Parse_CardNode());
            }
        }

        return program;
    }
    
    #endregion
    
    #region Parsing Effect
    public EffectNode Parse_EffectNode()
    {
        //efecto a devolver que se llena durante el parseo 
        EffectNode effect = new();
        Consume(Lexer.TokenType.Effect);
        Consume(Lexer.TokenType.OpenBrace);
        while(CurrentToken.Type != Lexer.TokenType.CloseBrace)
        {
            //el token siguiente es solo para saber en cual de las tres propiedades de efecto esperadas estoy (aunque puede no ser ninguna y reportar un error)
            //posibles casos
            if(CurrentToken.Type == Lexer.TokenType.Name || CurrentToken.Type == Lexer.TokenType.Params || CurrentToken.Type == Lexer.TokenType.Action)
            {
                //caso propiedad Name
                if (CurrentToken.Type == Lexer.TokenType.Name)
                {
                    Consume( Lexer.TokenType.Name);
                    Consume( Lexer.TokenType.Colon);
                    effect.Name = Parse_ExpressionNode();
                    
                }
                //caso propiedad Action
                else if (CurrentToken.Type == Lexer.TokenType.Action)
                {
                    Consume( Lexer.TokenType.Action);
                    Consume( Lexer.TokenType.Colon);
                    effect.Action = Parse_ActionBlockNode();
                }
                //caso propiedad Params
                //preguntar a esta gente aqui como se maneja el error d q nunca me cierren la llave del params
                else if (CurrentToken.Type == Lexer.TokenType.Params)
                {
                    Consume( Lexer.TokenType.Params);
                    Consume( Lexer.TokenType.Colon);
                    Consume( Lexer.TokenType.OpenBrace);
                    effect.Params = new List<AssignmentNode>();
                    while (CurrentToken.Type != Lexer.TokenType.CloseBrace)
                    {
                        effect.Params.Add(Parse_AssignmentNode());
                    }
                    Consume( Lexer.TokenType.CloseBrace);
                }

                if(CurrentToken.Type != Lexer.TokenType.CloseBrace)
                    Consume( Lexer.TokenType.Comma);
                
            }
            
            
            //caso que no me pongan ninguna de las propiedades esperadas
            else
                throw new Exception($"Token inesperado en los valores de Effect, se recibio {CurrentToken}");

        }

        Consume( Lexer.TokenType.CloseBrace);

        return effect;

    }
    public ActionBlockNode Parse_ActionBlockNode()
    {
        //se crea el nodo que se va a rellenar para luego retornar seguido del token que indica por cual token de la lista voy
        ActionBlockNode actionBlock = new ActionBlockNode();
        //se consumen todos los token esperados y se va almacenando 
        Consume(Lexer.TokenType.OpenParen);
        actionBlock.Targets.Identifier = Parse_ExpressionNode();
        Consume(Lexer.TokenType.Comma);
        actionBlock.Context.Identifier = Parse_ExpressionNode();
        Consume( Lexer.TokenType.CloseParen);
        Consume( Lexer.TokenType.Arrow);
        Consume( Lexer.TokenType.OpenBrace);
        while(CurrentToken.Type != Lexer.TokenType.CloseBrace && CurrentToken != null)
        {
            actionBlock.Statements.Add(Parse_StatementNode());
        }
        Consume(Lexer.TokenType.CloseBrace);

        return actionBlock;
    }
    
    #endregion

    #region Parsing Card
    public CardNode Parse_CardNode()
    {
        CardNode cardNode = new();
        List<Lexer.TokenType> cardTokenTypes = new();
        cardTokenTypes.Add(Lexer.TokenType.Name);
        cardTokenTypes.Add(Lexer.TokenType.Type);
        cardTokenTypes.Add(Lexer.TokenType.Faction);
        cardTokenTypes.Add(Lexer.TokenType.Power);
        cardTokenTypes.Add(Lexer.TokenType.Range);
        cardTokenTypes.Add(Lexer.TokenType.OnActivation);
        Consume(Lexer.TokenType.Card);
        Consume(Lexer.TokenType.OpenBrace);
        while(CurrentToken!=null && CurrentToken.Type!=Lexer.TokenType.CloseBrace)
        {
            //posibles casos
            if(Match(CurrentToken,cardTokenTypes))
            {
                //caso token Name
                if(Match(CurrentToken,Lexer.TokenType.Name))
                {
                    Consume(Lexer.TokenType.Name);
                    Consume(Lexer.TokenType.Colon);
                    cardNode.Name = Parse_ExpressionNode();
                }
                //caso token Type
                else if (Match(CurrentToken,Lexer.TokenType.Type))
                {
                    Consume(Lexer.TokenType.Type);
                    Consume(Lexer.TokenType.Colon);
                    cardNode.Type = Parse_ExpressionNode();   
                }
                //caso token Faction
                else if (Match(CurrentToken,Lexer.TokenType.Faction))
                {
                    Consume(Lexer.TokenType.Faction);
                    Consume(Lexer.TokenType.Colon);
                    cardNode.Faction = Parse_ExpressionNode();   
                }
                //caso token Power
                else if (Match(CurrentToken,Lexer.TokenType.Power))
                {
                    Consume(Lexer.TokenType.Power);
                    Consume(Lexer.TokenType.Colon);
                    cardNode.Power = Parse_ExpressionNode();   
                }
                //caso token Range
                else if (Match(CurrentToken,Lexer.TokenType.Range))
                {
                    Consume(Lexer.TokenType.Range);
                    Consume(Lexer.TokenType.Colon);
                    Consume(Lexer.TokenType.OpenBracket);
                    while(CurrentToken!=null && CurrentToken.Type!=Lexer.TokenType.CloseBracket)
                    {
                        cardNode.Range.Add(Parse_ExpressionNode());
                        if(CurrentToken.Type != Lexer.TokenType.CloseBracket)
                            Consume(Lexer.TokenType.Comma);
                    }
                    Consume(Lexer.TokenType.CloseBracket);
                }
                //caso token OnActivation
                else if (Match(CurrentToken,Lexer.TokenType.OnActivation))
                {
                    cardNode.OnActivation = Parse_OnActivationNode();
                }                


                if(CurrentToken.Type != Lexer.TokenType.CloseBrace)
                    Consume( Lexer.TokenType.Comma);
            }
            else 
                throw new Exception ($"Token inesperado en los valores de ParseCard, se recibio {CurrentToken}");
        }

        Consume(Lexer.TokenType.CloseBrace);
        return cardNode;
    }

    public OnActivationNode Parse_OnActivationNode()
    {
        OnActivationNode onActivationNode = new();
        Consume(Lexer.TokenType.OnActivation);
        Consume(Lexer.TokenType.Colon);
        Consume(Lexer.TokenType.OpenBracket);
        while(CurrentToken!=null && CurrentToken.Type != Lexer.TokenType.CloseBracket)
        {
            //annadir declaracion de efecto a la lista de activaciones
            onActivationNode.Activations.Add(Parse_EffectDeclarationNode());
            if(CurrentToken.Type != Lexer.TokenType.CloseBracket)
                Consume(Lexer.TokenType.Comma);
        }
        Consume(Lexer.TokenType.CloseBracket);

        return onActivationNode;


    }

    public EffectDeclarationNode Parse_EffectDeclarationNode(EffectDeclarationNode parent = null)
    {
        EffectDeclarationNode effectDeclarationNode = new EffectDeclarationNode(parent);

        //Azucar Sintactica (revisar el token siguiente para ver si es un string o una llave(caso que se parsea despues de esta condicional))
        if(GetNextToken(2).Type == Lexer.TokenType.String)
        {   
            Consume(Lexer.TokenType.EffectKeyword);
            Consume(Lexer.TokenType.Colon);
            effectDeclarationNode.Effect.Add(new AssignmentNode{Identifier = new IdentifierNode("Name"),Value = Parse_ExpressionNode()});
            Consume (Lexer.TokenType.Comma);
            return effectDeclarationNode;
        }

        //se espera token "{"
        Consume(Lexer.TokenType.OpenBrace);
        //a partir de aqui se esperan los valores de parseEffectDeclaration
        while(CurrentToken!=null && CurrentToken.Type != Lexer.TokenType.CloseBrace)
        {
            //verificar si el siguiente token es uno de los valores de parseEffectdDeclaration
            if (Match(CurrentToken,new List<Lexer.TokenType>(new[] {Lexer.TokenType.EffectKeyword,Lexer.TokenType.Selector,Lexer.TokenType.PostAction}) ))
            {
                //posibles casos
                //caso Effect
                if(Match(CurrentToken,Lexer.TokenType.EffectKeyword))
                {
                    Consume(Lexer.TokenType.EffectKeyword);
                    Consume(Lexer.TokenType.Colon);
                    Consume(Lexer.TokenType.OpenBrace);
                    while(CurrentToken!=null && CurrentToken.Type != Lexer.TokenType.CloseBrace)
                    {
                        effectDeclarationNode.Effect.Add(Parse_AssignmentNode()); 
                    }
                    Consume(Lexer.TokenType.CloseBrace);
                }
                //caso Selector
                else if(Match(CurrentToken,Lexer.TokenType.Selector))
                {
                    
                    effectDeclarationNode.Selector = Parse_SelectorNode();
                    //Consume(Lexer.TokenType.CloseBrace);
                }
                //caso PostAction
                else if (Match(CurrentToken,Lexer.TokenType.PostAction))
                {
                    Consume(Lexer.TokenType.PostAction);
                    Consume(Lexer.TokenType.Colon);
                    Consume(Lexer.TokenType.OpenBrace);
                    effectDeclarationNode.PostAction = Parse_EffectDeclarationNode(effectDeclarationNode);
                    Consume(Lexer.TokenType.CloseBrace);
                }

                if(CurrentToken.Type != Lexer.TokenType.CloseBrace)
                    Consume(Lexer.TokenType.Comma);
            }
            //en caso de no coincidir lanzar un error
            else 
            throw new Exception ($"Token inesperado en los valores de ParseEffectDeclaration, se recibio {CurrentToken}");
        }

        Consume(Lexer.TokenType.CloseBrace);
        return effectDeclarationNode;
    }

    public SelectorNode Parse_SelectorNode()
    {
        SelectorNode selectorNode = new();
        Consume(Lexer.TokenType.Selector);
        Consume(Lexer.TokenType.Colon);
        Consume(Lexer.TokenType.OpenBrace);

        while(CurrentToken != null && CurrentToken.Type != Lexer.TokenType.CloseBrace)
        {
            if(Match(CurrentToken, new List<Lexer.TokenType>(new[]{Lexer.TokenType.Source,Lexer.TokenType.Single,Lexer.TokenType.Predicate})))
            {
                //casos posibles
                //caso token Source 
                if(Match(CurrentToken,Lexer.TokenType.Source))
                {
                    Consume(Lexer.TokenType.Source);
                    Consume(Lexer.TokenType.Colon);
                    selectorNode.Source = Parse_ExpressionNode();
                }
                //caso token Single
                else if(Match(CurrentToken,Lexer.TokenType.Single))
                {
                    Consume(Lexer.TokenType.Single);
                    Consume(Lexer.TokenType.Colon);
                    selectorNode.Single = Parse_ExpressionNode();
                }
                //caso token Predicate
                else if (Match(CurrentToken,Lexer.TokenType.Predicate))
                {   
                    Consume(Lexer.TokenType.Predicate);
                    Consume(Lexer.TokenType.Colon);

                    PredicateExpressionNode predicate = new();
                    Consume(Lexer.TokenType.OpenParen);
                    predicate.Generic_Identifier = new IdentifierNode (CurrentToken.Value);
                    Consume(Lexer.TokenType.Identifier);
                    Consume(Lexer.TokenType.CloseParen);
                    Consume (Lexer.TokenType.Arrow);
                    Token possibleError = new Token(CurrentToken.Type,CurrentToken.Value,CurrentToken.Row,CurrentToken.Column);
                    predicate.Filter = Parse_ExpressionNode();
                    if(predicate.Filter is not BooleanBinaryExpressionNode && predicate.Filter is not BoolNode)
                        throw new Exception($"Esperaba una expresion booleana en el Predicate: {possibleError} y recibio expresion {predicate.Filter}");

                    selectorNode.Predicate = predicate;
                }

                //si al dar un valor no se recibe el token "}" es porque no se ha terminado de dar valores y se espera una coma para el siguiente o porque hay algun error
                if(CurrentToken.Type!=Lexer.TokenType.CloseBrace)
                    Consume(Lexer.TokenType.Comma);
            }
            else 
            throw new Exception ($"Token inesperado en los valores de ParseSelector, se recibio {CurrentToken}");
        }

        Consume(Lexer.TokenType.CloseBrace);
        return selectorNode;

    }

    #endregion
    
    #region Parsing Expressions
    public ExpressionNode Parse_ExpressionNode(int precedence = 0)
    {
        //nodo de Expression que se va a devolver
        ExpressionNode Left = Parse_PrimaryExpression(); 

        while(CurrentToken!=null && IsBinary_Expr_Operator(CurrentToken.Type) && BinaryExpressionNode.Levels[CurrentToken.Type]>precedence)
        {
            Lexer.TokenType _operator = CurrentToken.Type;
            Consume(_operator);
            ExpressionNode Right = Parse_ExpressionNode(BinaryExpressionNode.Levels[_operator]);

            if(IsBoolean_Operator(_operator))
            {
                Left = new BooleanBinaryExpressionNode{Left = Left , Operator = _operator , Right = Right};
            }
            else if (IsConcat_Operator(_operator))
            {
                if(_operator == Lexer.TokenType.CompConcat)
                    Left = new ConcatExpressionNode{Left = Left , Operator = _operator , Right = Right , IsComp = true };
                else 
                    Left = new ConcatExpressionNode{Left = Left , Operator = _operator , Right = Right , IsComp = false };    
            }
            else 
            {
                Left = new NumericBinaryExpressionNode{Left = Left , Operator = _operator , Right = Right};
            }
        } 
        return Left;
    }  
    public ExpressionNode Parse_PrimaryExpression()
    {
        ExpressionNode expressionNode = Parse_BasicPrimaryExpression();
        //Parsing Acceso a Propiedad o LLamada de Metodo
        if(CurrentToken.Type == Lexer.TokenType.OpenParen)
        {
            Consume(Lexer.TokenType.OpenParen);
            ExpressionNode argument = Parse_ExpressionNode();
            Consume(Lexer.TokenType.CloseParen);
            expressionNode = new MethodCallNode{Target = null , MethodName = expressionNode , Arguments = new List<ExpressionNode>{argument}};
        }

        while(CurrentToken != null && CurrentToken.Type == Lexer.TokenType.Dot)
        {
            Consume(Lexer.TokenType.Dot);
            ExpressionNode propertyName = Parse_BasicPrimaryExpression();
            //luego de q haya procesado recursivamente todos los accesos a propiedades se verifica si es una llamada de funcion o es un acceso a propiedad
            //llamada de funcion  
            if(CurrentToken.Type == Lexer.TokenType.OpenParen)
            {
                Consume(Lexer.TokenType.OpenParen);
                List<ExpressionNode> arguments = new();
                while(CurrentToken!=null && CurrentToken.Type != Lexer.TokenType.CloseParen)
                {
                    arguments.Add(Parse_ExpressionNode());
                }
                Consume(Lexer.TokenType.CloseParen);
                expressionNode = new MethodCallNode{MethodName = propertyName , Arguments = arguments , Target = expressionNode}; 
            }
            //acceso a propiedad
            else 
            {
                expressionNode = new PropertyAccesNode{Property_Name = propertyName, Target = expressionNode};
            }
            if (CurrentToken?.Type == Lexer.TokenType.OpenBracket)
            {
                CollectionIndexingNode Indexing = new CollectionIndexingNode();
                Consume(Lexer.TokenType.OpenBracket);
                Indexing.Collection_Name = expressionNode;
                Indexing.Index = Parse_ExpressionNode();
                Consume(Lexer.TokenType.CloseBracket);
                expressionNode = Indexing;
            }
        }
        //Parsing Predicados en Metodos y/o Metodos Comunes 
        if(CurrentToken!=null && CurrentToken.Type == Lexer.TokenType.OpenParen)
        {
            PredicateExpressionNode predicate = new();
            Consume(Lexer.TokenType.OpenParen);
            predicate.Generic_Identifier = new IdentifierNode (CurrentToken.Value);
            Consume(Lexer.TokenType.Identifier);
            Consume(Lexer.TokenType.CloseParen);
            Consume (Lexer.TokenType.Arrow);
            Token possibleError = new Token(CurrentToken.Type,CurrentToken.Value,CurrentToken.Row,CurrentToken.Column);
            predicate.Filter = Parse_ExpressionNode();
            if(predicate.Filter is not BooleanBinaryExpressionNode)
                throw new Exception($"Esperaba una expresion booleana en el Predicate: {possibleError} y recibio expresion {predicate.Filter.GetType}");
        
            return predicate;
            
        }
        //Parsing CollectionIndexingNode
        else if (CurrentToken!=null && CurrentToken.Type == Lexer.TokenType.OpenBracket)
        {
            CollectionIndexingNode Indexing = new CollectionIndexingNode();
            Consume(Lexer.TokenType.OpenBracket);
            Indexing.Collection_Name = expressionNode;
            Indexing.Index = Parse_ExpressionNode();
            Consume(Lexer.TokenType.CloseBracket);
            return Indexing;
        }

        return expressionNode;
    }
    public ExpressionNode Parse_BasicPrimaryExpression()
    {
        //caso en que la expresion primaria es una expresion unaria con prefijo
        if(CurrentToken.Type == Lexer.TokenType.Minus || CurrentToken.Type == Lexer.TokenType.MinusMinus || CurrentToken.Type == Lexer.TokenType.PlusPlus)
        {
            Lexer.TokenType _operator = CurrentToken.Type;
            Consume(_operator);
            ExpressionNode operand = Parse_PrimaryExpression();
            return new UnaryExpressionNode{Operand = operand , Operator = _operator ,IsPrefix = true};
        }
        //caso en q la expresion es un Identifier o una expresion unaria con sufijo    
        if(CurrentToken.Type == Lexer.TokenType.Identifier || CurrentToken.Type == Lexer.TokenType.Power || 
           CurrentToken.Type == Lexer.TokenType.Range || CurrentToken.Type == Lexer.TokenType.Faction ||
           CurrentToken.Type == Lexer.TokenType.Name || CurrentToken.Type == Lexer.TokenType.Type)
        {
            string ID = CurrentToken.Value;
            Consume(CurrentToken.Type);
            IdentifierNode identifierNode = new IdentifierNode(ID);
            //expresion unaria con sufijo
            if(CurrentToken.Type == Lexer.TokenType.PlusPlus || CurrentToken.Type == Lexer.TokenType.MinusMinus)
            {
                Token _operator = new(CurrentToken.Type,CurrentToken.Value,CurrentToken.Row,CurrentToken.Column);
                Consume(CurrentToken.Type);
                return new UnaryExpressionNode{Operand = identifierNode , Operator = _operator.Type , IsPrefix = false}; 
            }
            //caso Identifier
            return identifierNode;
        }
        //caso en que la expresion es un number
        else if (CurrentToken.Type == Lexer.TokenType.Number)
        {
            NumberNode numberNode = new NumberNode{Value = double.Parse(CurrentToken.Value)};
            Consume(Lexer.TokenType.Number);
            return numberNode;
        } 
        //caso en que la expresion es un string
        else if (CurrentToken.Type == Lexer.TokenType.String)
        {
            StringNode stringNode = new StringNode{Value = CurrentToken.Value};
            Consume(Lexer.TokenType.String);
            return stringNode;
        }
        //caso en que la expresion es un bool
        else if (CurrentToken.Type == Lexer.TokenType.Boolean)
        {
            BoolNode boolNode = new BoolNode(CurrentToken.Value);
            Consume(Lexer.TokenType.Boolean);
            return boolNode;
        }
        //caso en que la expresion comienza por parentesis
        else if (CurrentToken.Type == Lexer.TokenType.OpenParen)
        {
            Consume (Lexer.TokenType.OpenParen);
            ExpressionNode expressionNode = Parse_ExpressionNode();
            Consume (Lexer.TokenType.CloseParen);
            return expressionNode;
        }
        //caso en q es un tipo 
        else if(CurrentToken.Type == Lexer.TokenType.NumberType || CurrentToken.Type == Lexer.TokenType.StringType || CurrentToken.Type == Lexer.TokenType.BooleanType)
        {
            DataTypeNode dataTypeNode = new DataTypeNode();
            dataTypeNode.DataType = CurrentToken.Type;
            Consume(CurrentToken.Type);
            return dataTypeNode;
        }
        //errores
        else 
            throw new Exception($"Se esperaba una expresion primaria y se recibio {CurrentToken}");
    }


    #endregion
    
    #region Parsing Statements
    public StatementNode Parse_StatementNode()
    {
        if (CurrentToken.Type == Lexer.TokenType.Identifier)
        {
            ExpressionNode Left = Parse_ExpressionNode();
            if (Left is IdentifierNode || Left is PropertyAccesNode)
            {
                Token _operator = new Token(CurrentToken.Type, CurrentToken.Value, CurrentToken.Row, CurrentToken.Column);
                if (IsAssignment_Operator(_operator.Type))
                {
                    Consume(CurrentToken.Type);
                    ExpressionNode Right = Parse_ExpressionNode();
                    if (_operator.Type == Lexer.TokenType.Equals || _operator.Type == Lexer.TokenType.Colon)
                    {
                        Consume(Lexer.TokenType.Comma);
                        return new AssignmentNode
                        {
                            Identifier = Left,
                            Value = Right
                        };
                    }
                    else
                    {
                        Consume(Lexer.TokenType.Comma);
                        return new CompoundAssignmentNode
                        {
                            Identifier = Left,
                            Value = Right,
                            Operator = GetSimpleOperator(_operator.Type)
                        };
                    }
                }
                else
                {
                    throw new Exception($"Error en la asignación en la línea: {_operator.Row}, se esperaba un operador y se recibió:{_operator.Type}");
                }
            }
            else if (Left is MethodCallNode)
            {
                // Manejar llamadas a métodos
                Consume(Lexer.TokenType.Comma);
                return new AccesExpressionNode { Expression = Left };
            }
            else if (Left is PropertyAccesNode || Left is CollectionIndexingNode)
            {
                // Manejar acceso a propiedades o indexación de colecciones
                Token _operator = new Token(CurrentToken.Type, CurrentToken.Value, CurrentToken.Row, CurrentToken.Column);
                Consume(CurrentToken.Type);
                StatementNode statementNode;
                if (_operator.Type == Lexer.TokenType.Equals)
                {

                    statementNode = new AssignmentNode
                    {
                        Identifier = Left,
                        Value = Parse_ExpressionNode()
                    };
                }
                else
                {
                    statementNode = new CompoundAssignmentNode
                    {
                        Identifier = Left,
                        Value = Parse_ExpressionNode(),
                        Operator = _operator.Type
                    };
                }
                return statementNode;
            }
            else if(Left is UnaryExpressionNode)
            {
                Consume(Lexer.TokenType.Comma);
                return new AccesExpressionNode{Expression = Left};
            }
            else
            {
                throw new Exception($"Se esperaba una llamada a un método o un acceso a propiedad, pero se recibió:{CurrentToken}");
            }
        }
        else if (CurrentToken.Type == Lexer.TokenType.For)
        {
            return Parse_ForBlockNode();
        }
        else if (CurrentToken.Type == Lexer.TokenType.While)
        {
            return Parse_WhileBlockNode();
        }
        else
        {
            throw new Exception($"Se esperaba un statement en la fila:{CurrentToken.Row} pero se recibió: {CurrentToken}");
        }
    }

    public AssignmentNode Parse_AssignmentNode()
    {
        Token beforeExpression = new Token (CurrentToken.Type,CurrentToken.Value,CurrentToken.Row,CurrentToken.Column);
        ExpressionNode Left = Parse_PrimaryExpression();
        if(Left is IdentifierNode || Left is PropertyAccesNode)
        {
            Token _operator = new Token (CurrentToken.Type,CurrentToken.Value,CurrentToken.Row,CurrentToken.Column);
            if(IsAssignment_Operator(CurrentToken.Type))
            {
                Consume(CurrentToken.Type);
                ExpressionNode Right = Parse_ExpressionNode();
                if(_operator.Type == Lexer.TokenType.Equals || _operator.Type == Lexer.TokenType.Colon)
                {
                    Consume(Lexer.TokenType.Comma);
                    return new AssignmentNode {
                        Identifier = Left,
                        Value = Right
                    };
                }
                else 
                {
                    Consume(Lexer.TokenType.Comma);
                    return new CompoundAssignmentNode{
                        Identifier = Left,
                        Value = Right,
                        Operator = GetSimpleOperator(_operator.Type)
                    };
                }
            }
            else 
            {
                Consume(CurrentToken.Type);
                ExpressionNode Right = Parse_ExpressionNode();

                throw new Exception($"Error en la asignacionn de la linea: {_operator.Row}, se esperaba un operador y se recibio:{_operator.Type}");
            }
        }
        else 
        {
            throw new Exception($"Se esperaba PropertyAcces o Identifier, se recibio:{Left}");
        }
    }   

    public ForBlockNode Parse_ForBlockNode()
    {
        ForBlockNode forBlockNode = new();
        Consume( Lexer.TokenType.For);
        Consume(Lexer.TokenType.OpenParen); 
        forBlockNode.Element = Parse_ExpressionNode();
        Consume(Lexer.TokenType.In);
        forBlockNode.Collection = Parse_ExpressionNode();
        Consume(Lexer.TokenType.CloseParen);
        if(Match(CurrentToken,Lexer.TokenType.OpenBrace))
        {
            Consume(Lexer.TokenType.OpenBrace);
            while(CurrentToken != null && CurrentToken.Type != Lexer.TokenType.CloseBrace)
            {
                forBlockNode.Body.Add(Parse_StatementNode());
            }
            Consume(Lexer.TokenType.CloseBrace);

        }
        else 
            forBlockNode.Body.Add(Parse_StatementNode());

        return forBlockNode;
    
    }

    public WhileBlockNode Parse_WhileBlockNode()
    {
        WhileBlockNode whileBlockNode = new();
        Consume( Lexer.TokenType.While);
        Consume(Lexer.TokenType.OpenParen);
        whileBlockNode.Condition = Parse_ExpressionNode();
        Consume(Lexer.TokenType.CloseParen);
        if(Match(CurrentToken,Lexer.TokenType.OpenBrace))
        {
            Consume(Lexer.TokenType.OpenBrace);
            while(CurrentToken != null && CurrentToken.Type != Lexer.TokenType.CloseBrace)
            {
                whileBlockNode.Body.Add(Parse_StatementNode());
            }
            Consume(Lexer.TokenType.CloseBrace);

        }
        else 
            whileBlockNode.Body.Add(Parse_StatementNode());

        return whileBlockNode;
    }
    #endregion

    
    
    #endregion
}