// XNA
using Microsoft.Xna.Framework;

// GameEngine
using GameEngine;
using GameEngine.Physics;
using GameEngine.Scripting;

namespace IntroProject
{
	public enum ScreenType
	{
		Start,
		Pause,
		Select,
		Maze,
		BreakOut,
		RPG_Title,
		RPG_Help,
		RPG_FairyHome,
		RPG_FairyForest,
		RPG_Village,
		RPG_GeneralHome,
		RPG_PathToRuin,
		RPG_RuinL1,
		RPG_RuinL2,
		RPG_Basement,
		Test
	}

	public class ScreenManager : Singleton<ScreenManager>
	{
		#region Public
		#region Property
		public bool ShowTransition { set; get; }
		public bool IsTransitioning { set; get; }
		#endregion

		public ScreenManager()
		{
			ShowTransition = false;
			IsTransitioning = false;
			TransitionTime = 4;// (Camera.Self.DestinationWidth / Constants.TRANSITION_SPEED.X) / TimeManager.Self.DeltaTime;
			CoolDownCounter = 0;
			IsOnCoolDown = false;

			_currentScreen = null;

			// The transition layer
			_transition = new GameEngine.Physics.Rectangle();
			_transition.IsFilled = true;
			_transition.Offset = Camera.Self.ScreenCentre;
			_transition.Size = Vector2.Zero;
			_transition.Colour = Color.Black;
			_transition.IsVisible = false;
			EngineManager.Self.AddDrawable(_transition);
		}

		public void GoToScreen(ScreenType i_screenType)
		{
			if (ShowTransition)
			{
				_transition.IsVisible = true;
				IsTransitioning = true;
				IsOnCoolDown = true;

				// Start expanding transition animation
				ScriptingManager.Self.
					Do(() => UpdateTransitionLayer(Constants.TRANSITION_SPEED)).
					After(0).
					Until(() => _transition.Size.X >= Camera.Self.DestinationWidth);

				// Perform the transition
				ScriptingManager.Self.
					Do(() => TransitionScreen(i_screenType)).
					After(() => _transition.Size.X >= Camera.Self.DestinationWidth);

				// Cool down time to get rid of the expanding animation
				if (CurrScreenType == i_screenType)
				{
					_bForceTransition = true;
					ScriptingManager.Self.
						Do(() => ++CoolDownCounter).
						After(() => _bForceTransition == false).
						Until(() => CoolDownCounter == Constants.TRANSITION_COOL_DOWN);
				}
				else
				{
					ScriptingManager.Self.
						Do(() => ++CoolDownCounter).
						After(() => CurrScreenType == i_screenType).
						Until(() => CoolDownCounter == Constants.TRANSITION_COOL_DOWN);
				}

				// Finish the cool down
				ScriptingManager.Self.
					Do(() => IsOnCoolDown = false).
					After(() => CoolDownCounter == Constants.TRANSITION_COOL_DOWN);

				// Start shrinking transition animation
				ScriptingManager.Self.
					Do(() => UpdateTransitionLayer(-Constants.TRANSITION_SPEED)).
					After(() => IsOnCoolDown == false).
					Until(() => _transition.Size.X < 0.0f);

				ScriptingManager.Self.
					Do(() => ResetTransition()).
					After( () => _transition.Size.X < 0.0f).
					Until(() => IsTransitioning == false);
			}
			else
			{
				TransitionScreen(i_screenType);
			}
		}

		public void Update()
		{
			_currentScreen.Update();
		}
		#endregion

		#region Private
		IScreen _currentScreen;
		GameEngine.Physics.Rectangle _transition;

		#region Property
		private ScreenType CurrScreenType { set; get; }
		private double TransitionTime { set; get; }
		private bool IsOnCoolDown { set; get; }
		private uint CoolDownCounter { set; get; }

		private bool _bForceTransition = false;
		#endregion

		#region Methods
		private void UpdateTransitionLayer(Vector2 i_adjustment)
		{
			_transition.Size += i_adjustment;
			_transition.Offset -= (i_adjustment / 2);
			_transition.Z = 100;
		}

		private void TransitionScreen(ScreenType i_screenType)
		{
			if (_currentScreen != null)
				_currentScreen.OnLeave();

			_currentScreen = null;

			switch (i_screenType)
			{
				case ScreenType.Start:
					_currentScreen = new StartMenuScreen();						break;

				case ScreenType.Pause:
					//_currentScreen = new PauseMenuScreen();					break;

				case ScreenType.Select:
					_currentScreen = new SelectionMenuScreen();				break;

				case ScreenType.Maze:
					_currentScreen = new MazeGameScreen();						break;

				case ScreenType.BreakOut:
					_currentScreen = new BreakOutGameScreen();				break;

				case ScreenType.RPG_Title:
					_currentScreen = new RPG.TitleScreen();						break;

				case ScreenType.RPG_Help:
					_currentScreen = new RPG.HelpScreen();						break;

				case ScreenType.RPG_FairyHome:
					_currentScreen = new RPG.FairyHomeScreen();				break;

				case ScreenType.RPG_FairyForest:
					_currentScreen = new RPG.FairyForestScreen();			break;

				case ScreenType.RPG_Village:
					_currentScreen = new RPG.VillageScreen();					break;

				case ScreenType.RPG_GeneralHome:
					_currentScreen = new RPG.GeneralHomeScreen();			break;

				case ScreenType.RPG_PathToRuin:
					_currentScreen = new RPG.PathToRuinScreen();			break;

				case ScreenType.RPG_RuinL1:
					_currentScreen = new RPG.RuinL1Screen();					break;

				case ScreenType.RPG_RuinL2:
					_currentScreen = new RPG.RuinL2Screen();					break;

				case ScreenType.RPG_Basement:
					_currentScreen = new RPG.RuinBasementScreen();		break;

				case ScreenType.Test:
					_currentScreen = new MainGameScreen();
					break;
			}

			CurrScreenType = i_screenType;

			_bForceTransition = false;

			if (_currentScreen != null)
				_currentScreen.OnEnter();
		}

		private void ResetTransition()
		{
			_transition.IsVisible = false;
			_transition.Size = Vector2.Zero;
			_transition.Offset = Camera.Self.ScreenCentre;

			IsOnCoolDown = false;
			CoolDownCounter = 0;
			IsTransitioning = false;
			ShowTransition = false;
		}
		#endregion
		#endregion
	}
}
