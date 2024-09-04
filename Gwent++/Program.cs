using System.Text.RegularExpressions;
using interpreter;
namespace Interpreter;
/// <summary>
/// revisar en carta q no esta revisando bien la semantica de power 
/// y no me esta cogiendo las concaExpression
/// </summary>
class Program
{

    static void Main()
    {
        string input = TestCases.Semantic_Error_UndeclaredParam;

        Lexer lexer = new Lexer(input);
        var tokens = lexer.Tokenize();

        Parser parser = new Parser(tokens);
        ProgramNode programAST = parser.Parse_ProgramNode();
        SemanticAnalyzer semanticAnalyzer = new SemanticAnalyzer(programAST);


        ASTPrinter.PrintAST(programAST);

    }

}

