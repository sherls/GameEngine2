// XNA
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

// GameEngine
using GameEngine;
using GameEngine.Data;
using GameEngine.TileMap;
using GameEngine.Physics;
using GameEngine.Scripting;

namespace IntroProject.RPG
{
	public class FairyHomeScreen : IScreen
	{
		#region Public
		#region IScreen interface
		public bool IsFocused { set; get; }
		#endregion

		public FairyHomeScreen()
		{
			_tileMapInstance = new TileMap();
			_tileMapInstance.Load("FairyHome.tmx");

			var xmlData = DataLoadingManager.Self.ImportXml("FairyHomeScreen.xml");
			var createdItems = DataLoadingManager.Self.InstantiateObjectsFromXmlObject(xmlData);

			_player = createdItems["_player"] as SpritePlayer;
			_player.SetupPlayer(_tileMapInstance.PlayerLayerZ + 0.0001f);

			_soldier = createdItems["_soldier"] as NPC;
			_soldier.SetZ = _tileMapInstance.PlayerLayerZ + 0.0002f;

			_healthTB = createdItems["_healthTB"] as TriggerBox;

			_leaveTB = createdItems["_leaveTB"] as TriggerBox;
			_leaveTB.NextScreen = ScreenType.RPG_FairyForest;

			_bRestrictMovement = false;
			_bSoldierEnter = false;
			_bTalkedToGeneral = false;
		}
		#endregion

		#region Private
		#region Data
		TileMap _tileMapInstance;
		SpritePlayer _player;
		TriggerBox _leaveTB;
		TriggerBox _healthTB;

		NPC _soldier;

		uint _dialogSeq;
		bool _bRestrictMovement;
		bool _bSoldierEnter;
		bool _bTalkedToGeneral;
		#endregion

		#region Methods
		void IScreen.OnEnter()
		{
			IsFocused = true;
			_tileMapInstance.AddToEngine();
			_player.AddingToEngine();
			_soldier.AddingToEngine();

			_leaveTB.AddToEngine();
			_healthTB.AddToEngine();

			if (RPGHelper.Self.IsNewGame)
			{
				_bRestrictMovement = true;
				RPGHelper.Self.IsOnStory = true;

				ScriptingManager.Self.
					Do(() => ShowAnimation()).
					After(Constants.SCRIPT_DELAY);
			}
		}

