using System;

// XNA
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

// GameEngine
using GameEngine;
using GameEngine.UI;
using GameEngine.Data;
using GameEngine.Scripting;

namespace IntroProject.RPG
{
	public class SpritePlayer : GameEngine.Physics.Rectangle
	{
		#region Public
		#region Property
		public Movement Move { set; get; }
		#endregion

		#region Methods
		public void AddingToEngine()
		{
			base.AddToEngine();
			_sprite.AddToEngine();
			_healthBar.AddToEngine();
			_menu.AddToEngine();
		}

		public void RemovingFromEngine()
		{
			base.RemoveFromEngine();
			_sprite.RemoveFromEngine();
			_healthBar.RemoveFromEngine();
			_menu.RemoveFromEngine();
		}

		public SpritePlayer()
		{
			_bIsGameOver = false;
			_returnToTitle = false;
			_hasReturnToTitle = false;

			Size = Constants.TILE_SIZE;
			IsVisible = true;
			Colour = Color.Transparent;
			_animator = new SpriteAnimator();
			_animator.SetIdleDirection(RPGHelper.Self.PlayerIdleDirection);

			var xmlData = DataLoadingManager.Self.ImportXml("SpritePlayer.xml");
			var createdItems = DataLoadingManager.Self.InstantiateObjectsFromXmlObject(xmlData);

			_sprite = createdItems["_sprite"] as Sprite;
			_sprite.Parent = this;
			_sprite.TextureCoordinate = _animator.CurrAnimationIdx * _sprite.Size;

			_healthBar = createdItems["_healthBar"] as ProgressBar;
			_healthBar.MaxValue = RPGHelper.Self.PlayerMaxHP;
			_healthBar.CurrValue = RPGHelper.Self.PlayerCurrHP;

			_menu = createdItems["_menu"] as Menu;

			if (RPGHelper.Self.IsNewGame)
				_healthBar.IsVisible = false;
		}

		public override void Update()
		{
			if (_bIsGameOver && _returnToTitle && !_hasReturnToTitle)
				GoBackToTitle();

			if (ScreenManager.Self.IsTransitioning || _bIsGameOver || RPGHelper.Self.Query.IsVisible)
				return;

			if ((InputManager.Self.IsKeyDown(Keys.LeftShift) || InputManager.Self.IsKeyDown(Keys.RightShift)) && !RPGHelper.Self.Query.IsVisible)
				_menu.IsVisible = !_menu.IsVisible;

			if (_menu.IsVisible)
				return;

			if (!RPGHelper.Self.IsOnStory)
			{
				_healthBar.MaxValue = RPGHelper.Self.PlayerMaxHP;
				if (RPGHelper.Self.PlayerCurrHP < 0)
					_healthBar.CurrValue = 0;
				else
					_healthBar.CurrValue = RPGHelper.Self.PlayerCurrHP;
			}

			if (!RPGHelper.Self.IsOnStory)
			{
				Velocity = Vector2.Zero;

				if (InputManager.Self.IsKeyPressed(Keys.Up))
					Move = Movement.Up;
				else if (InputManager.Self.IsKeyPressed(Keys.Down))
					Move = Movement.Down;
				else if (InputManager.Self.IsKeyPressed(Keys.Left))
					Move = Movement.Left;
				else if (InputManager.Self.IsKeyPressed(Keys.Right))
					Move = Movement.Right;
				else
					Move = Movement.Max;
			}

			if (Move == Movement.Up)
			{
				Velocity = Constants.UP;
				_animator.SetAnimation(Animation.Up);
			}
			else if (Move == Movement.Down)
			{
				Velocity = Constants.DOWN;
				_animator.SetAnimation(Animation.Down);
			}
			else if (Move == Movement.Left)
			{
				Velocity = Constants.LEFT;
				_animator.SetAnimation(Animation.Left);
			}
			else if (Move == Movement.Right)
			{
				Velocity = Constants.RIGHT;
				_animator.SetAnimation(Animation.Right);
			}
			else
			{
				Velocity = Vector2.Zero;
				_animator.SetAnimation(Animation.Idle);
			}

			Velocity *= Constants.SPEED;

			CheckDeath();

			base.Update();
			_animator.Update();
			_sprite.TextureCoordinate = _animator.CurrAnimationIdx * _sprite.Size;
		}

		public void SetupPlayer(float i_Z)
		{
			Z = i_Z;
			_sprite.Z = i_Z;
			Offset = RPGHelper.Self.PlayerSpawnLocation;
		}

		public void RestoreHP()
		{
			RPGHelper.Self.RestorePlayerHP();
		}
		#endregion
		#endregion

		#region Private
		Sprite _sprite;
		ProgressBar _healthBar;
		Menu _menu;
		SpriteAnimator _animator;

		bool _bIsGameOver;
		bool _returnToTitle;
		bool _hasReturnToTitle;

		void CheckDeath()
		{
			if ((RPGHelper.Self.PlayerCurrHP <= 0) && !_bIsGameOver)
			{
				_bIsGameOver = true;
				ScriptingManager.Self.
					Do(() => _sprite.IsVisible = !_sprite.IsVisible).
					After(0).Until(Constants.DEATH_ANIMATION_TIME);
				ScriptingManager.Self.
					Do(() =>
					{
						_sprite.IsVisible = false;
						_returnToTitle = true;
					}).
					After(Constants.DEATH_ANIMATION_TIME);
			}
		}

		void GoBackToTitle()
		{
			_hasReturnToTitle = true;
			ScreenManager.Self.ShowTransition = true;
			ScreenManager.Self.GoToScreen(ScreenType.RPG_Title);
		}
		#endregion
	}
}
