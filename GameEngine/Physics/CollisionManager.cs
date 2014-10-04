using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

// XNA
using Microsoft.Xna.Framework;

namespace GameEngine.Physics
{
	public class CollisionManager : Singleton<CollisionManager>
	{
		#region Public
		public void Add(ICollidable i_entity)
		{
			_containedObjects.Add(i_entity);
		}

		public bool Bounce<T>(T i_first, T i_second) where T : Entity, ICollidable
		{
			bool bDidBounce = false;
			Vector2 repositionVector;

			if (i_first.DoesCollideAgainst(i_second, out repositionVector))
			{
				repositionVector.Normalize();
				i_first.TopParent.Velocity = Vector2.Reflect(i_first.TopParent.Velocity, repositionVector);
				bDidBounce = true;
			}

			return bDidBounce;
		}

		public bool Bounce<T,S>(T i_first, S i_second)
			where T : Entity, ICollidable
			where S : Entity, ICollidable
		{
			bool bDidBounce = false;
			Vector2 repositionVector;

			if (i_first.DoesCollideAgainst(i_second, out repositionVector))
			{
				repositionVector.Normalize();
				i_first.TopParent.Velocity = Vector2.Reflect(i_first.TopParent.Velocity, repositionVector);
				bDidBounce = true;
			}

			return bDidBounce;
		}

		public bool Separate<T>(T i_first, T i_second) where T : Entity, ICollidable
		{
			bool bDidSeparate = false;
			Vector2 repositionVector;

			if (i_first.DoesCollideAgainst(i_second, out repositionVector))
			{
				repositionVector *= -1;
				i_first.TopParent.Offset += repositionVector;
				i_first.TopParent.Velocity -= i_first.TopParent.Acceleration * TimeManager.Self.DeltaTimeFloat;
				bDidSeparate = true;
			}

			return bDidSeparate;
		}

		public bool Separate<T,S>(T i_first, S i_second)
			where T : Entity, ICollidable
			where S : Entity, ICollidable
		{
			bool bDidSeparate = false;
			Vector2 repositionVector;

			if (i_first.DoesCollideAgainst(i_second, out repositionVector))
			{
				repositionVector *= -1;
				i_first.TopParent.Offset += repositionVector;
				i_first.TopParent.Velocity -= i_first.TopParent.Acceleration * TimeManager.Self.DeltaTimeFloat;
				bDidSeparate = true;
			}

			return bDidSeparate;
		}

		public void RegisterCallback(ICollidable i_entity, Action<ICollidable, Vector2> i_callback)
		{
			_actions.Add(i_entity, i_callback);
		}

		internal void Update()
		{
			Vector2 normal;
			int containedCount = _containedObjects.Count;
			int containedMinusOne = containedCount - 1;
			for (int i = 0; i < containedMinusOne; ++i)
			{
				for (int j = i + 1; j < containedCount; ++j)
				{
					if (_containedObjects[i].DoesCollideAgainst(_containedObjects[j], out normal))
					{
						if( _actions.ContainsKey(_containedObjects[i]) )
							_actions[_containedObjects[i]](_containedObjects[j], normal);
						if (_actions.ContainsKey(_containedObjects[j]))
							_actions[_containedObjects[j]](_containedObjects[i], normal);
					}
				}
			}
		}
		#endregion

		#region Private
		List<ICollidable> _containedObjects = new List<ICollidable>();
		Dictionary<ICollidable, Action<ICollidable, Vector2>> _actions = new Dictionary<ICollidable, Action<ICollidable, Vector2>>();
		#endregion
	}
}
