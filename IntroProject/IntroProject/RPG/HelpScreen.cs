using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

// XNA
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

// GameEngine
using GameEngine;
using GameEngine.UI;

namespace IntroProject.RPG
{
	public partial class HelpScreen : IScreen
	{
		#region Public
		#region IScreen interface
		public bool IsFocused { set; get; }
		#endregion

		public HelpScreen()
		{
			InitializeLayout();

			_control1.Colour = Color.Black;
			_control2.Colour = Color.Black;
			_control3.Colour = Color.Black;
			_control4.Colour = Color.Black;
			_control5.Colour = Color.Black;
			_control6.Colour = Color.Black;
			_control7.Colour = Color.Black;
			_control8.Colour = Color.Black;
		}
		#endregion

		#region Private
		void IScreen.OnEnter()
		{
			IsFocused = true;
		}

		void IScreen.Update()
		{
			if (!IsFocused)
				return;

			if (InputManager.Self.IsKeyDown(Keys.Enter))
			{
				ScreenManager.Self.GoToScreen(ScreenType.RPG_Title);
			}
		}

		void IScreen.OnLeave()
		{
			IsFocused = false;
			_background.RemoveFromEngine();
			_exit.RemoveFromEngine();
			_control1.RemoveFromEngine();
			_control2.RemoveFromEngine();
			_control3.RemoveFromEngine();
			_control4.RemoveFromEngine();
			_control5.RemoveFromEngine();
			_control6.RemoveFromEngine();
			_control7.RemoveFromEngine();
			_control8.RemoveFromEngine();
		}

		#endregion
	}
}
