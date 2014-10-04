// XNA
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

// GameEngine
using GameEngine.Physics;

namespace GameEngine.UI
{
	public class ProgressBar : Entity, IVisible, IDrawable, IEngineAddable
	{
		#region Public
		#region Property
		#region IVisible interface
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

		#region IDrawable interface
		public float Z { set; get; }
		#endregion

		public int MinValue { set; get; }
		public int MaxValue { set; get; }
		public int CurrValue { set; get; }

		public Color BackgroundColour { set; get; }
		public Color ForegroundColour { set; get; }

		public float AbsoluteRight
		{
			get
			{
				return AbsoluteX + AbsoluteWidth;
			}
		}

		public float AbsoluteBottom
		{
			get
			{
				return AbsoluteY + AbsoluteHeight;
			}
		}
		#endregion

		public ProgressBar()
		{
			Size = new Vector2(160, 32);
			Offset = Vector2.Zero;
			IsVisible = true;
			Z = 150;

			BackgroundColour = Color.White;
			ForegroundColour = Color.GreenYellow;
		}

		#region IEngineAddable interface
		public void AddToEngine()
		{
			EngineManager.Self.AddEntity(this);
		}

		public void RemoveFromEngine()
		{
			EngineManager.Self.RemoveEntity(this);
		}
		#endregion
		#endregion

		#region Private
		void IDrawable.Draw(SpriteBatch i_spriteBatch)
		{
			if (this.GetIsAbsoluteVisible())
			{
				Vector2 bgTopLeft = AbsoluteOffset;
				Vector2 bgTopRight = new Vector2(AbsoluteRight, bgTopLeft.Y);

				Vector2 fgTopLeft = AbsoluteOffset + new Vector2(3, 3);
				float maxFgWidth = AbsoluteWidth - 6;
				float percentage = (float)CurrValue / (float)(MaxValue - MinValue);
				Vector2 fgTopRight = new Vector2( fgTopLeft.X + maxFgWidth * percentage, fgTopLeft.Y);
				float fgHeight = AbsoluteBottom - bgTopLeft.Y - 6;

				Line.DrawLine(i_spriteBatch, AbsoluteBottom - bgTopLeft.Y, BackgroundColour, bgTopLeft, bgTopRight);
				if( percentage > 0.0f )
					Line.DrawLine(i_spriteBatch, fgHeight, ForegroundColour, fgTopLeft, fgTopRight);
			}
		}
		#endregion
	}
}
