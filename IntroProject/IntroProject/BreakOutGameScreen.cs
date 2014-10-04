using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

// XNA
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

// GameEngine
using GameEngine;
using GameEngine.Data;
using GameEngine.Physics;

namespace IntroProject
{
	public class BreakOutGameScreen : IScreen
	{
		#region Public
		#region IScreen interface
		public bool IsFocused { set; get; }
		#endregion

		public BreakOutGameScreen()
		{
			XmlData xmlData = DataLoadingManager.Self.ImportXml("BreakOut.xml");

			var createdItems = DataLoadingManager.Self.InstantiateObjectsFromXmlObject(xmlData);
			foreach (var item in createdItems.Values)
			{
				Entity asEntity = item as Entity;
				if (item is IEngineAddable)
				{
					IEngineAddable asEngineAddable = item as IEngineAddable;
					asEngineAddable.AddToEngine();
				}

				if (item is Paddle)
					_player = item as Paddle;
				else if (item is Circle)
					_ball = item as Circle;
				else if (item is Wall)
				{
					if (asEntity.Name == "BottomWall")
						_bottomWall = item as Wall;
					else if( asEntity.Name.Contains("Brick") )
						_bricks.Add(item as Wall);
					else
						_walls.Add(item as Wall);
				}
			}

			_pressSpace = new Text();
			_pressSpace.Size = Camera.Self.ScreenCentre;
			_pressSpace.DisplayText = "Press [Space] to start the game";
			_pressSpace.HorizontalAlignment = HAlignment.Centre;
		}
		#endregion

		#region Private
		Paddle _player;
		Circle _ball;
		bool _bGameStarted = false;
		List<Wall> _walls = new List<Wall>();
		List<Wall> _bricks = new List<Wall>();
		Wall _bottomWall;
		Text _pressSpace;

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

			if (!_bGameStarted)
			{
				_ball.Velocity = Vector2.Zero;
				if (InputManager.Self.IsKeyPressed(Keys.A) || InputManager.Self.IsKeyPressed(Keys.Left))
					_ball.Velocity.X = -1.0f;
				else if (InputManager.Self.IsKeyPressed(Keys.D) || InputManager.Self.IsKeyPressed(Keys.Right))
					_ball.Velocity.X = 1.0f;
				else if (InputManager.Self.IsKeyDown(Keys.Space))
				{
					_ball.IsVisible = true;
					_ball.Velocity = new Vector2(1, -1);

					_bGameStarted = true;
					_pressSpace.IsVisible = false;
				}
				_ball.Velocity *= 200.0f;
			}
			else
			{
				if (_bricks.Count == 0)
				{
					_bGameStarted = false;
					_pressSpace.DisplayText = "You won!";
					_pressSpace.IsVisible = true;
					_ball.IsVisible = false;
					_player.IsVisible = false;
				}

				foreach (Wall wall in _walls)
				{
					CollisionManager.Self.Separate<Paddle, Wall>(_player, wall);
					CollisionManager.Self.Bounce<Circle, Wall>(_ball, wall);
				}

				CollisionManager.Self.Bounce<Circle, Paddle>(_ball, _player);

				List<Wall> bricksToRemove = new List<Wall>();
				foreach (Wall brick in _bricks)
				{
					if (CollisionManager.Self.Bounce<Circle, Wall>(_ball, brick))
					{
						brick.RemoveFromEngine();
						bricksToRemove.Add(brick);
					}
				}

				foreach (Wall brick in bricksToRemove)
					_bricks.Remove(brick);

				if (_ball.DoesCollideAgainst(_bottomWall))
					ScreenManager.Self.GoToScreen(ScreenType.Select);
			}
		}

		void IScreen.OnLeave()
		{
			IsFocused = false;

			_player.RemoveFromEngine();

			foreach (Wall wall in _walls)
				wall.RemoveFromEngine();

			foreach (Wall brick in _bricks)
				brick.RemoveFromEngine();

			_bottomWall.RemoveFromEngine();

			EngineManager.Self.RemoveDrawable(_pressSpace);
		}
		#endregion
	}
}
