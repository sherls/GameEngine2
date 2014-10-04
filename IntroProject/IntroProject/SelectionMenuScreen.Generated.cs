using GameEngine;
using GameEngine.Data;
using GameEngine.Physics;
namespace IntroProject
{
	public partial class SelectionMenuScreen
	{
		Text _titleText;
		public void InitializeLayout()
		{
			var xmlData = DataLoadingManager.Self.ImportXml("SelectionMenuScreen.xml");
			var createdItems = DataLoadingManager.Self.InstantiateObjectsFromXmlObject(xmlData);
			_titleText = createdItems["_titleText"] as Text;
			if(_titleText is IEngineAddable)
				(_titleText as IEngineAddable).AddToEngine();
		}
	}
}
