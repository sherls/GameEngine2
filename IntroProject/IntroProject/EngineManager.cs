using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace IntroProject
{
	public class EngineManager
	{
		static EngineManager mSelf;
		public static EngineManager Singleton
		{
			get
			{
				return mSelf;
			}
		}

		List<Sprite> mSpriteList = new List<Sprite>();
		SpriteBatch mSpriteBatch;
		ContentManager mContentManager;

		public EngineManager(SpriteBatch spriteBatch, ContentManager contentManager)
		{
			mSelf = this;
			mSpriteBatch = spriteBatch;
			mContentManager = contentManager;
		}

		public void AddSprite(Sprite sprite)
		{
			mSpriteList.Add(sprite);
		}

		public void Update(GameTime gameTime)
		{
			foreach (Sprite s in mSpriteList)
			{
				s.Offset.X++;
				if (s.Offset.X > 500)
					s.Offset.X = 100;
			}
		}

		public void Draw()
		{
			mSpriteBatch.Begin();

			foreach (Sprite s in mSpriteList)
			{
				s.Draw(mSpriteBatch);
			}

			mSpriteBatch.End();
		}

		public Texture2D LoadTexture2D(string name)
		{
			return mContentManager.Load<Texture2D>(name);
		}
	}
}