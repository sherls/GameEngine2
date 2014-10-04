using System.Collections.Generic;

// XNA
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

// GameEngine
using GameEngine;
using GameEngine.UI;

namespace IntroProject
{
	public partial class SelectionMenuScreen : IScreen
	{
		#region Public
		#region IScreen interface
		public bool IsFocused { set; get; }
		#endregion

		public SelectionMenuScreen()
		{
			InitializeLayout();

			_buttons = new List<Button>();
			Button tempButton = new Button();
			float buttonX = (Camera.Self.DestinationWidth / 2) - (tempButton.Size.X / 2);
			tempButton.Offset = new Vector2(buttonX, 288);
			tempButton.DisplayText = "Maze";
			_buttons.Add(tempButton);

			tempButton = new Button();
			tempButton.Offset = new Vector2(buttonX, 352);
			tempButton.DisplayText = "BreakOut";
			_buttons.Add(tempButton);

			tempButton = new Button();
			tempButton.Offset = new Vector2(buttonX, 416);
			tempButton.DisplayText = "RPG";
			_buttons.Add(tempButton);

			_currOption = Options.RPG;
			_buttons[(int)_currOption].IsFocused = true;
		}
		#endregion

		#region Private
		enum Options
		{
			Maze,
			BreakOut,
			RPG,
			Max
		}

		Options _currOption = Options.Max;
		List<Button> _buttons;

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
					case Options.BreakOut:
						ScreenManager.Self.GoToScreen(ScreenType.BreakOut);
						break;

					case Options.RPG:
						ScreenManager.Self.GoToScreen(ScreenType.RPG_Title);
						break;
				}
			}
		}

		void IScreen.OnLeave()
		{
			IsFocused = false;
			_titleText.RemoveFromEngine();
			foreach( var button in _buttons)
			{
				button.RemoveFromEngine();
			}
		}
		#endregion
	}
}
