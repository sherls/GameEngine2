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
	public class GeneralHomeScreen : IScreen
	{
		#region Public
		#region IScreen interface
		public bool IsFocused { set; get; }
		#endregion

		public GeneralHomeScreen()
		{
			_dialogSeq = 0;

			_tileMapInstance = new TileMap();
			_tileMapInstance.Load("GeneralHome.tmx");

			var xmlData = DataLoadingManager.Self.ImportXml("GeneralHomeScreen.xml");
			var createdItems = DataLoadingManager.Self.InstantiateObjectsFromXmlObject(xmlData);

			_player = createdItems["_player"] as SpritePlayer;
			_player.SetupPlayer(_tileMapInstance.PlayerLayerZ + 0.0001f);

			_toVillageTB = createdItems["_toVillageTB"] as TriggerBox;
			_toVillageTB.NextScreen = ScreenType.RPG_Village;

			_general = createdItems["_general"] as NPC;
			_general.SetZ = _tileMapInstance.PlayerLayerZ + 0.0001f;
		}
		#endregion

		#region Private
		#region Data
		TileMap _tileMapInstance;
		SpritePlayer _player;
		TriggerBox _toVillageTB;
		NPC _general;
		uint _dialogSeq;
		#endregion

		#region Methods
		void IScreen.OnEnter()
		{
			IsFocused = true;
			_tileMapInstance.AddToEngine();

			_player.AddingToEngine();
			_toVillageTB.AddToEngine();
			_general.AddingToEngine();
		}

		void IScreen.Update()
		{
			if (!IsFocused || ScreenManager.Self.IsTransitioning || RPGHelper.Self.Query.IsVisible)
				return;

			foreach (GameEngine.Physics.Rectangle collisionRect in _tileMapInstance.collisionRectangles)
				CollisionManager.Self.Separate<SpritePlayer, GameEngine.Physics.Rectangle>(_player, collisionRect);

			CollisionManager.Self.Separate<SpritePlayer, NPC>(_player, _general);

			if (_toVillageTB.DoesCollideAgainst(_player))
				_toVillageTB.DoAction();

			#region Interaction
			if (InputManager.Self.IsKeyDown(Keys.Enter))
			{
				if (RPGHelper.Self.Mission == MissionState.Unavailable)
				{
					if (RPGHelper.Self.Query.IsYes)
						RPGHelper.Self.Dialog.DisplayText = "I will tell the soldiers to let you pass.";
					else
						RPGHelper.Self.Dialog.DisplayText = "OK. Don't bother me again till I find the thief.";
				}
				else if (RPGHelper.Self.Mission == MissionState.Offered)
				{
					if (RPGHelper.Self.Query.IsYes)
						RPGHelper.Self.Dialog.DisplayText = "I will tell the soldiers to let you pass.";
					else
						RPGHelper.Self.Dialog.DisplayText = "OK. Don't bother me again till I find the thief.";
				}
			}
			if (InputManager.Self.IsKeyDown(Keys.Space))
			{
				Vector2 direction;
				if (_player.DoesCloseEnough(_general, 0.5f, out direction))
				{
					#region Mission unavailable
					if (RPGHelper.Self.Mission == MissionState.Unavailable)
					{
						switch (_dialogSeq)
						{
							case 0:
								_general.DialogText = "I don't have time to deal your nature business, Fairy. We are busy investigating the thief.";
								_general.ShowDialog(direction);
								break;

							case 1:
								RPGHelper.Self.Dialog.DisplayText = "If you are really that impatient, why don't you help us find the thief?";
								break;

							case 2:
								RPGHelper.Self.Query.IsVisible = true;
								break;

							case 3:
								if (RPGHelper.Self.Query.IsYes)
									RPGHelper.Self.Mission = MissionState.Accepted;
								else
									RPGHelper.Self.Mission = MissionState.Offered;
								RPGHelper.Self.Dialog.IsVisible = false;
								_dialogSeq = 0;
								break;
						}
						++_dialogSeq;
					}
					#endregion
					#region Mission offered
					else if (RPGHelper.Self.Mission == MissionState.Offered)
					{
						switch (_dialogSeq)
						{
							case 0:
							case 1:
								_general.DialogText = "Do you change your mind?";
								_general.ShowDialog(direction);
								RPGHelper.Self.Query.IsVisible = true;
								_dialogSeq = 1;
								break;

							case 2:
								if (RPGHelper.Self.Query.IsYes)
									RPGHelper.Self.Mission = MissionState.Accepted;
								RPGHelper.Self.Dialog.IsVisible = false;
								_dialogSeq = 0;
								break;
						}
							++_dialogSeq;
					}
					#endregion
					#region Mission accepted
					else if (RPGHelper.Self.Mission == MissionState.Accepted)
					{
						switch (_dialogSeq)
						{
							case 0:
							case 1:
								_general.DialogText = "I will not stop until I find the thief!";
								_general.ShowDialog(direction);
								break;

							default:
								RPGHelper.Self.Dialog.IsVisible = false;
								_dialogSeq = 0;
								break;
						}
						++_dialogSeq;
					}
					#region Mission completed
					else if (RPGHelper.Self.Mission == MissionState.Completed)
					{
						switch (_dialogSeq)
						{
							case 0:
							case 1:
								_general.DialogText = "Thank you. I promise I will keep the villager off of your forest.";
								_general.ShowDialog(direction);
								break;

							default:
								RPGHelper.Self.Dialog.IsVisible = false;
								_dialogSeq = 0;
								ScreenManager.Self.ShowTransition = true;
								ScreenManager.Self.GoToScreen(ScreenType.RPG_Title);
								break;
						}
						++_dialogSeq;
					}
					#endregion
					#endregion
				}
			}
			#endregion
		}

		void IScreen.OnLeave()
		{
			IsFocused = false;
			_tileMapInstance.RemoveFromEngine();
			
			_player.RemovingFromEngine();
			_toVillageTB.RemoveFromEngine();
			_general.RemovingFromEngine();
		}
		#endregion
		#endregion
	}
}
