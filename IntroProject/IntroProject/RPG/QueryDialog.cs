using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

// XNA
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

// GameEngine
using GameEngine;

namespace IntroProject.RPG
{
	public class QueryDialog : Entity, IVisible, IEngineAddable
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

		public bool IsYes { set; get; }
		#endregion

		#region IEngineAddable interface
		public void AddToEngine()
		{
			EngineManager.Self.AddEntity(this);
			_background.AddToEngine();

			_yes.AddToEngine();
			_no.AddToEngine();
		}

		public void RemoveFromEngine()
		{
			EngineManager.Self.RemoveEntity(this);
			_background.RemoveFromEngine();

			_yes.RemoveFromEngine();
			_no.RemoveFromEngine();
		}
		#endregion

		public QueryDialog()
		{
			IsYes = false;
			IsVisible = false;
			Offset = new Vector2(1120, 576);

			_background = new Sprite();
			_background.Parent = this;
			_background.Texture = EngineManager.Self.Load<Texture2D>("Textures/Query");
			_background.Z = 5.0f;

			_yes = new Text();
			_yes.Parent = this;
			_yes.Offset = new Vector2(32, 32);
			_yes.DisplayText = "Yes";
			_yes.Z = 5.001f;

			_no = new Text();
			_no.Parent = this;
			_no.Offset = new Vector2(32, 64);
			_no.DisplayText = "No";
			_no.Z = 5.001f;
		}

		public override void Update()
		{
			if( InputManager.Self.IsKeyDown(Keys.Up) || InputManager.Self.IsKeyDown(Keys.Down))
				IsYes = !IsYes;

			if (InputManager.Self.IsKeyDown(Keys.Enter))
				this.IsVisible = false;

			if (IsYes)
			{
				_yes.Colour = Color.Black;
				_no.Colour = Color.White;
			}
			else
			{
				_yes.Colour = Color.White;
				_no.Colour = Color.Black;
			}
		}
		#endregion

		#region Private
		Sprite _background;
		Text _yes;
		Text _no;
		#endregion
	}
}
