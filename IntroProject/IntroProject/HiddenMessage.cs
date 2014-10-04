// XNA
using Microsoft.Xna.Framework;

// GameEngine
using GameEngine;
using GameEngine.UI;
using GameEngine.Physics;

namespace IntroProject
{
	public partial class HiddenMessage : Entity, IVisible, IEngineAddable
	{
		#region Public
		#region Property
		#region IVisible interface
		public bool IsVisible { set; get; }

		// Explicit implementation
		IVisible IVisible.Parent
		{
			get
			{
				return Parent as IVisible;
			}
		}
		#endregion
		#endregion

		#region Methods
		public HiddenMessage()
		{
			IsVisible = true;
		}

		public HiddenMessage(string i_message, Color i_backgroundColour, Color i_textColour)
		{
			CollisionManager.Self.Add(_triggerBox);
			CollisionManager.Self.RegisterCallback(_triggerBox, TriggerBoxCollisionHandle);
		}

		public override void Update()
		{
			if (_bTimerActivated)
			{
				_timer += TimeManager.Self.DeltaTimeFloat;
				if (_timer > 0.5f)
					_bTimerActivated = false;
			}
			else
			{
				_message.IsVisible = false;
			}
		}
		#endregion

		#region IEngineAddable interface
		public void AddToEngine()
		{
			EngineManager.Self.AddEntity(this);
		}

		public void RemoveFromEngine()
		{
			EngineManager.Self.RemoveEntity(this);
		}
		#endregion

		#endregion

		#region Private
		float _timer;
		bool _bTimerActivated;

		void TriggerBoxCollisionHandle(ICollidable i_other, Vector2 i_normal)
		{
			if (i_other is GameEngine.Physics.Circle)
			{
				_message.IsVisible = true;
				_timer = 0.0f;
				_bTimerActivated = true;
			}
		}
		#endregion
	}
}
