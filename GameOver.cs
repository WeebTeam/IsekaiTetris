using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
using System.Collections.Generic;

namespace Tetris
{
    public class GameOver
    {
        private Texture2D background;

        public bool _lose;
        public bool _win;

        private bool _playagain = false;
        private Board _board;
        private GameScore _score;
        public Character _character;

        //buttons (and button textures)
        private Button playAgainButton, menuButton, quitButton;
        private Texture2D buttonNone, buttonHover, gameoverImg, congraImg;
        
        //soundeffects in list (win/lose)
        private List<SoundEffect> kazumaSE, aquaSE, meguminSE, darknessSE;

        public int kazumaSECount, aquaSECount, meguminSECount, darknessSECount = 0;

        //fonts
        private SpriteFont gameFont;

        public GameOver(GraphicsDevice graphicsDevice, ref GameScore score)
        {
            _score = score;

            kazumaSE = new List<SoundEffect>();
            aquaSE = new List<SoundEffect>();
            meguminSE = new List<SoundEffect>();
            darknessSE = new List<SoundEffect>();
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

            //Load buttons 
            playAgainButton = new Button(new Rectangle(440, 425, 400, 50), gameFont, "Play Again", Color.White, buttonNone, buttonHover, buttonNone); //245
            menuButton = new Button(new Rectangle(440, 485, 400, 50), gameFont, "Back To Menu", Color.White, buttonNone, buttonHover, buttonNone); //305
            quitButton = new Button(new Rectangle(440, 545, 400, 50), gameFont, "Quit", Color.White, buttonNone, buttonHover, buttonNone); //365

            //Load sound effects
            kazumaSE.Add(content.Load<SoundEffect>("audios/soundEffects/kazumaWin"));
            kazumaSE.Add(content.Load<SoundEffect>("audios/soundEffects/kazumaLose"));

            aquaSE.Add(content.Load<SoundEffect>("audios/soundEffects/aquaWin"));
            aquaSE.Add(content.Load<SoundEffect>("audios/soundEffects/aquaLose"));

            meguminSE.Add(content.Load<SoundEffect>("audios/soundEffects/meguminWin"));
            meguminSE.Add(content.Load<SoundEffect>("audios/soundEffects/meguminLose"));

            darknessSE.Add(content.Load<SoundEffect>("audios/soundEffects/darknessWin"));
            darknessSE.Add(content.Load<SoundEffect>("audios/soundEffects/darknessLose"));

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

        public virtual GameScore Score
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
                    GameStateManager.Instance.RemoveScreen(); //exit instruction screen
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

        // to make sure the sound effect only run once after selected
        public void Effect()
        {
            if (_lose) {
                if (_character == Character.Kazuma && kazumaSECount == 0)
                {
                    kazumaSE[1].Play();
                    kazumaSECount = 1;
                }
                else if (_character == Character.Aqua && aquaSECount == 0)
                {
                    aquaSE[1].Play();
                    aquaSECount = 1;
                }
                else if (_character == Character.Megumin && meguminSECount == 0)
                {
                    meguminSE[1].Play();
                    meguminSECount = 1;
                }
                else if (_character == Character.Darkness && darknessSECount == 0)
                {
                    darknessSE[1].Play();
                    darknessSECount = 1;
                }
            }
            else if (_win)
            {
                if (_character == Character.Kazuma && kazumaSECount == 0)
                {
                    kazumaSE[0].Play();
                    kazumaSECount = 1;
                }
                else if (_character == Character.Aqua && aquaSECount == 0)
                {
                    aquaSE[0].Play();
                    aquaSECount = 1;
                }
                else if (_character == Character.Megumin && meguminSECount == 0)
                {
                    meguminSE[0].Play();
                    meguminSECount = 1;
                }
                else if (_character == Character.Darkness && darknessSECount == 0)
                {
                    darknessSE[0].Play();
                    darknessSECount = 1;
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(background, new Vector2(0, 0), Color.White);

            if (_lose)
            {
                spriteBatch.Draw(gameoverImg, new Rectangle(410, 100, 450, 350), Color.White);
            }
            else if (_win)
            {
                spriteBatch.Draw(congraImg, new Rectangle(330, 20, 650, 400), Color.White);
            }

            Effect();

            string finalScore = "Score: " + _score.Value.ToString();

            Vector2 textArea = _board.timerFont.MeasureString(finalScore); //vector of the area the text will take
            Vector2 center = new Rectangle(0,0,1280,720).Center.ToVector2();

            center.X -= textArea.X / 2;
            center.Y = 30;

            spriteBatch.DrawString(_board.timerFont, finalScore, center, Color.Pink);

            //draw buttons
            playAgainButton.Draw(spriteBatch);
            menuButton.Draw(spriteBatch);
            quitButton.Draw(spriteBatch);
        }
    }
}