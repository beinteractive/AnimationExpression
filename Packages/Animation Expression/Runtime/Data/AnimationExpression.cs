using System;
using System.Collections.Generic;
using System.Linq;
using AnimationExpression.Compiler;
using AnKuchen.Map;
using UnityEngine;

namespace AnimationExpression.Data
{
    [CreateAssetMenu]
    public class AnimationExpression : ScriptableObject
    {
        [Serializable]
        public class ObjectMap
        {
            public string Name;
            public string Path;
        }
        
        [Serializable]
        public class Variable
        {
            public string Name;
            public float Value;
        }

        public List<ObjectMap> ObjectMaps;
        public List<Variable> Variables;
        public string Expression;

        public void Run(IMapper target, Action callback = null)
        {
            var program = Compile();
            var ctx = new Context(this, target, callback);
            program.Evaluate(ctx);
        }

        private ASTNode Compile()
        {
            return new Parser(new Scanner(Expression)).Parse();
        }

        private class Context : EvaluateContext
        {
            public readonly AnimationExpression Self;
            public readonly IMapper Target;
            public readonly Action CallbackAction;

            public Context(AnimationExpression self, IMapper target, Action callback)
            {
                Self = self;
                Target = target;
                CallbackAction = callback;
            }

            public override GameObject ResolveGameObject(string name)
            {
                var path = name;
                var e = Self.ObjectMaps.FirstOrDefault(_ => _.Name == name);
                if (e != null)
                {
                    path = e.Path;
                }
                return Target.Get(path);
            }

            public override float ResolveVariable(string name)
            {
                return Self.Variables.FirstOrDefault(_ => _.Name == name)?.Value ?? 0f;
            }

            public override void Callback()
            {
                CallbackAction?.Invoke();
            }
        }
    }
}