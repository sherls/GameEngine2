//#define USE_SINGLETON

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

// XNA
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameEngine.Physics
{
#if USE_SINGLETON
	public class Line : Singleton<Line>
	{
		#region Public
		public Line()
		{
		}

		public void Initialize(GraphicsDevice i_graphicsDevice)
		{
			_blank = new Texture2D(i_graphicsDevice, 1, 1, false, SurfaceFormat.Color);
			_blank.SetData(new[] { Color.White });
		}

		public void DrawLine(SpriteBatch i_spriteBatch, float width, Color i_colour,
			Vector2 i_startPoint, Vector2 i_endPoint)
		{
			float angle = (float)Math.Atan2(i_endPoint.Y - i_startPoint.Y, i_endPoint.X - i_startPoint.X);
			float length = Vector2.Distance(i_startPoint, i_endPoint);

			i_spriteBatch.Draw(_blank, i_startPoint, null, i_colour, angle, Vector2.Zero, new Vector2(length, width),
				SpriteEffects.None, 0);
		}
		#endregion

		#region Private
		Texture2D _blank;
		#endregion
	}
#else
	public class Line : Entity, IVisible, IDrawable, IEngineAddable
	{
		#region Public
		#region Property
		#region IVisible property
		public bool IsVisible { set; get; }

		// Explicit implementation
		IVisible IVisible.Parent
		{
			get
			{
				return Parent as IVisible;
			}
		}
		#endregion

		#region IDrawable
		public float Z { set; get; }
		#endregion

		public Vector2 EndPoint { set; get; }
		#endregion

		public Line()
		{
			IsVisible = true;
			Z = 1;
		}

		static public void DrawLine( SpriteBatch i_spriteBatch, float i_height, Color i_colour,
			Vector2 i_startPoint, Vector2 i_endPoint )
		{
			float length = Vector2.Distance(i_startPoint, i_endPoint);
			float angle = (float)Math.Atan2(i_endPoint.Y - i_startPoint.Y, i_endPoint.X - i_startPoint.X);

			if (_lineDrawingTexture == null)
			{
				_lineDrawingTexture = new Texture2D(i_spriteBatch.GraphicsDevice, 1, 1, false, SurfaceFormat.Color);
				_lineDrawingTexture.SetData(new[] { Color.White });
			}

			length = Vector2.Distance(i_startPoint, i_endPoint);
			i_spriteBatch.Draw(_lineDrawingTexture, i_startPoint, null, i_colour, angle, Vector2.Zero, new Vector2(length, i_height),
				SpriteEffects.None, 0);
		}

		#region IEngineAddable interface
		public void AddToEngine()
		{
			EngineManager.Self.AddDrawable(this);
		}

		public void RemoveFromEngine()
		{
			EngineManager.Self.RemoveDrawable(this);
		}
		#endregion
		#endregion

		#region Private
		static Texture2D _lineDrawingTexture = null;

		#region Override methods
		void IDrawable.Draw(SpriteBatch i_spriteBatch)
		{
			float length = Vector2.Distance(AbsoluteOffset, EndPoint);
			if (this.GetIsAbsoluteVisible() && length != 0)
			{
				DrawLine( i_spriteBatch, 1, Color.White, AbsoluteOffset, EndPoint);
			}
		}
		#endregion
		#endregion
	}
#endif
}
