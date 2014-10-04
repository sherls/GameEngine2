using System;
using System.Collections.Generic;

// XNA
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

// GameEngine
using GameEngine;

namespace IntroProject.RPG
{
	public class Menu : Entity, IVisible, IEngineAddable
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
		#endregion

		#region IEngineAddable interface
		public void AddToEngine()
		{
			EngineManager.Self.AddEntity(this);
			_background.AddToEngine();

			foreach (Text option in _options)
				option.AddToEngine();

			foreach (Text item in _items)
				item.AddToEngine();
		}

		public void RemoveFromEngine()
		{
			EngineManager.Self.RemoveEntity(this);
			_background.RemoveFromEngine();

			foreach (Text option in _options)
				option.RemoveFromEngine();

			foreach (Text item in _items)
				item.RemoveFromEngine();
		}
		#endregion

		public Menu()
		{
			this.IsVisible = false;
			float z = 5.0f;

			_background = new Sprite();
			_background.Parent = this;
			_background.HorizontalAlignment = HAlignment.Centre;
			_background.VerticalAlignment = VAlignment.Centre;
			_background.Texture = EngineManager.Self.Load<Texture2D>("Textures/Menu");
			_background.Offset = Camera.Self.ScreenCentre;
			_background.Z = z;
			z += 0.001f;

			_items = new List<Text>();
			float xPos = 336.0f;
			float yPos = 176.0f;
			for (uint i = 0; i < 14; ++i)
			{
				Text tempItem = new Text();
				tempItem.Parent = this;
				tempItem.DisplayText = "";
				tempItem.Size = new Vector2(768, 32);
				tempItem.Offset = new Vector2(xPos, yPos);
				tempItem.Z = z;
				yPos += 32;
				_items.Add(tempItem);
			}

			_options = new List<Text>();
			Text temp = new Text();
			temp.Parent = this;
			temp.DisplayText = "Status";
			temp.Size = new Vector2(160, 32);
			temp.Offset = new Vector2(176, 176);
			temp.Z = z;
			_options.Add(temp);

			_background.IsVisible = true;

			_prevOption = MenuOptions.Max;
			_currOption = MenuOptions.Status;
		}

		public override void Update()
		{
			switch (_currOption)
			{
				case MenuOptions.Status:
					_items[0].DisplayText = String.Format("Level: {0}", RPGHelper.Self.PlayerLevel);
					_items[1].DisplayText = String.Format("HP: {0}/{1}", RPGHelper.Self.PlayerCurrHP, RPGHelper.Self.PlayerMaxHP);
					_items[2].DisplayText = String.Format("Exp: {0}/{1}", RPGHelper.Self.PlayerCurrExp, RPGHelper.Self.PlayerNextExp);
					_items[4].DisplayText = String.Format("Attack: {0}", RPGHelper.Self.PlayerAttack);
					_items[5].DisplayText = String.Format("Defense: {0}", RPGHelper.Self.PlayerDefense);
					break;

				default:
					foreach (Text item in _items)
						item.DisplayText = "";
					break;
			}
		}
		#endregion

		#region Private
		enum MenuOptions
		{
			Status,
			Max
		}

		#region Data
		Sprite _background;
		List<Text> _options;
		List<Text> _items;
		MenuOptions _prevOption;
		MenuOptions _currOption;
		#endregion
		#endregion
	}
}
