namespace GameEngine
{
	public class Singleton<T> where T : new()
	{
		static T _self;
		public static T Self
		{
			get
			{
				if (_self == null)
					_self = new T();

				return _self;
			}
		}
	}
}
