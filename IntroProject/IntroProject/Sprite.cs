using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace IntroProject
{
	public class Sprite
	{
		public Texture2D Texture { get; set; }
		public Vector2 Offset;

		// Load? (for Texture)

		public bool IsVisible { get; set; }

		public Sprite()
		{

		}

		public void Draw(SpriteBatch spriteBatch)
		{
			spriteBatch.Draw(Texture, new Rectangle((int)Offset.X, (int)Offset.Y, 50, 50), Color.White);
		}
	}
}
