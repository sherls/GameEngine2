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
	public class RuinL1Screen : IScreen
	{
		#region Public
		#region IScreen interface
		public bool IsFocused { set; get; }
		#endregion

		public RuinL1Screen()
		{
			_enemies = new List<Enemy>();

			_tileMapInstance = new TileMap();
			_tileMapInstance.Load("RuinL1.tmx");

			var xmlData = DataLoadingManager.Self.ImportXml("RuinL1Screen.xml");
			var createdItems = DataLoadingManager.Self.InstantiateObjectsFromXmlObject(xmlData);

			_player = createdItems["_player"] as SpritePlayer;
			_player.SetupPlayer(_tileMapInstance.PlayerLayerZ + 0.0001f);

			_toPathTB = createdItems["_toPathTB"] as TriggerBox;
			_toPathTB.NextScreen = ScreenType.RPG_PathToRuin;

			_toBasement1TB = createdItems["_toBasement1TB"] as TriggerBox;
			_toBasement1TB.NextScreen = ScreenType.RPG_Basement;

			_toBasement2TB = createdItems["_toBasement2TB"] as TriggerBox;
			_toBasement2TB.NextScreen = ScreenType.RPG_Basement;

			_toL2TB = createdItems["_toL2TB"] as TriggerBox;
			_toL2TB.NextScreen = ScreenType.RPG_RuinL2;

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

			enemy = createdItems["_enemy10"] as Enemy;
			enemy.SetupEnemy(_tileMapInstance.PlayerLayerZ + 0.0001f);
			_enemies.Add(enemy);

			enemy = createdItems["_enemy11"] as Enemy;
			enemy.SetupEnemy(_tileMapInstance.PlayerLayerZ + 0.0001f);
			_enemies.Add(enemy);

			enemy = createdItems["_enemy12"] as Enemy;
			enemy.SetupEnemy(_tileMapInstance.PlayerLayerZ + 0.0001f);
			_enemies.Add(enemy);

			enemy = createdItems["_enemy13"] as Enemy;
			enemy.SetupEnemy(_tileMapInstance.PlayerLayerZ + 0.0001f);
			_enemies.Add(enemy);

			enemy = createdItems["_enemy14"] as Enemy;
			enemy.SetupEnemy(_tileMapInstance.PlayerLayerZ + 0.0001f);
			_enemies.Add(enemy);
		}
		#endregion

		#region Private
		#region Data
		TileMap _tileMapInstance;
		SpritePlayer _player;
		TriggerBox _toPathTB;
		TriggerBox _toBasement1TB;
		TriggerBox _toBasement2TB;
		TriggerBox _toL2TB;
		List<Enemy> _enemies;
		#endregion

		#region Methods
		void IScreen.OnEnter()
		{
			IsFocused = true;
			_tileMapInstance.AddToEngine();

			_player.AddingToEngine();
			_toPathTB.AddToEngine();
			_toBasement1TB.AddToEngine();
			_toBasement2TB.AddToEngine();
			_toL2TB.AddToEngine();

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

			if (_toPathTB.DoesCollideAgainst(_player))
				_toPathTB.DoAction();

			if (_toBasement1TB.DoesCollideAgainst(_player))
				_toBasement1TB.DoAction();

			if (_toBasement2TB.DoesCollideAgainst(_player))
				_toBasement2TB.DoAction();

			if (_toL2TB.DoesCollideAgainst(_player))
				_toL2TB.DoAction();
		}

		void IScreen.OnLeave()
		{
			IsFocused = false;
			_tileMapInstance.RemoveFromEngine();
			
			_player.RemovingFromEngine();
			_toPathTB.RemoveFromEngine();
			_toBasement1TB.RemoveFromEngine();
			_toBasement2TB.RemoveFromEngine();
			_toL2TB.RemoveFromEngine();

			foreach (Enemy enemy in _enemies)
				enemy.RemovingFromEngine();
		}
		#endregion
		#endregion
	}
}
