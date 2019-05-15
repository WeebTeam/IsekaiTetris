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
        //private TButton kazuma, aqua, megumin, darkness; //these are toggleable buttons
        private Character kazuma, aqua, megumin, darkness;
        private Texture2D kazumaNormal, kazumaHover, aquaNormal, aquaHover, meguminNormal, meguminHover, darknessNormal, darknessHover, buttonNone, buttonHover;

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
            kazumaNormal = content.Load<Texture2D>("Images/kazuButton");
            kazumaHover = content.Load<Texture2D>("Images/kazuHover");
            aquaNormal = content.Load<Texture2D>("Images/aquaButton");
            aquaHover = content.Load<Texture2D>("Images/aquaHover");
            meguminNormal = content.Load<Texture2D>("Images/meguButton");
            meguminHover = content.Load<Texture2D>("Images/meguHover");
            darknessNormal = content.Load<Texture2D>("Images/darknessButton");
            darknessHover = content.Load<Texture2D>("Images/darknessHover");

            buttonNone = content.Load<Texture2D>("Buttons/buttonTemplate");
            buttonHover = content.Load<Texture2D>("Buttons/buttonTemplateHover");

            // Load font
            gameFont = content.Load<SpriteFont>("gameFont");
            menuFont = content.Load<SpriteFont>("menuFont");

            //load characters
            kazuma = new Character(new Rectangle(120, 150, 260, 470), kazumaNormal, kazumaHover);
            aqua = new Character(new Rectangle(380, 150, 260, 470), aquaNormal, aquaHover);
            megumin = new Character(new Rectangle(640, 150, 260, 470), meguminNormal, meguminHover);
            darkness = new Character(new Rectangle(900, 150, 260, 470), darknessNormal, darknessHover);

            // Load buttons 
            nextButton = new Button(new Rectangle(440, 640, 400, 50), gameFont, "Next", Color.White, buttonNone, buttonHover, buttonNone);
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
            kazuma.Update(mouseState);
            if (kazuma.Selected)
            {
                aqua.Selected = false;
                megumin.Selected = false;
                darkness.Selected = false;
            }

            aqua.Update(mouseState);
            if (aqua.Selected)
            {
                kazuma.Selected = false;
                megumin.Selected = false;
                darkness.Selected = false;
            }

            megumin.Update(mouseState);
            if (megumin.Selected)
            {
                aqua.Selected = false;
                kazuma.Selected = false;
                darkness.Selected = false;
            }

            darkness.Update(mouseState);
            if (darkness.Selected)
            {
                aqua.Selected = false;
                megumin.Selected = false;
                kazuma.Selected = false;
            }

            nextButton.Update(mouseState);

            if (nextButton.State == Button.GuiButtonState.Released)
            {
                //run game
                if (kazuma.Selected)
                    GameStateManager.Instance.AddScreen(new Engine(_graphicsDevice));
            }

            if (keyboardState.IsKeyDown(Keys.Escape))
            {
                GameStateManager.Instance.RemoveScreen();
            }

        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            //spriteBatch.Draw(background, Vector2.Zero, Color.White);
            //spriteBatch.DrawString(menuFont, "Main Menu", new Vector2(200, 100), Color.Black);

            kazuma.Draw(spriteBatch);
            aqua.Draw(spriteBatch);
            megumin.Draw(spriteBatch);
            darkness.Draw(spriteBatch);

            nextButton.Draw(spriteBatch);
        }
    }
}