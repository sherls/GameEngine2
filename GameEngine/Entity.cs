using System;

using Microsoft.Xna.Framework;

namespace GameEngine
{
	public enum SizeUnits
	{
		Absolute,
		RelativeToParent
	}

	public class Entity // Vic's PositionedObject
	{
		#region Public
		#region Data
		public Vector2 Size;
		public Vector2 Offset;
		public Vector2 Velocity;
		public Vector2 Acceleration;
		public string Name;
		#endregion

		#region Property
		public Entity Parent { set; get; }
		public HAlignment HorizontalAlignment { set; get; }
		public VAlignment VerticalAlignment { set; get; }

		public Vector2 AbsoluteOffset
		{
			set
			{
				AbsoluteX = value.X;
				AbsoluteY = value.Y;
			}
			get
			{
				return new Vector2(AbsoluteX, AbsoluteY);
			}
		}

		public float AbsoluteX
		{
			set
			{
				if (Parent == null)
					Offset.X = value;
				else
					Offset.X = value - Parent.AbsoluteX;
			}
			get
			{
				if (Parent == null)
					return Offset.X;
				else
					return Parent.AbsoluteX + this.Offset.X;
			}
		}

		public float AbsoluteY
		{
			set
			{
				if (Parent == null)
					Offset.Y = value;
				else
					Offset.Y = value - Parent.AbsoluteY;
			}
			get
			{
				if (Parent == null)
					return Offset.Y;
				else
					return Parent.AbsoluteY + this.Offset.Y;
			}
		}

		public Entity TopParent
		{
			get
			{
				if (this.Parent != null)
					return this.Parent.TopParent;
				else
					return this;
			}
		}

		public SizeUnits SizeUnits { set; get; }

		public virtual Vector2 AbsoluteSize
		{
			set
			{
				AbsoluteWidth = value.X;
				AbsoluteHeight = value.Y;
			}
			get
			{
				return new Vector2(AbsoluteWidth, AbsoluteHeight);
			}
		}

		public virtual float AbsoluteWidth
		{
			set
			{
				if ((Parent == null) || (SizeUnits == SizeUnits.Absolute))
				{
					Size.X = value;
				}
				else
				{
					switch (SizeUnits)
					{
						case SizeUnits.RelativeToParent:
							Size.X = value - Parent.AbsoluteWidth;
							break;
						default:
#if DEBUG
							throw new Exception("Did not handle Size Unit " + SizeUnits);
#else
							Size.X = value - Parent.AbsoluteWidth;
							break;
#endif
					}
				}
			}
			get
			{
				if ((Parent == null) || (SizeUnits == SizeUnits.Absolute))
				{
					return Size.X;
				}
				else
				{
					switch (SizeUnits)
					{
						case SizeUnits.RelativeToParent:
							return Parent.AbsoluteWidth + this.Size.X;
						default:
#if DEBUG
							throw new Exception("Did not handle Size Unit " + SizeUnits);
#else
							return Size.X;
#endif
					}
				}
			}
		}

		public virtual float AbsoluteHeight
		{
			set
			{
				if ((Parent == null) || (SizeUnits == SizeUnits.Absolute))
				{
					Size.Y = value;
				}
				else
				{
					switch (SizeUnits)
					{
						case SizeUnits.RelativeToParent:
							Size.Y = value - Parent.AbsoluteHeight;
							break;
						default:
#if DEBUG
							throw new Exception("Did not handle Size Unit " + SizeUnits);
#else
							Size.Y = value;
							break;
#endif
					}

				}
			}
			get
			{
				if ((Parent == null) || (SizeUnits == SizeUnits.Absolute))
				{
					return Size.Y;
				}
				else
				{
					switch (SizeUnits)
					{
						case SizeUnits.RelativeToParent:
							return Parent.AbsoluteHeight + this.Size.Y;
						default:
#if DEBUG
							throw new Exception("Did not handle Size Unit " + SizeUnits);
#else
							return Size.Y;
#endif
					}
				}
			}
		}
		#endregion

		#region Methods
		public Entity()
		{
			Name = "No Name";
			SizeUnits = SizeUnits.Absolute;
		}
		#endregion

		#region Virtual methods
		virtual public void Update()
		{
			Velocity = Velocity + Acceleration * TimeManager.Self.DeltaTimeFloat;
			Offset += Velocity * TimeManager.Self.DeltaTimeFloat;
		}
		#endregion
		#endregion

		#region Private
		protected Vector2 GetDrawingOrigin()
		{
			Vector2 dimensions = AbsoluteSize;
			Vector2 origin = Vector2.Zero;

			if (HorizontalAlignment == HAlignment.Centre)
				origin.X = dimensions.X / 2;
			else if (HorizontalAlignment == HAlignment.Right)
				origin.X = dimensions.X;

			if (VerticalAlignment == VAlignment.Centre)
				origin.Y = dimensions.Y / 2;
			else if (VerticalAlignment == VAlignment.Bottom)
				origin.Y = dimensions.Y;

			return origin;
		}
		#endregion
	}
}
