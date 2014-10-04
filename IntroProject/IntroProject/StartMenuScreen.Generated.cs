using GameEngine;
using GameEngine.Data;
using GameEngine.Physics;
namespace IntroProject
{
	public partial class StartMenuScreen
	{
		Text _titleText;
		Text _controlText1;
		Text _controlText2;
		Text _controlText3;
		Text _controlText4;
		Text _controlText5;
		Text _enterText;
		Text _exitText;
		Text _backText;
		public void InitializeLayout()
		{
			var xmlData = DataLoadingManager.Self.ImportXml("StartMenuScreen.xml");
			var createdItems = DataLoadingManager.Self.InstantiateObjectsFromXmlObject(xmlData);
			_titleText = createdItems["_titleText"] as Text;
			if(_titleText is IEngineAddable)
				(_titleText as IEngineAddable).AddToEngine();
			_controlText1 = createdItems["_controlText1"] as Text;
			if(_controlText1 is IEngineAddable)
				(_controlText1 as IEngineAddable).AddToEngine();
			_controlText2 = createdItems["_controlText2"] as Text;
			if(_controlText2 is IEngineAddable)
				(_controlText2 as IEngineAddable).AddToEngine();
			_controlText3 = createdItems["_controlText3"] as Text;
			if(_controlText3 is IEngineAddable)
				(_controlText3 as IEngineAddable).AddToEngine();
			_controlText4 = createdItems["_controlText4"] as Text;
			if (_controlText4 is IEngineAddable)
				(_controlText4 as IEngineAddable).AddToEngine();
			_controlText5 = createdItems["_controlText5"] as Text;
			if (_controlText5 is IEngineAddable)
				(_controlText5 as IEngineAddable).AddToEngine();
			_enterText = createdItems["_enterText"] as Text;
			if(_enterText is IEngineAddable)
				(_enterText as IEngineAddable).AddToEngine();
			_exitText = createdItems["_exitText"] as Text;
			if(_exitText is IEngineAddable)
				(_exitText as IEngineAddable).AddToEngine();
			_backText = createdItems["_backText"] as Text;
			if(_backText is IEngineAddable)
				(_backText as IEngineAddable).AddToEngine();
		}
	}
}
