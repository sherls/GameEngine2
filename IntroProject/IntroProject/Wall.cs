// XNA
using Microsoft.Xna.Framework;

// GameEngine
using GameEngine.Physics;
using EngineRectangle = GameEngine.Physics.Rectangle;

namespace IntroProject
{
	class Wall : EngineRectangle
	{
		#region Public
		#region Methods
		public Wall()
		{
			IsVisible = true;
			SizeUnits = GameEngine.SizeUnits.RelativeToParent;
			Size = Vector2.Zero;
			IsFilled = true;
			CollisionManager.Self.Add(this);
		}

		public override void Update()
		{
			base.Update();
		}
		#endregion
		#endregion
	}
}
