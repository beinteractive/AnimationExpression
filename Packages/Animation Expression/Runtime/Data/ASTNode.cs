using System;
using System.Collections.Generic;
using UnityEngine;
using Uween;

namespace AnimationExpression.Data
{
    public abstract class ASTNode
    {
        public abstract void Evaluate(EvaluateContext ctx);
    }

    public class Statements : ASTNode
    {
        public readonly List<ASTNode> Nodes;

        public Statements(List<ASTNode> nodes)
        {
            Nodes = nodes;
        }

        public override void Evaluate(EvaluateContext ctx)
        {
            foreach (var node in Nodes)
            {
                node.Evaluate(ctx);
            }
        }
    }

    public class IfStatement : ASTNode
    {
        public readonly ExpressionNode Cond;
        public readonly ASTNode Body;

        public IfStatement(ExpressionNode cond, ASTNode body)
        {
            Cond = cond;
            Body = body;
        }

        public override void Evaluate(EvaluateContext ctx)
        {
            if (Cond.EvaluateAsBool(ctx))
            {
                Body.Evaluate(ctx);
            }
        }
    }

    public abstract class ExpressionNode : ASTNode
    {
        public override void Evaluate(EvaluateContext ctx)
        {
            EvaluateAsNumber(ctx);
        }

        public virtual float EvaluateAsNumber(EvaluateContext ctx) => EvaluateAsBool(ctx) ? 1f : 0f;
        public virtual bool EvaluateAsBool(EvaluateContext ctx) => EvaluateAsNumber(ctx) > 0f;
    }

    public class Conditional : ExpressionNode
    {
        public readonly ExpressionNode Condition;
        public readonly ExpressionNode Left;
        public readonly ExpressionNode Right;
        
        public Conditional(ExpressionNode cond, ExpressionNode left, ExpressionNode right)
        {
            Condition = cond;
            Left = left;
            Right = right;
        }

        public override float EvaluateAsNumber(EvaluateContext ctx)
        {
            return Condition.EvaluateAsBool(ctx) ? Left.EvaluateAsNumber(ctx) : Right.EvaluateAsNumber(ctx);
        }
    }

    public class Or : ExpressionNode
    {
        public readonly ExpressionNode Left;
        public readonly ExpressionNode Right;

        public Or(ExpressionNode left, ExpressionNode right)
        {
            Left = left;
            Right = right;
        }

        public override bool EvaluateAsBool(EvaluateContext ctx)
        {
            return Left.EvaluateAsBool(ctx) || Right.EvaluateAsBool(ctx);
        }
    }

    public class And : ExpressionNode
    {
        public readonly ExpressionNode Left;
        public readonly ExpressionNode Right;

        public And(ExpressionNode left, ExpressionNode right)
        {
            Left = left;
            Right = right;
        }

        public override bool EvaluateAsBool(EvaluateContext ctx)
        {
            return Left.EvaluateAsBool(ctx) && Right.EvaluateAsBool(ctx);
        }
    }

    public class Equal : ExpressionNode
    {
        public readonly ExpressionNode Left;
        public readonly ExpressionNode Right;
        
        public Equal(ExpressionNode left, ExpressionNode right)
        {
            Left = left;
            Right = right;
        }

        public override bool EvaluateAsBool(EvaluateContext ctx)
        {
            return Math.Abs(Left.EvaluateAsNumber(ctx) - Right.EvaluateAsNumber(ctx)) < Mathf.Epsilon;
        }
    }

    public class NotEqual : ExpressionNode
    {
        public readonly ExpressionNode Left;
        public readonly ExpressionNode Right;
        
        public NotEqual(ExpressionNode left, ExpressionNode right)
        {
            Left = left;
            Right = right;
        }

        public override bool EvaluateAsBool(EvaluateContext ctx)
        {
            return Math.Abs(Left.EvaluateAsNumber(ctx) - Right.EvaluateAsNumber(ctx)) >= Mathf.Epsilon;
        }
    }

    public class LessThan : ExpressionNode
    {
        public readonly ExpressionNode Left;
        public readonly ExpressionNode Right;
        
        public LessThan(ExpressionNode left, ExpressionNode right)
        {
            Left = left;
            Right = right;
        }

        public override bool EvaluateAsBool(EvaluateContext ctx)
        {
            return Left.EvaluateAsNumber(ctx) < Right.EvaluateAsNumber(ctx);
        }
    }

