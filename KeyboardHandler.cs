using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// Source from https://stackoverflow.com/questions/10154046/making-text-input-in-xna-for-entering-names-chatting
/// </summary>
namespace Tetris
{
    public class KeyboardHandler
    {
        private Keys[] lastPressedKeys;
        private string name = "";
        private int count = 0;

        public KeyboardHandler()
        {
            lastPressedKeys = new Keys[0];
        }

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        public void Update(GameTime gameTime)
        {
            KeyboardState keyboardState = Keyboard.GetState();
            Keys[] pressedKeys = keyboardState.GetPressedKeys();

            //to check whether previous update keys are no longer pressed
            foreach(Keys key in lastPressedKeys)
            {
                if (!pressedKeys.Contains(key))
                    OnKeyUp(key);
            }

            //to check whether pressed keys are pressed
            foreach (Keys key in pressedKeys)
            {
                if (!lastPressedKeys.Contains(key))
                    OnKeyDown(key);
            }

            //save the keys to be checked later
            lastPressedKeys = pressedKeys;
        }

        private void OnKeyUp(Keys key)
        {

        }

        private void OnKeyDown(Keys key)
        {
            //amount of characters to be entered to name
            if (count <= 18)
            {
                if (key == Keys.Back && name.Length > 0)
                {
                    //remove 1 character
                    name = name.Remove(name.Length - 1);
                    count--;
                }
                else if (key == Keys.CapsLock || key == Keys.Enter || key == Keys.Tab || key == Keys.Space)
                {
                    //do nothing
                }
                else
                {
                    //enter name
                    name += key.ToString();
                    count++;
                }
            }
        }
    }


}
