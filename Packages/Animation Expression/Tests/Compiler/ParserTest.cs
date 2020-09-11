using NUnit.Framework;

namespace AnimationExpression.Compiler
{
    public class ParserTest
    {
        [Test]
        public void SimpleAdd()
        {
            var expr = new Parser(new Scanner("1 + 2")).ParseOnlyExpression();
            Assert.That(expr.EvaluateAsNumber(null), Is.EqualTo(3f));
        }
        
        [Test]
        public void SimpleAdds()
        {
            var expr = new Parser(new Scanner("1 + 2 + 3")).ParseOnlyExpression();
            Assert.That(expr.EvaluateAsNumber(null), Is.EqualTo(6f));
        }
        
        [Test]
        public void MulSub()
        {
            var expr = new Parser(new Scanner("6 - 3 * 2")).ParseOnlyExpression();
            Assert.That(expr.EvaluateAsNumber(null), Is.EqualTo(0f));
        }
        
        [Test]
        public void Priority()
        {
            var expr = new Parser(new Scanner("(6 - 3) * 2")).ParseOnlyExpression();
            Assert.That(expr.EvaluateAsNumber(null), Is.EqualTo(6f));
        }
        
        [Test]
        public void Negate()
        {
            var expr = new Parser(new Scanner("-3")).ParseOnlyExpression();
            Assert.That(expr.EvaluateAsNumber(null), Is.EqualTo(-3f));
        }
        
        [Test]
        public void Not()
        {
            var expr = new Parser(new Scanner("!1")).ParseOnlyExpression();
            Assert.That(expr.EvaluateAsBool(null), Is.EqualTo(false));
        }
        
        [Test]
        public void ConditionalTrue()
        {
            var expr = new Parser(new Scanner("1f ? 2f : 3f")).ParseOnlyExpression();
            Assert.That(expr.EvaluateAsNumber(null), Is.EqualTo(2f));
        }
        
        [Test]
        public void ConditionalFalse()
        {
            var expr = new Parser(new Scanner("0f ? 2f : 3f")).ParseOnlyExpression();
            Assert.That(expr.EvaluateAsNumber(null), Is.EqualTo(3f));
        }
        
        [Test]
        public void LessThanTrue()
        {
            var expr = new Parser(new Scanner("1 < 2")).ParseOnlyExpression();
            Assert.That(expr.EvaluateAsBool(null), Is.EqualTo(true));
        }
        
        [Test]
        public void LessThanFalse()
        {
            var expr = new Parser(new Scanner("2 < 1")).ParseOnlyExpression();
            Assert.That(expr.EvaluateAsBool(null), Is.EqualTo(false));
        }
        
        [Test]
        public void GreaterThanTrue()
        {
            var expr = new Parser(new Scanner("2 > 1")).ParseOnlyExpression();
            Assert.That(expr.EvaluateAsBool(null), Is.EqualTo(true));
        }
        
        [Test]
        public void GreaterThanFalse()
        {
            var expr = new Parser(new Scanner("1 > 2")).ParseOnlyExpression();
            Assert.That(expr.EvaluateAsBool(null), Is.EqualTo(false));
        }
        
        [Test]
        public void LessEqualTrue()
        {
            var exprA = new Parser(new Scanner("2 <= 2")).ParseOnlyExpression();
            Assert.That(exprA.EvaluateAsBool(null), Is.EqualTo(true));
            var exprB = new Parser(new Scanner("1 <= 2")).ParseOnlyExpression();
            Assert.That(exprB.EvaluateAsBool(null), Is.EqualTo(true));
        }

        [Test]
        public void LessEqualFalse()
        {
            var expr = new Parser(new Scanner("2 <= 1")).ParseOnlyExpression();
            Assert.That(expr.EvaluateAsBool(null), Is.EqualTo(false));
        }
        
        [Test]
        public void GreaterEqualTrue()
        {
            var exprA = new Parser(new Scanner("2 >= 2")).ParseOnlyExpression();
            Assert.That(exprA.EvaluateAsBool(null), Is.EqualTo(true));
            var exprB = new Parser(new Scanner("2 >= 1")).ParseOnlyExpression();
            Assert.That(exprB.EvaluateAsBool(null), Is.EqualTo(true));
        }

        [Test]
        public void GreaterEqualFalse()
        {
            var expr = new Parser(new Scanner("1 >= 2")).ParseOnlyExpression();
            Assert.That(expr.EvaluateAsBool(null), Is.EqualTo(false));
        }
        
        [Test]
        public void EqualTrue()
        {
            var expr = new Parser(new Scanner("1 == 1")).ParseOnlyExpression();
            Assert.That(expr.EvaluateAsBool(null), Is.EqualTo(true));
        }
        
        [Test]
        public void EqualFalse()
        {
            var expr = new Parser(new Scanner("1 == 2")).ParseOnlyExpression();
            Assert.That(expr.EvaluateAsBool(null), Is.EqualTo(false));
        }
        
        [Test]
        public void NotEqualTrue()
        {
            var expr = new Parser(new Scanner("1 != 2")).ParseOnlyExpression();
            Assert.That(expr.EvaluateAsBool(null), Is.EqualTo(true));
        }
        
        [Test]
        public void NotEqualFalse()
        {
            var expr = new Parser(new Scanner("1 != 1")).ParseOnlyExpression();
            Assert.That(expr.EvaluateAsBool(null), Is.EqualTo(false));
        }
        
        [Test]
        public void OrTrue()
        {
            var expr = new Parser(new Scanner("1 == 2 || 1 == 1")).ParseOnlyExpression();
            Assert.That(expr.EvaluateAsBool(null), Is.EqualTo(true));
        }
        
        [Test]
        public void OrFalse()
        {
            var expr = new Parser(new Scanner("1 == 2 || 2 == 3")).ParseOnlyExpression();
            Assert.That(expr.EvaluateAsBool(null), Is.EqualTo(false));
        }
        
        [Test]
        public void AndTrue()
        {
            var expr = new Parser(new Scanner("1 == 1 && 2 == 2")).ParseOnlyExpression();
            Assert.That(expr.EvaluateAsBool(null), Is.EqualTo(true));
        }
        
        [Test]
        public void AndFalse()
        {
            var expr = new Parser(new Scanner("1 == 1 && 2 == 3")).ParseOnlyExpression();
            Assert.That(expr.EvaluateAsBool(null), Is.EqualTo(false));
        }
    }
}