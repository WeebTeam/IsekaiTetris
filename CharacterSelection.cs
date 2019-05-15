using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Tetris
{
    public class CharacterSelection : GameState
    {
        private Texture2D background;

        //buttons (and button textures)
        private Button nextButton;
        private TButton kazumaButton, aquaButton, meguminButton, darknessButton; //these are toggleable buttons
        private Texture2D kazuma, kazumahover, aqua, aquahover, megumin, meguminhover, darkness, darknesshover, buttonNone, buttonHover;

        //fonts
        private SpriteFont gameFont, menuFont;

        public CharacterSelection(GraphicsDevice graphicsDevice)
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

            //Load button textures
            kazuma = content.Load<Texture2D>("Images/kazuButton");
            kazumahover = content.Load<Texture2D>("Images/kazuHover");
            aqua = content.Load<Texture2D>("Images/aquaButton");
            aquahover = content.Load<Texture2D>("Images/aquaHover");
            megumin = content.Load<Texture2D>("Images/meguButton");
            meguminhover = content.Load<Texture2D>("Images/meguHover");
            darkness = content.Load<Texture2D>("Images/darknessButton");
            darknesshover = content.Load<Texture2D>("Images/darknessHover");

            buttonNone = content.Load<Texture2D>("Buttons/buttonTemplate");
            buttonHover = content.Load<Texture2D>("Buttons/buttonTemplateHover");


            // Load font
            gameFont = content.Load<SpriteFont>("gameFont");
            menuFont = content.Load<SpriteFont>("menuFont");

            // Load buttons 
            kazumaButton = new TButton(new Rectangle(120, 150, 260, 470), kazuma, kazumahover, kazumahover);
            aquaButton = new TButton(new Rectangle(380, 150, 260, 470), aqua, aquahover, aquahover);
            meguminButton = new TButton(new Rectangle(640, 150, 260, 470), megumin, meguminhover, meguminhover);
            darknessButton = new TButton(new Rectangle(900, 150, 260, 470), darkness, darknesshover, darknesshover);

            nextButton = new Button(new Rectangle(440, 640, 400, 50), gameFont, "Next", Color.White, buttonNone, buttonHover, buttonNone);

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

            //check if character is selected, then deselects other characters
            kazumaButton.Update(mouseState);
            if (kazumaButton.Selected)
            {
                aquaButton.Selected = false;
                meguminButton.Selected = false;
                darknessButton.Selected = false;
            }

            aquaButton.Update(mouseState);
            if (aquaButton.Selected)
            {
                kazumaButton.Selected = false;
                meguminButton.Selected = false;
                darknessButton.Selected = false;
            }

            meguminButton.Update(mouseState);
            if (meguminButton.Selected)
            {
                aquaButton.Selected = false;
                kazumaButton.Selected = false;
                darknessButton.Selected = false;
            }

            darknessButton.Update(mouseState);
            if (darknessButton.Selected)
            {
                aquaButton.Selected = false;
                meguminButton.Selected = false;
                kazumaButton.Selected = false;
            }

            nextButton.Update(mouseState);

            if (nextButton.State == Button.GuiButtonState.Released)
            {
                //run game
                if (kazumaButton.Selected)
                    GameStateManager.Instance.AddScreen(new Engine(_graphicsDevice));
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            //spriteBatch.Draw(background, Vector2.Zero, Color.White);
            //spriteBatch.DrawString(menuFont, "Main Menu", new Vector2(200, 100), Color.Black);

            kazumaButton.Draw(spriteBatch);
            aquaButton.Draw(spriteBatch);
            meguminButton.Draw(spriteBatch);
            darknessButton.Draw(spriteBatch);

            nextButton.Draw(spriteBatch);
        }
    }
}