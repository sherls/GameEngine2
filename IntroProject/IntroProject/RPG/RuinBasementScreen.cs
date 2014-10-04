using System.Collections.Generic;

// XNA
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

// GameEngine
using GameEngine;
using GameEngine.Data;
using GameEngine.TileMap;
using GameEngine.Physics;

namespace IntroProject.RPG
{
	public class RuinBasementScreen : IScreen
	{
		#region Public
		#region IScreen interface
		public bool IsFocused { set; get; }
		#endregion

		public RuinBasementScreen()
		{
			_enemies = new List<Enemy>();
			_tileMapInstance = new TileMap();
			_tileMapInstance.Load("RuinBasement.tmx");

			var xmlData = DataLoadingManager.Self.ImportXml("RuinBasementScreen.xml");
			var createdItems = DataLoadingManager.Self.InstantiateObjectsFromXmlObject(xmlData);

			_player = createdItems["_player"] as SpritePlayer;
			_player.SetupPlayer(_tileMapInstance.PlayerLayerZ + 0.0001f);

			_toL11TB = createdItems["_toL11TB"] as TriggerBox;
			_toL11TB.NextScreen = ScreenType.RPG_RuinL1;

			_toL12TB = createdItems["_toL12TB"] as TriggerBox;
			_toL12TB.NextScreen = ScreenType.RPG_RuinL1;

			_toL13TB = createdItems["_toL13TB"] as TriggerBox;
			_toL13TB.NextScreen = ScreenType.RPG_RuinL1;

			_treasureTB = createdItems["_treasureTB"] as TriggerBox;
			_treasureTB.NextScreen = ScreenType.RPG_GeneralHome;

			Enemy enemy = createdItems["_enemy1"] as Enemy;
			enemy.SetupEnemy(_tileMapInstance.PlayerLayerZ + 0.0001f);
			_enemies.Add(enemy);

			enemy = createdItems["_enemy2"] as Enemy;
			enemy.SetupEnemy(_tileMapInstance.PlayerLayerZ + 0.0001f);
			_enemies.Add(enemy);

			enemy = createdItems["_enemy3"] as Enemy;
			enemy.SetupEnemy(_tileMapInstance.PlayerLayerZ + 0.0001f);
			_enemies.Add(enemy);

			enemy = createdItems["_enemy4"] as Enemy;
			enemy.SetupEnemy(_tileMapInstance.PlayerLayerZ + 0.0001f);
			_enemies.Add(enemy);

			enemy = createdItems["_enemy5"] as Enemy;
			enemy.SetupEnemy(_tileMapInstance.PlayerLayerZ + 0.0001f);
			_enemies.Add(enemy);

			enemy = createdItems["_enemy6"] as Enemy;
			enemy.SetupEnemy(_tileMapInstance.PlayerLayerZ + 0.0001f);
			_enemies.Add(enemy);

			enemy = createdItems["_enemy7"] as Enemy;
			enemy.SetupEnemy(_tileMapInstance.PlayerLayerZ + 0.0001f);
			_enemies.Add(enemy);

			enemy = createdItems["_enemy8"] as Enemy;
			enemy.SetupEnemy(_tileMapInstance.PlayerLayerZ + 0.0001f);
			_enemies.Add(enemy);

			enemy = createdItems["_enemy9"] as Enemy;
			enemy.SetupEnemy(_tileMapInstance.PlayerLayerZ + 0.0001f);
			_enemies.Add(enemy);
		}
		#endregion

		#region Private
		#region Data
		TileMap _tileMapInstance;
		SpritePlayer _player;
		TriggerBox _toL11TB;
		TriggerBox _toL12TB;
		TriggerBox _toL13TB;
		TriggerBox _treasureTB;
		List<Enemy> _enemies;
		#endregion

		#region Methods
		void IScreen.OnEnter()
		{
			IsFocused = true;
			_tileMapInstance.AddToEngine();

			_player.AddingToEngine();
			_toL11TB.AddToEngine();
			_toL12TB.AddToEngine();
			_toL13TB.AddToEngine();
			_treasureTB.AddToEngine();

			foreach (Enemy enemy in _enemies)
				enemy.AddingToEngine();
		}

		void IScreen.Update()
		{
			if (!IsFocused || ScreenManager.Self.IsTransitioning)
				return;

			foreach (GameEngine.Physics.Rectangle collisionRect in _tileMapInstance.collisionRectangles)
				CollisionManager.Self.Separate<SpritePlayer, GameEngine.Physics.Rectangle>(_player, collisionRect);

			#region Collision with enemies
			foreach (Enemy enemy in _enemies)
			{
				if (enemy.IsVisible && (enemy.HP > 0) && _player.DoesCollideAgainst(enemy))
				{
					CollisionManager.Self.Separate<SpritePlayer, Enemy>(_player, enemy);
					enemy.Combat();
				}
			}
			#endregion

			if (_toL11TB.DoesCollideAgainst(_player))
				_toL11TB.DoAction();

			if (_toL12TB.DoesCollideAgainst(_player))
				_toL12TB.DoAction();

			if (_toL13TB.DoesCollideAgainst(_player))
				_toL13TB.DoAction();

			#region Treasure
			if (InputManager.Self.IsKeyDown(Keys.Space))
			{
				Vector2 direction;
				if (_player.DoesCloseEnough(_treasureTB, 0.5f, out direction)
					&& (_enemies[4].HP <= 0)
					&& (_enemies[5].HP <= 0)
					&& (_enemies[6].HP <= 0)
					&& (_enemies[7].HP <= 0)
					&& (_enemies[8].HP <= 0)
					)
				{
					RPGHelper.Self.Mission = MissionState.Completed;
					_treasureTB.DoAction();
				}
			}
			#endregion
		}

		void IScreen.OnLeave()
		{
			IsFocused = false;
			_tileMapInstance.RemoveFromEngine();
			
			_player.RemovingFromEngine();
			_toL11TB.RemoveFromEngine();
			_toL12TB.RemoveFromEngine();
			_toL13TB.RemoveFromEngine();
			_treasureTB.RemoveFromEngine();

			foreach (Enemy enemy in _enemies)
				enemy.RemovingFromEngine();
		}
		#endregion
		#endregion
	}
}
