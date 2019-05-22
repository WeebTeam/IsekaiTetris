using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace Tetris
{
    public class Menu : GameState
    {
        private Texture2D background, title;

        //buttons (and button textures)
        private Button playButton, scoreButton, quitButton, musicEnabledButton, musicDisabledButton;
        private Texture2D buttonNone, buttonHover;
        private Texture2D musicEnabled, musicDisabled;

        public int musicOn = 1;

        //song
        public Song menuMusic;

        //fonts
        private SpriteFont gameFont;

        public Menu(GraphicsDevice graphicsDevice)
        : base(graphicsDevice)
        {
            _graphicsDevice = graphicsDevice;
        }

        public override void Initialize()
        {

        }

        public override void LoadContent(ContentManager content)
        {
            //Load 2D textures
            background = content.Load<Texture2D>("textures/mainBack2");
            title = content.Load<Texture2D>("textures/title");

            //Load button textures
            buttonNone = content.Load<Texture2D>("textures/button_normal");
            buttonHover = content.Load<Texture2D>("textures/button_hover");

            musicEnabled = content.Load<Texture2D>("textures/music");
            musicDisabled = content.Load<Texture2D>("textures/no_music");

            // Load font
            gameFont = content.Load<SpriteFont>("spritefonts/gameFont");

            // Load music
            menuMusic = content.Load<Song>("audios/fantasticdreamer");

            // Load buttons 
            playButton = new Button(new Rectangle(75, 400, 400, 50), gameFont, "Start Game", Color.White, buttonNone, buttonHover, buttonNone); //440, 400, 400, 50
            scoreButton = new Button(new Rectangle(75, 460, 400, 50), gameFont, "High Score", Color.White, buttonNone, buttonHover, buttonNone);
            //settingButton = new Button(new Rectangle(75, 520, 400, 50), gameFont, "Settings", Color.White, buttonNone, buttonHover, buttonNone);
            quitButton = new Button(new Rectangle(75, 520, 400, 50), gameFont, "Quit", Color.White, buttonNone, buttonHover, buttonNone);

            musicEnabledButton = new Button(new Rectangle(1230,10,32,32), musicEnabled, musicEnabled, musicEnabled);
            musicDisabledButton = new Button(new Rectangle(1200, 10, 32, 32), musicDisabled, musicDisabled, musicDisabled);

            MediaPlayer.Play(menuMusic);
            MediaPlayer.Volume = 0.2f;
            MediaPlayer.IsRepeating = true;
        }

        public override void UnloadContent()
        {
            MediaPlayer.Stop();
        }

        public override void Update(GameTime gameTime)
        {
            // Gets keyboard input
            KeyboardState keyboardState = Keyboard.GetState();
            MouseState mouseState = Mouse.GetState();
            //if (keyboardState.IsKeyDown(Keys.Escape))  this.Exit();

            //menu input checking?
            playButton.Update(mouseState);
            scoreButton.Update(mouseState);
            //settingButton.Update(mouseState);
            quitButton.Update(mouseState);

            musicEnabledButton.Update(mouseState);
            musicDisabledButton.Update(mouseState);

            if (MediaPlayer.State != MediaState.Playing) //check if music is playing
            {
                MediaPlayer.Play(menuMusic);
                MediaPlayer.Volume = 0.2f;
                MediaPlayer.IsRepeating = true;
            }

            if (playButton.State == Button.GuiButtonState.Released)
            {
                //run game
                GameStateManager.Instance.AddScreen(new CharacterSelection(_graphicsDevice));
            }
            else if (scoreButton.State == Button.GuiButtonState.Released)
            {
                //load high score
                
            }
           // else if (playButton.State == Button.GuiButtonState.Released)
            //{
                //open settings

           // }
            else if (quitButton.State == Button.GuiButtonState.Released)
            {
                //quit game
                GameStateManager.Instance.QuitGame = true;
            }

            if (musicEnabledButton.State == Button.GuiButtonState.Released)
            {
                musicOn = 0;
                MediaPlayer.Resume();
            }
            else if (musicDisabledButton.State == Button.GuiButtonState.Released)
            {
                musicOn = 1;
                MediaPlayer.Pause();
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(background, new Vector2(0, 0), Color.White);

            spriteBatch.Draw(title, new Vector2(30, 50), Color.White); //draw title/logo   //280,50

            //draw buttons
            playButton.Draw(spriteBatch);
            scoreButton.Draw(spriteBatch);
            //settingButton.Draw(spriteBatch);
            quitButton.Draw(spriteBatch);

            //if (musicOn == 0)
            //{
               // musicEnabledButton.Draw(spriteBatch);
            //}
            //else if (musicOn == 1)
            //{
               // musicDisabledButton.Draw(spriteBatch);
            //}
        }
    }
}