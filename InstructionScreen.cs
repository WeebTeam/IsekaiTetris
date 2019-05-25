using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace Tetris
{
    class InstructionScreen : GameState
    {
        private Button nextButton;
        private Texture2D buttonNone, buttonHover, background, title;

        private CharacterProfile _characterProfile;

        private SpriteFont gameFont;

        public bool instructionDone = false;

        //musics
        private Song kazumaMusic, aquaMusic, meguminMusic, darknessMusic;

        public Character _character;

        public Score _player;

        public InstructionScreen(GraphicsDevice graphicsDevice, ref Score player) : base(graphicsDevice)
        {
            //_characterProfile = characterProfile;
            _player = player;
        }

        public Character Character
        {
            get { return _character; }
            set { _character = value; }
        }

        public CharacterProfile CharacterProfile
        {
            get { return _characterProfile; }
            set { _characterProfile = value; }
        }

        public override void Initialize()
        {
            
        }

        public override void LoadContent(ContentManager content)
        {
            //Load 2D textures
            background = content.Load<Texture2D>("textures/instructionBack");

            buttonNone = content.Load<Texture2D>("textures/button_normal");
            buttonHover = content.Load<Texture2D>("textures/button_hover");

            title = content.Load<Texture2D>("textures/instruction");

            gameFont = content.Load<SpriteFont>("spritefonts/gameFont");

            // Load musics
            kazumaMusic = content.Load<Song>("audios/kazuma");
            aquaMusic = content.Load<Song>("audios/aqua");
            meguminMusic = content.Load<Song>("audios/megumin");
            darknessMusic = content.Load<Song>("audios/darkness");
            
            nextButton = new Button(new Rectangle(440, 640, 400, 50), gameFont, "Next", Color.White, buttonNone, buttonHover, buttonNone);

        }

        public override void UnloadContent()
        {
            
        }

        public override void Update(GameTime gameTime)
        {
            instructionDone = false;

            MouseState mouseState = Mouse.GetState();
            nextButton.Update(mouseState);

            if (nextButton.State == Button.GuiButtonState.Released)
            {
                //run game
                if (_character == Character.Kazuma)
                {
                    MediaPlayer.Play(kazumaMusic);
                    GameStateManager.Instance.AddScreen(new Engine(_graphicsDevice, _player));
                }
                if (_character == Character.Aqua)
                {
                    MediaPlayer.Play(aquaMusic);
                    GameStateManager.Instance.AddScreen(new Engine(_graphicsDevice, _player));
                }
                if (_character == Character.Megumin)
                {
                    MediaPlayer.Play(meguminMusic);
                    GameStateManager.Instance.AddScreen(new Engine(_graphicsDevice, _player));
                }
                if (_character == Character.Darkness)
                {
                    MediaPlayer.Play(darknessMusic);
                    GameStateManager.Instance.AddScreen(new Engine(_graphicsDevice, _player));
                }
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(background, new Vector2(0, 0), Color.White);

            spriteBatch.Draw(title, new Rectangle(440, 0, 400, 200), Color.White);

            nextButton.Draw(spriteBatch);
        }
    }
}
