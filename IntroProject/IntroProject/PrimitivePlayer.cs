// XNA
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

// GameEngine
using GameEngine;
using GameEngine.Physics;

namespace IntroProject
{
	public partial class PrimitivePlayer : Circle
	{
		#region Public
		#region Methods
		public PrimitivePlayer()
		{
			Offset = new Vector2(48, 304);
			IsVisible = true;
			Colour = Color.Goldenrod;

			CollisionManager.Self.Add(this);
		}

		public override void Update()
		{
			if (IsVisible)
			{
				Velocity = Vector2.Zero;

				if (InputManager.Self.IsKeyPressed(Keys.W) || InputManager.Self.IsKeyPressed(Keys.Up))
					Velocity.Y = -1.0f;
				else if (InputManager.Self.IsKeyPressed(Keys.S) || InputManager.Self.IsKeyPressed(Keys.Down))
					Velocity.Y = 1.0f;

				if (InputManager.Self.IsKeyPressed(Keys.A) || InputManager.Self.IsKeyPressed(Keys.Left))
					Velocity.X = -1.0f;
				else if (InputManager.Self.IsKeyPressed(Keys.D) || InputManager.Self.IsKeyPressed(Keys.Right))
					Velocity.X = 1.0f;

				Velocity *= 200.0f;
			}

			base.Update();
		}
		#endregion
		#endregion
	}
}
