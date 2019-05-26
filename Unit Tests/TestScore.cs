using NUnit.Framework;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Tetris
{
    [TestFixture]
    class TestScore
    {
        [Test]
        public void TestScoreDefaultConstructor()
        {
            Score score = new Score("Aqua", 300, Character.Aqua);

            Assert.AreEqual("Aqua", score.Name);
            Assert.AreEqual(300, score.Point);
            Assert.AreEqual(Character.Aqua, score.Character);
        }

        [Test]
        public void TestName()
        {
            Score score = new Score("Aqua", 300, Character.Aqua);

            //checked that the name is Aqua before change
            Assert.AreEqual("Aqua", score.Name);

            //change name directly to the property
            score.Name = "Jeff";

            Assert.AreEqual("Jeff", score.Name);
        }

        [Test]
        public void TestPoint()
        {
            Score score = new Score("Aqua", 300, Character.Aqua);

            //checked that the point scored is 300 before change
            Assert.AreEqual(300, score.Point);

            //change point directly to the property
            score.Point = 500;

            Assert.AreEqual(500, score.Point);
        }

        [Test]
        public void TestCharacter()
        {
            Score score = new Score("Aqua", 300, Character.Aqua);

            //checked that the character is Aqua before change
            Assert.AreEqual(Character.Aqua, score.Character);

            //change character directly to the property
            score.Character = Character.Megumin;

            Assert.AreEqual(Character.Megumin, score.Character);
        }

        [Test]
        public void TestCompareTo()
        {
            Score score = new Score("Aqua", 300, Character.Aqua);
            Score score2 = new Score("Harry", 500, Character.Megumin);

            //compare the difference between two score objects
            Assert.AreEqual(200, score.CompareTo(score2));

            //to check that it returns 0 if the parameter is not a score object
            Assert.AreEqual(0, score.CompareTo(500));
        }
    }
}
