﻿#region Using Statements
using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.Input;
using MonoFlixel;

#endregion

namespace MonoFlixel.Examples
{

	/**
	 * A simple button class that calls a function when clicked by the mouse.
	 * 
	 * @author Ka Wing Chin
	 * @author Thomas Weston
	 */
	public class FlxButton : FlxSprite
	{
		static protected String ImgDefaultButton = "org/flixel/data/pack:button";

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
		protected Boolean _Pressed;
		/**
		 * Whether or not the button has initialized itself yet.
		 */
		private Boolean _initialized;

		/**
		 * Internal event listener.
		 */
		/*
		private const IEventListener mouseUpListener = new IEventListener()
		{
			
			public void onEvent(Event e)
			{
				onMouseUp((MouseEvent) e);
			}
		};
		*/
		/**
		 * Creates a new <code>FlxButton</code> object with a gray background and a
		 * callback function on the UI thread.
		 * 
		 * @param X The X position of the button.
		 * @param Y The Y position of the button.
		 * @param Label The text that you want to appear on the button.
		 * @param OnClick The function to call whenever the button is clicked.
		 */
		public FlxButton(float x = 0, float y = 0, String Label = null, Func<FlxObject, FlxObject, Boolean> OnClick = null)
		{
			base(x, y);
			if(Label != null)
			{
				Label = new FlxText(0, 0, 80, Label);
				Label.SetFormat(null, 8, 0x333333, "center");
				LabelOffset = new FlxPoint(-1, 3);
			}
			loadGraphic(ImgDefaultButton, true, false, 80, 20);
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
			_Pressed = false;
			_initialized = false;
		}
		
		public void destroy()
		{
			/*
			if(FlxG.getStage() != null)
				FlxG.getStage().removeEventListener(MouseEvent.MOUSE_UP, mouseUpListener);
			*/
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

		
		public void preUpdate()
		{
			base.preUpdate();
			if(!_initialized)
			{
				/*
				if(FlxG.getStage() != null)
				{
					FlxG.getStage().addEventListener(MouseEvent.MOUSE_UP, mouseUpListener);
					_initialized = true;
				}
				*/
				_initialized = true;
			}
		}

		
		public void update()
		{
			UpdateButton(); // Basic button logic

			// Default button appearance is to simply update
			// the Label appearance based on animation frame.
			if(Label == null)
				return;
			switch(GetFrame())
			{
			case Highlight: // Extra behavior to accommodate checkbox logic.
				Label.SetAlpha(1.0f);
				break;
			case Pressed:
				Label.SetAlpha(0.5f);
				Label.y++;
				break;
			case Normal:
			default:
				Label.SetAlpha(0.8f);
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
			if(FlxG.Mouse.GetVisible())
			{
				if(Cameras == null)
					Cameras = FlxG.Cameras;
				FlxCamera Camera;
				int i = 0;
				int l = Cameras.Count();
				int pointerId = 0;
				int totalPointers = FlxG.Mouse.ActivePointers + 1;
				Boolean offAll = true;
				while(i < l)
				{
					Camera = Cameras.get(i++);
					while(pointerId < totalPointers)
					{
						FlxG.Mouse.GetWorldPosition(pointerId, Camera, _point);
						if(OverlapsPoint(_point, true, Camera))
						{
							offAll = false;
							if(FlxG.Mouse.Pressed(pointerId))
							{
								Status = Pressed;
								if(FlxG.Mouse.JustPressed(pointerId))
								{
									if(OnDown != null)
									{
										OnDown.Callback();
									}
									if(SoundDown != null)
										SoundDown.Play(true);
								}
							}

							if(Status == Normal)
							{
								Status = Highlight;
								if(OnOver != null)
									OnOver.Callback();
								if(SoundOver != null)
									SoundOver.Play(true);
							}
						}
						++pointerId;
					}
				}
				if(offAll)
				{
					if(Status != Normal)
					{
						if(OnOut != null)
							OnOut.Callback();
						if(SoundOut != null)
							SoundOut.Play(true);
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
				SetFrame(Normal);
			else
				SetFrame(Status);
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
			base.ResetHelpers();
			if(Label != null)
			{
				Label.Width = Width;
				Label.CalcFrame();
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
			if(SoundOver != null)
				SoundOver = FlxG.loadSound(SoundOver, SoundOverVolume);
			if(SoundOut != null)
				SoundOut = FlxG.loadSound(SoundOut, SoundOutVolume);
			if(SoundDown != null)
				SoundDown = FlxG.loadSound(SoundDown, SoundDownVolume);
			if(SoundUp != null)
				SoundUp = FlxG.loadSound(SoundUp, SoundUpVolume);
		}

		/**
		 * Internal function for handling the actual callback call (for UI thread
		 * dependent calls like <code>FlxU.openURL()</code>).
		 */
		protected void OnMouseUp(MouseEvent e)
		{
			if(!Exists || !Visible || !Active || (Status != Pressed))
				return;
			if(OnUp != null)
				OnUp.Callback();
			if(SoundUp != null)
				SoundUp.Play(true);
		}
	}
}