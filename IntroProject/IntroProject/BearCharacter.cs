using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

// XNA
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using GameEngine;

namespace IntroProject
{
	public class BearCharacter : Entity, IVisible, IEngineAddable
	{
		#region Public

		#region Property
		#region IVisible interface
		public bool IsVisible { set; get; }

		IVisible IVisible.Parent
		{
			get
			{
				return Parent as IVisible;
			}
		}
		#endregion

		public int NumberOfLives
		{
			set
			{
				if (value < 0)
				{
					throw new Exception("NumberOfLives must be greater than 0");
				}
				_numberOfLives = value;

				if (value > MAXIMUM_NUMBER_OF_LIVES)
				{
					throw new Exception("New lives is larger than maximum number of lives");
				}

				for (int i = 0; i < _lives.Count; ++i)
				{
					_lives[i].IsVisible = i < _numberOfLives;
				}
			}
			get
			{
				return _numberOfLives;
			}
		}

		public float Z
		{
			set
			{
				_mainSprite.Z = value;
				foreach (var life in _lives)
				{
					life.Z = value + .001f;
				}
			}
		}
		#endregion

		#region IEngineAddable interface
		public void AddToEngine()
		{
			EngineManager.Self.AddEntity(this);
			EngineManager.Self.AddEntity(_mainSprite);
			foreach (var live in _lives)
				EngineManager.Self.AddEntity(live);
		}

		public void RemoveFromEngine()
		{
			EngineManager.Self.RemoveEntity(this);
			EngineManager.Self.RemoveEntity(_mainSprite);
			foreach (var live in _lives)
				EngineManager.Self.RemoveEntity(live);
		}
		#endregion

		public BearCharacter()
		{
			IsVisible = true;
			this.Size = new Vector2(0, 0);

			_numberOfLives = MAXIMUM_NUMBER_OF_LIVES;

			_mainSprite = new Sprite();
			_mainSprite.Texture = EngineManager.Self.Load<Texture2D>("Textures/Bear");
			_mainSprite.Parent = this;
			_mainSprite.SizeUnits = SizeUnits.RelativeToParent;
			_mainSprite.Size = new Vector2(32, 32);

			_lives = new List<Sprite>();
			for (int i = 0; i < _numberOfLives; ++i)
			{
				Sprite smallBear = new Sprite();
				smallBear.Texture = EngineManager.Self.Load<Texture2D>("Textures/Bear");

				// Set the Scale values *after* texture
				smallBear.Size = new Vector2(16, 16);

				// Set the OFfset
				smallBear.Offset.X = i * 16;
				smallBear.Offset.Y = -(_mainSprite.Size.Y / 2) + 15;

				// Set the parent
				smallBear.Parent = _mainSprite;

				_lives.Add(smallBear);
			}
		}

		public override void Update()
		{
			base.Update();
		}
		#endregion

		#region Private
		Sprite _mainSprite;
		List<Sprite> _lives;
		int _numberOfLives;

		const int MAXIMUM_NUMBER_OF_LIVES = 4;
		#endregion
	}
}
