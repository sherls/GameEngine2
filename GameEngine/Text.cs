using System;
using System.Text;

// XNA
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace GameEngine
{
	public enum HAlignment
	{
		Left,
		Centre,
		Right
	}

	public enum VAlignment
	{
		Top,
		Centre,
		Bottom
	}

	public class Text : Entity, IVisible, IDrawable, IEngineAddable
	{
		#region Public
		#region Data
		public bool bWrapped;
		#endregion

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

		#region Entity override
		public override Vector2 AbsoluteSize
		{
			get
			{
				return new Vector2(AbsoluteWidth, AbsoluteHeight);
			}
		}

		public override float AbsoluteWidth
		{
			get
			{
				if (SizeUnits == GameEngine.SizeUnits.RelativeToParent)
				{
					if (HorizontalAlignment == HAlignment.Centre)
						return -(Parent.AbsoluteWidth / 2 + this.Size.X);
					else
						return Parent.AbsoluteWidth + this.Size.X;
				}
				else
					return Font.MeasureString(this.DisplayText).X;
			}
		}

		public override float AbsoluteHeight
		{
			get
			{
				if (SizeUnits == GameEngine.SizeUnits.RelativeToParent)
					return Parent.AbsoluteHeight + this.Size.Y - Font.MeasureString(this.DisplayText).Y;
				else
					return Font.MeasureString(this.DisplayText).Y;
			}
		}
		#endregion

		public Color Colour { set; get; }

		public SpriteFont Font
		{
			set
			{
				_font = value;
				if (_font == null)
					throw new Exception("Null fonts are not supported");
			}
			get
			{
				return _font;
			}
		}

		public string DisplayText { set; get; }
		#endregion

		#region Methods
		public Text()
		{
			_font = null;
			Offset = Camera.Self.ScreenCentre;
			Size = new Vector2(160, 32);
			Colour = Color.White;
			Font = EngineManager.Self.Load<SpriteFont>("Fonts/font");
			HorizontalAlignment = HAlignment.Left;
			VerticalAlignment = VAlignment.Top;
			DisplayText = "";
			Z = 1;
			IsVisible = true;
		}

		#region IEngineAddable interface
		public void AddToEngine()
		{
			EngineManager.Self.AddText(this);
		}

		public void RemoveFromEngine()
		{
			EngineManager.Self.RemoveText(this);
		}
		#endregion
		#endregion

		#region Override methods
		override public void Update()
		{
			base.Update();
		}
		#endregion
		#endregion

		#region Private
		#region Data
		private SpriteFont _font;
		#endregion

		#region Property
		private string WrappedText
		{
			get
			{
				if ((DisplayText.Length > 0) && (_font != null))
				{
					string[] words = DisplayText.Split(new Char[] { ' ', ',', '\n', '\t' });

					StringBuilder sb = new StringBuilder();
					float lineWidth = 0.0f;
					float spaceWidth = Font.MeasureString(" ").X;
					foreach (string word in words)
					{
						Vector2 size = Font.MeasureString(word);
						float width = Parent.AbsoluteWidth + this.Size.X;
						if (lineWidth + size.X < (width - 16.0f))
						{
							sb.Append(word + " ");
							lineWidth += size.X + spaceWidth;
						}
						else
						{
							sb.Append("\n" + word + " ");
							lineWidth = size.X + spaceWidth;
						}
					}

					return sb.ToString();
				}
				return DisplayText;
			}
		}
		#endregion

		#region Methods
		void IDrawable.Draw(SpriteBatch i_spriteBatch)
		{
			if (!string.IsNullOrEmpty(DisplayText) && (Font != null) && this.GetIsAbsoluteVisible())
			{
#if DEBUG
				if (Font == null)
				{
					throw new ArgumentNullException("Null fonts are not supported");
				}
#endif
				Vector2 origin = GetDrawingOrigin();

				if (SizeUnits == GameEngine.SizeUnits.RelativeToParent)
				{
					i_spriteBatch.DrawString(_font, WrappedText, AbsoluteOffset, Colour, 0, origin,
						1, SpriteEffects.None, 1);
				}
				else
				{
					i_spriteBatch.DrawString(_font, DisplayText, AbsoluteOffset, Colour, 0, origin,
						1, SpriteEffects.None, 1);
				}
			}
		}
		#endregion
		#endregion
	}
}
