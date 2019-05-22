using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace Tetris
{
    public class Scoreboard : GameState
    {
        private Texture2D background, backgroundshade, title;

        //buttons (and button textures)
        private Button backButton;
        private Texture2D buttonNone, buttonHover;
        private Texture2D musicEnabled, musicDisabled;

        //fonts
        private SpriteFont gameFont, scoreFont;

        //filename
        private string _scoreFile = "score.json";

        //score
        private List<Score> _score;

        public Scoreboard(GraphicsDevice graphicsDevice)
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
            background = content.Load<Texture2D>("textures/characterMenu");
            backgroundshade = content.Load<Texture2D>("textures/pause");
            title = content.Load<Texture2D>("textures/characterTitle");

            //Load button textures
            buttonNone = content.Load<Texture2D>("textures/button_normal");
            buttonHover = content.Load<Texture2D>("textures/button_hover");

            musicEnabled = content.Load<Texture2D>("textures/music");
            musicDisabled = content.Load<Texture2D>("textures/no_music");

            // Load font
            gameFont = content.Load<SpriteFont>("spritefonts/gameFont");
            scoreFont = content.Load<SpriteFont>("spritefonts/scoreFont");

            // Load buttons 
            backButton = new Button(new Rectangle(440, 640, 400, 50), gameFont, "Back", Color.White, buttonNone, buttonHover, buttonNone); //440, 400, 400, 50

            LoadScore();
        }

        private void LoadScore()
        {
            // load score

            try
            {
                _score = JsonConvert.DeserializeObject<List<Score>>(File.ReadAllText(_scoreFile)); // reads the json string from file and parse to _score
            }
            catch (Exception) //if file not found, create it and open it
            {
                Console.Write("file not found, creating a new file");
                File.Create(_scoreFile).Close(); //when file is created it returns a file object, so we close it 

                //initialise the score list
                _score = new List<Score>();

                //write initial json structure into file
                File.WriteAllText(_scoreFile, JsonConvert.SerializeObject(_score));
            }
        }

        private void SaveScore()
        {
            //save to file;
            _score.Add(new Score("player 1", 1020, Character.Kazuma));
            _score.Add(new Score("player 2", 3232, Character.Megumin));
            _score.Add(new Score("player 3", 1020, Character.Darkness));
            File.WriteAllText(_scoreFile, JsonConvert.SerializeObject(_score));
        }

        public override void UnloadContent()
        {
        }

        public override void Update(GameTime gameTime)
        {
            // Gets keyboard input
            KeyboardState keyboardState = Keyboard.GetState();
            MouseState mouseState = Mouse.GetState();

            //button input checking
            backButton.Update(mouseState);

            if (backButton.State == Button.GuiButtonState.Released || keyboardState.IsKeyDown(Keys.Escape))
            {
                GameStateManager.Instance.RemoveScreen();
            }

        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(background, new Vector2(0, 0), Color.White);
            spriteBatch.Draw(backgroundshade, new Vector2(0, 0), Color.White);

            spriteBatch.Draw(title, new Rectangle(440, 0, 400, 200), Color.White);

            spriteBatch.DrawString(gameFont, "Rank    Name    Score    Character", new Vector2(200, 150), Color.White);
            for (int x = 1; x <= 10; x++)
            {
                try
                {
                    Score s = _score[x - 1];
                    spriteBatch.DrawString(scoreFont, s.Name, new Vector2(200, 200 + 10 * x), Color.White);
                }
                catch (Exception)
                {
                    Console.WriteLine("out of range. x=" + x);
                }
            }

            //draw buttons
            backButton.Draw(spriteBatch);
        }
    }
}