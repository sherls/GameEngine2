using System.Collections.Generic;

// XNA
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

// GameEngine
using GameEngine;
using GameEngine.UI;

namespace IntroProject.RPG
{
	class TitleScreen : IScreen
	{
		#region Public
		#region IScreen interface
		public bool IsFocused { set; get; }
		#endregion

		public TitleScreen()
		{
			_background = new Sprite();
			_background.Offset = Vector2.Zero;
			_background.Size = Camera.Self.ScreenSize;
			_background.Texture = EngineManager.Self.Load<Texture2D>("Textures/Background");
			_background.AddToEngine();

			_buttons = new List<Button>();
			Button tempButton = new Button();
			float buttonX = (Camera.Self.DestinationWidth / 2) - (tempButton.Size.X / 2);
			tempButton.Offset = new Vector2(buttonX, 480);
			tempButton.DisplayText = "START";
			_buttons.Add(tempButton);

			tempButton = new Button();
			tempButton.Offset = new Vector2(buttonX, 544);
			tempButton.DisplayText = "Help";
			_buttons.Add(tempButton);

			tempButton = new Button();
			tempButton.Offset = new Vector2(buttonX, 608);
			tempButton.DisplayText = "Exit";
			_buttons.Add(tempButton);

			_currOption = Options.Start;
			_buttons[(int)_currOption].IsFocused = true;
		}

		#endregion

		#region Private
		enum Options
		{
			Start,
			Help,
			Exit,
			Max
		}

		#region Data
		Sprite _background;
		Options _currOption = Options.Max;
		List<Button> _buttons;
		#endregion

		void IScreen.OnEnter()
		{
			IsFocused = true;
			foreach (var button in _buttons)
			{
				button.AddToEngine();
			}
		}

		void IScreen.Update()
		{
			if (!IsFocused)
				return;

			if (InputManager.Self.IsKeyDown(Keys.Up))
			{
				_buttons[(int)_currOption].IsFocused = false;
				_currOption = (Options)(((int)_currOption + (int)Options.Max - 1) % (int)Options.Max);
				_buttons[(int)_currOption].IsFocused = true;
			}
			else if (InputManager.Self.IsKeyDown(Keys.Down))
			{
				_buttons[(int)_currOption].IsFocused = false;
				_currOption = (Options)((int)(_currOption + 1) % (int)(Options.Max));
				_buttons[(int)_currOption].IsFocused = true;
			}
			else if (InputManager.Self.IsKeyDown(Keys.Enter))
			{
				switch (_currOption)
				{
					case Options.Start:
						ScreenManager.Self.ShowTransition = true;
						RPGHelper.Self.InitializeNewGame();
						ScreenManager.Self.GoToScreen(ScreenType.RPG_FairyHome);
						break;

					case Options.Help:
						ScreenManager.Self.GoToScreen(ScreenType.RPG_Help);
						break;

					case Options.Exit:
						ScreenManager.Self.GoToScreen(ScreenType.Select);
						break;
				}
			}
		}

		void IScreen.OnLeave()
		{
			IsFocused = false;

			_background.RemoveFromEngine();

			foreach (var button in _buttons)
			{
				button.RemoveFromEngine();
			}
		}
		#endregion
	}
}
