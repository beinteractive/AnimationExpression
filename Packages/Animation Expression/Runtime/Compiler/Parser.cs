using System;
using System.Collections.Generic;
using AnimationExpression.Data;
using UnityEngine;

namespace AnimationExpression.Compiler
{
    public class Parser
    {
        private readonly Scanner Scanner;
        private Token Token;
        
        public Parser(Scanner scanner)
        {
            Scanner = scanner;
        }
        
        private Token NextToken()
        {
            Token = Scanner.GetToken();

            /*
            switch (Token)
            {
                case Symbol s:
                    Debug.Log($"Symbol {s.Value}");
                    break;
                case Operator o:
                    Debug.Log($"Operator {o.Value}");
                    break;
                case Identifier id:
                    Debug.Log($"Identifier {id.Value}");
                    break;
                case Number n:
                    Debug.Log($"Number {n.Value}");
                    break;
            }
            */

            return Token;
        }

        private void Initialize()
        {
            Scanner.Rewind();
            NextToken();
        }

        private bool IsOperator(char c)
        {
            return Token is Operator o && o.Value == c;
        }

        private bool IsSymbol(char c)
        {
            return Token is Symbol s && s.Value == c;
        }

        private void SyntaxError(string s)
        {
            throw new ArgumentException($"SyntaxError on line {Scanner.LineNumber} {s}");
        }

        public ASTNode Parse()
        {
            Initialize();
            return ParseProgram();
        }

        public ExpressionNode ParseOnlyExpression()
        {
            Initialize();
            return ParseExpression();
        }

        private ASTNode ParseProgram()
        {
            var nodes = new List<ASTNode>();
            
            while (Token != null)
            {
                nodes.Add(ParseStatement());
            }

            if (nodes.Count == 1)
            {
                return nodes[0];
            }
            
            return new Statements(nodes);
        }

        private ASTNode ParseStatement()
        {
            switch (Token)
            {
                case Symbol s when s.Value == '{':
                    return ParseBlock();
                
                case Symbol s when s.Value == 'I':
                    return ParseIfStatement();
                
                default:
                    return ParseExpressionStatement();
            }
        }

        private ASTNode ParseBlock()
        {
            if (!IsSymbol('{'))
            {
                SyntaxError("Missing '{' of block");
            }

            NextToken();

            var block = ParseStatements();

            if (!IsSymbol('}'))
            {
                SyntaxError("Missing '}' of block");
            }

            NextToken();

            return block;
        }

        private ASTNode ParseIfStatement()
        {
            if (!IsSymbol('I'))
            {
                SyntaxError("Missing 'if' of if statement");
            }

            NextToken();

            if (!IsSymbol('('))
            {
                SyntaxError("Missing '(' of if condition");
            }

            NextToken();

            var cond = ParseExpression();

            if (!IsSymbol(')'))
            {
                SyntaxError("Missing ')' of if condition");
            }

            NextToken();

            var body = ParseStatement();
            
            return new IfStatement(cond, body);
        }

        private ASTNode ParseStatements()
        {
            var nodes = new List<ASTNode>();

            for (;;)
            {
                if (Token == null)
                {
                    break;
                }

                if (IsSymbol('}'))
                {
                    break;
                }
                
                nodes.Add(ParseStatement());
            }
            
            return new Statements(nodes);
        }

        private ExpressionNode ParseExpressionStatement()
        {
            var expr = ParseExpression();

            if (!IsSymbol(';'))
            {
                SyntaxError("Missing ';' on statement");
            }

            NextToken();

            return expr;
        }

        private ExpressionNode ParseExpression()
        {
            return ParseConditionalExpression();
        }

        private ExpressionNode ParseConditionalExpression()
        {
            var cond = ParseLogicalOrExpression();

            if (IsSymbol('?'))
            {
                NextToken();

                var left = ParseExpression();

                if (!IsSymbol(':'))
                {
                    SyntaxError("Missing ':'");
                }

                NextToken();

                var right = ParseExpression();
                
                return new Conditional(cond, left, right);
            }

            return cond;
        }

        private ExpressionNode ParseLogicalOrExpression()
        {
            var left = ParseLogicalAndExpression();

            while (IsOperator('|'))
            {
                NextToken();
                
                left = new Or(left, ParseLogicalAndExpression());
            }

            return left;
        }
        

        private ExpressionNode ParseLogicalAndExpression()
        {
            var left = ParseEqualityExpression();

            while (IsOperator('&'))
            {
                NextToken();
                
                left = new And(left, ParseEqualityExpression());
            }

            return left;
        }

        private ExpressionNode ParseEqualityExpression()
        {
            var left = ParseRelationalExpression();

            while (IsOperator('=') || IsOperator('N'))
            {
                var o = (Operator) Token;
                
                NextToken();

                switch (o.Value)
                {
                    case '=':
                        left = new Equal(left, ParseRelationalExpression());
                        break;
                    case 'N':
                        left = new NotEqual(left, ParseRelationalExpression());
                        break;
                }
            }

            return left;
        }

        private ExpressionNode ParseRelationalExpression()
        {
            var left = ParseAdditiveExpression();

            while (IsOperator('<') || IsOperator('>') || IsOperator('L') || IsOperator('G'))
            {
                var o = (Operator) Token;
                
                NextToken();

                switch (o.Value)
                {
                    case '<':
                        left = new LessThan(left, ParseAdditiveExpression());
                        break;
                    case '>':
                        left = new GreaterThan(left, ParseAdditiveExpression());
                        break;
                    case 'L':
                        left = new LessEqual(left, ParseAdditiveExpression());
                        break;
                    case 'G':
                        left = new GreaterEqual(left, ParseAdditiveExpression());
                        break;
                }
            }

            return left;
        }

