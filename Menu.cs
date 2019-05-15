using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Tetris
{
    public class Menu : GameState
    {
        private Texture2D background, title;

        //buttons (and button textures)
        private Button playButton, scoreButton, settingButton, quitButton;
        private Texture2D buttonNone, buttonPressed, buttonHover;

        //fonts
        private SpriteFont gameFont, menuFont;

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
            background = content.Load<Texture2D>("Images/mainmenubg");
            title = content.Load<Texture2D>("Images/title");

            //Load button textures
            buttonNone = content.Load<Texture2D>("Buttons/buttonTemplate");
            buttonPressed = content.Load<Texture2D>("Buttons/pressed");
            buttonHover = content.Load<Texture2D>("Buttons/buttonTemplateHover");

            // Load font
            gameFont = content.Load<SpriteFont>("gameFont");
            menuFont = content.Load<SpriteFont>("menuFont");

            // Load buttons 
            playButton = new Button(new Rectangle(440, 400, 400, 50), gameFont, "Start Game", buttonNone, buttonHover, buttonNone, Color.White);
            scoreButton = new Button(new Rectangle(440, 460, 400, 50), gameFont, "High Score", buttonNone, buttonHover, buttonNone, Color.White);
            settingButton = new Button(new Rectangle(440, 520, 400, 50), gameFont, "Settings", buttonNone, buttonHover, buttonNone, Color.White);
            quitButton = new Button(new Rectangle(440, 580, 400, 50), gameFont, "Quit", buttonNone, buttonHover, buttonNone, Color.White);

            /* 1280 x 720
            buttons are 400 x 50
            to center them:

            440 - 400 - 440


            */
        }

        public override void UnloadContent()
        {
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
                GameStateManager.Instance.ClearScreens();
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            //spriteBatch.Draw(background, Vector2.Zero, Color.White);
            //spriteBatch.DrawString(menuFont, "Main Menu", new Vector2(200, 100), Color.Black);

            spriteBatch.Draw(title, new Vector2(280, 50), Color.White);

            playButton.Draw(spriteBatch);
            scoreButton.Draw(spriteBatch);
            settingButton.Draw(spriteBatch);
            quitButton.Draw(spriteBatch);
        }
    }
}