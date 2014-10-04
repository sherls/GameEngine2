// XNA
using Microsoft.Xna.Framework;

namespace GameEngine.UI
{
	public class TextBox : Entity, IVisible, IEngineAddable
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

		public HAlignment TextHorizontalAlignment
		{
			set
			{
				_text.HorizontalAlignment = value;
				/*switch (_text.HorizontalAlignment)
				{
					case HAlignment.Left:
						_text.Offset = new Vector2(16, 16);
						break;
					case HAlignment.Right:
						_text.Offset = new Vector2(16, 16);
						break;
					case HAlignment.Centre:
						_text.Offset = new Vector2(AbsoluteWidth / 2, 16);
						break;
				}*/
			}
		}

		public VAlignment TextVerticalAlignment
		{
			set
			{
				_text.VerticalAlignment = value;
			}
		}

		public string DisplayText
		{
			set
			{
				_text.DisplayText = value;
			}
		}

		public bool IsBackgroundFilled
		{
			set
			{
				_background.IsFilled = value;
			}
		}

		public Color BackgroundColour
		{
			set
			{
				_background.Colour = value;
			}
		}

		public Color TextColour
		{
			set
			{
				_text.Colour = value;
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

		public TextBox()
		{
			IsVisible = true;
			Size = new Vector2(320, 64);

			_background = new GameEngine.Physics.Rectangle();
			_background.IsVisible = true;
			_background.SizeUnits = GameEngine.SizeUnits.RelativeToParent;
			_background.Size = Vector2.Zero;
			_background.Z = 2.0f;
			_background.IsFilled = true;
			_background.Parent = this;

			_text = new Text();
			_text.SizeUnits = GameEngine.SizeUnits.RelativeToParent;
			_text.Z = 3.0f;
			_text.Offset = new Vector2(16, 16);
			_text.Size = Vector2.Zero;
			_text.Parent = this;
		}
		#endregion

		#region Private
		#region Data
		private Text _text;
		private GameEngine.Physics.Rectangle _background;
		#endregion
		#endregion
	}
}
