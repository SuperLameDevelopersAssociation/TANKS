using System;
using TrueSync;

/// <summary>
/// Use this to call the specified action at the given time interval
/// </summary>
public class TrueSyncIntervalExecutor
{
	private Action intervalAction; // Action to be executed upon reaching a time interval
	private FP intervalTime; // Time interval in which the action is called (in seconds)
	private FP elapsedTime = FP.Zero; // Running time keeper, like a stop watch, when it reaches the intervalTime, call the action

	/// <summary>
	/// Constructor to initialize and interval timer
	/// </summary>
	/// <param name="intervalAction">Action to be called upon reaching intervalTime seconds</param>
	/// <param name="intervalTime">Time in seconds after which Action gets called</param>
	public TrueSyncIntervalExecutor(Action intervalAction, FP intervalTime)
	{
		this.intervalAction = intervalAction;
		this.intervalTime = intervalTime;
	}

	/// <summary>
	/// Handles processing of a single tick of the interval timer
	/// Call this every frame (probably inside OnSyncedUpdate) 
	/// </summary>
	public void Tick()
	{
		elapsedTime += TrueSyncManager.DeltaTime;
		if (elapsedTime >= intervalTime)
		{
			elapsedTime -= intervalTime; // Do not set to zero so that overlap time is preserved
			intervalAction.Invoke();
		}
	}
}
