namespace GameEngine
{
	public interface IVisible
	{
		bool IsVisible { set; get; }

		IVisible Parent { get; }
	}

	static public class IVisibleExtensions
	{
		static public bool GetIsAbsoluteVisible(this IVisible i_instanceToCheck)
		{
			if (i_instanceToCheck.Parent == null)
			{
				return i_instanceToCheck.IsVisible;
			}
			else
			{
				return i_instanceToCheck.IsVisible && GetIsAbsoluteVisible(i_instanceToCheck.Parent);
			}
		}
	}
}
