// XNA
using Microsoft.Xna.Framework;

namespace IntroProject
{
	static public class Constants
	{
		static public Vector2 TRANSITION_SPEED = new Vector2(80, 50.0f);
		static public Vector2 UP = new Vector2(0, -1);
		static public Vector2 DOWN = new Vector2(0, 1);
		static public Vector2 LEFT = new Vector2(-1, 0);
		static public Vector2 RIGHT = new Vector2(1, 0);
		static public Vector2 TILE_SIZE = new Vector2(32, 32);

		static public float SPEED = 100.0f;
		static public float SCRIPT_DELAY = 2.0f;
		static public float DIALOG_TIMER = 2.0f;
		static public float DEATH_ANIMATION_TIME = 0.5f;

		static public float TRANSITION_COOL_DOWN = 2;
	}
}
