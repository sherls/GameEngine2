using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace GameEngine
{
	public enum Animation
	{
		Up,
		Down,
		Left,
		Right,
		Idle
	}

	public enum Direction
	{
		Up,
		Down,
		Left,
		Right,
		Idle
	}

	public class SpriteAnimator
	{
		#region Public
		#region Properties
		public Vector2 CurrAnimationIdx { set; get; }
		#endregion

		public void SetAnimation( Animation i_animation )
		{
			_prevAnimation = _currAnimation;
			_currAnimation = i_animation;
		}

		public void SetIdleDirection(Direction i_direction)
		{
			switch (i_direction)
			{
				default:
				case Direction.Down:
					_idleAnimationIdx = new Vector2(_downAnimationStartIdx.X + 1, _downAnimationStartIdx.Y);
					break;

				case Direction.Up:
					_idleAnimationIdx = new Vector2(_upAnimationStartIdx.X + 1, _upAnimationStartIdx.Y);
					break;

				case Direction.Left:
					_idleAnimationIdx = new Vector2(_leftAnimationStartIdx.X + 1, _leftAnimationStartIdx.Y);
					break;

				case Direction.Right:
					_idleAnimationIdx = new Vector2(_rightAnimationStartIdx.X + 1, _rightAnimationStartIdx.Y);
					break;
			}
			CurrAnimationIdx = _idleAnimationIdx;
		}

		public void Update()
		{
			if (_prevAnimation != _currAnimation)
			{
				_ctr = 0;
				switch (_currAnimation)
				{
					case Animation.Idle:
						CurrAnimationIdx = _idleAnimationIdx;
						break;

					case Animation.Up:
						CurrAnimationIdx = _upAnimationStartIdx;
						_idleAnimationIdx = new Vector2(_upAnimationStartIdx.X + 1, _upAnimationStartIdx.Y);
						_currMaxAnimationIdx = (uint)_upAnimationStartIdx.X + _totalAnimation;
						break;

					case Animation.Down:
						CurrAnimationIdx = _downAnimationStartIdx;
						_idleAnimationIdx = new Vector2(_downAnimationStartIdx.X + 1, _downAnimationStartIdx.Y);
						_currMaxAnimationIdx = (uint)_downAnimationStartIdx.X + _totalAnimation;
						break;

					case Animation.Left:
						CurrAnimationIdx = _leftAnimationStartIdx;
						_idleAnimationIdx = new Vector2(_leftAnimationStartIdx.X + 1, _leftAnimationStartIdx.Y);
						_currMaxAnimationIdx = (uint)_leftAnimationStartIdx.X + _totalAnimation;
						break;

					case Animation.Right:
						CurrAnimationIdx = _rightAnimationStartIdx;
						_idleAnimationIdx = new Vector2(_rightAnimationStartIdx.X + 1, _rightAnimationStartIdx.Y);
						_currMaxAnimationIdx = (uint)_rightAnimationStartIdx.X + _totalAnimation;
						break;
				}
			}
			else if (_currAnimation != Animation.Idle)
			{
				if (_ctr % 8 == 0)
				{
					CurrAnimationIdx = new Vector2(((int)CurrAnimationIdx.X + 1) % _currMaxAnimationIdx,
						CurrAnimationIdx.Y);
				}
				++_ctr;
			}
		}
		#endregion

		#region Private
		private Animation _currAnimation = Animation.Idle;
		private Animation _prevAnimation = Animation.Idle;
		private Vector2 _textureIndexSize = new Vector2( 3, 4 );
		private uint _totalAnimation = 3;
		private uint _currMaxAnimationIdx = 0;
		private uint _ctr = 0;
		private Vector2 _downAnimationStartIdx = new Vector2(0, 0);
		private Vector2 _leftAnimationStartIdx = new Vector2(0, 1);
		private Vector2 _rightAnimationStartIdx = new Vector2(0, 2);
		private Vector2 _upAnimationStartIdx = new Vector2(0, 3);
		private Vector2 _idleAnimationIdx = new Vector2(1, 0);
		#endregion
	}
}
