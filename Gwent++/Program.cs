namespace Interpreter;

 class Program 
    {

        static void Main()
        {
            string input = @"
                effect
                Name: ""Damage""
                /* this is
                a multiline comment*/
                // This is a line comment
                Params:
                amount: Number

                Action: (targets, context) =>
                {
                    for (target in targets)
                    {
                        int i = 0;
                        while (i++ < amount)
                        {
                            target.Power -= 1; /* This is a block comment */
                        }
                    }
                }
                ";

                Lexer lexer = new Lexer(input);
                var tokens = lexer.Tokenize();

                foreach (var token in tokens)
                {
                    Console.WriteLine(token);
                }
        }
        
    }