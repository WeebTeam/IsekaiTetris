using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Tetris
{
    /// <summary>
    /// CharacterProfile class for displaying the characters(and their info) in character selection page
    /// </summary>
    public class CharacterProfile
    {
        private bool _selected = false, _highlight = false;
        private Rectangle _rectangle;
        private ButtonState _lastState;

        private Texture2D _normalTexture, _highlightTexture;

        //default: released texture same as hover texture
        public CharacterProfile(Rectangle rect, Texture2D normal, Texture2D highlight)
        {
            _rectangle = rect;
            _normalTexture = normal;
            _highlightTexture = highlight;
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

        public void Update(MouseState mouseState)
        {
            if (_rectangle.Contains(mouseState.X, mouseState.Y)) //if mouse in rectangle
            {
                ButtonState state = mouseState.LeftButton;

                _highlight = true;

                if (_lastState != state) _selected = true;

                _lastState = state;
            }
            else
            {
                _highlight = false;
            }
        }

        // Make sure Begin is called on s before you call this function
        public void Draw(SpriteBatch s)
        {
            if (_selected || _highlight) s.Draw(_highlightTexture, _rectangle, Color.White);
            else s.Draw(_normalTexture, _rectangle, Color.White);
        }
    }
}

