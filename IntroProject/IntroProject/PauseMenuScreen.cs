// XNA
using Microsoft.Xna.Framework.Graphics;

// GameEngine
using GameEngine;
using GameEngine.UI;

namespace IntroProject
{
	public class PauseMenuScreen
	{
		#region Public
		public PauseMenuScreen()
		{
			_scoreText = new Text();
			_scoreText.DisplayText = "000000";
			_scoreText.Offset.X = 20;
			_scoreText.Offset.Y = 20;
			EngineManager.Self.AddDrawable(_scoreText);

			_timeText = new Text();
			_timeText.DisplayText = "00:00:00";
			_timeText.Offset.X = Camera.Self.DestinationWidth - 20;
			_timeText.Offset.Y = 20;
			_timeText.HorizontalAlignment = HAlignment.Right;
			EngineManager.Self.AddDrawable(_timeText);

			_pausedText = new Text();
			_pausedText.DisplayText = "Paused";
			_pausedText.Offset = Camera.Self.ScreenCentre;
			_pausedText.HorizontalAlignment = HAlignment.Centre;
			EngineManager.Self.AddDrawable(_pausedText);

			_pausedSprite = new Sprite();
			_pausedSprite.Texture = EngineManager.Self.Load<Texture2D>("Textures/SpriteFrame");
			_pausedSprite.Offset = Camera.Self.ScreenCentre;
			_pausedSprite.HorizontalAlignment = HAlignment.Centre;
			_pausedSprite.VerticalAlignment = VAlignment.Centre;
			EngineManager.Self.AddDrawable(_pausedSprite);

			//_test = new TextBox();
			//_test.Offset = Camera.Self.ScreenCentre;
			//_test.DisplayText = "Hello";// "I am trying this only to fix the text and the box";
			//_test.TextHorizontalAlignment = HAlignment.Centre;
		}

		public void Update()
		{
		}
		#endregion

		#region Private
		Text _scoreText;
		Text _timeText;
		Text _pausedText;
		Sprite _pausedSprite;
		TextBox _test;
		#endregion
	}
}
