using NUnit.Framework;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Tetris
{
    [TestFixture]
    class TestButton
    {
        [Test]
        public void TestButtonDefaultConstructor()
        {
            Button button1 = new Button(new Rectangle());

            //check default button state
            Assert.AreEqual(Button.GuiButtonState.None, button1.State);

            //check default text colour
            Assert.AreEqual(Color.Black, button1.TextColor);
        }

        [Test]
        public void TestButtonHover()
        {
            //create a button at 0, 0 with size 10, 10
            Button button1 = new Button(new Rectangle(5, 5, 10, 10));

            //create a virtual mouse state (simulate not hovering over the button)
            MouseState state0 = new MouseState(1, 1, 0,
                ButtonState.Released,
                ButtonState.Released,
                ButtonState.Released,
                ButtonState.Released,
                ButtonState.Released
                );

            //create a virtual mouse state (simulate hovering over the button)
            MouseState state1 = new MouseState(10, 10, 0,
                ButtonState.Released,
                ButtonState.Released,
                ButtonState.Released,
                ButtonState.Released,
                ButtonState.Released
                );

            //update with not hover state
            button1.Update(state0);

            //the state should be none now
            Assert.AreEqual(Button.GuiButtonState.None, button1.State);

            //update with hover state
            button1.Update(state1);

            //the state should be hover now
            Assert.AreEqual(Button.GuiButtonState.Hover, button1.State);

        }
        [Test]
        public void TestButtonPressed()
        {
            //create a button at 0, 0 with size 10, 10
            Button button1 = new Button(new Rectangle(5, 5, 10, 10));

            //create a virtual mouse state (simulate clicking outisde the button)
            MouseState state0 = new MouseState(1, 1, 0,
                ButtonState.Pressed,
                ButtonState.Released,
                ButtonState.Released,
                ButtonState.Released,
                ButtonState.Released
                );

            //create a virtual mouse state (simulate clicking inside the button)
            MouseState state1 = new MouseState(10, 10, 0,
                ButtonState.Pressed,
                ButtonState.Released,
                ButtonState.Released,
                ButtonState.Released,
                ButtonState.Released
                );

            //update state0
            button1.Update(state0);

            //the state should be None
            Assert.AreEqual(Button.GuiButtonState.None, button1.State);

            //update state1
            button1.Update(state1);

            //the state should be Pressed now
            Assert.AreEqual(Button.GuiButtonState.Pressed, button1.State);
        }
        [Test]
        public void TestButtonReleased()
        {
            //create a button at 0, 0 with size 10, 10
            Button button1 = new Button(new Rectangle(5, 5, 10, 10));

            //create a virtual mouse state (simulate clicking inside the button)
            MouseState state0 = new MouseState(10, 10, 0,
                ButtonState.Pressed,
                ButtonState.Released,
                ButtonState.Released,
                ButtonState.Released,
                ButtonState.Released
                );

            //create a virtual mouse state (simulate releasing click inside the button)
            MouseState state1 = new MouseState(10, 10, 0,
                ButtonState.Released,
                ButtonState.Released,
                ButtonState.Released,
                ButtonState.Released,
                ButtonState.Released
                );

            //update state0
            button1.Update(state0);

            //the state should be Pressed
            Assert.AreEqual(Button.GuiButtonState.Pressed, button1.State);

            //update state1
            button1.Update(state1);

            //the state should be Released now
            Assert.AreEqual(Button.GuiButtonState.Released, button1.State);
        }
    }
}
