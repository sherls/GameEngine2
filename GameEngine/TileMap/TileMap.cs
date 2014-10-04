using System;
using System.Collections.Generic;

// XNA
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameEngine.TileMap
{
	public class TileMap : Entity, IVisible, IEngineAddable
	{
		#region Public
		public List<Physics.Rectangle> collisionRectangles;

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

		public float PlayerLayerZ { set; get; }
		#endregion

		#region Methods
		public TileMap()
		{
			_sprites = new List<Sprite>();
			collisionRectangles = new List<Physics.Rectangle>();
		}

		public TileMap Load(string i_fileName)
		{
			_tileMapData = TileMapLoadingManager.Self.ImportTmx(i_fileName);

			float z = 0.0f;
			Vector2 tileSize = new Vector2(_tileMapData.TileSet.TileWidth, _tileMapData.TileSet.TileHeight);
			Vector2 tileIndex = new Vector2(0, 0);
			Vector2 textureIndexSize = new Vector2( _tileMapData.TileSet.Image.Width/_tileMapData.TileSet.TileWidth,
				_tileMapData.TileSet.Image.Height/_tileMapData.TileSet.TileHeight);

			foreach (var layer in _tileMapData.Layers)
			{
				if( layer.Name == "PlayerLayer" )
					PlayerLayerZ = z;

				tileIndex = new Vector2(0, 0);
				foreach(var tile in layer.Data.Tiles)
				{
					if (layer.Name == "CollisionLayer" && tile.Gid >= _tileMapData.TileSet.FirstGid)
					{
						Physics.Rectangle newRectangle = new Physics.Rectangle();

						newRectangle.AbsoluteOffset = tileIndex * tileSize;
						newRectangle.Size = tileSize;

						//newRectangle.IsVisible = true;
						//newRectangle.Z = 2.0f;
						collisionRectangles.Add(newRectangle);
					}
					else
					{
						Sprite newSprite = new Sprite();

						newSprite.AbsoluteOffset = tileIndex * tileSize;
						string textureName = _tileMapData.TileSet.Image.Source;
						int index = textureName.IndexOf("/") + 1;
						textureName = textureName.Substring(index, textureName.Length - index - 4);
						newSprite.Texture = EngineManager.Self.Load<Texture2D>(textureName);
						newSprite.Size = tileSize;
						uint tileGid = tile.Gid - _tileMapData.TileSet.FirstGid;
						Vector2 textureIndex = new Vector2((uint)(tileGid % textureIndexSize.X), (uint)Math.Floor(tileGid / textureIndexSize.X));
						newSprite.TextureCoordinate = textureIndex * tileSize;
						newSprite.Z = z;
						newSprite.IsTiled = true;
						newSprite.Parent = this;

						_sprites.Add(newSprite);
					}

					tileIndex.X += 1;
					if( tileIndex.X >= _tileMapData.Width )
					{
						tileIndex.Y += 1;
						tileIndex.X = 0;
					}
				}

				z += 0.01f;
			}
			return this;
		}

		#region IEngineAddable interface
		public void AddToEngine()
		{
			this.IsVisible = true;
			foreach (Sprite sprite in _sprites)
			{
				sprite.AddToEngine();
			}

			foreach (Physics.Rectangle rectangle in collisionRectangles)
			{
				rectangle.AddToEngine();
			}
		}

		public void RemoveFromEngine()
		{
			this.IsVisible = false;
			foreach (Sprite sprite in _sprites)
			{
				sprite.RemoveFromEngine();
			}

			foreach (Physics.Rectangle rectangle in collisionRectangles)
			{
				rectangle.RemoveFromEngine();
			}
		}
		#endregion
		#endregion
		#endregion

		#region Private
		List<Sprite> _sprites;
		TileMapData _tileMapData;
		#endregion
	}
}
