// XNA
using Microsoft.Xna.Framework.Input;

namespace GameEngine
{
	public class InputManager : Singleton<InputManager>
	{
		#region Public
		public InputManager()
		{
		}

		public void Initialize()
		{
			_oldKeyboardState = new KeyboardState();
			_currentKeyboardState = new KeyboardState();
		}

		internal void Update()
		{
			_oldKeyboardState = _currentKeyboardState;
			_currentKeyboardState = Keyboard.GetState();
		}

		public bool IsKeyUp(Keys i_key)
		{
			return _currentKeyboardState.IsKeyUp(i_key) && _oldKeyboardState.IsKeyDown(i_key);
		}

		public bool IsKeyDown(Keys i_key)
		{
			return _currentKeyboardState.IsKeyDown(i_key) && _oldKeyboardState.IsKeyUp(i_key);
		}

		public bool IsKeyPressed(Keys i_key)
		{
			return (_currentKeyboardState.IsKeyDown(i_key) && _oldKeyboardState.IsKeyDown(i_key));
		}
		#endregion

		#region Private
		#region Data
		KeyboardState _oldKeyboardState;
		KeyboardState _currentKeyboardState;
		#endregion
		#endregion
	}
}
