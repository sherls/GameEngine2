// XNA
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

// GameEngine
using GameEngine;

namespace GameEngine.UI
{
	public class Button : Entity, IVisible, IEngineAddable
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

		public bool IsFocused
		{
			set
			{
				if (value == true)
				{
					_focusedButton.IsVisible = true;
					_unfocusedButton.IsVisible = false;
				}
				else
				{
					_focusedButton.IsVisible = false;
					_unfocusedButton.IsVisible = true;
				}
			}
		}

		public string DisplayText
		{
			set
			{
				_text.DisplayText = value;
			}
		}
		#endregion

		#region IEngineAddable interface
		public void AddToEngine()
		{
			EngineManager.Self.AddDrawable(_focusedButton);
			EngineManager.Self.AddDrawable(_unfocusedButton);
			EngineManager.Self.AddDrawable(_text);
		}

		public void RemoveFromEngine()
		{
			EngineManager.Self.RemoveDrawable(_focusedButton);
			EngineManager.Self.RemoveDrawable(_unfocusedButton);
			EngineManager.Self.RemoveDrawable(_text);
		}
		#endregion

		public Button()
		{
			IsVisible = true;
			Size = new Vector2(128, 0);

			_focusedButton = new Sprite();
			_focusedButton.Parent = this;
			_focusedButton.Size = new Vector2(0, 0);
			_focusedButton.Offset = new Vector2(0, 0);
			_focusedButton.SizeUnits = SizeUnits.RelativeToParent;
			_focusedButton.Z = 1;
			_focusedButton.Texture = EngineManager.Self.Load<Texture2D>("Textures/FocusedButton");

			_unfocusedButton = new Sprite();
			_unfocusedButton.Parent = this;
			_unfocusedButton.Size = new Vector2(0, 0);
			_unfocusedButton.Offset = new Vector2(0, 0);
			_unfocusedButton.SizeUnits = SizeUnits.RelativeToParent;
			_unfocusedButton.Z = 1;
			_unfocusedButton.Texture = EngineManager.Self.Load<Texture2D>("Textures/UnfocusedButton");

			_text = new Text();
			_text.Offset = new Vector2(16, 0);
			_text.Size = new Vector2(0, 32);
			_text.SizeUnits = SizeUnits.RelativeToParent;
			_text.HorizontalAlignment = HAlignment.Centre;
			_text.Colour = Color.Black;
			_text.Parent = this;
			_text.Z = 1.5f;

			IsFocused = false;
		}
		#endregion

		#region Private
		Sprite _focusedButton;
		Sprite _unfocusedButton;
		Text _text;
		#endregion
	}
}