    public class GreaterThan : ExpressionNode
    {
        public readonly ExpressionNode Left;
        public readonly ExpressionNode Right;
        
        public GreaterThan(ExpressionNode left, ExpressionNode right)
        {
            Left = left;
            Right = right;
        }

        public override bool EvaluateAsBool(EvaluateContext ctx)
        {
            return Left.EvaluateAsNumber(ctx) > Right.EvaluateAsNumber(ctx);
        }
    }

    public class LessEqual : ExpressionNode
    {
        public readonly ExpressionNode Left;
        public readonly ExpressionNode Right;
        
        public LessEqual(ExpressionNode left, ExpressionNode right)
        {
            Left = left;
            Right = right;
        }

        public override bool EvaluateAsBool(EvaluateContext ctx)
        {
            return Left.EvaluateAsNumber(ctx) <= Right.EvaluateAsNumber(ctx);
        }
    }

    public class GreaterEqual : ExpressionNode
    {
        public readonly ExpressionNode Left;
        public readonly ExpressionNode Right;
        
        public GreaterEqual(ExpressionNode left, ExpressionNode right)
        {
            Left = left;
            Right = right;
        }

        public override bool EvaluateAsBool(EvaluateContext ctx)
        {
            return Left.EvaluateAsNumber(ctx) >= Right.EvaluateAsNumber(ctx);
        }
    }

    public class Add : ExpressionNode
    {
        public readonly ExpressionNode Left;
        public readonly ExpressionNode Right;
        
        public Add(ExpressionNode left, ExpressionNode right)
        {
            Left = left;
            Right = right;
        }

        public override float EvaluateAsNumber(EvaluateContext ctx)
        {
            return Left.EvaluateAsNumber(ctx) + Right.EvaluateAsNumber(ctx);
        }
    }

    public class Sub : ExpressionNode
    {
        public readonly ExpressionNode Left;
        public readonly ExpressionNode Right;
        
        public Sub(ExpressionNode left, ExpressionNode right)
        {
            Left = left;
            Right = right;
        }

        public override float EvaluateAsNumber(EvaluateContext ctx)
        {
            return Left.EvaluateAsNumber(ctx) - Right.EvaluateAsNumber(ctx);
        }
    }

    public class Mul : ExpressionNode
    {
        public readonly ExpressionNode Left;
        public readonly ExpressionNode Right;
        
        public Mul(ExpressionNode left, ExpressionNode right)
        {
            Left = left;
            Right = right;
        }

        public override float EvaluateAsNumber(EvaluateContext ctx)
        {
            return Left.EvaluateAsNumber(ctx) * Right.EvaluateAsNumber(ctx);
        }
    }

    public class Div : ExpressionNode
    {
        public readonly ExpressionNode Left;
        public readonly ExpressionNode Right;
        
        public Div(ExpressionNode left, ExpressionNode right)
        {
            Left = left;
            Right = right;
        }

        public override float EvaluateAsNumber(EvaluateContext ctx)
        {
            return Left.EvaluateAsNumber(ctx) / Right.EvaluateAsNumber(ctx);
        }
    }

    public class Negate : ExpressionNode
    {
        public readonly ExpressionNode Expr;

        public Negate(ExpressionNode expr)
        {
            Expr = expr;
        }

        public override float EvaluateAsNumber(EvaluateContext ctx)
        {
            return -Expr.EvaluateAsNumber(ctx);
        }
    }

    public class Not : ExpressionNode
    {
        public readonly ExpressionNode Expr;

        public Not(ExpressionNode expr)
        {
            Expr = expr;
        }

        public override bool EvaluateAsBool(EvaluateContext ctx)
        {
            return !Expr.EvaluateAsBool(ctx);
        }
    }

    public class Arguments : ASTNode
    {
        public readonly List<ExpressionNode> Args;

        public Arguments(List<ExpressionNode> args)
        {
            Args = args;
        }
        
        public override void Evaluate(EvaluateContext ctx)
        {
        }
    }

    public abstract class TweenNode : ExpressionNode
    {
        public abstract Tween EvaluateAsTween(EvaluateContext ctx);

        public override void Evaluate(EvaluateContext ctx)
        {
            EvaluateAsTween(ctx);
        }

        public override float EvaluateAsNumber(EvaluateContext ctx)
        {
            return 0f;
        }

