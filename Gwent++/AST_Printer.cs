using Interpreter;

public static class ASTPrinter
{
    public static void PrintAST(ASTNode node, string indent = "", bool isLast = true)
    {
        if (node == null) return;

        Console.Write(indent);
        if (isLast)
        {
            Console.Write("└─");
            indent += "  ";
        }
        else
        {
            Console.Write("├─");
            indent += "| ";
        }

        Console.WriteLine(node.GetType().Name);

        PrintNodeDetails(node, indent);

        if (node is ProgramNode programNode)
        {
            for (int i = 0; i < programNode.Sections.Count; i++)
            {
                PrintAST(programNode.Sections[i], indent, i == programNode.Sections.Count - 1);
            }
        }
        else if (node is EffectNode effectNode)
        {
            PrintAST(effectNode.Name, indent, false);
            
            if(effectNode.Params != null)
            {
                Console.WriteLine($"{indent}Params:");
                for (int i = 0; i < effectNode.Params.Count; i++)
                {
                    PrintAST(effectNode.Params[i], indent, false);
                }
            }
            
            PrintAST(effectNode.Action, indent, true);
        }
        else if (node is ActionBlockNode actionBlockNode)
        {
            PrintAST(actionBlockNode.Targets, indent, false);
            PrintAST(actionBlockNode.Context, indent, false);
            for (int i = 0; i < actionBlockNode.Statements.Count; i++)
            {
                PrintAST(actionBlockNode.Statements[i], indent, i == actionBlockNode.Statements.Count - 1);
            }
        }
        else if (node is CardNode cardNode)
        {
            PrintAST(cardNode.Name, indent, false);
            PrintAST(cardNode.Type, indent, false);
            PrintAST(cardNode.Faction, indent, false);
            PrintAST(cardNode.Power, indent, false);
            for (int i = 0; i < cardNode.Range.Count; i++)
            {
                PrintAST(cardNode.Range[i], indent, i == cardNode.Range.Count - 1);
            }
            PrintAST(cardNode.OnActivation, indent, true);
        }
        else if (node is OnActivationNode onActivationNode)
        {
            for (int i = 0; i < onActivationNode.Activations.Count; i++)
            {
                PrintAST(onActivationNode.Activations[i], indent, i == onActivationNode.Activations.Count - 1);
            }
        }
        else if (node is EffectDeclarationNode effectDeclarationNode)
        {
            PrintAST(effectDeclarationNode.Selector, indent, false);
            for (int i = 0; i < effectDeclarationNode.Effect.Count; i++)
            {
                PrintAST(effectDeclarationNode.Effect[i], indent, false);
            }
            PrintAST(effectDeclarationNode.PostAction, indent, true);
        }
        else if (node is SelectorNode selectorNode)
        {
            PrintAST(selectorNode.Source, indent, false);
            PrintAST(selectorNode.Single, indent, false);
            PrintAST(selectorNode.Predicate, indent, true);
        }
        else if (node is AssignmentNode assignmentNode)
        {
            PrintAST(assignmentNode.Identifier, indent, false);
            PrintAST(assignmentNode.Value, indent, true);
        }
        else if (node is ForBlockNode forBlockNode)
        {
            PrintAST(forBlockNode.Element, indent, false);
            PrintAST(forBlockNode.Collection, indent, false);
            for (int i = 0; i < forBlockNode.Body.Count; i++)
            {
                PrintAST(forBlockNode.Body[i], indent, i == forBlockNode.Body.Count - 1);
            }
        }
        else if (node is WhileBlockNode whileBlockNode)
        {
            PrintAST(whileBlockNode.Condition, indent, false);
            for (int i = 0; i < whileBlockNode.Body.Count; i++)
            {
                PrintAST(whileBlockNode.Body[i], indent, i == whileBlockNode.Body.Count - 1);
            }
        }
        else if (node is UnaryExpressionNode unaryExpressionNode)
        {
            PrintAST(unaryExpressionNode.Operand, indent, true);
        }
        else if (node is BinaryExpressionNode binaryExpressionNode)
        {


            PrintAST(binaryExpressionNode.Left, indent, false);
            PrintAST(binaryExpressionNode.Right, indent, true);
        }
        else if (node is MethodCallNode methodCallNode)
        {
            PrintAST(methodCallNode.Target, indent, false);
            for (int i = 0; i < methodCallNode.Arguments.Count; i++)
            {
                PrintAST(methodCallNode.Arguments[i], indent, i == methodCallNode.Arguments.Count - 1);
            }
        }
        else if (node is PropertyAccesNode propertyAccesNode)
        {
            PrintAST(propertyAccesNode.Property_Name, indent, false);
            PrintAST(propertyAccesNode.Target, indent, true);
        }
        else if (node is CollectionIndexingNode collectionIndexingNode)
        {
            PrintAST(collectionIndexingNode.Collection_Name, indent, false);
            PrintAST(collectionIndexingNode.Index, indent, true);
        }
    }

    private static void PrintNodeDetails(ASTNode node, string indent)
    {
        Console.ForegroundColor = ConsoleColor.Green;
        switch (node)
        {
            case EffectNode effectNode:
                Console.WriteLine($"{indent}Name: {effectNode.Name}");
                
                break;
            case CardNode cardNode:
                Console.WriteLine($"{indent}Name: {cardNode.Name}");
                Console.WriteLine($"{indent}Type: {cardNode.Type}");
                Console.WriteLine($"{indent}Faction: {cardNode.Faction}");
                Console.WriteLine($"{indent}Power: {cardNode.Power}");
                break;
            case ForBlockNode forBlockNode:
                Console.WriteLine($"{indent}Element: {forBlockNode.Element}");
                Console.WriteLine($"{indent}Collection: {forBlockNode.Collection}");
                break;
            case AssignmentNode assignmentNode:
                Console.WriteLine($"{indent}Identifier: {assignmentNode.Identifier}");
                Console.WriteLine($"{indent}Value: {assignmentNode.Value}");
                break;
            case IdentifierNode identifierNode:
                Console.WriteLine($"{indent}Name: {identifierNode.Name}");
                break;
            case StringNode stringNode:
                Console.WriteLine($"{indent}Value: {stringNode.Value}");
                break;
            case NumberNode numberNode:
                Console.WriteLine($"{indent}Value: {numberNode.Value}");
                break;
            case BoolNode boolNode:
                Console.WriteLine($"{indent}Value: {boolNode.Value}");
                break;
            case UnaryExpressionNode unaryExpressionNode:
                Console.WriteLine($"{indent}Operator: {unaryExpressionNode.Operator}");
                break;
            case BinaryExpressionNode binaryExpressionNode:
                Console.WriteLine($"{indent}Operator: {binaryExpressionNode.Operator}");
                break;
            case MethodCallNode methodCallNode:
                Console.WriteLine($"{indent}MethodName: {methodCallNode.MethodName}");
                break;
            case PropertyAccesNode propertyAccesNode:
                Console.WriteLine($"{indent}PropertyName: {propertyAccesNode.Property_Name}");
                Console.WriteLine($"{indent}Target: {propertyAccesNode.Target}");
                break;
            case CollectionIndexingNode collectionIndexingNode:
                Console.WriteLine($"{indent}Collection_Name: {collectionIndexingNode.Collection_Name}");
                Console.WriteLine($"{indent}Index: {collectionIndexingNode.Index}");
                break;
            default:
                break;
        }
        Console.ForegroundColor = ConsoleColor.White;
    }
}
