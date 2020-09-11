using AnKuchen.Map;
using UnityEngine;

namespace AnimationExpression.Components
{
    public class AnimationExpressionPreview : MonoBehaviour
    {
        [SerializeField] public UICache LiveTarget;
        [SerializeField] public Data.AnimationExpression AnimationExpression;
        
        public void Preview()
        {
            if (LiveTarget == null || AnimationExpression == null)
            {
                return;
            }
            
            AnimationExpression.Run(LiveTarget, () => Debug.Log("Callback"));
        }
    }
}