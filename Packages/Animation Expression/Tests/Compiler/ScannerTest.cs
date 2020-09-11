using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace AnimationExpression.Compiler
{
    public class ScannerTest
    {
        [Test]
        public void Number()
        {
            var t = new Scanner("1.3f").GetToken();
            Assert.That(t, Is.TypeOf<Number>());
            Assert.That((t as Number).Value, Is.EqualTo(1.3f));
        }
        
        [Test]
        public void Numbers()
        {
            var s = new Scanner("1.3f 3.4f");
            var t1 = s.GetToken();
            var t2 = s.GetToken();
            Assert.That(t1, Is.TypeOf<Number>());
            Assert.That((t1 as Number).Value, Is.EqualTo(1.3f));
            Assert.That(t2, Is.TypeOf<Number>());
            Assert.That((t2 as Number).Value, Is.EqualTo(3.4f));
        }
        
        [Test]
        public void Hex()
        {
            var t = new Scanner("0xff").GetToken();
            Assert.That(t, Is.TypeOf<Number>());
            Assert.That((t as Number).Value, Is.EqualTo(255f));
        }
        
        [Test]
        public void Identifier()
        {
            var t = new Scanner("GameObject").GetToken();
            Assert.That(t, Is.TypeOf<Identifier>());
            Assert.That((t as Identifier).Value, Is.EqualTo("GameObject"));
        }
        
        [Test]
        public void Comment()
        {
            var t = new Scanner("// GameObject\nUnityEngine").GetToken();
            Assert.That(t, Is.TypeOf<Identifier>());
            Assert.That((t as Identifier).Value, Is.EqualTo("UnityEngine"));
        }
        
        [Test]
        public void LessEqual()
        {
            var t = new Scanner("<=").GetToken();
            Assert.That(t, Is.TypeOf<Operator>());
            Assert.That((t as Operator).Value, Is.EqualTo('L'));
        }
        
        [Test]
        public void GreaterEqual()
        {
            var t = new Scanner(">=").GetToken();
            Assert.That(t, Is.TypeOf<Operator>());
            Assert.That((t as Operator).Value, Is.EqualTo('G'));
        }
    }
}
