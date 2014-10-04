// XNA
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameEngine
{
	public class Camera : Singleton<Camera>
	{
		#region Public
		#region Property
		public float DestinationWidth
		{
			get
			{
				return GraphicsDevice.Viewport.Width;
			}
		}

		public float DestinationHeight
		{
			get
			{
				return GraphicsDevice.Viewport.Height;
			}
		}

		public Vector2 ScreenCentre
		{
			get
			{
				return new Vector2(DestinationWidth / 2, DestinationHeight / 2);
			}
		}

		public Vector2 ScreenSize
		{
			get
			{
				return new Vector2( DestinationWidth, DestinationHeight );
			}
		}
		#endregion

		public Camera()
		{
		}

		#endregion

		#region Private
		internal GraphicsDevice GraphicsDevice;
		#endregion
	}
}
