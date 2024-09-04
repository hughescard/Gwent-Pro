using System.ComponentModel;
using System.Formats.Asn1;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Runtime.ConstrainedExecution;
using System.Security.AccessControl;
using System.Security.Cryptography.X509Certificates;
using Interpreter;

namespace interpreter;

public  class SemanticAnalyzer:IVisitor
{   
    public ProgramNode Program{get;}
    private Dictionary<ExpressionNode, string> typeTable = new Dictionary<ExpressionNode, string>();
    private Dictionary<string,string>? MethodInParams = new Dictionary<string, string>();
    private Dictionary<string,string>? MethodReturnParams = new Dictionary<string, string>();
    private string[] possibleSources = new string[]{"hand","otherHand","deck","otherDeck","field","otherField","parent","board"};
    private Dictionary<string,List<(string,string)>> effectDebt = new Dictionary<string, List<(string, string)>>();
    public SemanticAnalyzer(ProgramNode program)
    {
        this.Program = program;
        MethodInParams["Find"] = "predicate";           MethodReturnParams["Find"] = "List<card>";
        MethodInParams["Push"] = "card";                MethodReturnParams["Push"] = "void";
        MethodInParams["SendBottom"] = "card";          MethodReturnParams["SendBottom"] = "void";
        MethodInParams["Pop"] = "void";                 MethodReturnParams["Pop"] = "card";
        MethodInParams["Remove"] = "card";              MethodReturnParams["Remove"] = "void";
        MethodInParams["Shuffle"] = "void";             MethodReturnParams["Shuffle"] = "void";
        MethodInParams["Add"] = "card";                 MethodReturnParams["Add"] = "void" ; 
        MethodInParams["FieldOfPlayer"] = "player";     MethodReturnParams["FieldOfPlayer"] = "List<card>";
        MethodInParams["HandOfPlayer"] = "player";      MethodReturnParams["HandOfPlayer" ] = "List<card>";
        MethodInParams["DeckOfPlayer"] = "player";      MethodReturnParams["DeckOfPlayer"] = "List<card>";
        MethodInParams["GraveyardOfPlayer"] = "player"; MethodReturnParams["GraveyardOfPlayer"] = "List<card>";
        
        Analyze_Semantic(Program);
    }
    private void Analyze_Semantic(ProgramNode program)
    {
        Scope scope = new Scope();
        scope.Define("TriggerPlayer","player");
        VisitProgramNode(program,scope);
    }
    public void VisitProgramNode(ProgramNode program, Scope scope)
    {
        foreach(var section in program.Sections)
        {
            section.Accept(this,scope);
        }
    }
    public void VisitEffectNode(EffectNode effect, Scope scope)
    {
        //verificando que ninguno de los campos Name y Action este vacio dado que no pueden estarlo
        if(effect.Name == null)
            throw new Exception($"El campo Name del efecto no puede ser null.");
        if(effect.Action == null) 
            throw new Exception($"El campo Name del efecto no puede ser null.");

        //si se declaro el campo Params se guardan las variables aqui dadas para obtener su valor despues en el campo Effect de la propiedad OnActivation de Card
        List<(string,string)>paramType = new List<(string,string)>();
        if(effect.Params != null)
        {
            foreach(var param in effect.Params!)
            {
                param.Accept(this,scope);
                paramType.Add(((param.Identifier as IdentifierNode)!.Name!,EvaluateExpressionType(param.Value!,scope)));
            }
        }
        //se hace lo mismo con la propiedad Name
        EvaluateLiteralExpresionNode(effect.Name);
        effectDebt[((StringNode)effect.Name).Value!] = paramType;
        //se crea un scope nuevo para el bloque de Action y se hace la verificacion semantica en el nodo de Action
        Scope actionBlockScope = new Scope(scope);
        effect.Action.Accept(this,actionBlockScope);
    }
    public void VisitActionBlockNode(ActionBlockNode actionBlock, Scope scope)
    {
        IdentifierNode contextId = (IdentifierNode) actionBlock.Context.Identifier!;
        scope.typeInfo[contextId.Name] = new ContextInfo(contextId.Name);
        string targetsName = ((IdentifierNode)actionBlock.Targets.Identifier!).Name;
        scope.Variables[targetsName] = "List<card>";
        scope.Variables[contextId.Name] = "context";
        foreach(var statement in actionBlock.Statements)
        {
            if(statement is AssignmentNode assignment)
            {
                assignment.Accept(this,scope);
                continue;
            }
            Scope statementScope = new Scope(scope);
            statement.Accept(this,statementScope);
        } 
    }
    public void VisitCardNode(CardNode card, Scope scope)
    {
        if(card.Name != null)
        {
            if(EvaluateExpressionType(card.Name,scope)!="string")
                throw new Exception($"El Campo Name de la carta debe ser de tipo string, se recibio tipo: {EvaluateExpressionType(card.Name,scope)}");
            EvaluateLiteralExpresionNode(card.Name);
            typeTable[card.Name] = "card";

        }
        card.Power?.Accept(this,scope);
        card.Faction?.Accept(this,scope);
        card.Type?.Accept(this,scope);
        if((string)card.Type!.Value! != "Oro" && (string)card.Type.Value! != "Plata" && (string)card.Type.Value! != "Aumento" && (string)card.Type.Value! != "Clima")
            throw new Exception($"Tipo de carta no aceptado:{(string)card.Type.Value!}. Se esperaba: Oro, Plata, Aumento o Clima.");
        if((string)card.Faction!.Value! != "Cyborgs y Robots" && (string)card.Faction.Value! != "Caballeros Medievales")
            throw new Exception($"Faccion: {(string)card.Faction.Value!} no aceptada, se esperaba: \"Cyborgs y Robots\" o \"Caballeros Medievales\"");
        if(EvaluateExpressionType(card.Power!,scope) != "double")   
            throw new Exception($"Se esperaba expresion de tipo Number en declaracion de Power de la carta, se recibio: {card.Power}. ");

        foreach(var range in card.Range!)
        {
            EvaluateLiteralExpresionNode(range);
            if(range.Value != "Melee" && range.Value != "Range" && range.Value != "Siege" )
                throw new Exception($"Rango de carta no aceptado: {range.Value}. Se esperaba: \"Melee\", \"Ranged\", \"Siege\"");
        }
        card.OnActivation!.Accept(this,scope);
    }
    public void VisitOnActivationNode(OnActivationNode onActivation, Scope scope)
    {
        foreach(var effectDeclaration in onActivation.Activations)
        {
            Scope effectDeclarationScope = new Scope(scope);
            effectDeclaration.Accept(this,effectDeclarationScope);
        }
    }
    public void VisitEffectDeclarationNode(EffectDeclarationNode effectDeclaration, Scope scope)
    {
        //vrificando semantica en campo Effect de OnActivation
        string effectName = "";
        List<(string,string)> paramsDefinition = new List<(string, string)>();
        foreach(var param in effectDeclaration.Effect!)
        {
            if(((IdentifierNode)param.Identifier!).Name == "Name")
            {
                EvaluateLiteralExpresionNode(param.Value!);
                if(effectDebt.ContainsKey((string)param.Value!.Value!) == false)
                    throw new Exception($"La carta esta haciendo referencia a un efecto no declarado: {param.Value.Value}.");
                effectName = param.Value.Value!;
            }
            else 
            {
                string assignmentType = EvaluateExpressionType(param.Value!,scope);
                paramsDefinition.Add(((param.Identifier as IdentifierNode)!.Name,assignmentType));
            }
        }
        List<(string,string)> paramsDiffer = new List<(string, string)>();
        if(effectDebt.ContainsKey(effectName))
        {
            foreach(var param in effectDebt[effectName])
            {
                if(!paramsDefinition.Contains(param))
                    paramsDiffer.Add(param);
            }
            if(paramsDiffer.Count!=0)
            {
                throw new Exception($"Los parametros dados en la definicion del efecto: {effectName} no coinciden con los recibidos en el campo Effect de la propiedad OnACtivation de la carta que lo declara.");
            }
        }
        //verificando semantica de campo Selector
        if(effectDeclaration.Parent == null && effectDeclaration.Selector == null)
            throw new Exception($"EL campo Selector de la propiedad OnActivation del efecto:{effectName} no puede ser null.");
        else if(effectDeclaration.Selector is null && effectDeclaration.Parent!=null)
            effectDeclaration.Selector = effectDeclaration.Parent.Selector;
        effectDeclaration.Selector!.Accept(this,scope);
        
        //verificando la semantica del campo post action
        if(effectDeclaration.PostAction != null)
            effectDeclaration.PostAction!.Accept(this,scope);
    }
    public void VisitSelectorNode(SelectorNode selector, Scope scope)
    {
        //verificando semantica propiedad Source
        EvaluateLiteralExpresionNode(selector.Source!);
        if(possibleSources.ToList().Contains(selector.Source!.Value!) == false)
            throw new Exception($"La fuente: {selector.Source.Value} dada en el campo Selector de la propiedad OnActivation de la carta que lo declara no es una de las fuentes permitidas.");
        //verificando semantica propiedad Single
        if(selector.Single == null)
            selector.Single = new BoolNode("false") ;
        else
            selector.Single.Accept(this,scope);
        if(EvaluateExpressionType(selector.Single,scope) != "bool")
            throw new Exception($"la expresion:{selector.Single} dada en la propiedad Single del campo Selector dado en la propiedad OnActivation que lo declara no es una expresion de tipo booleana.");
        //verificandoi semantica propiedad Predicate
        if(selector.Predicate!= null)
        {
            Scope predicateScope = new Scope(scope);
            selector.Predicate.Accept(this,predicateScope); 
        }

    }
    public void VisitAssignmentNode(AssignmentNode assignment, Scope scope)
    {
        assignment.Value!.Accept(this,scope);
        if(assignment.Identifier is null )
            throw new Exception($"Variable nula en la asignacion:{assignment.Value}");
        if(assignment.Identifier is IdentifierNode identifierNode)
        {
            string varName = identifierNode.Name;
            scope.Define(varName,EvaluateExpressionType(assignment.Value,scope));
        }
        else if (assignment.Identifier is PropertyAccesNode propertyAcces)
        {
            propertyAcces.Accept(this,scope);
            if(EvaluateExpressionType(propertyAcces,scope) != EvaluateExpressionType(assignment.Value,scope))
                throw new Exception ($"Se esta intentando modificar el valor de {propertyAcces.Property_Name!.Value} con un tipo {EvaluateExpressionType(assignment.Value,scope)} distinto al suyo.");
        }
        else if (assignment.Identifier is MethodCallNode methodCall)
        {
            methodCall.Accept(this,scope);
        }
        else 
            throw new Exception($"Tipo de Asignacion no esperada:{assignment.Identifier}");
    }
    public void VisitCompoundAssignmentNode(CompoundAssignmentNode compoundAssignment, Scope scope)
    {
        compoundAssignment.Identifier!.Accept(this,scope);
        compoundAssignment.Value!.Accept(this,scope);
        if(EvaluateExpressionType(compoundAssignment.Identifier,scope) != EvaluateExpressionType(compoundAssignment.Value,scope))
            throw new Exception($"Los valores no son del mismo tipo en la asignacion: {compoundAssignment.Identifier} y {compoundAssignment.Value}.");

    }
    public void VisitForBlockNode(ForBlockNode forBlock, Scope scope)
    {
        //verificando que la variable del for no haya estado declarada antes
        if(forBlock.Element!=null && forBlock.Element is IdentifierNode identifierNode && !typeTable.ContainsKey(forBlock.Element))
        {
            typeTable[forBlock.Element] = "card";
            scope.Define(identifierNode.Name,"card");
            scope.typeInfo[identifierNode.Name] = new CardInfo(identifierNode.Name);
        }
        //verificando semantica del Body 
        foreach(var statement in forBlock.Body!)
        {
            if(statement is AssignmentNode)
            {
                statement.Accept(this,scope);
                continue;
            }
            Scope newStatementScope = new Scope(scope);
            statement.Accept(this,newStatementScope);
        }

    }
    public void VisitWhileBLockNode(WhileBlockNode whileBlock, Scope scope)
    {
        //verificando que la condicion sea de tipo booleana
        if(whileBlock.Condition!= null && whileBlock.Condition is BooleanBinaryExpressionNode || whileBlock.Condition is BoolNode)
        {
            typeTable[whileBlock.Condition] = "bool";
            whileBlock.Condition.Accept(this,scope);
        }
        else 
            throw new Exception($"La condicion:{whileBlock.Condition} dada en el ciclo while no es de tipo booleana.");
        //verificando la semantica del Body
        foreach(var statement in whileBlock.Body!)
        {
            if(statement is AssignmentNode)
            {
                statement.Accept(this,scope);
                continue;
            }
            Scope newStatementScope = new Scope(scope);
            statement.Accept(this,newStatementScope);
        }
    }
    public void VisitAccesExpressionNode(AccesExpressionNode accesExpression, Scope scope)
    {
        accesExpression.Expression!.Accept(this,scope);
    }
    public void VisitIdentifierNode(IdentifierNode identifier, Scope scope)
    {
        if(identifier.Name != null && !scope.ContainsVariable(identifier.Name))
            throw new Exception($"La variable: {identifier.Name} no esta declarada.");
    }
    public void VisitDataTypeNode(DataTypeNode dataType, Scope scope)
    {
        EvaluateExpressionType(dataType,scope);
    }
    public void VisitStringNode(StringNode _string, Scope scope)
    {
        EvaluateExpressionType(_string,scope);
        EvaluateLiteralExpresionNode(_string);
    }
    public void VisitNumberNode(NumberNode number, Scope scope)
    {
        EvaluateExpressionType(number,scope);
    }
    public void VisitBoolNode(BoolNode _bool, Scope scope)
    {
        EvaluateExpressionType(_bool,scope);
    }
    public void VisitUnaryExpressionNode(UnaryExpressionNode unaryExpression, Scope scope)
    {
        unaryExpression.Operand?.Accept(this,scope);
        string operandID = (unaryExpression.Operand as IdentifierNode)!.Name;
        if(!scope.ContainsVariable(operandID))
            throw new Exception ($"La variable:{operandID} no esta declarada.");
        if(scope.Resolve(operandID) != "double")
            throw new Exception($"No se puede aplicar la operacion:{unaryExpression.Operator} a una variable que no es de tipo Number.");

    }
    public void VisitBinaryExpressionNode(BinaryExpressionNode binaryExpression, Scope scope)
    {
        throw new NotImplementedException();
    }
    public void VisitBooleanBinaryExpressionNode(BooleanBinaryExpressionNode booleanBinaryExpression, Scope scope)
    {
        booleanBinaryExpression.Left?.Accept(this,scope);
        booleanBinaryExpression.Right?.Accept(this,scope);
        EvaluateExpressionType(booleanBinaryExpression,scope);
    }
    public void VisitNumericBinaryExpressionNode(NumericBinaryExpressionNode numericBinaryExpression, Scope scope)
    {
        numericBinaryExpression.Left?.Accept(this,scope);
        numericBinaryExpression.Right?.Accept(this,scope);
        EvaluateExpressionType(numericBinaryExpression,scope);
    }
    public void VisitPredicateExpressionNode(PredicateExpressionNode predicateExpression, Scope scope)
    {
        if(scope.ContainsVariable(predicateExpression.Generic_Identifier!.Name))
            throw new Exception($"La variable: {predicateExpression.Generic_Identifier.Name} usada en la propiedad Predicate del campo Selector de la propiedad OnActivation de la carta que lo declara ya existe.");
     
        scope.Define(predicateExpression.Generic_Identifier.Name,"card");
        scope.typeInfo[predicateExpression.Generic_Identifier.Name] = new CardInfo(predicateExpression.Generic_Identifier.Name);
        predicateExpression.Filter!.Accept(this,scope);    
    }
    public void VisitConcatExpressionNode(ConcatExpressionNode concatExpression, Scope scope)
    {
        concatExpression.Left?.Accept(this,scope);
        concatExpression.Right?.Accept(this,scope);
        EvaluateExpressionType(concatExpression,scope);
    }
    public void VisitPropertyAccesNode(PropertyAccesNode propertyAcces, Scope scope)
    {
        propertyAcces.Target?.Accept(this,scope);
        if(propertyAcces.Target is PropertyAccesNode prop && !ContainsProperty(prop.Property_Name!,(IdentifierNode)propertyAcces.Property_Name!,scope))
        {
            throw new Exception($"El objeto: {prop.Property_Name} no contiene la propiedad: {((IdentifierNode)propertyAcces.Property_Name!).Name} ");
        }
        else if (propertyAcces.Target is IdentifierNode iden && (!ContainsProperty((propertyAcces.Target as IdentifierNode)!, (IdentifierNode)propertyAcces.Property_Name!,scope) || !scope.ContainsVariable((propertyAcces.Target as IdentifierNode)!.Name!)))
        {
            throw new Exception($"El obejto: {(propertyAcces.Target as IdentifierNode)!.Name} no contiene la propiedad: {((IdentifierNode)propertyAcces.Property_Name!).Name}");
        }
    }
    public void VisitMethodCallNode(MethodCallNode methodCall, Scope scope)
    {
        methodCall.Target?.Accept(this,scope);
        if(methodCall.Arguments != null)
        {
            foreach(var argument in methodCall.Arguments)
            {
                argument.Accept(this,scope);
            }
        }
        if(!MethodInParams!.ContainsKey(methodCall.MethodName!.Name))
        {
            throw new Exception($"El método: {methodCall.MethodName.Name} no es valido");
        }
        if (methodCall.Arguments!.Count == 0 && MethodInParams[methodCall.MethodName.Name] != "void")
        {
            throw new Exception($"El metodo: {methodCall.MethodName.Name} recibe argumentos que no han sido asignados");
        }
        else if (methodCall.Arguments.Count!=0 && MethodInParams[methodCall.MethodName.Name]!= EvaluateExpressionType(methodCall.Arguments.ElementAt(0),scope))
        {
            throw new Exception($"El metodo: {methodCall.MethodName.Name} recibe argumentos de tipo: {MethodInParams[methodCall.MethodName.Name]} y recibio: {EvaluateExpressionType(methodCall.Arguments.ElementAt(0),scope)}");
        }
        if(methodCall.Target is null && MethodInParams[methodCall.MethodName.Name]!="player")
        {
            throw new Exception($"El metodo: {methodCall.MethodName.Name} debe ser aplicado a una coleccion");
        }
    }
    public void VisitCollectionIndexingNode(CollectionIndexingNode collectionIndexing, Scope scope)
    {
        collectionIndexing.Collection_Name?.Accept(this,scope);
        collectionIndexing.Index?.Accept(this,scope);
        if(EvaluateExpressionType(collectionIndexing.Index!,scope) != "double")
            throw new Exception($"no se puede Indexar en un valor que no sea de tipo Number: {EvaluateExpressionType(collectionIndexing.Index!,scope)}");
        if(EvaluateExpressionType(collectionIndexing.Collection_Name!,scope) != "List<card>")
            throw new Exception ($"El objeto: {collectionIndexing.Collection_Name} no es indexable.");
    }
    public void VisitStatementNode(StatementNode statementNode,Scope scope)
    {
        throw new NotImplementedException();
    }
    public void VisitExpressionNode(ExpressionNode expressionNode,Scope scope)
    {
        throw new NotImplementedException();
    }
    public string EvaluateExpressionType(ExpressionNode expression,Scope scope)
    {
        if(expression is IdentifierNode identifier)
        {
            if(scope.ContainsVariable(identifier.Name))
                return scope.Resolve(identifier.Name);
            else 
                throw new Exception ($"La variable: {identifier.Name} no existe en el contexto actual");
        }
        else if (expression is DataTypeNode dataType)
        {
            if(dataType.DataType is Lexer.TokenType.NumberType)
                return "double";
            else if(dataType.DataType is Lexer.TokenType.StringType)
                return "string";
            else if (dataType.DataType is Lexer.TokenType.BooleanType)
                return "bool";
            else 
                throw new Exception($"Tipo de dato no soportado: {dataType.DataType}.");
        }
        else if (expression is StringNode)
        {
            return "string";
        }
        else if(expression is NumberNode)
        {
            return "double";
        }
        else if (expression is BoolNode)
        {
            return "bool";
        }
        else if (expression is ConcatExpressionNode concatExpression)
        {
            string leftType = EvaluateExpressionType(concatExpression.Left!,scope);
            string rightType = EvaluateExpressionType(concatExpression.Right!,scope);
            if(leftType == rightType && leftType == "string" &&  rightType == "string")
                return "string";
            else if(leftType != "string")
                throw new Exception($"Se esperaba tipo string en expresion:{concatExpression.Left}y se recibio: {leftType}");
            else if(rightType != "string")
                throw new Exception($"Se esperaba tipo string en expresion:{concatExpression.Right} y se recibio: {rightType}");
            else 
                throw new Exception($"Error en la expresion:{concatExpression} se esperaba tipo string y se recibio:{leftType} y {rightType}");
        }
        else if (expression is NumericBinaryExpressionNode numericBinaryExpression)
        {
            string leftType = EvaluateExpressionType(numericBinaryExpression.Left!,scope);
            string rightType = EvaluateExpressionType(numericBinaryExpression.Right!,scope);
            if(leftType == rightType && leftType == "double")
                return "double";
            else if (leftType!="double")
                throw new Exception($"Se esperaba tipo Number en expresion: {numericBinaryExpression.Left!.Value} y se recibio tipo:{leftType}");
            else 
                throw new Exception($"Se esperaba tipo Number en expresion: {numericBinaryExpression.Right!.Value} y se recibio tipo:{rightType}");
        }
        else if (expression is BooleanBinaryExpressionNode booleanBinaryExpression)
        {
            string leftType = EvaluateExpressionType(booleanBinaryExpression.Left!,scope);
            string rightType = EvaluateExpressionType(booleanBinaryExpression.Right!,scope);
            if(leftType == rightType)
                return "bool";
            else if(booleanBinaryExpression.Operator!.Type is Lexer.TokenType.And || booleanBinaryExpression.Operator!.Type is Lexer.TokenType.Or)   
            {
                if(leftType != "bool")
                    throw new Exception ($"No se puede aplicar operacion: {booleanBinaryExpression.Operator.Value} a {booleanBinaryExpression.Left} porque no es de tipo \"Bool\"");
                if(rightType != "bool")
                    throw new Exception ($"No se puede aplicar operacion: {booleanBinaryExpression.Operator.Value} a {booleanBinaryExpression.Right} porque no es de tipo \"Bool\"");
                else 
                    throw new NotImplementedException();
            }
            else 
                throw new Exception ($"Imposible comparar expresiones: {booleanBinaryExpression.Left} y {booleanBinaryExpression.Right} porque no sel mismo tipo.");
        }
        else if (expression is UnaryExpressionNode unaryExpression)
        {
            if( EvaluateExpressionType(unaryExpression.Operand!,scope) == "double")
                return "double";
            else 
                throw new Exception($"Imposible realizar operacion: {unaryExpression.Operator} a expresion de tipo: {EvaluateExpressionType(unaryExpression.Operand!,scope)}");
        }
        else if (expression is PropertyAccesNode propertyAcces)
        {
            TypeInfo access = new CardInfo("empty");
            if(propertyAcces.Target is PropertyAccesNode prop)
            {
                access = scope.ResolveTypeInfo(((IdentifierNode)prop.Property_Name!).Name!)!;
            }
            else if(propertyAcces.Target is IdentifierNode id)
            {
                access = scope.ResolveTypeInfo(id.Name!)!;
            }
            if(access is null || !access.Properties!.ContainsKey(((IdentifierNode)propertyAcces.Property_Name!).Name!))
            {
                throw new Exception($"La propiedad {((IdentifierNode)propertyAcces.Property_Name!).Name!} no existe en {propertyAcces.Target}");
            }
            else
                return access.Properties![((IdentifierNode)propertyAcces.Property_Name!).Name!];
        }
        else if(expression is MethodCallNode methodCall)
        {
            return MethodReturnParams![((IdentifierNode)methodCall.MethodName!).Name];
        }
        else if(expression is CollectionIndexingNode )
        {
            return "card";
        }
        else if (expression is PredicateExpressionNode)
        {
            return "predicate";
        }
        else 
            throw new Exception($"Tipo de expresion no esperada: {expression.GetType}");
    }
    public static void EvaluateLiteralExpresionNode(ExpressionNode expression)
    {
        if(expression is StringNode stringNode)
        {
              stringNode.Value = (stringNode.Value! as String).Trim('/').Trim().Trim('"');
        }
        else if(expression is ConcatExpressionNode concatExpression)
        {
            EvaluateLiteralExpresionNode(concatExpression.Left!);
            EvaluateLiteralExpresionNode(concatExpression.Right!);
            if(concatExpression.IsComp)
                concatExpression.Value = (concatExpression.Left!).Value + " " +  (concatExpression.Right!).Value;
            else
                concatExpression.Value = (concatExpression.Left!).Value +  (concatExpression.Right!).Value;
        }
    }
    public bool ContainsProperty(ExpressionNode target, IdentifierNode property,Scope scope)
    {
            TypeInfo access = new CardInfo("fake");
            if(target is PropertyAccesNode prop)
            {
                access = scope.ResolveTypeInfo(((IdentifierNode)prop.Property_Name!).Name!)!;
            }
            else if(target is IdentifierNode id)
            {
                access = scope.ResolveTypeInfo(id.Name!)!;
            }
            else if(target is MethodCallNode method)
            {
                throw new NotImplementedException();
            }
            if(access is null || !access.Properties!.ContainsKey(property.Name!))
                return false;
            return true;
    }
}
