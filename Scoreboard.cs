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
        private SpriteFont scoreFont;

        //filename
        private static string _scoreFile = "score.json";

        //score
        private static List<Score> _score;

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
            title = content.Load<Texture2D>("textures/scoreboard");

            //Load button textures
            buttonNone = content.Load<Texture2D>("textures/button_normal");
            buttonHover = content.Load<Texture2D>("textures/button_hover");

            musicEnabled = content.Load<Texture2D>("textures/music");
            musicDisabled = content.Load<Texture2D>("textures/no_music");

            // Load font
            scoreFont = content.Load<SpriteFont>("spritefonts/scoreFont");

            // Load buttons 
            backButton = new Button(new Rectangle(440, 640, 400, 50), scoreFont, "Back", Color.White, buttonNone, buttonHover, buttonNone); //440, 400, 400, 50

            LoadScore();
        }

        public static void LoadScore()
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


        public static void SaveScore(Score playerScore)
        {
            //load score lol
            LoadScore();

            //save to file;
            if (_score.Count >= 10)
            {
                //sort the list
                _score.Sort();

                //if new score is more than lowest score
                if (playerScore.Point > _score[9].Point)
                {
                    _score.RemoveAt(9);
                    _score.Add(playerScore);

                    //sort again
                    _score.Sort();
                }
            }
            //just add if scorefile has less than 10 scores
            else _score.Add(playerScore);

            //save to file
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

            spriteBatch.DrawString(scoreFont, "Name", new Vector2(280, 150), Color.White);
            spriteBatch.DrawString(scoreFont, "Score", new Vector2(500, 150), Color.White);
            spriteBatch.DrawString(scoreFont, "Character", new Vector2(750, 150), Color.White);
            for (int x = 0; x < _score.Count; x++)
            {
                try
                {
                    Score s = _score[x];
                    spriteBatch.DrawString(scoreFont, s.Name, new Vector2(280, 200 + 30 * x), Color.White);
                    spriteBatch.DrawString(scoreFont, "" + s.Point, new Vector2(500, 200 + 30 * x), Color.White);
                    spriteBatch.DrawString(scoreFont, "" + s.Character, new Vector2(750, 200 + 30 * x), Color.White);
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