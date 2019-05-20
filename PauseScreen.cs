using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Tetris
{
    public class PauseScreen
    {
        private Texture2D background;

        private bool _unpause = false;

        //buttons (and button textures)
        private Button resumeButton, settingButton, menuButton, quitButton;
        private Texture2D buttonNone, buttonHover;

        //fonts
        private SpriteFont gameFont;

        public PauseScreen(GraphicsDevice graphicsDevice)
        {
        }

        public void Initialize()
        {

        }

        public void LoadContent(ContentManager content)
        {

            //Load 2D textures
            background = content.Load<Texture2D>("textures/pause");

            //font
            gameFont = content.Load<SpriteFont>("spritefonts/gameFont");

            //Load button textures
            buttonNone = content.Load<Texture2D>("textures/button_normal");
            buttonHover = content.Load<Texture2D>("textures/button_hover");

            // Load buttons 
            resumeButton = new Button(new Rectangle(440, 245, 400, 50), gameFont, "Resume Game", Color.White, buttonNone, buttonHover, buttonNone);
            settingButton = new Button(new Rectangle(440, 305, 400, 50), gameFont, "Settings", Color.White, buttonNone, buttonHover, buttonNone);
            menuButton = new Button(new Rectangle(440, 365, 400, 50), gameFont, "Exit to Menu", Color.White, buttonNone, buttonHover, buttonNone);
            quitButton = new Button(new Rectangle(440, 425, 400, 50), gameFont, "Quit Game", Color.White, buttonNone, buttonHover, buttonNone);

        }

        public bool Unpause
        {
            get { return _unpause; }
            set { _unpause = value; }
        }

        public void UnloadContent()
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

        public void Update(GameTime gameTime)
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
                //resume screen
                _unpause = true;
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
                    MediaPlayer.Stop();
                    GameStateManager.Instance.RemoveScreen(); //exit game screen
                    GameStateManager.Instance.RemoveScreen(); //exit character screen
                }
            }
            else if (quitButton.State == Button.GuiButtonState.Released)
            {
                //quit game
                if (ConfirmQuit())
                    GameStateManager.Instance.QuitGame = true;
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(background, new Vector2(0, 0), Color.White);
            //draw buttons
            resumeButton.Draw(spriteBatch);
            settingButton.Draw(spriteBatch);
            menuButton.Draw(spriteBatch);
            quitButton.Draw(spriteBatch);
        }
    }
}