
namespace Interpreter ;

public class TestCases
{
    // Grupo 1: Pruebas para el Lexer

    // Subgrupo 1.1: Códigos sin errores
    public static string Lexer_NoError_EffectSimple = @"
effect {
    Name: ""Heal"",
    Params: {
        amount: Number,
    },
    Action: (targets, context) =>
    {
        for (target in targets)
            target.Power += amount,
    }
}
";
    public static string Lexer_NoError_CartaSimple = @"
card {
    Type: ""Oro"",
    Name: ""Geralt"",
    Faction: ""Cyborgs y Robots"",
    Power: 15,
    Range: [""Melee""],
    OnActivation: [
        {
            Effect: ""Damage"",
        }
    ]
}
";

    // Subgrupo 1.2: Códigos con errores
    // Error: Nombre del efecto no está terminado con comillas
    public static string Lexer_Error_EffectNameNotTerminated = @"
effect {
    Name: ""Heal,
    Params: {
        amount: Number
    },
    Action: (targets, context) =>
        for target in targets
            target.Power += amount;
}
";

    // Error: Falta la coma después de "Effect": "Damage""
    public static string Lexer_Error_CartaSyntaxError = @"
card {
    Type: ""Oro"",
    Name: ""Geralt"",
    Faction: ""Cyborgs y Robots"",
    Power: 15,
    Range: [""Melee""],
    OnActivation: [
        {
            Effect: ""Damage""
            Selector: {
                Source: ""board"",
                Single: true,
                Predicate: (unit) => unit.Faction == ""Monster""
            }
        }
    ]
}
";

    // Grupo 2: Pruebas para el Parser

    // Subgrupo 2.1: Códigos sin errores
    public static string Parser_NoError_MultipleParams = @"
effect {
    Name: ""Boost"",
    Params: {
        amount: Number,
        duration: Number,
    },
    Action: (targets, context) =>
    {
            for (target in targets) {
                target.Power += amount,
                context.Board.Shuffle(),
            }

    }
}
";

    public static string Parser_NoError_NestedEffects = @"
card {
    Type: ""Oro"",
    Name: ""Triss"",
    Faction: ""Caballeros Medievales"",
    Power: 12,
    Range: [""Range""],
    OnActivation: [
        {
            Effect: ""Draw"",
        }
    ]
}
";

    // Subgrupo 2.2: Códigos con errores
    // Error: "Numbr" está mal escrito, debería ser "Number"
    public static string Parser_Error_ParamDeclaration = @"
effect {
    Name: ""Boost"",
    Params: {
        duration: Number,
        amount: Numbr,
    },
    Action: (targets, context) =>
    {
        for (target in targets)
        {
            target.Power += amount,
            context.Board.Shuffle(),
        }
    }
    
}
";

    // Error: "dekk" está mal escrito, debería ser "deck"
    public static string Parser_Error_SelectorSource = @"
card 
{
    Type: ""Oro"",
    Name: ""Triss"",
    Faction: ""Cyborgs y Robots"",
    Power: 12,
    Range: [""Range""],
    OnActivation: [
        {
            Effect:
            {
                Name: ""Draw"",
            },
            Selector: {
                Source: ""dekk"",
                Single: true,
                Predicate: (unit) => true,
            },
            PostAction: {
                Effect: ""Damage""
            }
        }
    ]
}
";

    // Grupo 3: Pruebas para el Análisis Semántico

    // Subgrupo 3.1: Códigos sin errores
    public static string Semantic_NoError_UseContextCorrectly = @"
effect {
    Name: ""DrawAndBoost"",
    Action: (targets, context) => {
        topCard = context.Deck.Pop(),
        context.Hand.Add(topCard),
        for (target in targets)
            target.Power += 2,
    }
}
";

    public static string Semantic_NoError_ReferEffectsCorrectly = @"
effect {
    Name: ""Shield"",
    Params: {
        amount: Number,
    },
    Action: (targets, context) =>
    {
        for (target in targets)
            target.Power += amount,

    }
}

card {
    Type: ""Plata"",
    Name: ""Shieldmaiden"",
    Faction: ""Caballeros Medievales"",
    Power: 5,
    Range: [""Melee""],
    OnActivation: [
        {
            Effect:
            {
                Name: ""Shield"",
                amount: 5,
            },
            Selector: {
                Source: ""board"",
                Single: false,
                Predicate: (unit) => unit.Faction == ""Skellige"",
            }
        }
    ]
}
";

    // Subgrupo 3.2: Códigos con errores
    // Error: "boostAmount" no está declarado
    public static string Semantic_Error_UndeclaredParam = @"
effect {
    Name: ""Boost"",
    Action: (targets, context) =>
    {
            for (target in targets)
                for(gaspar in target)
                {
                    boostAmount = 24 -3 -2,
                    target.Power += boostAmount,  // Error: boostAmount no está declarado
                    gaspar.Power = target.Power,
                }
            

    }
}
";

    // Error: "Teleport" no está declarado como efecto
    public static string Semantic_Error_UndeclaredEffect = @"
card {
    Type: ""Oro"",
    Name: ""Ciri"",
    Faction: ""Cyborgs y Robots"",
    Power: 10,
    Range: [""Melee"", ""Range""],
    OnActivation: [
        {
            Effect: 
            {
                Name:""Teleport"",  // Error: Teleport no está declarado
                Amount: 2,
            },
            Selector: {
                Source: ""board"",
                Single: true,
                Predicate: (unit) => unit.Name == ""Nilfgaard"",
            }
        }
    ]
}
";
}
