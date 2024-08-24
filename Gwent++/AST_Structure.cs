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
    /// <summary>
    ///aqui en esta lista se guardan todos los hijos del nodo 
    ///program(que seran nodeos de efeccto y carta y considerare
    ///a cada uno como una seccion x eso se llama sections) 
    /// </summary>
    public List<ASTNode> Sections ;  
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

public class OnActivationNode 
{
    public List<EffectDeclarationNode> Activations;
}

public class PostActionNode
{
    public List<EffectDeclarationNode> Activations;
}

public class EffectDeclarationNode
{
    public List<AssignmentNode> Effect;
    public SelectorNode Selector;
    public PostActionNode PostAction;
}

/// <summary>
///en el nodo selector todos los campos son expresiones porque source es un string, single es un booleano
///perdicate es una funcion
/// </summary>
public class SelectorNode
{
    public ExpressionNode Source;
    public ExpressionNode Single;
    public ExpressionNode Predicate;
} 

#endregion 

#region StatementNode and derivatives
public abstract class StatementNode :ASTNode
{

}
public class AssignmentNode: StatementNode
{
    /// <summary>
    /// identifierNode es un nodo terminal que  va a tener sus propiedades especificas que ya vere despues
    /// </summary>
    public IdentifierNode Identifier;
    /// <summary>
    /// el value de la asignacion no lo pongo directamente como un valor ya q se pueden poner juguetones y 
    /// expresiones matematicas o las q les de la gana x eso uso expressionNode (polimorfismmo ya despues se
    /// hara mas especifico que tipo de expresion es)
    /// </summary>
    public ExpressionNode Value;
}

public class ForBlockNode: StatementNode
{
    /// <summary>
    ///Element es el nombre de la variable que va a tener dentro el elemento de la lista x la q esta 
    ///iterando el for(esta variable tiene directamente el elemento de la lista ya q el for en el DSL
    ///es un foreach)
    /// </summary>
    public IdentifierNode Element;
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
    string Name;
}
/// <summary>
/// los siguientes tres son nodos terminales que lo unico que llevan es valor
/// </summary>
public class StringNode: ExpressionNode
{
    string Value;
}
public class NumberNode :ExpressionNode
{
    double Value;
}
public class BoolNode: ExpressionNode
{
    bool Value;
}
public class UnaryExpressionNode: ExpressionNode
{
    ExpressionNode Operand;
    Token Operator;
    bool IsPrefix;
}
public class BinaryExpressionNode: ExpressionNode
{
    ExpressionNode Left;
    ExpressionNode Right;
    Token Operator;
}
public class BooleanBinaryExpressionNode: BinaryExpressionNode
{
    ExpressionNode Left;
    ExpressionNode Right;
    Token Operator;
}
public class NumericBinaryExpressionNode: BinaryExpressionNode
{
    ExpressionNode Left;
    ExpressionNode Right;
    Token Operator;
}  

#endregion



