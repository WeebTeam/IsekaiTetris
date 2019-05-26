using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Audio;

namespace Tetris
{
    public class CharacterSelection : GameState
    {
        private Texture2D background;

        //buttons (and button textures)
        private Button nextButton;
        private CharacterProfile kazuma, aqua, megumin, darkness;
        private Texture2D kazumaNormal, kazumaHover, aquaNormal, aquaHover, meguminNormal, meguminHover, darknessNormal, darknessHover, buttonNone, buttonHover, title, nameField;

        private KeyboardHandler kbHandler;

        //fonts
        private SpriteFont gameFont, menuFont;

        //soundEffect
        private SoundEffect kazumaSE, aquaSE, meguminSE, darknessSE;
        //countofOccurrence
        private int kazumaSECount, aquaSECount, meguminSECount, darknessSECount = 0;

        //musics
        private Song kazumaMusic, aquaMusic, meguminMusic, darknessMusic;
        
        private Score player;


        public CharacterSelection(GraphicsDevice graphicsDevice)
        : base(graphicsDevice)
        {
            _graphicsDevice = graphicsDevice;
        }

        public override void Initialize()
        {
            kbHandler = new KeyboardHandler();
        }

        public override void LoadContent(ContentManager content)
        {

            //Load 2D textures
            background = content.Load<Texture2D>("textures/characterMenu");
            title = content.Load<Texture2D>("textures/characterTitle");

            //Load button textures
            kazumaNormal = content.Load<Texture2D>("textures/character/kazumaButton");
            kazumaHover = content.Load<Texture2D>("textures/character/kazumaHover");
            aquaNormal = content.Load<Texture2D>("textures/character/aquaButton");
            aquaHover = content.Load<Texture2D>("textures/character/aquaHover");
            meguminNormal = content.Load<Texture2D>("textures/character/meguminButton");
            meguminHover = content.Load<Texture2D>("textures/character/meguminHover");
            darknessNormal = content.Load<Texture2D>("textures/character/darknessButton");
            darknessHover = content.Load<Texture2D>("textures/character/darknessHover");

            buttonNone = content.Load<Texture2D>("textures/button_normal");
            buttonHover = content.Load<Texture2D>("textures/button_hover");

            nameField = content.Load<Texture2D>("textures/name");

            // Load font
            gameFont = content.Load<SpriteFont>("spritefonts/gameFont");
            menuFont = content.Load<SpriteFont>("spritefonts/menuFont");

            // Load musics
            kazumaMusic = content.Load<Song>("audios/kazuma");
            aquaMusic = content.Load<Song>("audios/aqua");
            meguminMusic = content.Load<Song>("audios/megumin");
            darknessMusic = content.Load<Song>("audios/darkness");

            // Load sound effects
            kazumaSE = content.Load<SoundEffect>("audios/soundEffects/kazumaSE1");
            aquaSE = content.Load<SoundEffect>("audios/soundEffects/aquaSE1");
            meguminSE = content.Load<SoundEffect>("audios/soundEffects/meguminSE1");
            darknessSE = content.Load<SoundEffect>("audios/soundEffects/darknessSE1");

            // Load characters
            kazuma = new CharacterProfile(new Rectangle(120, 150, 260, 470), kazumaNormal, kazumaHover);
            aqua = new CharacterProfile(new Rectangle(380, 150, 260, 470), aquaNormal, aquaHover);
            megumin = new CharacterProfile(new Rectangle(640, 150, 260, 470), meguminNormal, meguminHover);
            darkness = new CharacterProfile(new Rectangle(900, 150, 260, 470), darknessNormal, darknessHover);

            // Load buttons 
            nextButton = new Button(new Rectangle(760, 640, 400, 50), gameFont, "Next", Color.White, buttonNone, buttonHover, buttonNone);
            MediaPlayer.Volume = 0.1f;
        }

        public override void UnloadContent()
        {
        }

