using System;
using System.Collections.Generic;

// XNA
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

// GameEngine
using GameEngine.Physics;
using GameEngine.Scripting;

namespace GameEngine
{
	public class EngineManager : Singleton<EngineManager>
	{
		#region Public
		public EngineManager()
		{
		}

		public void Initialize(SpriteBatch spriteBatch, ContentManager contentManager)
		{
			_spriteBatch = spriteBatch;
			_contentManager = contentManager;
			Camera.Self.GraphicsDevice = spriteBatch.GraphicsDevice;

			// Initialization
			InputManager.Self.Initialize();
		}

		public void AddEntity(Entity i_entity)
		{
			if (!_entityList.Contains(i_entity))
				_entityList.Add(i_entity);

			if (i_entity is IDrawable)
				_drawableList.Add(i_entity as IDrawable);
		}

		public void AddSprite(Sprite i_sprite, bool i_bUpdateAutomatically = false)
		{
			AddDrawable(i_sprite);

			if (i_bUpdateAutomatically)
				AddEntity(i_sprite);
		}

		public void AddText(Text i_text, bool i_bUpdateAutomatically = false)
		{
			AddDrawable(i_text);

			if (i_bUpdateAutomatically)
				AddEntity(i_text);
		}

		public void AddDrawable(IDrawable i_drawable)
		{
			_drawableList.Add(i_drawable);
		}

		public void RemoveEntity(Entity i_entity)
		{
			_entityList.Remove(i_entity);

			if( i_entity is IDrawable )
			{
				IDrawable entityAsDrawable = i_entity as IDrawable;
				_drawableList.Remove(entityAsDrawable);
			}
		}

		public void RemoveText(Text i_text)
		{
			RemoveDrawable(i_text);
		}

		public void RemoveDrawable(IDrawable i_drawable)
		{
			_drawableList.Remove(i_drawable);
		}

		public void Update(GameTime i_gameTime)
		{
			TimeManager.Self.Update(i_gameTime);
			ScriptingManager.Self.Update();

			InputManager.Self.Update();

			foreach (var objectToUpdate in _entityList)
				objectToUpdate.Update();

			CollisionManager.Self.Update();
		}

		public void Draw()
		{
			_spriteBatch.Begin();

			_drawableList.Sort(CompareDrawable);

			foreach (var item in _drawableList)
				item.Draw(_spriteBatch);

			_spriteBatch.End();
		}

		public T Load<T>(string i_name)
		{
			return _contentManager.Load<T>(i_name);
		}
		#endregion

		#region Private
		#region Data
		List<Entity> _entityList = new List<Entity>();
		List<IDrawable> _drawableList = new List<IDrawable>();
		SpriteBatch _spriteBatch;
		ContentManager _contentManager;
		#endregion

		#region Methods
		private int CompareDrawable(IDrawable i_first, IDrawable i_second)
		{
			return Math.Sign(i_first.Z - i_second.Z);
		}
		#endregion
		#endregion
	}
}
