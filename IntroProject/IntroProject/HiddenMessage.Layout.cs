// XNA
using Microsoft.Xna.Framework;

// GameEngine
using GameEngine;
using GameEngine.UI;
using GameEngine.Physics;

namespace IntroProject
{
	public partial class HiddenMessage
	{
		#region Public
		#region Property
		public string SetDisplayText
		{
			set { _message.DisplayText = value; }
		}

		public bool IsBackgroundFilled
		{
			set
			{
				_message.IsBackgroundFilled = value;
			}
		}
		#endregion

		public void InitializeHiddenMessage()
		{
			IsVisible = true;

			_triggerBox = new GameEngine.Physics.Circle();
			_triggerBox.IsVisible = false;
			_triggerBox.Colour = Color.Red;
			_triggerBox.Radius = 16.0f;
			_triggerBox.Parent = this;

			_message = new TextBox();
			_message.DisplayText = "";
			_message.Offset = Vector2.Zero;
			_message.Size = Vector2.Zero;
			_message.SizeUnits = SizeUnits.RelativeToParent;
			_message.Parent = this;
			_message.IsVisible = false;
			_message.BackgroundColour = Color.Black;
			_message.TextColour = Color.White;
			_message.TextHorizontalAlignment = HAlignment.Centre;
		}
		#endregion

		#region Private
		Circle _triggerBox;
		TextBox _message;
		#endregion
	}
}
