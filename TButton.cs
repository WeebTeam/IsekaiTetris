using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Tetris
{
    /// <summary>
    /// TButton for Toggle Button, this button acts like a toggle switch
    /// clicking once keeps it in pressed state, clicking again depresses it
    /// basically a toggle switch
    /// </summary>
    public class TButton : Button
    {
        private bool _selected = false;
        private GuiButtonState _lastState = GuiButtonState.None;

        //default: released texture same as hover texture
        public TButton(Rectangle rect, Texture2D noneTexture, Texture2D hoverTexture, Texture2D selectedTexture)
            : base()
        {
            _rectangle = rect;
            _textColor = Color.Black;

            _textures = new Dictionary<GuiButtonState, Texture2D>{
                { GuiButtonState.None, noneTexture },
                { GuiButtonState.Hover, hoverTexture },
                { GuiButtonState.Pressed, selectedTexture }
            };
        }

        //with text
        public TButton(Rectangle rect, SpriteFont font, string text, Texture2D noneTexture, Texture2D hoverTexture, Texture2D selectedTexture)
            : this(rect, noneTexture, hoverTexture, selectedTexture)
        {
            _buttonText = text;
            _buttonFont = font;
        }

        //with text, text color
        public TButton(Rectangle rect, SpriteFont font, string text, Color textColor, Texture2D noneTexture, Texture2D hoverTexture, Texture2D selectedTexture) :
            this(rect, font, text, noneTexture, hoverTexture, selectedTexture)
        {
            _textColor = textColor;
        }

        public bool Selected
        {
            get { return _selected; }
            set { _selected = value; } // you can throw some events here if you'd like
        }

        public void Toggle()
        {
            // toggle the value
            _selected = !_selected;
        }

        public override void Update(MouseState mouseState)
        {
            if (_rectangle.Contains(mouseState.X, mouseState.Y)) //if mouse in rectangle
            {
                if (mouseState.LeftButton == ButtonState.Pressed) //if left button is clicked
                {
                    State = GuiButtonState.Pressed;
                }
                else
                    //(condition) ? [true path] : [false path];
                    State = GuiButtonState.Hover;

                if (mouseState.LeftButton == ButtonState.Released) //if left button is clicked
                {
                    if (_state != _lastState) //software debouncing lol
                    {
                        Toggle();
                    }
                }
                _lastState = _state;
            }
            else
            {
                State = GuiButtonState.None;
            }
        }

        // Make sure Begin is called on s before you call this function
        public override void Draw(SpriteBatch s)
        {
            if (_selected) s.Draw(_textures[GuiButtonState.Pressed], _rectangle, Color.White);
            else s.Draw(_textures[State], _rectangle, Color.White);

            if (_buttonText != null) //draw text if available
            {
                Vector2 textBox = _buttonFont.MeasureString(_buttonText); //vector of the area the text will take
                Vector2 center = _rectangle.Center.ToVector2();

                center.X -= textBox.X / 2;
                center.Y -= textBox.Y / 2;

                s.DrawString(_buttonFont, _buttonText, center, _textColor);
            }
        }
    }
}
