using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

// XNA
using Microsoft.Xna.Framework;

// GameEngine
using GameEngine;

namespace IntroProject.RPG
{
	public enum Movement
	{
		Idle,
		Up,
		Down,
		Left,
		Right,
		Max
	}

	public struct EnemyData
	{
		public uint HP;
		public uint EXP;
		public uint Attack;
		public uint Defense;
		public string Texture;
	}

	public class GameState : Singleton<GameState>
	{
		#region Public
		#region Property
		public bool IsNewGame { set; get; }
		public bool IsOnStory { set; get; }
		public uint PlayerHP { set; get; }
		public uint PlayerMaxHP { set; get; }
		public uint PlayerCurrExp { set; get; }
		public uint PlayerNextExp { set; get; }
		public uint PlayerLevel { set; get; }
		public Vector2 PlayerSpawnLocation { set; get; }
		#endregion

		#region Data
		public List<EnemyData> enemyData;
		#endregion

		#region Methods
		public GameState()
		{
			enemyData = new List<EnemyData>();

			EnemyData enemy = new EnemyData();
			enemy.Texture = "Tiles/Scorpio1";
			enemy.HP = 5;
			enemy.EXP = 5;
			enemy.Attack = 1;
			enemy.Defense = 0;
			enemyData.Add(enemy);

			enemy = new EnemyData();
			enemy.Texture = "Tiles/Scorpio2";
			enemy.HP = 10;
			enemy.EXP = 10;
			enemy.Attack = 1;
			enemy.Defense = 0;
			enemyData.Add(enemy);

			enemy.Texture = "Tiles/Scorpio3";
			enemy.HP = 20;
			enemy.EXP = 30;
			enemy.Attack = 2;
			enemy.Defense = 0;
			enemyData.Add(enemy);

			enemy.Texture = "Tiles/Scorpio4";
			enemy.HP = 40;
			enemy.EXP = 50;
			enemy.Attack = 1;
			enemy.Defense = 0;
			enemyData.Add(enemy);

			enemy.Texture = "Tiles/RedSatan";
			enemy.HP = 60;
			enemy.EXP = 80;
			enemy.Attack = 1;
			enemy.Defense = 0;
			enemyData.Add(enemy);

			enemy.Texture = "Tiles/BrownSatan";
			enemy.HP = 75;
			enemy.EXP = 95;
			enemy.Attack = 1;
			enemy.Defense = 0;
			enemyData.Add(enemy);

			enemy.Texture = "Tiles/GreenSatan";
			enemy.HP = 85;
			enemy.EXP = 110;
			enemy.Attack = 1;
			enemy.Defense = 0;
			enemyData.Add(enemy);

			enemy.Texture = "Tiles/Brain";
			enemy.HP = 100;
			enemy.EXP = 140;
			enemy.Attack = 5;
			enemy.Defense = 2;
			enemyData.Add(enemy);
		}

		public void InitializeNewGame()
		{
			IsNewGame = true;

			PlayerHP = 20;
			PlayerCurrExp = 0;
			PlayerNextExp = 30;
			PlayerLevel = 1;
			PlayerSpawnLocation = new Vector2(1024, 180);
		}

		public void Update()
		{

		}
		#endregion
		#endregion

		#region Private
		#region Method
		#endregion
		#endregion
	}
}
