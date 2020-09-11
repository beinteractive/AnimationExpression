namespace AnimationExpression.Compiler
{
    public abstract class Token
    {
    }

    public class Identifier : Token
    {
        public string Value { get; private set; }
        
        public Identifier(string value)
        {
            Value = value;
        }
    }

    public class Number : Token
    {
        public float Value { get; private set; }
        
        public Number(float value)
        {
            Value = value;
        }
    }

    public class Operator : Token
    {
        public char Value { get; private set; }

        public Operator(char value)
        {
            Value = value;
        }
    }

    public class Symbol : Token
    {
        public char Value { get; private set; }

        public Symbol(char value)
        {
            Value = value;
        }
    }
}