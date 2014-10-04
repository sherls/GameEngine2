// XNA
using Microsoft.Xna.Framework.Input;

// GameEngine
using GameEngine;
using GameEngine.Data;
using GameEngine.TileMap;
using GameEngine.Physics;

namespace IntroProject.RPG
{
	public class Village : IScreen
	{
		#region Public
		#region IScreen interface
		public bool IsFocused { set; get; }
		#endregion

		public Village()
		{
			_tileMapInstance = new TileMap();
			_tileMapInstance.Load("Outdoor.tmx");

			_player = new RPG.SpritePlayer();
			_player.SetZ = _tileMapInstance.PlayerLayerZ + 0.0001f;
		}
		#endregion

		#region Private
		TileMap _tileMapInstance;
		RPG.SpritePlayer _player;

		#region Methods
		void IScreen.OnEnter()
		{
			IsFocused = true;
			_tileMapInstance.AddToEngine();
			_player.AddingToEngine();
		}

		void IScreen.Update()
		{
			if (!IsFocused || ScreenManager.Self.IsTransitioning)
				return;

			_player.Update();

			if (InputManager.Self.IsKeyDown(Keys.Enter))
			{
				ScreenManager.Self.ShowTransition = true;
				ScreenManager.Self.GoToScreen(ScreenType.RPG_FairyHome);
			}

			if (InputManager.Self.IsKeyDown(Keys.Back))
			{
				ScreenManager.Self.ShowTransition = false;
				ScreenManager.Self.GoToScreen(ScreenType.Select);
			}

			foreach (GameEngine.Physics.Rectangle collisionRect in _tileMapInstance.collisionRectangles)
			{
				CollisionManager.Self.Separate<RPG.SpritePlayer, GameEngine.Physics.Rectangle>(_player, collisionRect);
			}
		}

		void IScreen.OnLeave()
		{
			IsFocused = false;
			_tileMapInstance.RemoveFromEngine();
			_player.RemovingFromEngine();
		}
		#endregion
		#endregion
	}
}
