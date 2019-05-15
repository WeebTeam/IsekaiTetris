using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Tetris
{
    public class PauseScreen : GameState
    {
        private Texture2D background, title;

        //buttons (and button textures)
        private Button resumeButton, settingButton, menuButton, quitButton;
        private Texture2D buttonNone, buttonPressed, buttonHover;

        //fonts
        private SpriteFont gameFont, menuFont;

        public PauseScreen(GraphicsDevice graphicsDevice)
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
            background = content.Load<Texture2D>("Images/pause");

            menuFont = content.Load<SpriteFont>("menuFont");

            // Load buttons 
            resumeButton = new Button(new Rectangle(440, 245, 400, 50), gameFont, "Resume Game", Color.White, buttonNone, buttonHover, buttonNone);
            settingButton = new Button(new Rectangle(440, 305, 400, 50), gameFont, "Settings", Color.White, buttonNone, buttonHover, buttonNone);
            menuButton = new Button(new Rectangle(440, 365, 400, 50), gameFont, "Exit to Menu", Color.White, buttonNone, buttonHover, buttonNone);
            quitButton = new Button(new Rectangle(440, 425, 400, 50), gameFont, "Quit Game", Color.White, buttonNone, buttonHover, buttonNone);

        }

        public override void UnloadContent()
        {
        }

        public bool ConfirmMenu()
        {
            return true;
        }

        public bool ConfirmQuit()
        {
            return true;
        }

        public override void Update(GameTime gameTime)
        {
            // Gets keyboard input
            KeyboardState keyboardState = Keyboard.GetState();
            MouseState mouseState = Mouse.GetState();

            resumeButton.Update(mouseState);
            settingButton.Update(mouseState);
            menuButton.Update(mouseState);
            quitButton.Update(mouseState);

            if (resumeButton.State == Button.GuiButtonState.Released)
            {
                //resume screen, so remove this gamestate
                GameStateManager.Instance.RemoveScreen();
            }
            else if (settingButton.State == Button.GuiButtonState.Released)
            {
                //go to setting

            }
            else if (menuButton.State == Button.GuiButtonState.Released)
            {
                //quit to menu
                if (ConfirmMenu())
                {
                    //remove current pause screen, and then remove the game screen
                    GameStateManager.Instance.RemoveScreen();
                    GameStateManager.Instance.RemoveScreen();
                }
            }
            else if (quitButton.State == Button.GuiButtonState.Released)
            {
                //quit game
                if (ConfirmQuit())
                    GameStateManager.Instance.QuitGame = true;
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {

            spriteBatch.Draw(title, new Vector2(280, 50), Color.White); //draw title/logo

            //draw buttons
            resumeButton.Draw(spriteBatch);
            settingButton.Draw(spriteBatch);
            menuButton.Draw(spriteBatch);
            quitButton.Draw(spriteBatch);
        }
    }
}