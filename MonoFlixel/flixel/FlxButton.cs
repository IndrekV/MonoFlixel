#region Using Statements
using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.Input;
using MonoFlixel;

#endregion

namespace MonoFlixel.Examples
{
	public delegate void FlxButtonClick();

	/**
	 * A simple button class that calls a function when clicked by the mouse.
	 * 
	 * @author Ka Wing Chin
	 * @author Thomas Weston
	 */
	public class FlxButton : FlxSprite
	{
		static protected String ImgDefaultButton = "MonoFlixel/button";

		/**
		 * Used with public variable <code>Status</code>, means not Highlighted or
		 * Pressed.
		 */
		public const int Normal = 0;
		/**
		 * Used with public variable <code>Status</code>, means Highlighted (usually
		 * from mouse over).
		 */
		public const int Highlight = 1;
		/**
		 * Used with public variable <code>Status</code>, means Pressed (usually
		 * from mouse click).
		 */
		public const int Pressed = 2;

		/**
		 * The text that appears on the button.
		 */
		public FlxText Label;
		/**
		 * Controls the offset (from top left) of the text from the button.
		 */
		public FlxPoint LabelOffset;
		/**
		 * This function is called when the button is released. We recommend
		 * assigning your main button behavior to this function via the
		 * <code>FlxButton</code> constructor.
		 */
		//public IFlxButton onUp;
		/**
		 * This function is called when the button is Pressed down.
		 */
		//public IFlxButton onDown;
		/**
		 * This function is called when the mouse goes over the button.
		 */
		//public IFlxButton onOver;
		/**
		 * This function is called when the mouse leaves the button area.
		 */
		//public IFlxButton onOut;
		/**
		 * Shows the current state of the button.
		 */
		public int Status;
		/**
		 * Set this to play a Sound when the mouse goes over the button. We
		 * recommend using the helper function setSounds()!
		 */
		public FlxSound SoundOver;
		/**
		 * Set this to play a Sound when the mouse leaves the button. We recommend
		 * using the helper function setSounds()!
		 */
		public FlxSound SoundOut;
		/**
		 * Set this to play a Sound when the button is Pressed down. We recommend
		 * using the helper function setSounds()!
		 */
		public FlxSound SoundDown;
		/**
		 * Set this to play a Sound when the button is released. We recommend using
		 * the helper function setSounds()!
		 */
		public FlxSound SoundUp;

		/**
		 * Used for checkbox-style behavior.
		 */
		protected bool _onToggle { get; set; }

		/**
		 * Tracks whether or not the button is currently Pressed.
		 */
		protected Boolean _pressed;
		/**
		 * Whether or not the button has initialized itself yet.
		 */
		private Boolean _initialized;

		/// <summary>
		/// This function is called when the button is clicked.
		/// </summary>
		protected FlxButtonClick _callback;

		/**
		 * Creates a new <code>FlxButton</code> object with a gray background and a
		 * callback function on the UI thread.
		 * 
		 * @param X The X position of the button.
		 * @param Y The Y position of the button.
		 * @param Label The text that you want to appear on the button.
		 * @param OnClick The function to call whenever the button is clicked.
		 */
		public FlxButton(float x = 0, float y = 0, String label = null, FlxButtonClick Callback = null) : base(x, y)
		{
			if(label != null)
			{
				Label = new FlxText(x-1, y+3, 80, label);
				Label.setFormat(null, 8, new Color(0x33,0x33,0x33), "center", Color.Transparent);
				LabelOffset = new FlxPoint(-1, 3);
			}
			loadGraphic(ImgDefaultButton, true, false, 80, 20);
			_callback = Callback;
			/*
			onUp = OnClick;

			onDown = null;
			onOut = null;
			onOver = null;
*/
			SoundOver = null;
			SoundOut = null;
			SoundDown = null;
			SoundUp = null;

			Status = Normal;
			_onToggle = false;
			_pressed = false;
			_initialized = false;
		}
		
		public override void destroy()
		{
			/*
			if(FlxG.getStage() != null)
				FlxG.getStage().removeEventListener(MouseEvent.MOUSE_UP, mouseUpListener);
			*/

			if (FlxG.mouse != null)
				FlxG.mouse.removeMouseListener(OnMouseUp);

			if(Label != null)
			{
				Label.destroy();
				Label = null;
			}
			/*
			OnUp = null;
			OnDown = null;
			OnOut = null;
			OnOver = null;
			*/
			if(SoundOver != null)
				SoundOver.destroy();
			if(SoundOut != null)
				SoundOut.destroy();
			if(SoundDown != null)
				SoundDown.destroy();
			if(SoundUp != null)
				SoundUp.destroy();
			base.destroy();
		}

		
		public override void preUpdate()
		{
			base.preUpdate();
			if(!_initialized)
			{
				if (!_initialized)
				{
					if (FlxG.State == null) return;
					FlxG.mouse.addMouseListener(OnMouseUp);
					_initialized = true;
				}
				_initialized = true;
			}
		}

		
		public override void update()
		{
			UpdateButton(); // Basic button logic

			// Default button appearance is to simply update
			// the Label appearance based on animation frame.
			if(Label == null)
				return;
			switch(Frame)
			{
			case Highlight: // Extra behavior to accommodate checkbox logic.
				Label.Alpha = 1.0f;
				Console.Write ("Highlighted");
				break;
			case Pressed:
				Label.Alpha = 0.5f;
				Console.Write ("Pressed");
				Label.Y++;
				break;
			case Normal:
			default:
				Label.Alpha = 0.8f;
				break;
			}
		}

