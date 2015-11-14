using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

using Microsoft.Xna.Framework.Input.Touch;

namespace MonoFlixel
{
	/// <summary>
	/// This class helps contain and track the mouse pointer in your game.
	/// Automatically accounts for parallax scrolling, etc.
	/// </summary>
	public class FlxMouse
	{
		private string ImgDefaultCursor = "cursor";

		private MouseState _curMouse;
		private MouseState _lastMouse;
		private TouchLocationState _lastTouch;
		private TouchLocationState _curTouch;

		private EventHandler<FlxMouseEvent> _mouseEvent;
		private bool lastTouchEvent;

		public void addMouseListener(EventHandler<FlxMouseEvent> MouseEvent)
		{
			_mouseEvent += MouseEvent;
		}
		public void removeMouseListener(EventHandler<FlxMouseEvent> MouseEvent)
		{
			_mouseEvent -= MouseEvent;
		}

		public float x
		{
			#if XBOX360
			get { return 0; }
			#elif __IOS__
			get
			{
			//if (_curMouse.X >= FlxG._game.targetLeft)
			//{
			return (float)(((_touchL.Position.X - FlxG._game.targetLeft) / FlxG.scale) * FlxG.iPad) - FlxG.scroll.X;
			//}
			//else
			//{
			//    return 0;
			//}
			}
			#elif __ANDROID__
			get
			{
			//if (_curMouse.X >= FlxG._game.targetLeft)
			//{
			return (float)(((_touchL.Position.X - FlxG._game.targetLeft) / FlxG.scale) * FlxG.iPad) - FlxG.scroll.X;
			//}
			//else
			//{
			//    return 0;
			//}
			}
			#else
			get
			{
				return (float)((_curMouse.X));// - FlxG._game.targetLeft) / FlxG.Scale) - FlxG.Scroll.X;
			}
			#endif
		}
		public float y
		{
			#if XBOX360
			get { return 0; }
			#elif __IOS__
			get { return (((float)_touchL.Position.Y / FlxG.scale) * FlxG.iPad) - FlxG.scroll.Y; }
			#elif __ANDROID__
			get { return (((float)_touchL.Position.Y / FlxG.scale) * FlxG.iPad) - FlxG.scroll.Y; }
			#else
			get { return ((float)_curMouse.Y);}// / FlxG.Scale) - FlxG.Scroll.Y; }
			#endif
		}

		/// <summary>
		/// Current "delta" value of mouse wheel.  If the wheel was just scrolled up, it will have a positive value.  If it was just scrolled down, it will have a negative value.  If it wasn't just scroll this frame, it will be 0.
		/// </summary>
		public int wheel;

		/// <summary>
		/// Current X position of the mouse pointer on the screen.
		/// </summary>
		public int screenX;
		/// <summary>
		/// Current Y position of the mouse pointer on the screen.
		/// </summary>
		public int screenY;
		/// <summary>
		/// Graphical representation of the mouse pointer.
		/// </summary>
		public FlxSprite cursor;


		/// <summary>
		/// Allows the mouse cursor to disappear after being inactive
		/// for the amount of time specified
		/// </summary>
		public float hideAfterInactiveTime = 0.0f;

		/// <summary>
		/// counter for hiding time.
		/// </summary>
		private float timeElapsed;

		/// <summary>
		/// holds the last position of the cursor.
		/// </summary>
		private Vector2 lastPosition;

		private TouchCollection touchCollection;
		private TouchLocation _touchL;


		/// <summary>
		/// Constructor.
		/// </summary>
		public FlxMouse()
		{
			screenX = 0;
			screenY = 0;
			cursor = new FlxSprite();
			cursor.Visible = false;
			lastTouchEvent = false;
			_lastTouch = TouchLocationState.Released;
		}

		public void show()
		{
			show("cursor", 0, 0);
		}

		/// <summary>
		/// Either show an existing cursor or load a new one.
		/// </summary>
		/// <param name="Graphic">The image you want to use for the cursor.</param>
		public void show(string Graphic)
		{
			show(Graphic, 0, 0);
		}
		/// <summary>
		/// Either show an existing cursor or load a new one.
		/// </summary>
		/// <param name="Graphic">The image you want to use for the cursor.</param>
		/// <param name="XOffset">The number of pixels between the mouse's screen position and the graphic's top left corner.</param>
		/// <param name="YOffset">The number of pixels between the mouse's screen position and the graphic's top left corner. </param>
		public void show(string Graphic,int XOffset,int YOffset)
		{
			if(Graphic != null)
				load(Graphic,XOffset,YOffset);
			else if(cursor != null)
				cursor.Visible = true;
			else
				load(null);
		}

		/// <summary>
		/// Hides the mouse cursor
		/// </summary>
		public void hide()
		{
			if(cursor != null)
			{
				cursor.Visible = false;
			}
		}

		/// <summary>
		/// Load a new mouse cursor graphic
		/// </summary>
		/// <param name="Graphic">The image you want to use for the cursor.</param>
		public void load(string Graphic)
		{
			load(Graphic, 0, 0);
		}
		/// <summary>
		/// Load a new mouse cursor graphic
		/// </summary>
		/// <param name="Graphic">The image you want to use for the cursor.</param>
		/// <param name="XOffset">The number of pixels between the mouse's screen position and the graphic's top left corner.</param>
		/// <param name="YOffset">The number of pixels between the mouse's screen position and the graphic's top left corner. </param>
		public void load(string Graphic, int XOffset, int YOffset)
		{
			if(Graphic == null)
				Graphic = ImgDefaultCursor;

			cursor = new FlxSprite(screenX,screenY,Graphic);
			cursor.Solid = false;
			cursor.Offset.X = XOffset;
			cursor.Offset.Y = YOffset;
		}

