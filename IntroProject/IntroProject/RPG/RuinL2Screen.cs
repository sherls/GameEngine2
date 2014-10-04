using System.Collections.Generic;

// GameEngine
using GameEngine;
using GameEngine.Data;
using GameEngine.TileMap;
using GameEngine.Physics;

namespace IntroProject.RPG
{
	public class RuinL2Screen : IScreen
	{
		#region Public
		#region IScreen interface
		public bool IsFocused { set; get; }
		#endregion

		public RuinL2Screen()
		{
			_enemies = new List<Enemy>();

			_tileMapInstance = new TileMap();
			_tileMapInstance.Load("RuinL2.tmx");

			var xmlData = DataLoadingManager.Self.ImportXml("RuinL2Screen.xml");
			var createdItems = DataLoadingManager.Self.InstantiateObjectsFromXmlObject(xmlData);

			_player = createdItems["_player"] as SpritePlayer;
			_player.SetupPlayer(_tileMapInstance.PlayerLayerZ + 0.0001f);

			_toL1TB = createdItems["_toL1TB"] as TriggerBox;
			_toL1TB.NextScreen = ScreenType.RPG_RuinL1;

			_secretTB = createdItems["_secretTB"] as TriggerBox;
			_secretTB.NextScreen = ScreenType.RPG_Basement;

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
		}
		#endregion

		#region Private
		#region Data
		TileMap _tileMapInstance;
		SpritePlayer _player;
		TriggerBox _toL1TB;
		TriggerBox _secretTB;
		List<Enemy> _enemies;
		#endregion

		#region Methods
		void IScreen.OnEnter()
		{
			IsFocused = true;
			_tileMapInstance.AddToEngine();

			_player.AddingToEngine();
			_toL1TB.AddToEngine();
			_secretTB.AddToEngine();

			foreach (Enemy enemy in _enemies)
				enemy.AddingToEngine();
		}

		void IScreen.Update()
		{
			uint totalEnemyDefeated = 0;

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
				else if (enemy.HP < 0)
					++totalEnemyDefeated;
			}
			#endregion

			if (_toL1TB.DoesCollideAgainst(_player))
				_toL1TB.DoAction();

			if (_secretTB.DoesCollideAgainst(_player) && (totalEnemyDefeated == _enemies.Count))
				_secretTB.DoAction();
		}

		void IScreen.OnLeave()
		{
			IsFocused = false;
			_tileMapInstance.RemoveFromEngine();
			
			_player.RemovingFromEngine();
			_toL1TB.RemoveFromEngine();
			_secretTB.RemoveFromEngine();

			foreach (Enemy enemy in _enemies)
				enemy.RemovingFromEngine();
		}
		#endregion
		#endregion
	}
}
