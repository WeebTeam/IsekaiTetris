using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Tetris
{
	/// <summary>
	/// This is a game component that implements IUpdateable.
	/// </summary>
	public class Score
	{
		// Graphic
		protected SpriteFont font;

		// Counters
		protected int value = 0;
		protected int level = 0;
		protected int recordScore = 0;
		protected string recordPlayer = "Player 1";

		public Score (SpriteFont font)
        {
			this.font = font;
		}

		/// <summary>
		/// Allows the game component to perform any initialization it needs to before starting
		/// to run.  This is where it can query for any required services and load content.
		/// </summary>
		public void Initialize ()
		{
			value = 0;
			level = 1;
            recordScore = 0;
		}

		public int Value {
			set { this.value = value; }
			get { return value; }
		}

		//public int Level {
		//	set { level = value; }
		//	get { return level; }
		//}

		public int RecordScore {
			set { recordScore = value; }
			get { return recordScore; }
		}

		public string RecordPlayer {
			set { recordPlayer = value; }
			get { return recordPlayer; }
		}

		/// <summary>
		/// This is called when the game should draw itself.
		/// </summary>
		/// <param name="gameTime">Provides a snapshot of timing values.</param>
		public void Draw (SpriteBatch sBatch)
		{
            //sBatch.DrawString(font, "you suck", new Vector2(0, 0), Color.Green);

            //sBatch.DrawString (font, "Score:\n" + value + "\nLevel: " + level, new Vector2 (0,0), Color.Green); // 1.5f * 24, 3 * 24

            //sBatch.DrawString (font, "Record:\n" + recordPlayer + "\n" + recordScore, new Vector2 (0,10), Color.Orange); //1.5f * 24, 13 * 24
        }
	}
}