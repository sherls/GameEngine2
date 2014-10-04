using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

// XNA
using Microsoft.Xna.Framework.Graphics;

namespace GameEngine.Physics
{
	public class Rectangle : Entity, IVisible, ICollidable, IDrawable, IEngineAddable
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

		public bool IsFilled { set; get; }

		public Color Colour { set; get; }

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

		#region Methods
		public Rectangle()
		{
			Z = 1.0f;
			Size = new Vector2( 16, 16 );
			IsVisible = false;
			IsFilled = false;
			Colour = Color.White;
		}

		#region ICollidable interface
		public bool DoesCollideAgainst(ICollidable i_other)
		{
			bool bKnowsAboutOtherType = i_other is Rectangle;

			if (bKnowsAboutOtherType)
				return DoesCollideAgainstFallback(i_other);
			else
				return i_other.DoesCollideAgainstFallback(this);
		}

		public bool DoesCollideAgainst(ICollidable i_other, out Vector2 o_v2RepositionVector)
		{
			o_v2RepositionVector = Vector2.Zero;

			bool bReturnValue = DoesCollideAgainst(i_other);

			if (bReturnValue)
			{
				if (i_other is Rectangle)
				{
					Rectangle otherAsRectangle = i_other as Rectangle;

					Vector2 thisMin = this.AbsoluteOffset;
					Vector2 thisMax = new Vector2(this.AbsoluteRight, this.AbsoluteBottom);
					Vector2 otherMin = otherAsRectangle.AbsoluteOffset;
					Vector2 otherMax = new Vector2(otherAsRectangle.AbsoluteRight, otherAsRectangle.AbsoluteBottom);

					float left = otherMin.X - thisMax.X;
					float right = otherMax.X - thisMin.X;
					float top = otherMin.Y - thisMax.Y;
					float bottom = otherMax.Y - thisMin.Y;

					if (Math.Abs(left) < right)
						o_v2RepositionVector.X = left;
					else
						o_v2RepositionVector.X = right;

					if (Math.Abs(top) < bottom)
						o_v2RepositionVector.Y = top;
					else
						o_v2RepositionVector.Y = bottom;

					if( Math.Abs(o_v2RepositionVector.X) < Math.Abs(o_v2RepositionVector.Y) )
						o_v2RepositionVector.Y = 0.0f;
					else
						o_v2RepositionVector.X = 0.0f;

					o_v2RepositionVector = Vector2.Normalize(o_v2RepositionVector);

					if (Math.Abs(left) < right)
						o_v2RepositionVector.X *= left;
					else
						o_v2RepositionVector.X *= -right;

					if (Math.Abs(top) < bottom)
						o_v2RepositionVector.Y *= top;
					else
						o_v2RepositionVector.Y *= -bottom;
				}
			}
			return bReturnValue;
		}

		public bool DoesCollideAgainstFallback(ICollidable i_other)
		{
#if DEBUG
			if (i_other == null)
			{
				throw new ArgumentNullException( "DoesCollideAgainst requires a non-null argument");
			}
#endif

			if( i_other is Rectangle )
			{
				Rectangle otherAsRectangle = i_other as Rectangle;

				return this.AbsoluteRight > otherAsRectangle.AbsoluteX && this.AbsoluteX < otherAsRectangle.AbsoluteRight &&
					this.AbsoluteBottom > otherAsRectangle.AbsoluteY && this.AbsoluteY < otherAsRectangle.AbsoluteBottom;
			}
			else
			{
				// Don't do anything, this is unhandled type
				return false;
			}
		}

		public bool DoesCloseEnough(ICollidable i_other, float i_distance, out Vector2 o_v2direction)
		{
#if DEBUG
			if (i_other == null)
			{
				throw new ArgumentNullException( "DoesCloseEnough requires a non-null argument");
			}
#endif
			o_v2direction = Vector2.Zero;

			if (i_other is Rectangle)
			{
				Rectangle otherAsRectangle = i_other as Rectangle;

				Vector2 thisMin = this.AbsoluteOffset;
				Vector2 thisMax = new Vector2(this.AbsoluteRight, this.AbsoluteBottom);
				Vector2 otherMin = otherAsRectangle.AbsoluteOffset;
				Vector2 otherMax = new Vector2(otherAsRectangle.AbsoluteRight, otherAsRectangle.AbsoluteBottom);

				float left = otherMin.X - thisMax.X;
				float right = otherMax.X - thisMin.X;
				float top = otherMin.Y - thisMax.Y;
				float bottom = otherMax.Y - thisMin.Y;

				float xDistance = 0.0f;
				float xDirection = 0.0f;
				if (Math.Abs(left) < right)
				{
					xDistance = Math.Abs(left);
					xDirection = 1.0f;
				}
				else
				{
					xDistance = Math.Abs(right);
					xDirection = -1.0f;
				}

				float yDistance = 0.0f;
				float yDirection = 0.0f;
				if (Math.Abs(top) < bottom)
				{
					yDistance = Math.Abs(top);
					yDirection = -1.0f;
				}
				else
				{
					yDistance = Math.Abs(bottom);
					yDirection = 1.0f;
				}

				if((xDistance < i_distance) && (yDistance < this.Size.Y))
				{
					o_v2direction.X = xDirection;
					return true;
				}
				else if ((yDistance < i_distance) && (xDistance < this.Size.X))
				{
					o_v2direction.Y = yDirection;
					return true;
				}
				else
					return false;
			}

			return false;
		}
		#endregion

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
		#endregion

		#region Private
		void IDrawable.Draw(SpriteBatch i_spriteBatch)
		{
			if (this.GetIsAbsoluteVisible())
			{
				Vector2 topLeft = AbsoluteOffset;
				Vector2 topRight = new Vector2(AbsoluteRight, topLeft.Y);
				if (IsFilled)
				{
					Line.DrawLine(i_spriteBatch, AbsoluteBottom - topLeft.Y, Colour, topLeft, topRight);
				}
				else
				{
					Vector2 bottomLeft = new Vector2(topLeft.X, AbsoluteBottom);
					Vector2 bottomRight = topLeft + AbsoluteSize;

					Line.DrawLine(i_spriteBatch, 1, Colour, topLeft, topRight);
					Line.DrawLine(i_spriteBatch, 1, Colour, topRight, bottomRight);
					Line.DrawLine(i_spriteBatch, 1, Colour, bottomRight, bottomLeft);
					Line.DrawLine(i_spriteBatch, 1, Colour, bottomLeft, topLeft);
				}
			}
		}
		#endregion
	}
}
