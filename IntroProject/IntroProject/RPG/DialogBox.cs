// XNA
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

// GameEngine
using GameEngine;

namespace IntroProject.RPG
{
	public class DialogBox : Entity, IVisible, IEngineAddable
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
			EngineManager.Self.AddDrawable(_background);
			EngineManager.Self.AddDrawable(_text);
		}

		public void RemoveFromEngine()
		{
			EngineManager.Self.RemoveDrawable(_background);
			EngineManager.Self.RemoveDrawable(_text);
		}
		#endregion

		public DialogBox()
		{
			IsVisible = false;
			Size = Vector2.Zero;// new Vector2(1280, 224);
			Offset = new Vector2(0, 576);

			_background = new Sprite();
			_background.Parent = this;
			_background.Size = new Vector2(0, 0);
			_background.Offset = new Vector2(0, 0);
			_background.SizeUnits = SizeUnits.RelativeToParent;
			_background.Z = 1.5f;
			_background.Texture = EngineManager.Self.Load<Texture2D>("Textures/Dialog");

			_text = new Text();
			_text.Offset = new Vector2(32, 32);
			_text.Size = new Vector2(1280, 160);
			_text.SizeUnits = SizeUnits.RelativeToParent;
			_text.Colour = Color.White;
			_text.Parent = this;
			_text.Z = 2.0f;
		}

		#endregion

		#region Private
		#region Data
		private Text _text;
		private Sprite _background;
		#endregion
		#endregion
	}
}
