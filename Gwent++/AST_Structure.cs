using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Data.Common;
using System.Linq.Expressions;

namespace Interpreter;

//AST Structure Implementation
            
public abstract class ASTNode
{

}

#region ProgramNode
public class ProgramNode: ASTNode
{
    public ProgramNode()
    {
        Sections = new List<ASTNode>();
    }
    /// <summary>
    ///aqui en esta lista se guardan todos los hijos del nodo 
    ///program(que seran nodeos de efeccto y carta y considerare
    ///a cada uno como una seccion x eso se llama sections) 
    /// </summary>
    public List<ASTNode> Sections;  
}
#endregion

#region EffectNode and derivatives
public class EffectNode: ASTNode
{
    /// <summary>
    ///una declaracion de efecto siempre recibira el campo Name (el cual se crea como expressionNode
    ///para usar el polimorfismo y asi no tener q tener en mente q tipo de expresion es especificamente
    ///(ya sea string o concatenacion de string o cualquier ootra que este erronea))                           
    /// </summary>
    public ExpressionNode Name;
    /// <summary>
    /// Params es en el DSL una lista de identifiers a los que les asigno el tipo de variable que van a
    /// hacer y el valor de este identifier lo toma despues en la carta que vaya a usar este efecto y 
    /// ahi se asigna el valor del param  
    /// </summary>
    public List<AssignmentNode> Params;
    /// <summary>
    /// action es un bloque ya que aqui es donde como tal se programa lo que hace el efecto y
    /// tendra tanto expresiones como asignaciones de todos los tipos
    /// </summary>
    public ActionBlockNode Action;

}                   
public class ActionBlockNode: ASTNode
{
    public ActionBlockNode()
    {
        Targets = new AssignmentNode();
        Context = new AssignmentNode();
        Statements = new List<StatementNode>();
    }
    /// <summary>
    /// targetses una variable(que adentro puede tener cualquier tipo de estructura que desconozco) 
    /// que x logica tenian q haber dado en params pero no, la dan aqui asi q toco ponerla como propiedad 
    /// y seguir la misma logica que en params de ponerlas como assignment y no como identifiers para que
    /// ya de aqui cargue el identifier y el value lo cogera despues
    /// </summary>
    public AssignmentNode Targets;
    public AssignmentNode Context;
    /// <summary>
    /// en el actionblock despues de dar el context y los targets empiezan con la parte de programacion de 
    /// verdad del efecto, dond lo q van a haber son bloques de instrucciones (statements) x eso es una 
    /// lista de statments
    /// </summary>
    public List<StatementNode> Statements;

}
#endregion

#region CardNode and derivatives
/// <summary>
/// el nodo de la carta que es d los nodos principales del AST tiene campos especiales con sus 
/// respectivas palabras clave. Todos se declaran como ExpressionNode ya que pueden poner operaciones
/// en la declaracion (o sea expresiones ya sean string, concat string, trabajo con numeros, identifiers)
/// </summary>
public class CardNode: ASTNode
{
    public CardNode()
    {
        Range = new List<ExpressionNode>();
    }
    /// <summary>
    /// el nombre de ;la carta que no se declara como srting ya q se pueden poner creativos y poner 
    /// operacion de concatenacion
    /// </summary>
    public ExpressionNode Name;
    /// <summary>
    /// el tipo de la carta ya sea oro plata clima lo que sea(se crea como nodo de expression x lo 
    /// mismo que el de arriba)
    /// </summary>
    public ExpressionNode Type;
    /// <summary>
    /// nombre de la faccion de la carta
    /// </summary>
    public ExpressionNode Faction;
    /// <summary>
    /// poder de la carta
    /// </summary>
    public ExpressionNode Power;
    /// <summary>
    /// el rango de la carta o sea la zona de juego donde se va a poder jugar la carta
    /// </summary>
    public List<ExpressionNode> Range;

    public OnActivationNode OnActivation;
}

public class OnActivationNode:ASTNode
{
    public OnActivationNode()
    {
        Activations = new List<EffectDeclarationNode>();
    }
    public List<EffectDeclarationNode> Activations;
}

public class EffectDeclarationNode:ASTNode
{
    public EffectDeclarationNode(EffectDeclarationNode parent)
    {
        Effect = new List<AssignmentNode>();
        this.Parent = parent;
    }
    public EffectDeclarationNode Parent;
    public List<AssignmentNode> Effect;
    public SelectorNode Selector;
    public EffectDeclarationNode PostAction;
}

/// <summary>
///en el nodo selector todos los campos son expresiones porque source es un string, single es un booleano
///perdicate es una funcion
/// </summary>
public class SelectorNode:ASTNode
{
    public ExpressionNode Source;
    public ExpressionNode Single;
    public PredicateExpressionNode Predicate;
} 

#endregion 

