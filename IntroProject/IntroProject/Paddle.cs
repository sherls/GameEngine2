using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

// XNA
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

// GameEngine
using GameEngine;
using GameEngine.Physics;

namespace IntroProject
{
	public class Paddle : GameEngine.Physics.Rectangle
	{
		#region Public
		#region Property
		public UInt16 Score { set; get; }
		#endregion


		public Paddle()
		{
			Size = new Vector2( 80, 16 );
			Offset = new Vector2(Camera.Self.ScreenCentre.X, 416);
			IsFilled = true;
			IsVisible = true;
			Colour = Color.Aqua;

			CollisionManager.Self.Add(this);
		}

		public override void Update()
		{
			if (IsVisible)
			{
				Velocity = Vector2.Zero;

				if (InputManager.Self.IsKeyPressed(Keys.A) || InputManager.Self.IsKeyPressed(Keys.Left))
					Velocity.X = -1.0f;
				else if (InputManager.Self.IsKeyPressed(Keys.D) || InputManager.Self.IsKeyPressed(Keys.Right))
					Velocity.X = 1.0f;

				Velocity *= 200.0f;
			}

			base.Update();
		}
		#endregion
	}
}