        private ExpressionNode ParseAdditiveExpression()
        {
            var left = ParseMultiplicativeExpression();

            while (IsOperator('+') || IsOperator('-'))
            {
                var o = (Operator) Token;

                NextToken();
                
                switch (o.Value)
                {
                    case '+':
                        left = new Add(left, ParseMultiplicativeExpression());
                        break;
                    case '-':
                        left = new Sub(left, ParseMultiplicativeExpression());
                        break;
                }
            }

            return left;
        }

        private ExpressionNode ParseMultiplicativeExpression()
        {
            var left = ParseUnaryExpression();

            while (IsOperator('*') || IsOperator('/'))
            {
                var o = (Operator) Token;

                NextToken();
                
                switch (o.Value)
                {
                    case '*':
                        left = new Mul(left, ParseUnaryExpression());
                        break;
                    case '/':
                        left = new Div(left, ParseUnaryExpression());
                        break;
                }
            }

            return left;
        }

        private ExpressionNode ParseUnaryExpression()
        {
            switch (Token)
            {
                case Operator o when o.Value == '+':
                    NextToken();
                    return ParseUnaryExpression();
                case Operator o when o.Value == '-':
                    NextToken();
                    return new Negate(ParseUnaryExpression());
                case Operator o when o.Value == '!':
                    NextToken();
                    return new Not(ParseUnaryExpression());
                default:
                    return ParseCallExpression();
            }
        }

        private ExpressionNode ParseCallExpression()
        {
            var left = ParsePrimaryExpression();

            if (IsSymbol('('))
            {
                if (left is Variable v)
                {
                    var t = (TweenNode) null;
                    
                    if (CallTween.IsValidId(v.Value))
                    {
                        var args = ParseArguments();
                        t = new CallTween(v.Value, args);
                    }
                    else if (v.Value == "Active")
                    {
                        var args = ParseArguments();
                        return new SetActive(args);
                    }
                    else if (v.Value == "Callback")
                    {
                        ParseArguments();
                        return new Callback();
                    }
                    else
                    {
                        SyntaxError($"{v.Value} is not valid tween name");
                    }

                    while (IsSymbol('.'))
                    {
                        NextToken();
                        
                        var member = ParsePrimaryExpression();
                        if (member is Variable m)
                        {
                            if (!IsSymbol('('))
                            {
                                SyntaxError("Missing '(' after method access");
                            }

                            if (m.Value == "Then")
                            {
                                NextToken();
                                
                                var body = ParseStatement();

                                if (!IsSymbol(')'))
                                {
                                    SyntaxError("Missing ')' after Then body");
                                }

                                NextToken();
                                
                                t = new Then(t, body);
                            }
                            else
                            {
                                var args = ParseArguments();
                            
                                switch (m.Value)
                                {
                                    case "From":
                                    {
                                        t = new From(t, args, false);
                                        break;
                                    }

                                    case "FromRelative":
                                    {
                                        t = new From(t, args, true);
                                        break;
                                    }

                                    case "Delay":
                                    {
                                        t = new Delay(t, args);
                                        break;
                                    }

                                    case "Animate":
                                    {
                                        t = new Animate(t, args);
                                        break;
                                    }
                                
                                    case string name when name.StartsWith("Ease"):
                                    {
                                        if (!Ease.IsValidName(name))
                                        {
                                            SyntaxError($"{name} is not valid easing name");
                                        }
                                        t = new Ease(name, t, args);
                                        break;
                                    }
                                
                                    default:
                                        SyntaxError($"{m.Value} is not valid method name");
                                        break;
                                }
                            }
                        }
                        else
                        {
                            SyntaxError("Invalid method access");
                        }
                    }

                    return t;
                }
                else
                {
                    SyntaxError("Invalid function call");
                }
            }

            return left;
        }

        private Arguments ParseArguments()
        {
            if (!IsSymbol('('))
            {
                SyntaxError("Missing '(' of arguments");
            }

            NextToken();
            
            var args = new List<ExpressionNode>();

            if (!IsSymbol(')'))
            {
                for (;;)
                {
                    args.Add(ParseExpression());

                    if (IsSymbol(','))
                    {
                        NextToken();
                        continue;
                    }

                    break;
                }
            }

            if (!IsSymbol(')'))
            {
                SyntaxError("Missing ')' of arguments");
            }

            NextToken();
            
            return new Arguments(args);
        }

        private ExpressionNode ParsePrimaryExpression()
        {
            switch (Token)
            {
                case Number n:
                {
                    NextToken();
                    return new ConstantNumber(n.Value);
                }
                case Identifier id:
                {
                    NextToken();
                    return new Variable(id.Value);
                }
                case Symbol s when s.Value == '(':
                {
                    NextToken();
                    var expr = ParseExpression();
                    if (!IsSymbol(')'))
                    {
                        SyntaxError("Missing ')'");
                    }
                    NextToken();
                    return expr;
                }
                case Operator o:
                {
                    SyntaxError($"Unknown operator '{o.Value}'");
                    break;
                }
                case Symbol s:
                {
                    SyntaxError($"Unknown symbol '{s.Value}'");
                    break;
                }
            }
            return null;
        }
    }
}