        public override bool EvaluateAsBool(EvaluateContext ctx)
        {
            return true;
        }
    }

    public class CallTween : TweenNode
    {
        public readonly string Id;
        public readonly Arguments Args;

        public CallTween(string id, Arguments args)
        {
            Id = id;
            Args = args;
        }

        public static bool IsValidId(string id)
        {
            return Factory.ContainsKey(id);
        }
        
        private static readonly Dictionary<string, Func<EvaluateContext, Arguments, Tween>> Factory = new Dictionary<string, Func<EvaluateContext, Arguments, Tween>>
        {
            { "S", CreateTweenVec2<TweenSXY> },
            { "SXY", CreateTweenVec2<TweenSXY> },
            { "Scale", CreateTweenVec2<TweenSXY> },
        };

        private static Tween CreateTweenVec2<T>(EvaluateContext ctx, Arguments args) where T : TweenVec2, new()
        {
            var g = ((Variable) args.Args[0]).GetGameObject(ctx);
            var d = args.Args[1].EvaluateAsNumber(ctx);
            
            switch (args.Args.Count)
            {
                case 2:
                {
                    return TweenVec2.Add<T>(g, d);
                }
                case 3:
                {
                    var v = args.Args[2].EvaluateAsNumber(ctx);
                    return TweenVec2.Add<T>(g, d, v, v);
                }
                case 4:
                {
                    var v1 = args.Args[2].EvaluateAsNumber(ctx);
                    var v2 = args.Args[3].EvaluateAsNumber(ctx);
                    return TweenVec2.Add<T>(g, d, v1, v2);
                }
                default:
                    throw new ArgumentException("Invalid size of arguments");
            }
        }

        public override Tween EvaluateAsTween(EvaluateContext ctx)
        {
            return Factory[Id].Invoke(ctx, Args);
        }
    }

    public class From : TweenNode
    {
        public readonly TweenNode Expr;
        public readonly Arguments Args;

        public From(TweenNode expr, Arguments args)
        {
            Expr = expr;
            Args = args;
        }

        public override Tween EvaluateAsTween(EvaluateContext ctx)
        {
            var tween = Expr.EvaluateAsTween(ctx);
            
            switch (tween)
            {
                case TweenVec2 tv2:
                {
                    switch (Args.Args.Count)
                    {
                        case 1:
                            tv2.From(Args.Args[0].EvaluateAsNumber(ctx));
                            break;
                        case 2:
                            tv2.From(Args.Args[0].EvaluateAsNumber(ctx), Args.Args[1].EvaluateAsNumber(ctx));
                            break;
                    }
                    break;
                }
            }

            return tween;
        }
    }

    public class Ease : TweenNode
    {
        public readonly string Id;
        public readonly TweenNode Expr;
        public readonly Arguments Args;

        public Ease(string id, TweenNode expr, Arguments args)
        {
            Id = id;
            Expr = expr;
            Args = args;
        }