        // to make sure the sound effect only run once after selected, not looping
        public void Effect()
        {
            if (kazuma.Selected && kazumaSECount == 0)
            {
                kazumaSE.Play(1.0f, 0.0f, 0.0f);
                kazumaSECount = 1;
                aquaSECount = 0;
                meguminSECount = 0;
                darknessSECount = 0;
            }
            else if (aqua.Selected && aquaSECount == 0)
            {
                aquaSE.Play(1.0f, 0.0f, 0.0f);
                kazumaSECount = 0;
                aquaSECount = 1;
                meguminSECount = 0;
                darknessSECount = 0;
            }
            else if (megumin.Selected && meguminSECount == 0)
            {
                meguminSE.Play(1.0f, 0.0f, 0.0f);
                kazumaSECount = 0;
                aquaSECount = 0;
                meguminSECount = 1;
                darknessSECount = 0;
            }
            else if (darkness.Selected && darknessSECount == 0)
            {
                darknessSE.Play(1.0f, 0.0f, 0.0f);
                kazumaSECount = 0;
                aquaSECount = 0;
                meguminSECount = 0;
                darknessSECount = 1;
            }
        }

        public override void Update(GameTime gameTime)
        {
            // Gets keyboard input
            KeyboardState keyboardState = Keyboard.GetState();
            MouseState mouseState = Mouse.GetState();

            kbHandler.Update(gameTime);

            //check if character is selected, then deselects other characters
            kazuma.Update(mouseState);

            //check if it should play the sound effect or not
            Effect();

            if (kazuma.Selected)
            {
                aqua.Selected = false;
                megumin.Selected = false;
                darkness.Selected = false;
            }

            aqua.Update(mouseState);
            if (aqua.Selected)
            {
                kazuma.Selected = false;
                megumin.Selected = false;
                darkness.Selected = false;
            }

            megumin.Update(mouseState);
            if (megumin.Selected)
            {
                aqua.Selected = false;
                kazuma.Selected = false;
                darkness.Selected = false;
            }

            darkness.Update(mouseState);
            if (darkness.Selected)
            {
                aqua.Selected = false;
                megumin.Selected = false;
                kazuma.Selected = false;
            }
            
            nextButton.Update(mouseState);

            if (nextButton.State == Button.GuiButtonState.Released && kbHandler.Name != string.Empty)
            {
                //run game
                if (kazuma.Selected)
                {
                    player = new Score(kbHandler.Name, 0, Character.Kazuma);
                    GameStateManager.Instance.AddScreen(new InstructionScreen(_graphicsDevice, ref player));
                }
                if (aqua.Selected)
                {
                    player = new Score(kbHandler.Name, 0, Character.Aqua);
                    GameStateManager.Instance.AddScreen(new InstructionScreen(_graphicsDevice, ref player));
                }
                if (megumin.Selected)
                {
                    player = new Score(kbHandler.Name, 0, Character.Megumin);
                    GameStateManager.Instance.AddScreen(new InstructionScreen(_graphicsDevice, ref player));
                }
                if (darkness.Selected)
                {
                    player = new Score(kbHandler.Name, 0, Character.Darkness);
                    GameStateManager.Instance.AddScreen(new InstructionScreen(_graphicsDevice, ref player));
                }
            }

            if (keyboardState.IsKeyDown(Keys.Escape))
            {
                MediaPlayer.Volume = 0.2f;
                GameStateManager.Instance.RemoveScreen();
            }

        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(background, Vector2.Zero, Color.White);
            spriteBatch.Draw(title, new Rectangle(440, 0, 400, 200), Color.White);

            spriteBatch.Draw(nameField, new Rectangle(120, 615, 400 ,100), Color.White);

            kazuma.Draw(spriteBatch);
            aqua.Draw(spriteBatch);
            megumin.Draw(spriteBatch);
            darkness.Draw(spriteBatch);

            spriteBatch.DrawString(gameFont, kbHandler.Name, new Vector2(230, 658), Color.Orange);

            nextButton.Draw(spriteBatch);
        }
    }
}