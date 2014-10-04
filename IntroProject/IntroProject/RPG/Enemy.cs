// XNA
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

// GameEngine
using GameEngine;
using GameEngine.Scripting;

namespace IntroProject.RPG
{
	public class Enemy : GameEngine.Physics.Rectangle
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

		public Direction Dir
		{
			set
			{
				_animator.SetIdleDirection(value);
			}
		}

		public uint XPositionIndex { set; get; }
		public uint YPositionIndex { set; get; }

		public uint ID { set; get; }
		public int HP { set; get; }
		public uint EXP { set; get; }
		public uint Attack { set; get; }
		public uint Defense { set; get; }
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
		#endregion

		public Enemy()
		{
			Size = Constants.TILE_SIZE;
			IsVisible = true;
			Colour = Color.Transparent;
			Move = Movement.Max;

			_animator = new SpriteAnimator();
			Move = Movement.Idle;
			Dir = Direction.Down;

			_sprite = new Sprite();
			_sprite.Parent = this;
			_sprite.Offset = Vector2.Zero;
		}

		public void SetupEnemy(float i_z)
		{
			SetZ = i_z;
			Offset = new Vector2(XPositionIndex * 32, YPositionIndex * 32);

			EnemyStats data = RPGHelper.Self.EnemiesData[(int)ID];
			HP = data.HP;
			EXP = data.EXP;
			Attack = data.Attack;
			Defense = data.Defense;

			_sprite.Texture = EngineManager.Self.Load<Texture2D>(data.Texture);
			_sprite.Size = Constants.TILE_SIZE;
			_sprite.IsTiled = true;
			_sprite.TextureCoordinate = _animator.CurrAnimationIdx * _sprite.Size;
		}

		public void Combat()
		{
			RPGHelper.Self.PlayerCurrHP -= (int)(Attack - RPGHelper.Self.PlayerDefense);
			HP -= (int)(RPGHelper.Self.PlayerAttack - Defense);

			if (HP <= 0)
			{
				RPGHelper.Self.PlayerCurrExp += EXP;
				RPGHelper.Self.PlayerExpCheckAndLevelUp();
				ScriptingManager.Self.
					Do(() => _sprite.IsVisible = !_sprite.IsVisible).
					After(0).Until(Constants.DEATH_ANIMATION_TIME);
				ScriptingManager.Self.
					Do(() => IsVisible = false).
					After(Constants.DEATH_ANIMATION_TIME);
			}
		}

		public void MoveRightTo(float i_position)
		{
			float timer = TimeManager.Self.DeltaTimeFloat * 2;

			ScriptingManager.Self.
				Do(() => Move = Movement.Right).
				After(timer).
				Until(() => AbsoluteX >= i_position);

			ScriptingManager.Self.
				Do(() => Move = Movement.Idle).
				After(() => AbsoluteX >= i_position);
		}

		public void MoveLeftTo(float i_position)
		{
			float timer = TimeManager.Self.DeltaTimeFloat * 2;

			ScriptingManager.Self.
				Do(() => Move = Movement.Left).
				After(timer).
				Until(() => AbsoluteX <= i_position);

			ScriptingManager.Self.
				Do(() => Move = Movement.Idle).
				After(() => AbsoluteX <= i_position);
		}

		public void MoveUpTo(float i_position)
		{
			float timer = TimeManager.Self.DeltaTimeFloat * 2;

			ScriptingManager.Self.
				Do(() => Move = Movement.Up).
				After(timer).
				Until(() => AbsoluteY <= i_position);

			ScriptingManager.Self.
				Do(() => Move = Movement.Idle).
				After(() => AbsoluteY <= i_position);
		}

		public void MoveDownTo(float i_position)
		{
			float timer = TimeManager.Self.DeltaTimeFloat * 2;

			ScriptingManager.Self.
				Do(() => Move = Movement.Down).
				After(timer).
				Until(() => AbsoluteY >= i_position);

			ScriptingManager.Self.
				Do(() => Move = Movement.Idle).
				After(() => AbsoluteY >= i_position);
		}

		public override void Update()
		{
			if (!IsVisible)
				return;

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
		#endregion

		#region Private
		SpriteAnimator _animator;
		Sprite _sprite;

		public Movement Move { set; get; }
		#endregion
	}
}