        private static readonly Dictionary<string, Action<Tween>> Map = new Dictionary<string, Action<Tween>>()
        {
            {"EaseInBack", _ => _.EaseInBack()},
            {"EaseInOutBack", _ => _.EaseInOutBack()},
            {"EaseOutBack", _ => _.EaseOutBack()},
            {"EaseOutInBack", _ => _.EaseOutInBack()},
            {"EaseInBounce", _ => _.EaseInBounce()},
            {"EaseInOutBounce", _ => _.EaseInOutBounce()},
            {"EaseOutBounce", _ => _.EaseOutBounce()},
            {"EaseOutInBounce", _ => _.EaseOutInBounce()},
            {"EaseInCircular", _ => _.EaseInCircular()},
            {"EaseInOutCircular", _ => _.EaseInOutCircular()},
            {"EaseOutCircular", _ => _.EaseOutCircular()},
            {"EaseOutInCircular", _ => _.EaseOutInCircular()},
            {"EaseInCirc", _ => _.EaseInCirc()},
            {"EaseInOutCirc", _ => _.EaseInOutCirc()},
            {"EaseOutCirc", _ => _.EaseOutCirc()},
            {"EaseOutInCirc", _ => _.EaseOutInCirc()},
            {"EaseInCubic", _ => _.EaseInCubic()},
            {"EaseInOutCubic", _ => _.EaseInOutCubic()},
            {"EaseOutCubic", _ => _.EaseOutCubic()},
            {"EaseOutInCubic", _ => _.EaseOutInCubic()},
            {"EaseInElastic", _ => _.EaseInElastic()},
            {"EaseInOutElastic", _ => _.EaseInOutElastic()},
            {"EaseOutElastic", _ => _.EaseOutElastic()},
            {"EaseOutInElastic", _ => _.EaseOutInElastic()},
            {"EaseInExponential", _ => _.EaseInExponential()},
            {"EaseInOutExponential", _ => _.EaseInOutExponential()},
            {"EaseOutExponential", _ => _.EaseOutExponential()},
            {"EaseOutInExponential", _ => _.EaseOutInExponential()},
            {"EaseInExpo", _ => _.EaseInExpo()},
            {"EaseInOutExpo", _ => _.EaseInOutExpo()},
            {"EaseOutExpo", _ => _.EaseOutExpo()},
            {"EaseOutInExpo", _ => _.EaseOutInExpo()},
            {"EaseInQuadratic", _ => _.EaseInQuadratic()},
            {"EaseInOutQuadratic", _ => _.EaseInOutQuadratic()},
            {"EaseOutQuadratic", _ => _.EaseOutQuadratic()},
            {"EaseOutInQuadratic", _ => _.EaseOutInQuadratic()},
            {"EaseInQuad", _ => _.EaseInQuad()},
            {"EaseInOutQuad", _ => _.EaseInOutQuad()},
            {"EaseOutQuad", _ => _.EaseOutQuad()},
            {"EaseOutInQuad", _ => _.EaseOutInQuad()},
            {"EaseInQuartic", _ => _.EaseInQuartic()},
            {"EaseInOutQuartic", _ => _.EaseInOutQuartic()},
            {"EaseOutQuartic", _ => _.EaseOutQuartic()},
            {"EaseOutInQuartic", _ => _.EaseOutInQuartic()},
            {"EaseInQuart", _ => _.EaseInQuart()},
            {"EaseInOutQuart", _ => _.EaseInOutQuart()},
            {"EaseOutQuart", _ => _.EaseOutQuart()},
            {"EaseOutInQuart", _ => _.EaseOutInQuart()},
            {"EaseInQuintic", _ => _.EaseInQuintic()},
            {"EaseInOutQuintic", _ => _.EaseInOutQuintic()},
            {"EaseOutQuintic", _ => _.EaseOutQuintic()},
            {"EaseOutInQuintic", _ => _.EaseOutInQuintic()},
            {"EaseInQuint", _ => _.EaseInQuint()},
            {"EaseInOutQuint", _ => _.EaseInOutQuint()},
            {"EaseOutQuint", _ => _.EaseOutQuint()},
            {"EaseOutInQuint", _ => _.EaseOutInQuint()},
            {"EaseInSine", _ => _.EaseInSine()},
            {"EaseInOutSine", _ => _.EaseInOutSine()},
            {"EaseOutSine", _ => _.EaseOutSine()},
            {"EaseOutInSine", _ => _.EaseOutInSine()},
        };

        public static bool IsValidName(string name)
        {
            return Map.ContainsKey(name);
        }

        public override Tween EvaluateAsTween(EvaluateContext ctx)
        {
            var tween = Expr.EvaluateAsTween(ctx);
            Map[Id].Invoke(tween);
            return tween;
        }
    }

    public class Delay : TweenNode
    {
        public readonly TweenNode Expr;
        public readonly Arguments Args;

        public Delay(TweenNode expr, Arguments args)
        {
            Expr = expr;
            Args = args;
        }

        public override Tween EvaluateAsTween(EvaluateContext ctx)
        {
            var tween = Expr.EvaluateAsTween(ctx);
            tween.Delay(Args.Args[0].EvaluateAsNumber(ctx));
            return tween;
        }
    }

    public class ConstantNumber : ExpressionNode
    {
        public readonly float Value;
        
        public ConstantNumber(float val)
        {
            Value = val;
        }
        
        public override float EvaluateAsNumber(EvaluateContext ctx)
        {
            return Value;
        }
    }

    public class Variable : ExpressionNode
    {
        public readonly string Value;

        public Variable(string val)
        {
            Value = val;
        }

        public GameObject GetGameObject(EvaluateContext ctx)
        {
            return ctx.ResolveGameObject(Value);
        }

        public override float EvaluateAsNumber(EvaluateContext ctx)
        {
            return ctx.ResolveVariable(Value);
        }
    }
}