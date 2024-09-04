using System.Linq.Expressions;
using Interpreter;

namespace interpreter;

public interface IVisitor
{
    void VisitProgramNode(ProgramNode program,Scope scope);
    void VisitEffectNode(EffectNode effect,Scope scope);
    void VisitActionBlockNode(ActionBlockNode actionBlock,Scope scope);
    void VisitCardNode(CardNode card,Scope scope);
    void VisitOnActivationNode(OnActivationNode onActivation,Scope scope);
    void VisitEffectDeclarationNode(EffectDeclarationNode effectDeclaration,Scope scope);
    void VisitSelectorNode(SelectorNode selector,Scope scope);
    void VisitAssignmentNode (AssignmentNode assignment,Scope scope);
    void VisitCompoundAssignmentNode(CompoundAssignmentNode compoundAssignment,Scope scope);
    void VisitForBlockNode (ForBlockNode forBlock,Scope scope);
    void VisitWhileBLockNode(WhileBlockNode whileBlock,Scope scope);
    void VisitAccesExpressionNode(AccesExpressionNode accesExpression,Scope scope);
    void VisitIdentifierNode(IdentifierNode identifier,Scope scope);
    void VisitDataTypeNode(DataTypeNode dataType,Scope scope);
    void VisitStringNode(StringNode _string,Scope scope);
    void VisitNumberNode(NumberNode number,Scope scope);
    void VisitBoolNode(BoolNode _bool,Scope scope);
    void VisitUnaryExpressionNode(UnaryExpressionNode unaryExpression,Scope scope);
    void VisitBinaryExpressionNode(BinaryExpressionNode binaryExpression,Scope scope);
    void VisitBooleanBinaryExpressionNode(BooleanBinaryExpressionNode booleanBinaryExpression,Scope scope);
    void VisitNumericBinaryExpressionNode(NumericBinaryExpressionNode numericBinaryExpression,Scope scope);
    void VisitPredicateExpressionNode (PredicateExpressionNode predicateExpression,Scope scope);
    void VisitConcatExpressionNode(ConcatExpressionNode concatExpression,Scope scope);
    void VisitPropertyAccesNode(PropertyAccesNode propertyAcces,Scope scope);
    void VisitMethodCallNode(MethodCallNode methodCall,Scope scope);
    void VisitCollectionIndexingNode(CollectionIndexingNode collectionIndexing,Scope scope);

}
