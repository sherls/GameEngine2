// XNA
using Microsoft.Xna.Framework;

// GameEngine
using GameEngine;

namespace IntroProject.RPG
{
	public class TriggerBox : GameEngine.Physics.Rectangle
	{
		#region Public
		#region Property
		public bool IsRegenerateHealth { set; get; }
		public ScreenType NextScreen { set; get; }
		public Vector2 PlayerSpawnLocation
		{
			set
			{
				PlayerSpawnLocationX = value.X;
				PlayerSpawnLocationY = value.Y;
			}
			get
			{
				return new Vector2(PlayerSpawnLocationX, PlayerSpawnLocationY);
			}
		}
		public float PlayerSpawnLocationX { set; get; }
		public float PlayerSpawnLocationY { set; get; }
		public uint PlayerEnterDirection { set; get; }
		#endregion

		public TriggerBox()
		{
			Size = Constants.TILE_SIZE;
			IsRegenerateHealth = false;

			IsVisible = false;
			IsFilled = true;
			Colour = Color.Transparent;
		}

		public void DoAction()
		{
			RPGHelper.Self.PlayerSpawnLocation = PlayerSpawnLocation;
			RPGHelper.Self.PlayerIdleDirection = (Direction)PlayerEnterDirection;

			ScreenManager.Self.ShowTransition = true;
			ScreenManager.Self.GoToScreen(NextScreen);
		}
		#endregion
	}
}
