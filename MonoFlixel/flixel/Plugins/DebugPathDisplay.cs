using System;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.Linq;

namespace MonoFlixel
{
	public class DebugPathDisplay : FlxBasic
	{
		protected List<FlxPath> _paths;

		/**
		 * Instantiates a new debug path display manager.
		 */
		public DebugPathDisplay()
		{
			_paths = new List<FlxPath>();
			active = false; // don't call update on this plugin
		}

		/**
		 * Clean up memory.
		 */
			
		public override void destroy()
		{
			clear();
			_paths = null;
			base.destroy();
		}

		/**
		 * Called by <code>FlxG.drawPlugins()</code> after the game state has been
		 * drawn. Cycles through cameras and calls <code>drawDebug()</code> on each
		 * one.
		 */

		public override void draw()
		{
			FlxCamera camera = FlxG.getActiveCamera();

			if(cameras == null)
				cameras = FlxG.cameras;

			if(!cameras.Contains(camera))
				return;

			if(FlxG.visualDebug && !ignoreDrawDebug)
				drawDebug(camera);
		}

		/**
		 * Similar to <code>FlxObject</code>'s <code>drawDebug()</code>
		 * functionality, this function calls <code>drawDebug()</code> on each
		 * <code>FlxPath</code> for the specified camera. Very helpful for
		 * debugging!
		 * 
		 * @param Camera Which <code>FlxCamera</code> object to draw the debug data
		 *        to.
		 */
		public override void drawDebug(FlxCamera Camera)
		{
			if(Camera == null)
				Camera = FlxG.camera;

			int i = _paths.Count() - 1;
			FlxPath path;
			while(i >= 0)
			{
				path = _paths.ElementAt(i--);
				if((path != null) && !path.ignoreDrawDebug)
					path.drawDebug(Camera);
			}
		}

		/**
		 * Similar to <code>FlxObject</code>'s <code>drawDebug()</code>
		 * functionality, this function calls <code>drawDebug()</code> on each
		 * <code>FlxPath</code> for the specified camera. Very helpful for
		 * debugging!
		 */

		public override void drawDebug()
		{
			drawDebug(null);
		}

		/**
		 * Add a path to the path debug display manager. Usually called
		 * automatically by <code>FlxPath</code>'s constructor.
		 * 
		 * @param Path The <code>FlxPath</code> you want to add to the manager.
		 */
		public void add(FlxPath Path)
		{
			_paths.Add(Path);
		}

		/**
		 * Remove a path from the path debug display manager. Usually called
		 * automatically by <code>FlxPath</code>'s <code>destroy()</code> function.
		 * 
		 * @param Path The <code>FlxPath</code> you want to remove from the manager.
		 */
			public void remove(FlxPath Path)
			{
				int index = _paths.IndexOf(Path, true);
				if(index >= 0)
					_paths.RemoveAt(index);
			}

			/**
		 * Removes all the paths from the path debug display manager.
		 */
			public void clear()
			{
				if(_paths != null)
				{
				int i = _paths.Count() - 1;
					FlxPath path;
					while(i >= 0)
					{
						path = _paths.ElementAt(i--);
						if(path != null)
							path.destroy();
					}
					_paths.Clear();
				}
			}
	}
}

