using System.Collections.Generic;

// XNA
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

// GameEngine
using GameEngine;
using GameEngine.Data;
using GameEngine.Physics;

namespace IntroProject
{
	public class MazeGameScreen : IScreen
	{
		#region Public

		#region IScreen interface
		public bool IsFocused { set; get; }
		#endregion

		public MazeGameScreen()
		{
			XmlData xmlData = DataLoadingManager.Self.ImportXml("Maze.xml");

			var createItems = DataLoadingManager.Self.InstantiateObjectsFromXmlObject(xmlData);
			foreach (var item in createItems.Values)
			{
				if (item is IEngineAddable)
				{
					IEngineAddable asEngineAddable = item as IEngineAddable;
					asEngineAddable.AddToEngine();
				}

				if (item is PrimitivePlayer)
				{
					_player = item as PrimitivePlayer;
				}
				else if (item is HiddenMessage)
				{
					HiddenMessage asHiddenMessage = item as HiddenMessage;
					asHiddenMessage.SetDisplayText = asHiddenMessage.Name;
					_hiddenMessages.Add(asHiddenMessage);
				}
				if (item is Wall)
				{
					Wall asWall = item as Wall;
					_mazeWall.Add(asWall);
				}
			}
		}
		#endregion

		#region Private
		#region Data
		PrimitivePlayer _player;
		List<Wall> _mazeWall = new List<Wall>();
		List<HiddenMessage> _hiddenMessages = new List<HiddenMessage>();
		#endregion

		#region Methods
		void IScreen.OnEnter()
		{
			IsFocused = true;
			_player.IsVisible = true;

			foreach (Wall mazeWall in _mazeWall)
				mazeWall.IsVisible = true;
		}

		void IScreen.Update()
		{
			if (!IsFocused)
				return;

			if (InputManager.Self.IsKeyDown(Keys.Back))
				ScreenManager.Self.GoToScreen(ScreenType.Select);

			foreach (Wall mazeWall in _mazeWall)
				CollisionManager.Self.Separate<PrimitivePlayer, Wall>(_player, mazeWall);
		}

		void IScreen.OnLeave()
		{
			IsFocused = false;

			_player.RemoveFromEngine();
			foreach (HiddenMessage hiddenMessage in _hiddenMessages)
				hiddenMessage.RemoveFromEngine();
			foreach (Wall wall in _mazeWall)
				wall.RemoveFromEngine();
		}
		#endregion
		#endregion
	}
}
