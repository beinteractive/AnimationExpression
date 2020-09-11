using UnityEngine;

namespace AnimationExpression.Data
{
    public abstract class EvaluateContext
    {
        public abstract GameObject ResolveGameObject(string name);
        public abstract float ResolveVariable(string name);
        public abstract void Callback();
    }
}