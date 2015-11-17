using MonoFlixel;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace MonoFlixel
{
	public class FlxText : FlxSprite
	{
	/*
	 * Text rectangle size
	 */
		private Rectangle bounds;

	/*
	 * Keeps track of the center of the text rect
	 */
		private Vector2 origin = Vector2.Zero;
	
	/*
	 * Font based rect size
	 */
		private Vector2 _fontmeasure = Vector2.Zero;
	
	/*
	 * Scaling size
	 */
		private float _scale = 1f;

	/*
	 * Text width based on Font
	 */
		public int textWidth {
			get { return (int)_fontmeasure.X; }
		}
	/*
	 * Text height based on Font
	 */
		public int textHeight {
			get { return (int)_fontmeasure.Y; }
		}

	/**
	* Internal tracker for the alignment of the text.
	*/
		protected String _alignment = "CENTER";
	/**
	 * The default vertex shader that will be used.
	 */
		//private final String VERTEX = "org/flixel/data/shaders/vertex.glsl";
	/**
	 * The default fragment shader that will be used for distance field.
	 */
		//public final String DISTANCE_FIELD_FRAGMENT = "org/flixel/data/shaders/distance_field.glsl";
	/*
	 * Internal reference to a libgdx <code>BitmapFontCache</code> object.
	 */
		protected SpriteBatch _textField;
	/**
   * Loaded font
   */
		protected SpriteFont LoadedFont = FlxG.defaultFont;
	/**
	 * Internal tracker for the text shadow color, default is clear/transparent.
	 */
		protected Color _shadow = Color.Transparent;

	/**
	 * Internal tracker for the alignment of the text.
	 */
		//	protected HAlignment _alignment;
		/**
	 * Internal reference to the text to be drawn.
	 */
		public string text { get; set; }

	/**
	 * Internal helper for rotation.
	 */
		//	protected Matrix4 _matrix;
	/**
	 * Internal reference to the font.
	 */
		protected String _font;
	/**
	 * Internal tracker for the size of the text.
	 */
		protected int _size;

	/**
	 * Internal tracker for the x-position of the shadow, default is 1.
	 */
		protected float _shadowX;
	/**
	 * Internal tracker for the y-position of the shadow, default is 1.
	 */
		protected float _shadowY;

	/**
	 * Parameters that is used for generating the <code>BitmapFont</code>.
	 */
		//private BitmapFontParameter _bitmapFontParameter;
	/**
	 * The shader program that is used for distance field.
	 */
		//private ShaderProgram _distanceFieldShader;
	/**
	 * Internal tracker whether the distance field is enabled.
	 */
		private Boolean _distanceFieldEnabled;
	/**
	 * The padding that is set for generating the bitmap font.
	 */
		private int _padding;
	/**
	 * The smoothness of the font.
	 */
		private float _smoothness;

	/**
	 * Creates a new <code>FlxText</code> object at the specified position.
	 * 
	 * @param X The X position of the text.
	 * @param Y The Y position of the text.
	 * @param Width The width of the text object (height is determined
	 *        automatically).
	 * @param Text The actual text you would like to display initially.
	 * @param EmbeddedFont Whether this text field uses embedded fonts or not.
	 */
		public FlxText (float x, float y, int width, String Text = null, Boolean EmbeddedFont = true) : base (x, y)
		{
			if (Text == null)
				Text = "";

			Width = FrameWidth = width;
			text = Text;
			AllowCollisions = None;
			//_bitmapFontParameter = new BitmapFontParameter();
			//_bitmapFontParameter.flip = true;
			setFormat (null, 8, Color.White, "left", Color.Black, 1f, 1f);
			X = x;
			Y = y;
		}

	/**
	 * Clean up memory.
	 */

		public override void destroy ()
		{
			_textField.Dispose ();
			_textField = null;
			text = null;
			//_bitmapFontParameter = null;
			//_distanceFieldShader = null;
			base.destroy ();
		}


	/**
	 * You can use this if you have a lot of text parameters to set instead of
	 * the individual properties.
	 * 
	 * @param Font The name of the font face for the text display.
	 * @param Size The size of the font (in pixels essentially).
	 * @param Color The color of the text in 0xRRGGBB format.
	 * @param Alignment A string representing the desired alignment
	 *        ("left,"right" or "center").
	 * @param ShadowColor An int representing the desired text shadow color
	 *        0xAARRGGBB format.
	 * @param ShadowX The x-position of the shadow, default is 1.
	 * @param ShadowY The y-position of the shadow, default is 1.
	 * 
	 * @return This FlxText instance (nice for chaining stuff together, if
	 *         you're into that).
	 */
		public FlxText setFormat (String Font, float size, Color color, String Alignment, Color ShadowColor, float ShadowX = 1f, float ShadowY = 1f)
		{
			if (Font == null)
				Font = _font;

			if (Font != null && (!Font.Equals (_font) || size != _size)) {
				try {
					//_textField = new SpriteBatch(FlxG.loadFont(Font, FlxU.round(Size), _bitmapFontParameter));
					//_textField = new SpriteBatch(GraphicsDevice);

				} catch (Exception e) {
					Console.WriteLine (e.Message);
					FlxG.log(e.Message);
					//_textField = new SpriteBatch(FlxG.loadFont("org/flixel/data/font/nokiafc.fnt", 22, _bitmapFontParameter));
				}

				_font = Font;
				_size = (int)FlxU.floor (size);
				LoadedFont = FlxS.ContentManager.Load<SpriteFont> (_font);
			}

			Color = color;
			if (Alignment != null)
				_alignment = (Alignment.ToUpper ());
			_shadow = ShadowColor;
			_shadowX = ShadowX;
			_shadowY = ShadowY;

			calcFrame ();

			return this;
		}

	/**
	 * You can use this if you have a lot of text parameters to set instead of
	 * the individual properties.
	 * 
	 * @param Font The name of the font face for the text display.
	 * @param Size The size of the font (in pixels essentially).
	 * @param Color The color of the text in 0xRRGGBB format.
	 * @param Alignment A string representing the desired alignment
	 *        ("left,"right" or "center").
	 * 
	 * @return This FlxText instance (nice for chaining stuff together, if
	 *         you're into that).
	 */
		public FlxText setFormat (String Font, float size, Color color, String Alignment) {

			return setFormat (Font, size, color, Alignment, Color.Transparent);
		}

	/**
	 * The text being displayed.
	 */
		public String getText ()
		{
			return text;
		}

	/**
	 * The text being displayed.
	 */
		public void setText (string Text)
		{
			text = Text;
			calcFrame ();
		}

	/**
	 * The size of the text being displayed.
	 */
		public float getSize ()
		{
			return _size;
		}

	/**
	 * The size of the text being displayed.
	 */
		public void setSize (float Size)
		{
			if (Size == _size)
				return;

			setFormat (_font, Size, _color, getAlignment (), _shadow, _shadowX, _shadowY);
		}

	/**
	 * The font used for this text.
	 */
		public String getFont ()
		{
			return _font;
		}

	/**
	 * The font used for this text.
	 */
		public void setFont (String Font, float Size)
		{
			if (Font == null || Font.Equals (_font))
				return;

			setFormat (Font, Size, _color, getAlignment (), _shadow, _shadowX, _shadowY);
		}

	/**
	 * The font used for this text.
	 */
		public void setFont (String Font)
		{
			setFont (Font, _size);
		}

	/**
	 * The alignment of the font ("left", "right", or "center").
	 */
		public String getAlignment ()
		{
			return _alignment.ToLower();
		}

	/**
	 * The alignment of the font ("left", "right", or "center").
	 */
		public void setAlignment (String Alignment)
		{
			if (Alignment == null)
				return;
			_alignment = Alignment.ToUpper ();
			calcFrame ();
		}

		/**
	 * The color of the text shadow in 0xAARRGGBB hex format.
	 */
		public Color getShadow ()
		{
			return _shadow;
		}

		/**
	 * The color of the text shadow in 0xAARRGGBB hex format.
	 */
		public void setShadow (Color color)
		{
			_shadow = color;
		}

		/**
	 * The position of the text shadow.
	 * 
	 * @param ShadowX The x-position
	 * @param ShadowY The y-position
	 */
		public void setShadow (float ShadowX, float ShadowY)
		{
			_shadowX = ShadowX;
			_shadowY = ShadowY;
		}

		/**
	 * The x-position of the text shadow.
	 */
		public void setShadowX (float ShadowX)
		{
			_shadowX = ShadowX;
		}

		/**
	 * The y-position of the text shadow.
	 */
		public void setShadowY (float ShadowY)
		{
			_shadowY = ShadowY;
		}

		/**
	 * Sets whether the font should render as distance field.
	 * 
	 * @param Enabled Whether the font should render as distance field or not.
	 * @param Padding The padding that is set to generate the bitmap font.
	 * @param Smoothness The smoothness between 0 and 1.
	 * @param Name The name of the shader.
	 * @param Fragment A custom fragment that will be used for creating the
	 *        shader.
	 * @return The shader program that will be used
	 */
		/*
		public ShaderProgram setDistanceField(Boolean Enabled, int Padding, float Smoothness, String Name, String Fragment)
		{
			_distanceFieldEnabled = Enabled;
			if(!Enabled)
				return null;

			_padding = -Padding;
			_smoothness = Smoothness;

			_bitmapFontParameter.genMipMaps = true;
			_bitmapFontParameter.minFilter = TextureFilter.MipMapLinearNearest;
			_bitmapFontParameter.magFilter = TextureFilter.Linear;

			return _distanceFieldShader = FlxG.loadShader(Name, VERTEX, Fragment);
		}
		*/
		/**
	 * Sets whether the font should render as distance field.
	 * 
	 * @param Enabled Whether the font should render as distance field or not.
	 * @param Padding The padding that is set to generate the bitmap font.
	 * @param Smoothness The smoothness between 0 and 1.
	 * @param Name The name of the shader.
	 * @return The shader program that will be used
	 */
		/*
		public ShaderProgram setDistanceField(boolean Enabled, int Padding, float Smoothness, String Name)
		{
			return setDistanceField(Enabled, Padding, Smoothness, Name, DISTANCE_FIELD_FRAGMENT);
		}
		*/
		/**
	 * Sets whether the font should render as distance field.
	 * 
	 * @param Enabled Whether the font should render as distance field or not.
	 * @param Padding The padding that is set to generate the bitmap font.
	 * @param Smoothness The smoothness between 0 and 1.
	 * @return The shader program that will be used
	 */
		/*
		public ShaderProgram setDistanceField(boolean Enabled, int Padding, float Smoothness)
		{
			return setDistanceField(Enabled, Padding, Smoothness, _font, DISTANCE_FIELD_FRAGMENT);
		}
		*/
		/**
	 * Sets the distance field shader in the batch.
	 */
		private void drawDistanceField ()
		{
			//FlxG.Batch. flush();
			/*
			_distanceFieldShader.begin();
			float delta = 0.5f * MathUtils.clamp(_smoothness / scale.x, 0, 1);
			_distanceFieldShader.setUniformf("u_lower", 0.5f - delta);
			_distanceFieldShader.setUniformf("u_upper", 0.5f + delta);
			_distanceFieldShader.end();
			FlxG.batch.setShader(_distanceFieldShader);
			*/

			_tagPoint.Y += Scale.X * _padding;
			//_textField.setPosition(_tagPoint.X, _tagPoint.Y);
		}

		protected override void calcFrame ()
		{
			try {
				_fontmeasure = LoadedFont.MeasureString (text) * _scale;
				origin = new Vector2 (_fontmeasure.X / 2, _fontmeasure.Y / 2);

				bounds = new Rectangle ((int)X, (int)Y, (int)textWidth, (int)textHeight);
			} catch {
				_fontmeasure = Vector2.Zero;
			}
			//TextBounds bounds = _textField.setWrappedText(_text, 2, 3, Width, _alignment);
			// bounds.height is shorter than it should be.
			// After some trial and error, adding seven seems to make it about right
			// in most cases.
			//Height = FrameHeight = (int) FlxU.ceil(bounds.height + 7);
		}

		public override void draw ()
		{
			if (_flickerTimer != 0) {
				_flicker = !_flicker;
				if (_flicker)
					return;
			}

			FlxCamera camera = FlxG.camera;//._activeCamera;

			if (Cameras == null)
				Cameras = FlxG.cameras;

			if (!Cameras.Contains (camera))
				return;

			if (!onScreen (camera))
				return;

			_tagPoint.X = X - (camera.Scroll.X * ScrollFactor.X) - Offset.X;
			_tagPoint.Y = Y - (camera.Scroll.Y * ScrollFactor.Y) - Offset.Y;
			_tagPoint.X += (_tagPoint.X > 0) ? 0.0000001f : -0.0000001f;
			_tagPoint.Y += (_tagPoint.Y > 0) ? 0.0000001f : -0.0000001f;

			// scaling
			if (Scale.X != 1f || Scale.Y != 1f) {
				//_textField.getFont().setScale(Scale.X, Scale.Y);
				calcFrame ();
			}

			// position
			//_textField.setPosition(_tagPoint.X, _tagPoint.Y);

			// rotation
			if (Angle != 0) {
				/*
				_matrix = FlxG.Batch.getTransformMatrix().cpy();

				Matrix4 rotationMatrix = FlxG.batch.getTransformMatrix();
				rotationMatrix.translate(_textField.getX() + (width / 2), _textField.getY() + (height / 2), 0);
				rotationMatrix.rotate(0, 0, 1, angle);
				rotationMatrix.translate(-(_textField.getX() + (width / 2)), -(_textField.getY() + (height / 2)), 0);

				FlxG.batch.setTransformMatrix(rotationMatrix);
				*/
			}
			/*
			// blending
			if(blend != null && currentBlend != blend)
			{
				int[] blendFunc = BlendMode.getOpenGLBlendMode(blend);
				FlxG.batch.setBlendFunction(blendFunc[0], blendFunc[1]);
			}
			else if(FlxG.batchShader == null || ignoreBatchShader)
			{
				// OpenGL ES 2.0 shader render
				renderShader();
				// OpenGL ES 2.0 blend mode render
				renderBlend();
			}
			*/

			// distance field
			if (_distanceFieldEnabled)
				drawDistanceField ();

			int tintColor;
			/*
			// Render shadow behind the text
			if(_shadow != 0)
			{
				// tinting
				tintColor = FlxU.multiplyColors(_shadow, camera.getColor());
				_textField.setColors(((tintColor >> 16) & 0xFF) * 0.00392f, ((tintColor >> 8) & 0xFF) * 0.00392f, (tintColor & 0xFF) * 0.00392f, ((_shadow >> 24) & 0xFF) * _alpha
					* 0.00392f);
				_textField.translate(_shadowX, _shadowY);
				_textField.draw(FlxG.batch);
				_textField.translate(-_shadowX, -_shadowY);
			}

			// tinting
			tintColor = FlxU.multiplyColors(_color, camera.getColor());
			_textField.setColors(((tintColor >> 16) & 0xFF) * 0.00392f, ((tintColor >> 8) & 0xFF) * 0.00392f, (tintColor & 0xFF) * 0.00392f, _alpha);

			_textField.draw(FlxG.batch);

			// turn off distance field
			if(_distanceFieldEnabled)
				FlxG.batch.setShader(null);

			// rotation
			if(Angle != 0)
				FlxG.batch.setTransformMatrix(_matrix);

			_VISIBLECOUNT++;

			if(FlxG.visualDebug && !ignoreDrawDebug)
				drawDebug(camera);
			*/

			Point point = bounds.Center;
			Vector2 pos = new Vector2 (point.X, point.Y);

			if (_alignment == "RIGHT")
				pos.X += Width - textWidth;

			if (_alignment == "CENTER")
				pos.X += ((Width - textWidth) / 2);
			pos.Y += ((Height - textHeight) / 2);

			// Render shadow behind the text
			if(_shadow != Color.Transparent)
			{
				FlxS.SpriteBatch.DrawString (LoadedFont, text, new Vector2(pos.X+_shadowX, pos.Y+_shadowY), _shadow, 0, origin, 1, SpriteEffects.None, 0);	
			}

			FlxS.SpriteBatch.DrawString (LoadedFont, text, pos, Color, 0, origin, 1, SpriteEffects.None, 0);
		}



	}
}
