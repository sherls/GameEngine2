using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameEngine.Scripting
{
	public class ScriptingManager : Singleton<ScriptingManager>
	{
		public ScriptingManager()
		{
		}

		public void Update()
		{
			// Check the timed action
			for (int i = 0; i < _timedActions.Count; ++i)
			{
				//if ((_timedActions[i].ConditionToExecute != null) && (_timedActions[i].ShouldExecute))
				if(_timedActions[i].ShouldExecute)
				{
					_timedActions[i].WhatToExecute();

					if ((_timedActions[i].ExecuteUntil != null) || (_timedActions[i].TimeToStop < Double.MaxValue))
					{
						_timedActions[i].StartTimeToStop = TimeManager.Self.TotalTime;
						_continuallyExecutingActions.Add(_timedActions[i]);
						ForceSortExecutingActions();
					}

					_timedActions.RemoveAt(i);
					--i;
				}
			}

			for( int i = 0; i < _continuallyExecutingActions.Count; ++i )
			{
				if (_continuallyExecutingActions[i].ShouldStop)
				{
					_continuallyExecutingActions.RemoveAt(i);
					--i;
				}
				else
				{
					_continuallyExecutingActions[i].WhatToExecute();
				}
			}
		}

		public void Add(TimedAction i_action)
		{
			_timedActions.Add(i_action);

			ForceSortTimedActions();
		}

		public IAfterable Do(Action whatToExecute)
		{
			TimedAction newAction = new TimedAction();
			newAction.WhatToExecute = whatToExecute;

			Add(newAction);

			return newAction;
		}

		internal void ForceSortTimedActions()
		{
			// this can be MUCH faster!!!
			_timedActions.Sort((a, b) => a.TimeToExecute.CompareTo(b.TimeToExecute));
		}

		internal void ForceSortExecutingActions()
		{
			_continuallyExecutingActions.Sort((a, b) => a.TimeToStop.CompareTo(b.TimeToStop));
		}

		List<TimedAction> _timedActions = new List<TimedAction>();
		List<TimedAction> _continuallyExecutingActions = new List<TimedAction>();
	}
}
