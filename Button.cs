using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
/// <summary>
/// taken from http://community.monogame.net/t/mouse-and-buttons-how-to-simplify/7726/2
/// </summary>
namespace Tetris
{
    public class Button
    {
        public enum GuiButtonState
        {
            None,
            Pressed,
            Hover,
            Released
        }

        public Color _buttonColor { get; set; }

        private Rectangle _rectangle;
        private GuiButtonState _state;

        private string _buttonText;
        private SpriteFont _buttonFont;

        public GuiButtonState State
        {
            get { return _state; }
            set { _state = value; } // you can throw some events here if you'd like
        }

        private Dictionary<GuiButtonState, Texture2D> _textures;

        public Button(Rectangle rectangle, Texture2D noneTexture, Texture2D hoverTexture, Texture2D pressedTexture)
        {
            _rectangle = rectangle;
            _buttonText = null;
            _buttonFont = null;
            _textures = new Dictionary<GuiButtonState, Texture2D>{
                { GuiButtonState.None, noneTexture },
                { GuiButtonState.Hover, hoverTexture },
                { GuiButtonState.Pressed, pressedTexture },
                { GuiButtonState.Released, hoverTexture }
            };
            _buttonColor = Color.Black;
        }

        //with text
        public Button(Rectangle rectangle, SpriteFont font, string text, Texture2D noneTexture, Texture2D hoverTexture, Texture2D pressedTexture)
        {
            _rectangle = rectangle;
            _buttonText = text;
            _buttonFont = font;
            _textures = new Dictionary<GuiButtonState, Texture2D>{
                { GuiButtonState.None, noneTexture },
                { GuiButtonState.Hover, hoverTexture },
                { GuiButtonState.Pressed, pressedTexture },
                { GuiButtonState.Released, hoverTexture }
            };
            _buttonColor = Color.Black;
        }

        //overloader with text color
        public Button(Rectangle rectangle, SpriteFont font, string text, Texture2D noneTexture, Texture2D hoverTexture, Texture2D pressedTexture, Color newColor)
        {
            _rectangle = rectangle;
            _buttonText = text;
            _buttonFont = font;
            _textures = new Dictionary<GuiButtonState, Texture2D>{
                { GuiButtonState.None, noneTexture },
                { GuiButtonState.Hover, hoverTexture },
                { GuiButtonState.Pressed, pressedTexture },
                { GuiButtonState.Released, hoverTexture }
            };
            _buttonColor = newColor;
        }

        public void Update(MouseState mouseState)
        {
            if (_rectangle.Contains(mouseState.X, mouseState.Y))
            {
                if (mouseState.LeftButton == ButtonState.Pressed)
                    State = GuiButtonState.Pressed;
                else
                    State = State == GuiButtonState.Pressed ? GuiButtonState.Released : GuiButtonState.Hover;
            }
            else
            {
                State = GuiButtonState.None;
            }
        }

        // Make sure Begin is called on s before you call this function
        public void Draw(SpriteBatch s)
        {
            s.Draw(_textures[State], _rectangle, Color.White);

            if (_buttonText != null) //draw text if available
            {
                Vector2 textBox = _buttonFont.MeasureString(_buttonText); //vector of the area the text will take
                Vector2 center = _rectangle.Center.ToVector2();

                center.X -= textBox.X / 2;
                center.Y -= textBox.Y / 2;

                s.DrawString(_buttonFont, _buttonText, center, _buttonColor);
            }
            
        }

    }
}
