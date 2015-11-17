using System;
using System.Collections.Generic;
using System.Collections;

namespace MonoFlixel
{
	public class TimerManager : FlxBasic
	{
		protected List<FlxTimer> _timers;

		/**
	 * Instantiates a new timer manager.
	 */
		public TimerManager ()
		{
			_timers = new List<FlxTimer> ();
			Visible = false; // don't call draw on this plugin
		}

		/**
	 * Clean up memory.
	 */
		public override void destroy ()
		{
			clear ();
			_timers = null;
			base.destroy ();
		}

		/**
	 * Called by <code>FlxG.updatePlugins()</code> before the game state has
	 * been updated. Cycles through timers and calls <code>update()</code> on
	 * each one.
	 */
		public override void update ()
		{
			int i = _timers.Count - 1;
			FlxTimer timer;
			while (i >= 0) {
				timer = _timers [i--];
				if ((timer != null) && !timer.paused && !timer.finished && (timer.time > 0))
					timer.update ();
			}
		}

		/**
	 * Add a new timer to the timer manager. Usually called automatically by
	 * <code>FlxTimer</code>'s constructor.
	 * 
	 * @param Timer The <code>FlxTimer</code> you want to add to the manager.
	 */
		public void add (FlxTimer Timer)
		{
			_timers.Add (Timer);
		}

		/**
	 * Remove a timer from the timer manager. Usually called automatically by
	 * <code>FlxTimer</code>'s <code>stop()</code> function.
	 * 
	 * @param Timer The <code>FlxTimer</code> you want to remove from the
	 *        manager.
	 */
		public void remove (FlxTimer Timer)
		{
			int index = _timers.IndexOf (Timer);
			if (index >= 0)
				_timers.RemoveAt (index);
		}

		/**
	 * Removes all the timers from the timer manager.
	 */
		public void clear ()
		{
			if (_timers != null) {
				int i = _timers.Count - 1;
				FlxTimer timer;
				while (i >= 0) {
					timer = _timers [i--];
					if (timer != null)
						timer.destroy ();
				}
				_timers.Clear ();
			}
		}
	}
}

