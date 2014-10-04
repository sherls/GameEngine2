// XNA
using Microsoft.Xna.Framework;

// GameEngine
using GameEngine;
using Microsoft.Xna.Framework.Input;

namespace IntroProject
{
	public partial class StartMenuScreen : IScreen
	{
		#region Public

		#region IScreen interface
		public bool IsFocused { set; get; }
		#endregion

		public StartMenuScreen()
		{
			InitializeLayout();
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

			if (InputManager.Self.IsKeyDown(Keys.Enter) || InputManager.Self.IsKeyDown(Keys.Back))
				ScreenManager.Self.GoToScreen(ScreenType.Select);

			if (InputManager.Self.IsKeyDown(Keys.F1))
				ScreenManager.Self.GoToScreen(ScreenType.Test);
		}

		void IScreen.OnLeave()
		{
			IsFocused = false;
			_titleText.RemoveFromEngine();
			_enterText.RemoveFromEngine();
			_exitText.RemoveFromEngine();
			_backText.RemoveFromEngine();
			_controlText1.RemoveFromEngine();
			_controlText2.RemoveFromEngine();
			_controlText3.RemoveFromEngine();
			_controlText4.RemoveFromEngine();
			_controlText5.RemoveFromEngine();
		}
		#endregion
	}
}