using System;
using System.Collections.Generic;

// XNA
using Microsoft.Xna.Framework;

// GameEngine
using GameEngine;
using GameEngine.UI;
using GameEngine.Scripting;

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

	public enum MissionState
	{
		Unavailable,
		Offered,
		Accepted,
		Completed
	}

	public struct EnemyStats
	{
		public int HP;
		public uint EXP;
		public uint Attack;
		public uint Defense;
		public string Texture;
	}

	public class RPGHelper : Singleton<RPGHelper>
	{
		#region Public
		#region Property
		public bool IsNewGame { set; get; }
		public bool IsOnStory { set; get; }
		public MissionState Mission { set; get; }

		#region Player stats
		public int PlayerCurrHP { set; get; }
		public int PlayerMaxHP { set; get; }
		public uint PlayerCurrExp { set; get; }
		public uint PlayerNextExp { set; get; }
		public uint PlayerLevel { set; get; }
		public uint PlayerAttack { set; get; }
		public uint PlayerDefense { set; get; }
		#endregion

		public Vector2 PlayerSpawnLocation { set; get; }
		public Direction PlayerIdleDirection { set; get; }
		public Vector2 PlayerBedLocation { set; get; }
		#endregion

		#region Data
		public ProgressBar EnemyHealthBar;
		public DialogBox Dialog;
		public QueryDialog Query;
		public List<EnemyStats> EnemiesData;
		#endregion

		#region Methods
		public RPGHelper()
		{
			Dialog = new DialogBox();
			Dialog.AddToEngine();

			Query = new QueryDialog();
			Query.AddToEngine();

			EnemyHealthBar = new ProgressBar();
			EnemyHealthBar.IsVisible = false;

			#region Enemy data
			EnemiesData = new List<EnemyStats>();

			EnemyStats enemy = new EnemyStats();
			enemy.Texture = "Tiles/Scorpio1";
			enemy.HP = 5;
			enemy.EXP = 5;
			enemy.Attack = 3;
			enemy.Defense = 0;
			EnemiesData.Add(enemy);

			enemy = new EnemyStats();
			enemy.Texture = "Tiles/Scorpio2";
			enemy.HP = 12;
			enemy.EXP = 15;
			enemy.Attack = 3;
			enemy.Defense = 0;
			EnemiesData.Add(enemy);

			enemy = new EnemyStats();
			enemy.Texture = "Tiles/Scorpio3";
			enemy.HP = 20;
			enemy.EXP = 30;
			enemy.Attack = 2;
			enemy.Defense = 0;
			EnemiesData.Add(enemy);

			enemy = new EnemyStats();
			enemy.Texture = "Tiles/Scorpio4";
			enemy.HP = 40;
			enemy.EXP = 50;
			enemy.Attack = 1;
			enemy.Defense = 0;
			EnemiesData.Add(enemy);

			enemy = new EnemyStats();
			enemy.Texture = "Tiles/RedSatan";
			enemy.HP = 60;
			enemy.EXP = 80;
			enemy.Attack = 1;
			enemy.Defense = 0;
			EnemiesData.Add(enemy);

			enemy = new EnemyStats();
			enemy.Texture = "Tiles/BrownSatan";
			enemy.HP = 75;
			enemy.EXP = 95;
			enemy.Attack = 1;
			enemy.Defense = 0;
			EnemiesData.Add(enemy);

			enemy = new EnemyStats();
			enemy.Texture = "Tiles/GreenSatan";
			enemy.HP = 85;
			enemy.EXP = 110;
			enemy.Attack = 1;
			enemy.Defense = 0;
			EnemiesData.Add(enemy);

			enemy = new EnemyStats();
			enemy.Texture = "Tiles/Brain";
			enemy.HP = 100;
			enemy.EXP = 140;
			enemy.Attack = 5;
			enemy.Defense = 2;
			EnemiesData.Add(enemy);
			#endregion
		}

		public void InitializeNewGame()
		{
			IsNewGame = true;
			Mission = MissionState.Unavailable;

			PlayerMaxHP = 20;
			PlayerCurrHP = PlayerMaxHP;
			PlayerCurrExp = 0;
			PlayerNextExp = 30;
			PlayerLevel = 1;
			PlayerAttack = 2;
			PlayerDefense = 0;
			PlayerIdleDirection = Direction.Down;
			PlayerBedLocation = new Vector2(960, 214);
			PlayerSpawnLocation = PlayerBedLocation;
		}

		public void StartDialogTimer()
		{
			ScriptingManager.Self.
				Do(() => Dialog.IsVisible = false).
				After(Constants.DIALOG_TIMER);
		}

		public void PlayerExpCheckAndLevelUp()
		{
			if (PlayerCurrExp >= PlayerNextExp)
			{
				++PlayerLevel;

				PlayerNextExp = PlayerNextExp + PlayerLevel * 20 - PlayerCurrExp / 5;

				PlayerAttack += PlayerLevel;
				PlayerDefense += PlayerLevel;

				PlayerMaxHP += (int)(PlayerAttack + PlayerDefense);
				PlayerCurrHP = PlayerMaxHP;

				Dialog.IsVisible = true;
				Dialog.DisplayText = String.Format("You level up!\r\nAttack increase by {0}\nDefense increase by {1}", RPGHelper.Self.PlayerLevel, RPGHelper.Self.PlayerLevel);
				StartDialogTimer();
			}
		}

		public void RestorePlayerHP()
		{
			PlayerCurrHP = PlayerMaxHP;
		}
		#endregion
		#endregion

		#region Private
		#region Method
		#endregion
		#endregion
	}
}
