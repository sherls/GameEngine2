using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

// GameEngine
using GameEngine;
using GameEngine.Data;
using GameEngine.TileMap;
using GameEngine.Physics;

namespace IntroProject.RPG
{
	public class FairyForestScreen : IScreen
	{
		#region Public
		#region IScreen interface
		public bool IsFocused { set; get; }
		#endregion

		public FairyForestScreen()
		{
			_tileMapInstance = new TileMap();
			_tileMapInstance.Load("FairyForest.tmx");

			var xmlData = DataLoadingManager.Self.ImportXml("FairyForestScreen.xml");
			var createdItems = DataLoadingManager.Self.InstantiateObjectsFromXmlObject(xmlData);

			_player = createdItems["_player"] as SpritePlayer;
			_player.SetupPlayer(_tileMapInstance.PlayerLayerZ + 0.0001f);

			_enterHomeTB = createdItems["_enterHomeTB"] as TriggerBox;
			_enterHomeTB.NextScreen = ScreenType.RPG_FairyHome;

			_toVillageTB = createdItems["_toVillageTB"] as TriggerBox;
			_toVillageTB.NextScreen = ScreenType.RPG_Village;
		}
		#endregion

		#region Private
		#region Data
		TileMap _tileMapInstance;
		SpritePlayer _player;
		TriggerBox _enterHomeTB;
		TriggerBox _toVillageTB;
		#endregion

		#region Methods
		void IScreen.OnEnter()
		{
			IsFocused = true;
			_tileMapInstance.AddToEngine();

			_player.AddingToEngine();
			_enterHomeTB.AddToEngine();
			_toVillageTB.AddToEngine();
		}

		void IScreen.Update()
		{
			if (!IsFocused || ScreenManager.Self.IsTransitioning)
				return;

			foreach (GameEngine.Physics.Rectangle collisionRect in _tileMapInstance.collisionRectangles)
				CollisionManager.Self.Separate<SpritePlayer, GameEngine.Physics.Rectangle>(_player, collisionRect);

			if (_enterHomeTB.DoesCollideAgainst(_player))
				_enterHomeTB.DoAction();

			if (_toVillageTB.DoesCollideAgainst(_player))
				_toVillageTB.DoAction();
		}

		void IScreen.OnLeave()
		{
			IsFocused = false;
			_tileMapInstance.RemoveFromEngine();

			_player.RemovingFromEngine();
			_enterHomeTB.RemoveFromEngine();
			_toVillageTB.RemoveFromEngine();
		}
		#endregion
		#endregion
	}
}
