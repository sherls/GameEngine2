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
	public class VillageScreen : IScreen
	{
		#region Public
		#region IScreen interface
		public bool IsFocused { set; get; }
		#endregion

		public VillageScreen()
		{
			_tileMapInstance = new TileMap();
			_tileMapInstance.Load("Village.tmx");

			var xmlData = DataLoadingManager.Self.ImportXml("VillageScreen.xml");
			var createdItems = DataLoadingManager.Self.InstantiateObjectsFromXmlObject(xmlData);

			_player = createdItems["_player"] as SpritePlayer;
			_player.SetupPlayer(_tileMapInstance.PlayerLayerZ + 0.0001f);

			_toGeneralHomeTB = createdItems["_toGeneralHomeTB"] as TriggerBox;
			_toGeneralHomeTB.NextScreen = ScreenType.RPG_GeneralHome;

			_toRuinPathTB = createdItems["_toRuinPathTB"] as TriggerBox;
			_toRuinPathTB.NextScreen = ScreenType.RPG_PathToRuin;

			_toFairyForestTB = createdItems["_toFairyForestTB"] as TriggerBox;
			_toFairyForestTB.NextScreen = ScreenType.RPG_FairyForest;

			_soldier1 = createdItems["_soldier1"] as NPC;
			_soldier1.Dir = Direction.Right;
			_soldier1.SetZ = _tileMapInstance.PlayerLayerZ + 0.0001f;
			_soldier2 = createdItems["_soldier2"] as NPC;
			_soldier2.Dir = Direction.Right;
			_soldier2.SetZ = _tileMapInstance.PlayerLayerZ + 0.0001f;
			if (RPGHelper.Self.Mission == MissionState.Accepted)
			{
				_soldier1.IsVisible = false;
				_soldier2.IsVisible = false;
			}

			_person1 = createdItems["_person1"] as NPC;
			_person1.SetZ = _tileMapInstance.PlayerLayerZ + 0.0001f;

			_person2 = createdItems["_person2"] as NPC;
			_person2.SetZ = _tileMapInstance.PlayerLayerZ + 0.0001f;

			_person3 = createdItems["_person3"] as NPC;
			_person3.SetZ = _tileMapInstance.PlayerLayerZ + 0.0001f;
		}
		#endregion

		#region Private
		TileMap _tileMapInstance;
		RPG.SpritePlayer _player;
		TriggerBox _toGeneralHomeTB;
		TriggerBox _toRuinPathTB;
		TriggerBox _toFairyForestTB;
		NPC _soldier1;
		NPC _soldier2;
		NPC _person1;
		NPC _person2;
		NPC _person3;

		#region Methods
		void IScreen.OnEnter()
		{
			IsFocused = true;
			_tileMapInstance.AddToEngine();

			_player.AddingToEngine();
			_toGeneralHomeTB.AddToEngine();
			_toRuinPathTB.AddToEngine();
			_toFairyForestTB.AddToEngine();
			_soldier1.AddingToEngine();
			_soldier2.AddingToEngine();
			_person1.AddingToEngine();
			_person2.AddingToEngine();
			_person3.AddingToEngine();
		}

		void IScreen.Update()
		{
			if (!IsFocused || ScreenManager.Self.IsTransitioning)
				return;

			if (RPGHelper.Self.Dialog.IsVisible)
			{
				if (InputManager.Self.IsKeyDown(Keys.Space))
				{
					RPGHelper.Self.IsOnStory = false;
					RPGHelper.Self.Dialog.IsVisible = false;
				}
				return;
			}

			foreach (GameEngine.Physics.Rectangle collisionRect in _tileMapInstance.collisionRectangles)
				CollisionManager.Self.Separate<SpritePlayer, GameEngine.Physics.Rectangle>(_player, collisionRect);

			if (_toGeneralHomeTB.DoesCollideAgainst(_player))
				_toGeneralHomeTB.DoAction();

			if (_toRuinPathTB.DoesCollideAgainst(_player))
				_toRuinPathTB.DoAction();

			if (_toFairyForestTB.DoesCollideAgainst(_player))
				_toFairyForestTB.DoAction();

			#region Collision with NPCs
			if (RPGHelper.Self.Mission != MissionState.Accepted)
			{
				CollisionManager.Self.Separate<SpritePlayer, NPC>(_player, _soldier1);
				CollisionManager.Self.Separate<SpritePlayer, NPC>(_player, _soldier2);
			}
			CollisionManager.Self.Separate<SpritePlayer, NPC>(_player, _person1);
			CollisionManager.Self.Separate<SpritePlayer, NPC>(_player, _person2);
			CollisionManager.Self.Separate<SpritePlayer, NPC>(_player, _person3);
			#endregion

			#region Interaction
			if (InputManager.Self.IsKeyDown(Keys.Space))
			{
				Vector2 direction;
				if (_player.DoesCloseEnough(_person1, 0.5f, out direction))
				{
					_person1.ShowDialog(direction);
					RPGHelper.Self.IsOnStory = true;
				}
				else if (_player.DoesCloseEnough(_person2, 0.5f, out direction))
				{
					_person2.ShowDialog(direction);
					RPGHelper.Self.IsOnStory = true;
				}
				else if (_player.DoesCloseEnough(_person3, 0.5f, out direction))
				{
					_person3.ShowDialog(direction);
					RPGHelper.Self.IsOnStory = true;
				}
				if (RPGHelper.Self.Mission != MissionState.Accepted)
				{
					if (_player.DoesCloseEnough(_soldier1, 0.5f, out direction))
					{
						_soldier1.ShowDialog(direction);
						RPGHelper.Self.IsOnStory = true;
					}
					else if (_player.DoesCloseEnough(_soldier2, 0.5f, out direction))
					{
						_soldier2.ShowDialog(direction);
						RPGHelper.Self.IsOnStory = true;
					}
				}
			}
			#endregion
		}

		void IScreen.OnLeave()
		{
			IsFocused = false;
			_tileMapInstance.RemoveFromEngine();

			_player.RemovingFromEngine();
			_toGeneralHomeTB.RemoveFromEngine();
			_toRuinPathTB.RemoveFromEngine();
			_toFairyForestTB.RemoveFromEngine();
			_soldier1.RemovingFromEngine();
			_soldier2.RemovingFromEngine();
			_person1.RemovingFromEngine();
			_person2.RemovingFromEngine();
			_person3.RemovingFromEngine();
		}

		#endregion
		#endregion
	}
}