		void IScreen.Update()
		{
			if (!IsFocused || ScreenManager.Self.IsTransitioning)
				return;

			#region Leaving trigger box
			if (_leaveTB.DoesCollideAgainst(_player))
			{
				if (_bSoldierEnter)
				{
					RPGHelper.Self.Dialog.IsVisible = false;

					_bSoldierEnter = false;
					_player.Move = Movement.Idle;
					_player.Offset.Y = 528;
					RPGHelper.Self.IsOnStory = true;

					_soldier.Move = Movement.Up;
					ScriptingManager.Self.
						Do(() => { _soldier.Move = Movement.Idle; RPGHelper.Self.IsOnStory = false; }).
						After(() => _soldier.Offset.Y <= 320);
				}
				else if (_bRestrictMovement)
				{
					_player.Offset.Y = 528;
					RPGHelper.Self.Dialog.IsVisible = true;

					if (_bTalkedToGeneral)
						RPGHelper.Self.Dialog.DisplayText = "I better follow the soldier order.";
					else
						RPGHelper.Self.Dialog.DisplayText = "I should talk with the soldier first. It's rude to ignore him.";

					RPGHelper.Self.StartDialogTimer();
				}
				else
				{
					_leaveTB.DoAction();
				}
			}
			#endregion

			#region Health restoration
			if (_healthTB.DoesCollideAgainst(_player))
			{
				if (_bRestrictMovement && _bTalkedToGeneral && RPGHelper.Self.IsNewGame)
				{
					RPGHelper.Self.PlayerSpawnLocation = RPGHelper.Self.PlayerBedLocation;
					RPGHelper.Self.IsOnStory = false;
					ScreenManager.Self.ShowTransition = true;
					ScreenManager.Self.GoToScreen(ScreenType.RPG_FairyHome);
				}
				else if (!RPGHelper.Self.IsNewGame)
				{
					_player.RestoreHP();
				}
			}
			#endregion

			if (!RPGHelper.Self.IsOnStory)
			{
				CollisionManager.Self.Separate<SpritePlayer, NPC>(_player, _soldier);

				foreach (GameEngine.Physics.Rectangle collisionRect in _tileMapInstance.collisionRectangles)
					CollisionManager.Self.Separate<SpritePlayer, GameEngine.Physics.Rectangle>(_player, collisionRect);
			}

			#region Interaction
			if (InputManager.Self.IsKeyDown(Keys.Space))
			{
				if (!_bTalkedToGeneral && !RPGHelper.Self.IsOnStory)
				{
					Vector2 direction;
					bool check = _player.DoesCloseEnough(_soldier, 0.5f, out direction);

					if (check)
					{
						if (direction.X >= 1.0f)
							_soldier.Dir = Direction.Left;
						else if (direction.X <= -1.0f)
							_soldier.Dir = Direction.Right;
						else if (direction.Y >= 1.0f)
							_soldier.Dir = Direction.Down;
						else if (direction.Y <= -1.0f)
							_soldier.Dir = Direction.Up;

						RPGHelper.Self.IsOnStory = true;
						_bTalkedToGeneral = true;

						RPGHelper.Self.Dialog.IsVisible = true;
						RPGHelper.Self.Dialog.DisplayText = "Soldier: Do you see any suspicious person?";
					}
				}
				else if (_bTalkedToGeneral && RPGHelper.Self.IsOnStory)
				{
					switch( _dialogSeq)
					{
						case 0:
							RPGHelper.Self.Dialog.DisplayText = "Fairy: No. I was sleeping till you knock on my door.";
							break;

						case 1:
							RPGHelper.Self.Dialog.DisplayText = "Soldier: OK. Don't leave your house now. Let us know if you see someone suspicious.";
							break;

						case 2:
							RPGHelper.Self.Dialog.DisplayText = "Fairy: Understood, Sir.";
							//RPGHelper.Self.StartDialogTimer();
							break;

						case 3:
							RPGHelper.Self.Dialog.IsVisible = false;
							_soldier.Move = Movement.Down;
							ScriptingManager.Self.
								Do(() => { _soldier.IsVisible = false; RPGHelper.Self.IsOnStory = false; }).
								After(() => _soldier.Offset.Y >= 576);
							break;
					}
					++_dialogSeq;
				}
			#endregion
			}
		}

		void IScreen.OnLeave()
		{
			RPGHelper.Self.IsNewGame = false;
			IsFocused = false;
			_tileMapInstance.RemoveFromEngine();
			_player.RemovingFromEngine();

			_leaveTB.RemoveFromEngine();
			_healthTB.RemoveFromEngine();
		}

		void ShowAnimation()
		{
			RPGHelper.Self.IsOnStory = true;
			_bSoldierEnter = true;

			RPGHelper.Self.Dialog.IsVisible = true;
			RPGHelper.Self.Dialog.DisplayText = "Knock! Knock!";

			ScriptingManager.Self.
				Do(() => _player.Move = Movement.Down).
				After(Constants.SCRIPT_DELAY);

			ScriptingManager.Self.
				Do(() => _player.Move = Movement.Left).
				After(() => _player.Offset.Y >= 222);

			ScriptingManager.Self.
				Do(() => {
					_player.Move = Movement.Idle;
					RPGHelper.Self.IsOnStory = false;
					_soldier.IsVisible = true;
				}).
				After(() => _player.Offset.X <= 928);
		}
		#endregion
		#endregion
	}
}
