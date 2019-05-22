using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Tetris
{
    public class GameOver
    {
        private Texture2D background;

        public bool _lose;
        public bool _win;

        private bool _playagain = false;
        private Board _board;
        private Score _score;

        //buttons (and button textures)
        private Button playAgainButton, menuButton, quitButton;
        private Texture2D buttonNone, buttonHover, gameoverImg, congraImg;

        //fonts
        private SpriteFont gameFont;

        public GameOver(GraphicsDevice graphicsDevice, ref Score score)
        {
            _score = score;
        }

        public void Initialize()
        {
            
        }

        public void LoadContent(ContentManager content)
        {

            //Load 2D textures
            background = content.Load<Texture2D>("textures/pause");
            gameoverImg = content.Load<Texture2D>("textures/gameoverImg");
            congraImg = content.Load<Texture2D>("textures/congraImg");

            //font
            gameFont = content.Load<SpriteFont>("spritefonts/gameFont");

            //Load button textures
            buttonNone = content.Load<Texture2D>("textures/button_normal");
            buttonHover = content.Load<Texture2D>("textures/button_hover");

            // Load buttons 
            playAgainButton = new Button(new Rectangle(440, 425, 400, 50), gameFont, "Play Again", Color.White, buttonNone, buttonHover, buttonNone); //245
            //settingButton = new Button(new Rectangle(440, 305, 400, 50), gameFont, "Settings", Color.White, buttonNone, buttonHover, buttonNone);
            menuButton = new Button(new Rectangle(440, 485, 400, 50), gameFont, "Back To Menu", Color.White, buttonNone, buttonHover, buttonNone); //305
            quitButton = new Button(new Rectangle(440, 545, 400, 50), gameFont, "Quit", Color.White, buttonNone, buttonHover, buttonNone); //365

        }

        public virtual bool Lose
        {
            set { _lose = value; }
            get { return _lose; }
        }

        public virtual bool PlayAgain
        {
            set { _playagain = value; }
            get { return _playagain; }
        }

        public bool ConfirmedPlayAgain()
        {
            return true;
        }

        public virtual Board Board
        {
            set { _board = value; }
            get { return _board; }
        }

        public virtual Score Score
        {
            set { _score = value; }
            get { return _score; }
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

            _playagain = false;

            playAgainButton.Update(mouseState);
            //settingButton.Update(mouseState);
            menuButton.Update(mouseState);
            quitButton.Update(mouseState);

            if (playAgainButton.State == Button.GuiButtonState.Released)
            {
                _playagain = true;
            }
            else if (menuButton.State == Button.GuiButtonState.Released)
            {
                //quit to menu
                if (ConfirmMenu())
                {
                    //play another sound
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
            
            if(_lose)
                spriteBatch.Draw(gameoverImg, new Rectangle(410, 100, 450, 350), Color.White);
            else if (_win)
                spriteBatch.Draw(congraImg, new Rectangle(330, 20, 650, 400), Color.White);

            spriteBatch.DrawString(_board.timerFont, "Score: " + _score.Value.ToString(), new Vector2(490, 120), Color.White);

            //draw buttons
            playAgainButton.Draw(spriteBatch);
            menuButton.Draw(spriteBatch);
            quitButton.Draw(spriteBatch);
        }
    }
}