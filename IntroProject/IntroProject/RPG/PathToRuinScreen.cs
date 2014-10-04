using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

// XNA
using Microsoft.Xna.Framework;

// GameEngine
using GameEngine;
using GameEngine.Data;
using GameEngine.TileMap;
using GameEngine.Physics;
using GameEngine.Scripting;

namespace IntroProject.RPG
{
	public class PathToRuinScreen : IScreen
	{
		#region Public
		#region IScreen interface
		public bool IsFocused { set; get; }
		#endregion

		public PathToRuinScreen()
		{
			_enemies = new List<Enemy>();

			_tileMapInstance = new TileMap();
			_tileMapInstance.Load("PathToRuin.tmx");

			var xmlData = DataLoadingManager.Self.ImportXml("PathToRuinScreen.xml");
			var createdItems = DataLoadingManager.Self.InstantiateObjectsFromXmlObject(xmlData);

			_player = createdItems["_player"] as SpritePlayer;
			_player.SetupPlayer(_tileMapInstance.PlayerLayerZ + 0.0001f);
			_player.Offset = new Vector2(1216, 144);

			_toRuinTB = createdItems["_toRuinTB"] as TriggerBox;
			_toRuinTB.NextScreen = ScreenType.RPG_RuinL1;

			_toVillageTB = createdItems["_toVillageTB"] as TriggerBox;
			_toVillageTB.NextScreen = ScreenType.RPG_Village;

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

			enemy = createdItems["_enemy15"] as Enemy;
			enemy.SetupEnemy(_tileMapInstance.PlayerLayerZ + 0.0001f);
			_enemies.Add(enemy);
		}
		#endregion

		#region Private
		#region Data
		TileMap _tileMapInstance;
		SpritePlayer _player;
		TriggerBox _toRuinTB;
		TriggerBox _toVillageTB;
		List<Enemy> _enemies;
		#endregion

		#region Methods
		void IScreen.OnEnter()
		{
			IsFocused = true;
			_tileMapInstance.AddToEngine();

			_player.AddingToEngine();
			_toRuinTB.AddToEngine();
			_toVillageTB.AddToEngine();

			foreach (Enemy enemy in _enemies)
				enemy.AddingToEngine();

			Enemy0MoveRightAndContinue();
			Enemy2MoveRightAndContinue();
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

			if (_toRuinTB.DoesCollideAgainst(_player))
				_toRuinTB.DoAction();

			if (_toVillageTB.DoesCollideAgainst(_player))
				_toVillageTB.DoAction();
		}

		void IScreen.OnLeave()
		{
			IsFocused = false;
			_tileMapInstance.RemoveFromEngine();
			
			_player.RemovingFromEngine();
			_toRuinTB.RemoveFromEngine();
			_toVillageTB.RemoveFromEngine();

			foreach (Enemy enemy in _enemies)
				enemy.RemovingFromEngine();
		}

		#region Enemy0 movement
		void Enemy0MoveLeftAndContinue()
		{
			_enemies[0].MoveLeftTo(864);
			ScriptingManager.Self.
				Do(() => Enemy0MoveRightAndContinue()).
				After(() => _enemies[0].AbsoluteX <= 864);
		}

		void Enemy0MoveRightAndContinue()
		{
			_enemies[0].MoveRightTo(1056);
			ScriptingManager.Self.
				Do(() => Enemy0MoveLeftAndContinue()).
				After(() => _enemies[0].AbsoluteX >= 1056);
		}
		#endregion

		#region Enemy2 movement
		void Enemy2MoveLeftAndContinue()
		{
			_enemies[2].MoveLeftTo(736);
			ScriptingManager.Self.
				Do(() => Enemy2MoveRightAndContinue()).
				After(() => _enemies[2].AbsoluteX <= 736);
		}

		void Enemy2MoveRightAndContinue()
		{
			_enemies[2].MoveRightTo(1088);
			ScriptingManager.Self.
				Do(() => Enemy2MoveLeftAndContinue()).
				After(() => _enemies[2].AbsoluteX >= 1088);
		}
		#endregion
		#endregion
		#endregion
	}
}
