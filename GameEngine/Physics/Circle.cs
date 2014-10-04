using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

// XNA
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameEngine.Physics
{
	public class Circle : Entity, IVisible, ICollidable, IDrawable, IEngineAddable
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

		public float Radius { set; get; }

		#endregion

		#region Methods
		public Circle()
		{
			Radius = 16;
			Z = 1.0f;
			IsVisible = false;
			Colour = Color.White;
			IsFilled = true;
			EngineManager.Self.AddDrawable(this);
		}

		#region ICollidable interface
		public bool DoesCollideAgainst(ICollidable i_other)
		{
			bool bKnowsAboutOtherType = i_other is Circle || i_other is Rectangle;

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
				if (i_other is Circle)
				{
					Circle otherAsCircle = i_other as Circle;
					Vector2 normal = otherAsCircle.AbsoluteOffset - this.AbsoluteOffset;
					o_v2RepositionVector = Vector2.Normalize(normal);

					// I'm using Length() for simplicity - if this is slow, consider using LengthSquared()
					// - or even not doing Vector2 operations at all
					float distanceApart = (this.AbsoluteOffset - otherAsCircle.AbsoluteOffset).Length();
					float combinedRadii = this.Radius + otherAsCircle.Radius;
					float repositionAmount = combinedRadii - distanceApart;

					o_v2RepositionVector *= repositionAmount;
				}
				else if (i_other is Rectangle)
				{
					Rectangle otherAsRectangle = i_other as Rectangle;
					float rectangleLeft = otherAsRectangle.AbsoluteX;// rectangleRight - otherAsRectangle.Dimension.X;
					float rectangleRight = otherAsRectangle.AbsoluteRight;
					float rectangleTop = otherAsRectangle.AbsoluteY;//rectangleBottom - otherAsRectangle.Dimension.Y;
					float rectangleBottom = otherAsRectangle.AbsoluteBottom;

					// Find the closest point to the circle within the rectangle
					float closestX = MathHelper.Clamp(this.AbsoluteX, rectangleLeft, rectangleRight);
					float closestY = MathHelper.Clamp(this.AbsoluteY, rectangleTop, rectangleBottom);

					// Calculate the distance between the circle's centre and this closest point
					float distanceX = this.AbsoluteX - closestX;
					float distanceY = this.AbsoluteY - closestY;

					o_v2RepositionVector = new Vector2(distanceX, distanceY);
					o_v2RepositionVector = Vector2.Normalize(o_v2RepositionVector);
					o_v2RepositionVector.X *= (Math.Abs(distanceX) - this.Radius);
					o_v2RepositionVector.Y *= (Math.Abs(distanceY) - this.Radius);
				}
			}

			return bReturnValue;
		}

		public bool DoesCollideAgainstFallback(ICollidable i_other)
		{
#if DEBUG
			if (i_other == null)
			{
				throw new ArgumentNullException("DoesCollideAgainst requires a non-null argument");
			}
#endif

			if (i_other is Circle)
			{
				Circle otherAsCircle = i_other as Circle;
				float distanceSquared = (this.AbsoluteOffset - otherAsCircle.AbsoluteOffset).LengthSquared();

				return (distanceSquared < Math.Pow((this.Radius + otherAsCircle.Radius), 2));
			}
			else if (i_other is Rectangle)
			{
				Rectangle otherAsRectangle = i_other as Rectangle;
				float rectangleLeft = otherAsRectangle.AbsoluteX;// rectangleRight - otherAsRectangle.Dimension.X;
				float rectangleRight = otherAsRectangle.AbsoluteRight;
				float rectangleTop = otherAsRectangle.AbsoluteY;//rectangleBottom - otherAsRectangle.Dimension.Y;
				float rectangleBottom = otherAsRectangle.AbsoluteBottom;

				// Find the closest point to the circle within the rectangle
				float closestX = MathHelper.Clamp(this.AbsoluteX, rectangleLeft, rectangleRight);
				float closestY = MathHelper.Clamp(this.AbsoluteY, rectangleTop, rectangleBottom);

				// Calculate the distance between the circle's centre and this closest point
				float distanceX = this.AbsoluteX - closestX;
				float distanceY = this.AbsoluteY - closestY;

				// If the distance is less than the circle's radius, an intersection occurs
				float distanceSquared = (distanceX * distanceX) + (distanceY * distanceY);
				return (distanceSquared < (this.Radius * this.Radius));
			}
			else
			{
				// Don't do anything, this is an unhandled type
				return false;
			}
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
				if (IsFilled)
				{
					for (double i = 0; i < (2 * Math.PI); i += 0.01)
					{
						float x = AbsoluteX + (float)(Math.Cos(i)) * Radius;
						float y = AbsoluteY + (float)(Math.Sin(i)) * Radius;
						Line.DrawLine(i_spriteBatch, 1, Colour, AbsoluteOffset, new Vector2(x, y));
					}
				}
				else
				{
					for (uint i = 0; i < 500; ++i)
					{
						double angle = 2 * Math.PI / 315 * i;
						float x1 = AbsoluteX + (float)Math.Sin(angle) * Radius;
						float y1 = AbsoluteY + (float)Math.Cos(angle) * Radius;
						++i;
						angle = 2 * Math.PI / 315 * i;
						float x2 = AbsoluteX + (float)Math.Sin(angle) * Radius;
						float y2 = AbsoluteY + (float)Math.Cos(angle) * Radius;
						Line.DrawLine(i_spriteBatch, 1, Colour, new Vector2(x1, y1), new Vector2(x2, y2));
					}
				}
			}
		}
		#endregion
	}
}
