using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Tetris
{
    public class Menu : GameState
    {
        private Texture2D background;

        //buttons (and button textures)
        private Button playButton, settingButton, quitButton;
        private Texture2D buttonNone, buttonPressed, buttonHover;

        //fonts
        private SpriteFont gameFont, menuFont;

        public Menu(GraphicsDevice graphicsDevice)
        : base(graphicsDevice)
        {
        }

        public override void Initialize()
        {

        }

        public override void LoadContent(ContentManager content)
        {

            //Load 2D textures
            background = content.Load<Texture2D>("Images/mainmenubg");

            //Load button textures
            buttonNone = content.Load<Texture2D>("Buttons/button");
            buttonPressed = content.Load<Texture2D>("Buttons/pressed");
            buttonHover = content.Load<Texture2D>("Buttons/hover");

            // Load font
            gameFont = content.Load<SpriteFont>("gameFont");
            menuFont = content.Load<SpriteFont>("menuFont");

            // Load buttons 
            playButton = new Button(new Rectangle(400, 400, 400, 50), gameFont, "Play Game", buttonNone, buttonHover, buttonPressed);
            settingButton = new Button(new Rectangle(400, 460, 400, 50), gameFont, "Settings", buttonNone, buttonHover, buttonPressed);
            quitButton = new Button(new Rectangle(400, 520, 400, 50), gameFont, "Quit", buttonNone, buttonHover, buttonPressed);
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
            settingButton.Update(mouseState);
            quitButton.Update(mouseState);

            if (playButton.State == Button.GuiButtonState.Released)
            {
                //run game
            }

            if (quitButton.State == Button.GuiButtonState.Released)
            {
                //quit game
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            spriteBatch.Draw(background, Vector2.Zero, Color.White);
            spriteBatch.DrawString(menuFont, "Main Menu", new Vector2(200, 100), Color.Black);

            playButton.Draw(spriteBatch);
            settingButton.Draw(spriteBatch);
            quitButton.Draw(spriteBatch);
            spriteBatch.End();
        }
    }
}