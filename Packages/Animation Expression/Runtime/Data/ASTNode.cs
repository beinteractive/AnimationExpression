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
            { "A", CreateTweenVec1<TweenA> },
            { "Alpha", CreateTweenVec1<TweenA> },
            { "C", CreateTweenVec3<TweenC> },
            { "Color", CreateTweenVec3<TweenC> },
            { "CA", CreateTweenVec4<TweenCA> },
            { "ColorAndAlpha", CreateTweenVec4<TweenCA> },
            { "FillAmount", CreateTweenVec1<TweenFillAmount> },
            { "P", CreateTweenVec2<TweenXY> },
            { "Position", CreateTweenVec2<TweenXY> },
            { "P3", CreateTweenVec3<TweenXYZ> },
            { "Position3D", CreateTweenVec3<TweenXYZ> },
            { "R", CreateTweenVec1<TweenRZ> },
            { "Rotation", CreateTweenVec1<TweenRZ> },
            { "R3", CreateTweenVec3<TweenRXYZ> },
            { "Rotation3D", CreateTweenVec3<TweenRXYZ> },
            { "RX", CreateTweenVec1<TweenRX> },
            { "RotationX", CreateTweenVec1<TweenRX> },
            { "RY", CreateTweenVec1<TweenRY> },
            { "RotationY", CreateTweenVec1<TweenRY> },
            { "RZ", CreateTweenVec1<TweenRX> },
            { "RotationZ", CreateTweenVec1<TweenRZ> },
            { "RXY", CreateTweenVec2<TweenRXY> },
            { "RotationXY", CreateTweenVec2<TweenRXY> },
            { "RXZ", CreateTweenVec2<TweenRXZ> },
            { "RotationXZ", CreateTweenVec2<TweenRXZ> },
            { "RYZ", CreateTweenVec2<TweenRYZ> },
            { "RotationYZ", CreateTweenVec2<TweenRYZ> },
            { "RXYZ", CreateTweenVec3<TweenRXYZ> },
            { "RotationXYZ", CreateTweenVec3<TweenRXYZ> },
            { "S", CreateTweenVec2<TweenSXY> },
            { "Scale", CreateTweenVec2<TweenSXY> },
            { "S3", CreateTweenVec3<TweenSXYZ> },
            { "Scale3D", CreateTweenVec3<TweenSXYZ> },
            { "SX", CreateTweenVec1<TweenSX> },
            { "ScaleX", CreateTweenVec1<TweenSX> },
            { "SY", CreateTweenVec1<TweenSY> },
            { "ScaleY", CreateTweenVec1<TweenSY> },
            { "SZ", CreateTweenVec1<TweenSX> },
            { "ScaleZ", CreateTweenVec1<TweenSZ> },
            { "SXY", CreateTweenVec2<TweenSXY> },
            { "ScaleXY", CreateTweenVec2<TweenSXY> },
            { "SXZ", CreateTweenVec2<TweenSXZ> },
            { "ScaleXZ", CreateTweenVec2<TweenSXZ> },
            { "SYZ", CreateTweenVec2<TweenSYZ> },
            { "ScaleYZ", CreateTweenVec2<TweenSYZ> },
            { "SXYZ", CreateTweenVec3<TweenSXYZ> },
            { "ScaleXYZ", CreateTweenVec3<TweenSXYZ> },
            { "X", CreateTweenVec1<TweenX> },
            { "PositionX", CreateTweenVec1<TweenX> },
            { "Y", CreateTweenVec1<TweenY> },
            { "PositionY", CreateTweenVec1<TweenY> },
            { "Z", CreateTweenVec1<TweenX> },
            { "PositionZ", CreateTweenVec1<TweenZ> },
            { "XY", CreateTweenVec2<TweenXY> },
            { "PositionXY", CreateTweenVec2<TweenXY> },
            { "XZ", CreateTweenVec2<TweenXZ> },
            { "PositionXZ", CreateTweenVec2<TweenXZ> },
            { "YZ", CreateTweenVec2<TweenYZ> },
            { "PositionYZ", CreateTweenVec2<TweenYZ> },
            { "XYZ", CreateTweenVec3<TweenXYZ> },
            { "PositionXYZ", CreateTweenVec3<TweenXYZ> },
        };

        private static Tween CreateTweenVec1<T>(EvaluateContext ctx, Arguments args) where T : TweenVec1, new()
        {
            var g = ((Variable) args.Args[0]).GetGameObject(ctx);
            var d = args.Args[1].EvaluateAsNumber(ctx);
            
            switch (args.Args.Count)
            {
                case 2:
                {
                    return TweenVec1.Add<T>(g, d);
                }
                default:
                    throw new ArgumentException("Invalid size of arguments");
            }
        }

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

        private static Tween CreateTweenVec3<T>(EvaluateContext ctx, Arguments args) where T : TweenVec3, new()
        {
            var g = ((Variable) args.Args[0]).GetGameObject(ctx);
            var d = args.Args[1].EvaluateAsNumber(ctx);
            
            switch (args.Args.Count)
            {
                case 2:
                {
                    return TweenVec3.Add<T>(g, d);
                }
                case 3:
                {
                    var v = args.Args[2].EvaluateAsNumber(ctx);
                    return TweenVec3.Add<T>(g, d, v, v, v);
                }
                case 5:
                {
                    var v1 = args.Args[2].EvaluateAsNumber(ctx);
                    var v2 = args.Args[3].EvaluateAsNumber(ctx);
                    var v3 = args.Args[4].EvaluateAsNumber(ctx);
                    return TweenVec3.Add<T>(g, d, v1, v2, v3);
                }
                default:
                    throw new ArgumentException("Invalid size of arguments");
            }
        }

        private static Tween CreateTweenVec4<T>(EvaluateContext ctx, Arguments args) where T : TweenVec4, new()
        {
            var g = ((Variable) args.Args[0]).GetGameObject(ctx);
            var d = args.Args[1].EvaluateAsNumber(ctx);
            
            switch (args.Args.Count)
            {
                case 2:
                {
                    return TweenVec4.Add<T>(g, d);
                }
                case 3:
                {
                    var v = args.Args[2].EvaluateAsNumber(ctx);
                    return TweenVec4.Add<T>(g, d, v, v, v, v);
                }
                case 6:
                {
                    var v1 = args.Args[2].EvaluateAsNumber(ctx);
                    var v2 = args.Args[3].EvaluateAsNumber(ctx);
                    var v3 = args.Args[4].EvaluateAsNumber(ctx);
                    var v4 = args.Args[5].EvaluateAsNumber(ctx);
                    return TweenVec4.Add<T>(g, d, v1, v2, v3, v4);
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
        public readonly bool Relative;

        public From(TweenNode expr, Arguments args, bool relative)
        {
            Expr = expr;
            Args = args;
            Relative = relative;
        }

        public override Tween EvaluateAsTween(EvaluateContext ctx)
        {
            var tween = Expr.EvaluateAsTween(ctx);
            
            switch (tween)
            {
                case TweenVec1 t:
                {
                    if (Relative)
                    {
                        t.FromRelative(Args.Args[0].EvaluateAsNumber(ctx));
                    }
                    else
                    {
                        t.From(Args.Args[0].EvaluateAsNumber(ctx));
                    }
                    break;
                }
                case TweenVec2 t:
                {
                    switch (Args.Args.Count)
                    {
                        case 1:
                            if (Relative)
                            {
                                t.FromRelative(Args.Args[0].EvaluateAsNumber(ctx));
                            }
                            else
                            {
                                t.From(Args.Args[0].EvaluateAsNumber(ctx));
                            }
                            break;
                        default:
                            if (Relative)
                            {
                                t.From(Args.Args[0].EvaluateAsNumber(ctx), Args.Args[1].EvaluateAsNumber(ctx));
                            }
                            else
                            {
                                t.FromRelative(Args.Args[0].EvaluateAsNumber(ctx), Args.Args[1].EvaluateAsNumber(ctx));
                            }
                            break;
                    }
                    break;
                }
                case TweenVec3 t:
                {
                    switch (Args.Args.Count)
                    {
                        case 1:
                            if (Relative)
                            {
                                t.FromRelative(Args.Args[0].EvaluateAsNumber(ctx));
                            }
                            else
                            {
                                t.From(Args.Args[0].EvaluateAsNumber(ctx));
                            }
                            break;
                        default:
                            if (Relative)
                            {
                                t.From(Args.Args[0].EvaluateAsNumber(ctx), Args.Args[1].EvaluateAsNumber(ctx), Args.Args[2].EvaluateAsNumber(ctx));
                            }
                            else
                            {
                                t.FromRelative(Args.Args[0].EvaluateAsNumber(ctx), Args.Args[1].EvaluateAsNumber(ctx), Args.Args[2].EvaluateAsNumber(ctx));
                            }
                            break;
                    }
                    break;
                }
                case TweenVec4 t:
                {
                    switch (Args.Args.Count)
                    {
                        case 1:
                            if (Relative)
                            {
                                t.FromRelative(Args.Args[0].EvaluateAsNumber(ctx));
                            }
                            else
                            {
                                t.From(Args.Args[0].EvaluateAsNumber(ctx));
                            }
                            break;
                        default:
                            if (Relative)
                            {
                                t.From(Args.Args[0].EvaluateAsNumber(ctx), Args.Args[1].EvaluateAsNumber(ctx), Args.Args[2].EvaluateAsNumber(ctx), Args.Args[3].EvaluateAsNumber(ctx));
                            }
                            else
                            {
                                t.FromRelative(Args.Args[0].EvaluateAsNumber(ctx), Args.Args[1].EvaluateAsNumber(ctx), Args.Args[2].EvaluateAsNumber(ctx), Args.Args[3].EvaluateAsNumber(ctx));
                            }
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

    public class Animate : TweenNode
    {
        public readonly TweenNode Expr;
        public readonly Arguments Args;

        public Animate(TweenNode expr, Arguments args)
        {
            Expr = expr;
            Args = args;
        }

        public override Tween EvaluateAsTween(EvaluateContext ctx)
        {
            var tween = Expr.EvaluateAsTween(ctx);
            tween.Animate(Args.Args[0].EvaluateAsBool(ctx));
            return tween;
        }
    }

    public class Then : TweenNode
    {
        public readonly TweenNode Expr;
        public readonly ASTNode Body;

        public Then(TweenNode expr, ASTNode body)
        {
            Expr = expr;
            Body = body;
        }

        public override Tween EvaluateAsTween(EvaluateContext ctx)
        {
            var tween = Expr.EvaluateAsTween(ctx);
            tween.Then(() => Body.Evaluate(ctx));
            return tween;
        }
    }

    public class SetActive : ExpressionNode
    {
        public readonly Arguments Args;

        public SetActive(Arguments args)
        {
            Args = args;
        }

        public override void Evaluate(EvaluateContext ctx)
        {
            var g = ((Variable) Args.Args[0]).GetGameObject(ctx);
            var active = Args.Args[1].EvaluateAsBool(ctx);
            
            g.SetActive(active);
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

    public class Callback : ExpressionNode
    {
        public override void Evaluate(EvaluateContext ctx)
        {
            ctx.Callback();
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