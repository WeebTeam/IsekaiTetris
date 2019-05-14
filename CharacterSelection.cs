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
        private Button kazumaButton, aquaButton, meguminButton, darknessButton;
        private Texture2D kazuma, kazumahover, aqua, megumin, darkness;

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
            kazuma = content.Load<Texture2D>("Images/kazuma");
            kazumahover = content.Load<Texture2D>("Images/kazuma_hover");
            aqua = content.Load<Texture2D>("Images/aqua");
            megumin = content.Load<Texture2D>("Images/megumin");
            darkness = content.Load<Texture2D>("Images/darkness");

            // Load font
            gameFont = content.Load<SpriteFont>("gameFont");
            menuFont = content.Load<SpriteFont>("menuFont");

            // Load buttons 
            kazumaButton = new Button(new Rectangle(120, 150, 260, 470), kazuma, kazumahover, kazumahover);
            aquaButton = new Button(new Rectangle(380, 150, 260, 470), aqua, aqua, aqua);
            meguminButton = new Button(new Rectangle(640, 150, 260, 470), megumin, megumin, megumin);
            darknessButton = new Button(new Rectangle(900, 150, 260, 470), darkness, darkness, darkness);

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
            kazumaButton.Update(mouseState);
            aquaButton.Update(mouseState);
            meguminButton.Update(mouseState);
            darknessButton.Update(mouseState);

            if (kazumaButton.State == Button.GuiButtonState.Released)
            {
                //run game
                GameStateManager.Instance.AddScreen(new Engine(_graphicsDevice));
            }

            if (aquaButton.State == Button.GuiButtonState.Released)
            {
                //quit game
                //GameStateManager.Instance.ClearScreens();
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
        }
    }
}