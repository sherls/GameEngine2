// XNA
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace GameEngine
{
	public class Sprite : Entity, IVisible, IDrawable, IEngineAddable
	{
		#region Public
		#region Property
		#region IVisible property
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

		#region IDrawable
		public float Z { set; get; }
		#endregion

		public Texture2D Texture
		{
			set
			{
				_texture = value;

				if (_texture != null)
				{
					this.Size.X = _texture.Width;
					this.Size.Y = _texture.Height;
				}
			}
			get
			{
				return _texture;
			}
		}

		public Vector2 TextureCoordinate { set; get; }

		public bool IsTiled { set; get; }
		#endregion

		#region Methods
		public Sprite()
		{
			Size.X = 32;
			Size.Y = 32;

			IsTiled = false;
			IsVisible = true;
		}

		#region IEngineAddable interface
		public void AddToEngine()
		{
			EngineManager.Self.AddEntity(this);
			EngineManager.Self.AddDrawable(this);
		}

		public void RemoveFromEngine()
		{
			EngineManager.Self.RemoveEntity(this);
			EngineManager.Self.RemoveDrawable(this);
		}
		#endregion
		#endregion

		#region Override methods
		override public void Update()
		{
			base.Update();	// Call the inherited's Update f'n
		}
		#endregion
		#endregion

		#region Private
		#region Data
		private Texture2D _texture;
		#endregion

		#region Methods
		void IDrawable.Draw(SpriteBatch i_spriteBatch)
		{
			if (_texture == null)
				return;

			if (this.GetIsAbsoluteVisible())
			{
				Vector2 origin = GetDrawingOrigin();

				if (IsTiled)
				{
					i_spriteBatch.Draw(_texture,
						new Rectangle((int)this.AbsoluteX, (int)this.AbsoluteY, (int)AbsoluteSize.X, (int)AbsoluteSize.Y),
						new Rectangle((int)this.TextureCoordinate.X, (int)this.TextureCoordinate.Y, (int)this.Size.X, (int)this.Size.Y),
						Color.White, 0.0f, origin, SpriteEffects.None, 0);
				}
				else
				{
					i_spriteBatch.Draw(_texture,
						new Rectangle((int)this.AbsoluteX, (int)this.AbsoluteY, (int)AbsoluteSize.X, (int)AbsoluteSize.Y),
						null,
						Color.White, 0.0f, origin, SpriteEffects.None, 0);
				}
			}
		}
		#endregion
		#endregion
	}
}
