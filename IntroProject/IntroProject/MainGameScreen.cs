using System;

// XNA
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

using DataCodeGeneration;

// GameEngine
using GameEngine;
using GameEngine.UI;
using GameEngine.Data;
using GameEngine.TileMap;
using GameEngine.Physics;
using GameEngine.Scripting;
using IntroProject.RPG;

namespace IntroProject
{
	public class MainGameScreen : IScreen
	{
		#region Public
		#region IScreen interface
		public bool IsFocused { set; get; }
		#endregion

		//Circle _circle;
		public MainGameScreen()
		{
			var xmlData = DataLoadingManager.Self.ImportXml("PathToRuinScreen.xml");
			var createdItems = DataLoadingManager.Self.InstantiateObjectsFromXmlObject(xmlData);

			Enemy _enemy1 = createdItems["_enemy1"] as Enemy;
			_enemy1.SetupEnemy(5.0f);
			_enemy1.AddingToEngine();

			/*Button _test = new Button();
			_test.Offset = Camera.Self.ScreenCentre;
			_test.DisplayText = "This is a long testing";
			_test.AddToEngine();

			RPG.SpritePlayer newPlayer = new RPG.SpritePlayer();
			newPlayer.Offset = Camera.Self.ScreenCentre;
			newPlayer.SetZ = 100;
			newPlayer.AddToEngine();*/
			/*_circle = new Circle();
			_circle.Radius = 16;
			_circle.IsVisible = true;
			_circle.Offset = new Vector2(16, 16);

			EngineManager.Self.AddDrawable(_circle);
			EngineManager.Self.AddEntity(_circle);

			// I want it to
			// * Move to the right
			_circle.Velocity.X = 100;
			// * Stop
			ScriptingManager.Self.Do(() => _circle.Velocity.X = 0).After(5);
			// * Grow a bit
			//ScriptingManager.Self.Do(() => _circle.Radius = 32).After(3);
			ScriptingManager.Self.Do(() => _circle.Radius++).After(6).Until(() => _circle.Radius >= 32);
			// * Move down
			ScriptingManager.Self.Do(() => _circle.Velocity.Y = 100).After(7);
			// * Stop
			ScriptingManager.Self.Do(() => _circle.Velocity.Y = 0).After(8);
			// * Shrink
			ScriptingManager.Self.Do(() => _circle.Radius = 16).After(9);*/
		}
		#endregion


		#region Private
		#region Data
		PrimitivePlayer _player;
		#endregion

		#region Property
		#endregion

		#region Methods
		void IScreen.OnEnter()
		{
			IsFocused = true;
		}

		void IScreen.Update()
		{
			if (!IsFocused)
				return;

			if (InputManager.Self.IsKeyDown(Keys.Back))
				ScreenManager.Self.GoToScreen(ScreenType.Select);
		}

		void IScreen.OnLeave()
		{
			IsFocused = false;
		}
		#endregion
		#endregion
	}
}
