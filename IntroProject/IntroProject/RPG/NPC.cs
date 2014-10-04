// XNA
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

// GameEngine
using GameEngine;

namespace IntroProject.RPG
{
	public class NPC : GameEngine.Physics.Rectangle
	{
		#region Public
		#region Property
		public float SetZ
		{
			set
			{
				Z = value;
				_sprite.Z = value;
			}
		}

		public Texture2D Texture
		{
			set
			{
				_sprite.Texture = value;
				_sprite.Size = Constants.TILE_SIZE;
			}
		}

		public string DialogText { set; get; }

		public Movement Move { set; get; }

		public Direction Dir
		{
			set
			{
				_animator.SetIdleDirection(value);
			}
		}
		#endregion

		#region Methods
		public void AddingToEngine()
		{
			_sprite.AddToEngine();
			base.AddToEngine();
		}

		public void RemovingFromEngine()
		{
			_sprite.RemoveFromEngine();
			base.RemoveFromEngine();
		}

		public NPC()
		{
			Size = Constants.TILE_SIZE;
			IsVisible = true;
			Colour = Color.Transparent;
			Move = Movement.Max;

			_animator = new SpriteAnimator();
			Dir = Direction.Down;

			_sprite = new Sprite();
			_sprite.Parent = this;
			_sprite.Offset = Vector2.Zero;
			_sprite.Size = Constants.TILE_SIZE;
			_sprite.IsTiled = true;
			_sprite.TextureCoordinate = _animator.CurrAnimationIdx * _sprite.Size;
		}

		public override void Update()
		{
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

			base.Update();
			_animator.Update();
			_sprite.TextureCoordinate = _animator.CurrAnimationIdx * _sprite.Size;
		}

		public void ShowAutomaticDialog(Vector2 i_direction)
		{
			SetDirection(i_direction);

			RPGHelper.Self.Dialog.IsVisible = true;
			RPGHelper.Self.Dialog.DisplayText = DialogText;
			RPGHelper.Self.StartDialogTimer();
		}

		public void ShowDialog(Vector2 i_direction)
		{
			SetDirection(i_direction);

			RPGHelper.Self.Dialog.IsVisible = true;
			RPGHelper.Self.Dialog.DisplayText = DialogText;
		}
		#endregion
		#endregion

		#region Private
		SpriteAnimator _animator;
		Sprite _sprite;

		void SetDirection(Vector2 i_direction)
		{
			if (i_direction.X >= 1.0f)
				Dir = Direction.Left;
			else if (i_direction.X <= -1.0f)
				Dir = Direction.Right;
			else if (i_direction.Y >= 1.0f)
				Dir = Direction.Down;
			else if (i_direction.Y <= -1.0f)
				Dir = Direction.Up;
		}
		#endregion
	}
}
