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
        private Button playButton, scoreButton, settingButton, quitButton;
        private Texture2D buttonNone, buttonHover;

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

            // Load font
            gameFont = content.Load<SpriteFont>("spritefonts/gameFont");

            // Load music
            menuMusic = content.Load<Song>("audios/fantasticdreamer");

            // Load buttons 
            playButton = new Button(new Rectangle(75, 400, 400, 50), gameFont, "Start Game", Color.White, buttonNone, buttonHover, buttonNone); //440, 400, 400, 50
            scoreButton = new Button(new Rectangle(75, 460, 400, 50), gameFont, "High Score", Color.White, buttonNone, buttonHover, buttonNone);
            settingButton = new Button(new Rectangle(75, 520, 400, 50), gameFont, "Settings", Color.White, buttonNone, buttonHover, buttonNone);
            quitButton = new Button(new Rectangle(75, 580, 400, 50), gameFont, "Quit", Color.White, buttonNone, buttonHover, buttonNone);

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
            settingButton.Update(mouseState);
            quitButton.Update(mouseState);

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
            else if (playButton.State == Button.GuiButtonState.Released)
            {
                //open settings

            }
            else if (quitButton.State == Button.GuiButtonState.Released)
            {
                //quit game
                GameStateManager.Instance.QuitGame = true;
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(background, new Vector2(0, 0), Color.White);

            spriteBatch.Draw(title, new Vector2(30, 50), Color.White); //draw title/logo   //280,50

            //draw buttons
            playButton.Draw(spriteBatch);
            scoreButton.Draw(spriteBatch);
            settingButton.Draw(spriteBatch);
            quitButton.Draw(spriteBatch);
        }
    }
}