using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.ComponentModel;
using Microsoft.Xna.Framework.Content;
using Tetris;

namespace Tetris_test
{
    public class MainMenu : State
    {
        //place buttons
        private List<Component> _components;

        public Basic2d bkg;

        public MainMenu(Game1 game, GraphicsDevice graphics, ContentManager content) : base(game, graphics, content)
        {
            var buttonTexture = _content.Load<Texture2D>("");
            var buttonFont = _content.Load<SpriteFont>("");

            var newGameButton = new Button(buttonTexture, buttonFont)
            {
                Position = new Vector2(300, 200),
                Text = "New Game",
            };

            _components = new List<Component>()
            {

            };
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            throw new NotImplementedException();
        }

        public override void PostUpdate(GameTime gameTime)
        {
            throw new NotImplementedException();
        }

        public override void Update(GameTime gameTime)
        {
            throw new NotImplementedException();
        }
    }
}
