using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Tetris
{
    /// <summary>
    /// taken from http://community.monogame.net/t/mouse-and-buttons-how-to-simplify/7726/2
    /// Momentary button
    /// State changes based on if user is clicking it
    /// basically a momentary switch
    /// </summary>
    public class Button
    {
        public enum GuiButtonState
        {
            None,
            Hover,
            Pressed,
            Released
        }

        private Color _textColor;
        protected Rectangle _rectangle;
        protected GuiButtonState _state;

        protected string _buttonText = null;
        protected SpriteFont _buttonFont = null;

        public Color TextColor
        {
            get { return _textColor; }
            set { _textColor = value; }
        }

        public virtual GuiButtonState State
        {
            get { return _state; }
            set { _state = value; } // you can throw some events here if you'd like
        }

        public Dictionary<GuiButtonState, Texture2D> Textures
        {
            get { return _textures; }
            set { _textures = value; }
        }

        protected Dictionary<GuiButtonState, Texture2D> _textures;

        public Button(Rectangle rect) : this(rect, null, null, null, null)
        {
        }

        //released texture same as hover texture
        public Button(Rectangle rect, Texture2D noneTexture, Texture2D hoverTexture, Texture2D pressedTexture)
        {
            _rectangle = rect;
            _textColor = Color.Black;

            _textures = new Dictionary<GuiButtonState, Texture2D>{
                { GuiButtonState.None, noneTexture },
                { GuiButtonState.Hover, hoverTexture },
                { GuiButtonState.Pressed, pressedTexture },
                { GuiButtonState.Released, hoverTexture },
            };
        }

        //with saparate released texture
        public Button(Rectangle rect, Texture2D noneTexture, Texture2D hoverTexture, Texture2D pressedTexture, Texture2D releasedTexture)
            : this(rect, noneTexture, hoverTexture, pressedTexture)
        {
            //set custom released texture
            _textures[GuiButtonState.Released] = releasedTexture;

        }

        //with text
        public Button(Rectangle rect, SpriteFont font, string text, Texture2D noneTexture, Texture2D hoverTexture, Texture2D pressedTexture)
            : this(rect, noneTexture, hoverTexture, pressedTexture)
        {
            _buttonText = text;
            _buttonFont = font;
        }

        //with text, text color
        public Button(Rectangle rect, SpriteFont font, string text, Color textColor, Texture2D noneTexture, Texture2D hoverTexture, Texture2D pressedTexture) :
            this(rect, font, text, noneTexture, hoverTexture, pressedTexture)
        {
            _textColor = textColor;
        }

        public virtual void Update(MouseState mouseState)
        {
            if (_rectangle.Contains(mouseState.X, mouseState.Y))
            {
                if (mouseState.LeftButton == ButtonState.Pressed)
                    State = GuiButtonState.Pressed;
                else
                    //(condition) ? [true path] : [false path];
                    State = State == GuiButtonState.Pressed ? GuiButtonState.Released : GuiButtonState.Hover;
            }
            else
            {
                State = GuiButtonState.None;
            }
        }

        // Make sure Begin is called on s before you call this function
        public virtual void Draw(SpriteBatch s)
        {
            s.Draw(_textures[State], _rectangle, Color.White);

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