#region StatementNode and derivatives
public abstract class StatementNode :ASTNode
{

}
public class AssignmentNode: StatementNode
{
    public ExpressionNode Identifier;
    public ExpressionNode Value;
}
public class CompoundAssignmentNode: AssignmentNode 
{
    public Lexer.TokenType Operator;
}
public class CompundAssignment: AssignmentNode 
{
    Lexer.TokenType simpleOperator;
}
public class ForBlockNode: StatementNode
{
    public ForBlockNode()
    {
        Body = new List<StatementNode>();
    }
    /// <summary>
    ///Element es el nombre de la variable que va a tener dentro el elemento de la lista x la q esta 
    ///iterando el for(esta variable tiene directamente el elemento de la lista ya q el for en el DSL
    ///es un foreach)
    /// </summary>
    public ExpressionNode Element;
    /// <summary>
    /// CollectionVariable es el nommbre de la lista sobre la q se va a iterar y es un ExpressionNode ya q 
    /// puede ser un identifier o el acceso a una propiedad del identifier q sea otro identifier q guarde 
    /// una coleccion en su value y asi recursivamente
    /// </summary>
    public ExpressionNode Collection;
    /// <summary>
    /// Body es el set de instrucciones del for que obviamente son mas statements
    /// </summary>
    public List<StatementNode> Body;
}

public class WhileBlockNode: StatementNode
{
     public WhileBlockNode()
    {
        Body = new List<StatementNode>();
    }
    /// <summary>
    /// Condition es la condicion que permite que el while siga o no(una vez mas se usa polimorfismo ya que
    /// en si la expression es booleanbinaryExpression pero esta hereda de ExpressionNode asi q a ahorrarse
    /// trabajo)
    /// </summary>
    public ExpressionNode Condition;
    /// <summary>
    /// lo mismo q el Body del for
    /// </summary>
    public List<StatementNode> Body;
}

public class AccesExpressionNode: StatementNode
{
    public ExpressionNode Expression;
}

#endregion 

#region ExpressionNode and derivatives
public abstract class ExpressionNode: ASTNode
{

}
public class IdentifierNode: ExpressionNode
{
    /// <summary>
    /// identifierNode es un nodo terminal del arbol(hoja) lo q tiene son solo propiedades y no tiene 
    /// ningun otro nodo adentro
    /// </summary>
    public string Name{set;get;}
    public IdentifierNode(string name)
    {this.Name = name;}

    // public override string ToString()
    // {
    //     return $"Name:{Name}";
    // }
}
/// <summary>
/// los siguientes tres son nodos terminales que lo unico que llevan es valor
/// </summary>
public class DataTypeNode: ExpressionNode
{
    public Lexer.TokenType DataType;
    // public override string ToString()
    // {
    //     return $"DataType: {DataType.GetType}";
    // }
}
public class StringNode: ExpressionNode
{
    public string Value;

    // public override string ToString()
    // {
    //     return $"Value:{Value}";
    // }
}
public class NumberNode :ExpressionNode
{
    public double Value;
}
public class BoolNode: ExpressionNode
{
    public string Value;
    public bool Real_Value{get;set;}
    public BoolNode(string value)
    {
        this.Value = value;
        if(Value == "true")Real_Value = true;
        else Real_Value = false;
    }
}
public class UnaryExpressionNode: ExpressionNode
{
    public ExpressionNode Operand;
    public Lexer.TokenType Operator;
    public bool IsPrefix;
}
public abstract class BinaryExpressionNode: ExpressionNode
{
    public static Dictionary<Lexer.TokenType, int> Levels = new Dictionary<Lexer.TokenType, int>
    {
        { Lexer.TokenType.Plus, 1 },
        { Lexer.TokenType.Minus, 1 },
        { Lexer.TokenType.Multiply, 2 },
        { Lexer.TokenType.Divide, 2 },
        { Lexer.TokenType.And, 1 },
        { Lexer.TokenType.Or, 1 },
        { Lexer.TokenType.EqualValue,1},
        { Lexer.TokenType.NotEqualValue, 1 },
        { Lexer.TokenType.LessThan, 1 },
        { Lexer.TokenType.BiggerThan, 1 },
        { Lexer.TokenType.LessOrEqualThan, 1 },
        { Lexer.TokenType.BiggerOrEqualThan, 1 },
        { Lexer.TokenType.SimpleConcat, 1 },
        { Lexer.TokenType.CompConcat, 1 }
    };
    public ExpressionNode Left;
    public ExpressionNode Right;
    public Token Operator;
}
public class BooleanBinaryExpressionNode: BinaryExpressionNode
{
    public ExpressionNode Left;
    public ExpressionNode Right;
    public Lexer.TokenType Operator;
}
public class NumericBinaryExpressionNode: BinaryExpressionNode
{
    public ExpressionNode Left;
    public ExpressionNode Right;
    public Lexer.TokenType Operator;
}  
public class PredicateExpressionNode: ExpressionNode
{
    public IdentifierNode Generic_Identifier;
    public ExpressionNode Filter;
}
public class ConcatExpressionNode: BinaryExpressionNode
{   
    public ExpressionNode Left;
    public ExpressionNode Right;
    public Lexer.TokenType Operator;
    public bool IsComp;
}
public class PropertyAccesNode:ExpressionNode
{
    public ExpressionNode Property_Name ;
    public ExpressionNode Target;
}
public class MethodCallNode: ExpressionNode
{
    public MethodCallNode()
    {
        Arguments = new List<ExpressionNode>();
    }
    public ExpressionNode MethodName {set;get;}
    public List<ExpressionNode> Arguments{get;set;}
    public ExpressionNode Target{get;set;}  
}
public class CollectionIndexingNode: ExpressionNode
{
    public ExpressionNode Collection_Name{get;set;}
    public ExpressionNode Index{get;set;}

}
#endregion



