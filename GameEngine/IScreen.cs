using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameEngine
{
	public interface IScreen
	{
		bool IsFocused { set; get; }
		void OnEnter();
		void Update();
		void OnLeave();
	}
}
