using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameEngine.Scripting
{
	public interface IAfterable
	{
		IUntilable After(double i_time);
		IUntilable After(Func<bool> i_predicate);
	}

	public interface IUntilable
	{
		void Until(Func<bool> i_predicate);
		void Until(double i_time);
	}

	public class TimedAction : IAfterable, IUntilable
	{
		#region Property
		public double TimeToExecute { set; get; }
		public double TimeToStop { set; get; }
		public Action WhatToExecute { set; get; }
		public Func<bool> ExecuteUntil { set; get; }
		public Func<bool> ConditionToExecute { set; get; }

		public float StartTimeToStop { set; get; }
		public float StartTimeToExecute { set; get; }

		public bool ShouldExecute
		{
			get
			{
				if (TimeToExecute < Double.MaxValue)
				{
					float elapsedTime = TimeManager.Self.TotalTime - StartTimeToExecute;
					return elapsedTime >= TimeToExecute;
				}
				else
				{
					bool retval = ConditionToExecute();
					return retval;
				}
			}
		}
		public bool ShouldStop
		{
			get
			{
				if (TimeToStop < Double.MaxValue)
				{
					float elapsedTime = TimeManager.Self.TotalTime - StartTimeToStop;
					return elapsedTime >= TimeToStop;
				}
				else
					return ExecuteUntil();
			}
		}
		#endregion

		IUntilable IAfterable.After(double i_timeToExecute)
		{
			TimeToExecute = i_timeToExecute;
			ScriptingManager.Self.ForceSortTimedActions();

			ConditionToExecute = null;

			StartTimeToExecute = TimeManager.Self.TotalTime;

			return this;
		}

		IUntilable IAfterable.After(Func<bool> i_predicate)
		{
			ConditionToExecute = i_predicate;

			TimeToExecute = Double.MaxValue;
			ScriptingManager.Self.ForceSortTimedActions();

			StartTimeToExecute = TimeManager.Self.TotalTime;

			return this;
		}

		void IUntilable.Until(double i_timeToStop)
		{
			TimeToStop = i_timeToStop;

			ExecuteUntil = null;
		}

		void IUntilable.Until(Func<bool> i_predicate)
		{
			ExecuteUntil = i_predicate;

			TimeToStop = Double.MaxValue;
		}
	}
}
