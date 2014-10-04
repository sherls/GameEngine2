using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

// XNA
using Microsoft.Xna.Framework;

namespace GameEngine.Physics
{
	public interface ICollidable
	{
		bool DoesCollideAgainst(ICollidable i_other);
		bool DoesCollideAgainst(ICollidable i_other, out Vector2 o_v2RepositionVector);
		bool DoesCollideAgainstFallback(ICollidable i_other);
	}
}
