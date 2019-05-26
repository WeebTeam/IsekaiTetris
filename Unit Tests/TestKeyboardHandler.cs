using NUnit.Framework;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Tetris
{
    [TestFixture]
    class TestKeyboardHandler
    {
        [Test]
        public void TestKeyboardHanlderDefaultConstructor()
        {
            KeyboardHandler keyboardHandler = new KeyboardHandler();

            //to check that the name should be empty after creating the object
            Assert.AreEqual(string.Empty, keyboardHandler.Name);

            //to check that the counter of number of characters is 15
            Assert.AreEqual(0, keyboardHandler.Count);
        }

        [Test]
        public void TestName()
        {
            KeyboardHandler keyboardHandler = new KeyboardHandler();

            keyboardHandler.Name = "lolipoplls";

            //check if the name is correct
            Assert.AreEqual("lolipoplls", keyboardHandler.Name);
        }

        [Test]
        public void TestKeys()
        {
            KeyboardHandler keyboardHandler = new KeyboardHandler();

            //typed B && C
            Keys key1 = Keys.B;
            Keys key2 = Keys.C;

            keyboardHandler.OnKeyDown(key1);
            keyboardHandler.OnKeyDown(key2);

            //check is B and C entered or not
            Assert.AreEqual("BC", keyboardHandler.Name);
        }

        [Test]
        public void TestExceptionKeys()
        {
            KeyboardHandler keyboardHandler = new KeyboardHandler();

            //typed A
            Keys key1 = Keys.A;
            keyboardHandler.OnKeyDown(key1);

            #region the exception keys should return nothing when typed
            Keys key2 = Keys.Enter;
            keyboardHandler.OnKeyDown(key2);

            Keys key3 = Keys.CapsLock;
            keyboardHandler.OnKeyDown(key3);

            Keys key4 = Keys.Tab;
            keyboardHandler.OnKeyDown(key4);

            Keys key5 = Keys.Space;
            keyboardHandler.OnKeyDown(key5);
            #endregion

            //check is A entered or not
            Assert.AreEqual("A", keyboardHandler.Name);
        }

        [Test]
        public void TestBackspaceInKey()
        {
            KeyboardHandler keyboardHandler = new KeyboardHandler();

            //typed A
            Keys key1 = Keys.A;
            keyboardHandler.OnKeyDown(key1);

            //check is A entered or not
            Assert.AreEqual("A", keyboardHandler.Name);

            //backspace
            Keys backspaceKey = Keys.Back;
            keyboardHandler.OnKeyDown(backspaceKey);

            //check is A entered or not
            Assert.AreEqual(string.Empty, keyboardHandler.Name);
        }
    }
}