		/// <summary>
		/// Unload the current cursor graphic.  If the current cursor is visible,
		/// then the default system cursor is loaded up to replace the old one.
		/// </summary>
		public void unload()
		{
			if(cursor != null)
			{
				if(cursor.Visible)
					load(null);
				else
					cursor = null;
			}
		}

		/// <summary>
		/// Called by the internal game loop to update the mouse pointer's position in the game world.
		/// Also updates the just pressed/just released flags.
		/// </summary>
		public void update()
		{

			_lastMouse = _curMouse;
			_curMouse = Mouse.GetState(); 

			#if __IOS__
			_lastTouch = _curTouch;
			touchCollection = TouchPanel.GetState();
			foreach (TouchLocation tl in touchCollection)  
			{  
			_curTouch = tl.State;
			_touchL = tl;


			screenX = (int)tl.Position.X;
			screenY = (int)tl.Position.Y;
			} 

			#elif __ANDROID__
			_lastTouch = _curTouch;
			touchCollection = TouchPanel.GetState();
			foreach (TouchLocation tl in touchCollection)  
			{  
			_curTouch = tl.State;
			_touchL = tl;


			screenX = (int)tl.Position.X;
			screenY = (int)tl.Position.Y;
			} 
			#else
			cursor.X = x;
			cursor.Y = y;

			screenX = _curMouse.X;
			screenY = _curMouse.Y;
			#endif

			if (_mouseEvent != null)
			{
				if (justPressed())
				{
					_mouseEvent(this, new FlxMouseEvent(MouseEventType.MouseDown));
				}
				else if (justReleased())
				{
					_mouseEvent(this, new FlxMouseEvent(MouseEventType.MouseUp));
				}
			}



			if (hideAfterInactiveTime > 0.0f)
			{
				Vector2 thisPosition = new Vector2(x, y);

				if (thisPosition == lastPosition)
				{
					timeElapsed += FlxG.elapsed;

					if (timeElapsed > hideAfterInactiveTime)
					{
						cursor.Visible = false;
					}
				}
				else
				{
					cursor.Visible = true;
				}
			}

			lastPosition = new Vector2(x, y);
		}

		/// <summary>
		/// Reset.
		/// </summary>
		public void reset()
		{
			_curMouse = _lastMouse;
			//also get rid of all current event listeners
			_mouseEvent = null;
		}

		/// <summary>
		/// Is _either_ mouse button pressed down?
		/// </summary>
		/// <returns>Returns a value whether it was just pressed or not.</returns>
		public bool pressed()
		{


			return (_curMouse.LeftButton == ButtonState.Pressed ||
				_curMouse.RightButton == ButtonState.Pressed || touchCollection.Count > 0

			);
		}

		/// <summary>
		/// Was _either_ mouse button just pressed down?
		/// </summary>
		/// <returns>Return true only when the mouse was just pressed.</returns>
		public bool justPressed()
		{

			//Console.WriteLine ("XY {0} {1}", x, y);


			return ((_curMouse.LeftButton == ButtonState.Pressed && _lastMouse.LeftButton == ButtonState.Released) ||
				(_curMouse.RightButton == ButtonState.Pressed && _lastMouse.RightButton == ButtonState.Released)  ||
				(_curTouch == TouchLocationState.Pressed && _lastTouch == TouchLocationState.Released) 
			);
		}
		/// <summary>
		/// Was _either_ mouse button just released?
		/// </summary>
		/// <returns>Return true only when the mouse was just released.</returns>
		public bool justReleased()
		{
			return ((_curMouse.LeftButton == ButtonState.Released && _lastMouse.LeftButton == ButtonState.Pressed) ||
				(_curMouse.RightButton == ButtonState.Released && _lastMouse.RightButton == ButtonState.Pressed) ||
				(_curTouch == TouchLocationState.Released && _curTouch == TouchLocationState.Pressed));
		}


		/// <summary>
		/// Is right mouse button pressed down?
		/// </summary>
		/// <returns>Returns a value whether it was just pressed or not.</returns>
		public bool pressedRightButton()
		{
			return (_curMouse.RightButton == ButtonState.Pressed);
		}

		/// <summary>
		/// Was right mouse button just pressed down?
		/// </summary>
		/// <returns>Return true only when the mouse was just pressed.</returns>
		public bool justPressedRightButton()
		{
			return (_curMouse.RightButton == ButtonState.Pressed && _lastMouse.RightButton == ButtonState.Released);
		}
		/// <summary>
		/// Was right mouse button just released?
		/// </summary>
		/// <returns>Return true only when the mouse was just released.</returns>
		public bool justReleasedRightButton()
		{
			return (_curMouse.RightButton == ButtonState.Released && _lastMouse.RightButton == ButtonState.Pressed);
		}



		/// <summary>
		/// Is left mouse button pressed down?
		/// </summary>
		/// <returns>Returns a value whether it was just pressed or not.</returns>
		public bool pressedLeftButton()
		{
			return (_curMouse.LeftButton == ButtonState.Pressed);
		}

		/// <summary>
		/// Was left mouse button just pressed down?
		/// </summary>
		/// <returns>Return true only when the mouse was just pressed.</returns>
		public bool justPressedLeftButton()
		{
			return (_curMouse.LeftButton == ButtonState.Pressed && _lastMouse.LeftButton == ButtonState.Released);
		}
		/// <summary>
		/// Was left mouse button just released?
		/// </summary>
		/// <returns>Return true only when the mouse was just released.</returns>
		public bool justReleasedLeftButton()
		{
			return (_curMouse.LeftButton == ButtonState.Released && _lastMouse.LeftButton == ButtonState.Pressed) ;
		}
	}

}
