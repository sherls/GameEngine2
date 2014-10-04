using GameEngine;
using GameEngine.UI;
using GameEngine.Data;
using GameEngine.Physics;
namespace IntroProject.RPG
{
	public partial class HelpScreen
	{
		Sprite _background;
		Text _control1;
		Text _control2;
		Text _control3;
		Text _control4;
		Text _control5;
		Text _control6;
		Text _control7;
		Text _control8;
		Button _exit;
		public void InitializeLayout()
		{
			var xmlData = DataLoadingManager.Self.ImportXml("HelpScreen.xml");
			var createdItems = DataLoadingManager.Self.InstantiateObjectsFromXmlObject(xmlData);
			_background = createdItems["_background"] as Sprite;
			if(_background is IEngineAddable)
				(_background as IEngineAddable).AddToEngine();
			_control1 = createdItems["_control1"] as Text;
			if(_control1 is IEngineAddable)
				(_control1 as IEngineAddable).AddToEngine();
			_control2 = createdItems["_control2"] as Text;
			if(_control2 is IEngineAddable)
				(_control2 as IEngineAddable).AddToEngine();
			_control3 = createdItems["_control3"] as Text;
			if(_control3 is IEngineAddable)
				(_control3 as IEngineAddable).AddToEngine();
			_control4 = createdItems["_control4"] as Text;
			if(_control4 is IEngineAddable)
				(_control4 as IEngineAddable).AddToEngine();
			_control5 = createdItems["_control5"] as Text;
			if(_control5 is IEngineAddable)
				(_control5 as IEngineAddable).AddToEngine();
			_control6 = createdItems["_control6"] as Text;
			if (_control6 is IEngineAddable)
				(_control6 as IEngineAddable).AddToEngine();
			_control7 = createdItems["_control7"] as Text;
			if (_control7 is IEngineAddable)
				(_control7 as IEngineAddable).AddToEngine();
			_control8 = createdItems["_control8"] as Text;
			if (_control8 is IEngineAddable)
				(_control8 as IEngineAddable).AddToEngine();
			_exit = createdItems["_exit"] as Button;
			if(_exit is IEngineAddable)
				(_exit as IEngineAddable).AddToEngine();
		}
	}
}
