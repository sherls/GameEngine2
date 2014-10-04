// XNA
using Microsoft.Xna.Framework;

namespace GameEngine
{
	public class TimeManager : Singleton<TimeManager>
	{
		#region Public
		public double DeltaTime
		{
			get
			{
				return _deltaTime;
			}
		}

		public float DeltaTimeFloat
		{
			get
			{
				return (float)_deltaTime;
			}
		}

		public float TotalTime
		{
			get
			{
				return (float)_totalTime;
			}
		}

		public TimeManager()
		{
		}

		internal void Update(GameTime i_gameTime)
		{
			_deltaTime = i_gameTime.ElapsedGameTime.TotalSeconds;
			_totalTime = i_gameTime.TotalGameTime.TotalSeconds;
		}

		#endregion Public

		#region Private
		double _deltaTime;
		double _totalTime;
		#endregion
	} 
}
