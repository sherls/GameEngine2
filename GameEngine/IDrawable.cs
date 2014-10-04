// XNA
using Microsoft.Xna.Framework.Graphics;

namespace GameEngine
{
	public interface IDrawable
	{
		float Z { get; }
		void Draw(SpriteBatch i_spriteBatch);
	}
}