		/**
		 * Basic button update logic
		 */
		protected void UpdateButton()
		{
			if(Status == Pressed)
				Status = Normal;

			// Figure out if the button is Highlighted or Pressed or what
			// (ignore checkbox behavior for now).

			if(FlxG.mouse.cursor.Visible)
			{
				if(Cameras == null)
					Cameras = FlxG.cameras;
				FlxCamera Camera;
				int i = 0;
				int l = Cameras.Count;
				int pointerId = 0;
				//int totalPointers = FlxG.mouse.ActivePointers + 1;
				int totalPointers = 2;
				Boolean offAll = true;
				while(i < l)
				{
					Camera = Cameras[i++];
					while(pointerId < totalPointers)
					{
						//FlxG.mouse.GetWorldPosition(pointerId, Camera, _tagPoint);
						if(overlapsPoint(_tagPoint, true, Camera))
						{
							offAll = false;
							if(FlxG.mouse.pressed())
							{
								Status = Pressed;
								//if(FlxG.mouse.justPressed(pointerId))
								if(FlxG.mouse.justPressed())
								{
									/*
									if(OnDown != null)
									{
										OnDown.Callback();
									}
									if(SoundDown != null)
										SoundDown.Play(true);
									*/
								}
							}

							if(Status == Normal)
							{
								Status = Highlight;
								/*
								if(OnOver != null)
									OnOver.Callback();
								if(SoundOver != null)
									SoundOver.Play(true);
								*/

							}
						}
						++pointerId;
					}
				}
				if(offAll)
				{
					if(Status != Normal)
					{
						/*
						if(OnOut != null)
							OnOut.Callback();

						if(SoundOut != null)
							SoundOut.Play(true);
						*/

					}
					Status = Normal;
				}
			}

			// Then if the Label and/or the Label offset exist,
			// position them to match the button.
			if(Label != null)
			{
				Label.X = X;
				Label.Y = Y;
			}
			if(LabelOffset != null)
			{
				Label.X += LabelOffset.X;
				Label.Y += LabelOffset.Y;
			}

			// Then pick the appropriate frame of animation
			if((Status == Highlight) && (_onToggle || FlxG.mobile))
				Frame = Normal;
			else
				Frame = Status;

		}

		/**
		 * Just draws the button graphic and text Label to the screen.
		 */
		
		public override void draw()
		{
			base.draw();
			if(Label != null)
			{
				Label.ScrollFactor = ScrollFactor;
				Label.Cameras = Cameras;
				Label.draw();
			}
		}

		/**
		 * Updates the size of the text field to match the button.
		 */
		
		protected void ResetHelpers()
		{
			base.resetHelpers();
			if(Label != null)
			{
				Label.Width = Width;
				//Label.CalcFrame();
			}
		}

		/**
		 * Set Sounds to play during mouse-button interactions. These operations can
		 * be done manually as well, and the public Sound variables can be used
		 * after this for more fine-tuning, such as positional audio, etc.
		 * 
		 * @param SoundOver What embedded Sound effect to play when the mouse goes
		 *        over the button. Default is null, or no Sound.
		 * @param SoundOverVolume How load the that Sound should be.
		 * @param SoundOut What embedded Sound effect to play when the mouse leaves
		 *        the button area. Default is null, or no Sound.
		 * @param SoundOutVolume How load the that Sound should be.
		 * @param SoundDown What embedded Sound effect to play when the mouse
		 *        presses the button down. Default is null, or no Sound.
		 * @param SoundDownVolume How load the that Sound should be.
		 * @param SoundUp What embedded Sound effect to play when the mouse releases
		 *        the button. Default is null, or no Sound.
		 * @param SoundUpVolume How load the that Sound should be.
		 */
		public void setSounds(String SoundOver = null, float SoundOverVolume = 1.0f, String SoundOut = null, float SoundOutVolume = 1.0f, String SoundDown = null, float SoundDownVolume = 1.0f, String SoundUp = null,
			float SoundUpVolume = 1.0f)
		{
			/*
			if(SoundOver != null)
				SoundOver = FlxG.loadSound(SoundOver, SoundOverVolume);
			if(SoundOut != null)
				SoundOut = FlxG.loadSound(SoundOut, SoundOutVolume);
			if(SoundDown != null)
				SoundDown = FlxG.loadSound(SoundDown, SoundDownVolume);
			if(SoundUp != null)
				SoundUp = FlxG.loadSound(SoundUp, SoundUpVolume);
				*/
		}

		/**
		 * Internal function for handling the actual callback call (for UI thread
		 * dependent calls like <code>FlxU.openURL()</code>).
		 */

		protected void OnMouseUp(object Sender, FlxMouseEvent MouseEvent)
		{
			/*
			if(!Exists || !Visible || !Active || (Status != Pressed))
				return;
			if(OnUp != null)
				OnUp.Callback();
			if(SoundUp != null)
				SoundUp.Play(true);
			*/
			if (!Exists || !Visible || !Active || !FlxG.mouse.justReleased() || /*(FlxG.pause && !pauseProof) || */(_callback == null)) return;

			if (overlapsPoint(new FlxPoint (FlxG.mouse.x, FlxG.mouse.y)) /*&& _counter > 0.5f*/)
			{
				Console.WriteLine("calling back from mouse press");
				//on = true;
				_callback();
			}
		}
	}
}
