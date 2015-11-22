using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using MonoFlixel;

namespace MonoFlixel
{
    /// <summary>
    /// This is a simple particle class that extends the default behavior
    /// of <code>FlxSprite</code> to have slightly more specialized behavior
    /// common to many game scenarios.  You can override and extend this class
    /// just like you would <code>FlxSprite</code>. While <code>FlxEmitter</code>
    /// used to work with just any old sprite, it now requires a
    /// <code>FlxParticle</code> based class.
    /// </summary>
	public class FlxParticle : FlxSprite
	{
        private Texture2D _texture;

        /// <summary>
        /// How long this particle lives before it disappears.
        /// NOTE: this is a maximum, not a minimum; the object
        /// could get recycled before its lifespan is up.
        /// </summary>
		public float Lifespan { get; set; }

        /// <summary>
        /// Determines how quickly the particles come to rest on the ground.
        /// Only used if the particle has gravity-like acceleration applied.
        /// @default 500
        /// </summary>
		public float Friction { get; set; }

        /// <summary>
        /// Instantiate a new particle.  Like <code>FlxSprite</code>, all meaningful creation
        /// happens during <code>loadGraphic()</code> or <code>makeGraphic()</code> or whatever.
        /// </summary>
		public FlxParticle() : base()
		{
			Lifespan = 0;
			Friction = 500;
		}

        /// <summary>
        /// The particle's main update logic.  Basically it checks to see if it should
        /// be dead yet, and then has some special bounce behavior if there is some gravity on it.
        /// </summary>
		override public void update()
		{
			// lifespan behavior
			if (Lifespan <= 0)
			{
                return;			    
			}

			Lifespan = Lifespan - FlxG.elapsed;
			if (Lifespan <= 0)
			{
                kill();			    
			}

			// simpler bounce/spin behavior for now
			if(Convert.ToBoolean(Touching))
			{
				if(AngularVelocity != 0)
					AngularVelocity = -AngularVelocity;
			}

            //special behavior for particles with gravity
			if(Acceleration.Y > 0) 
			{
				if (Convert.ToBoolean(Touching) & Convert.ToBoolean(Floor))
				{
				    Drag.X = Friction;

				    if (!(Convert.ToBoolean(WasTouching) & Convert.ToBoolean(Floor)))
				    {
				        if (Velocity.Y < -Elasticity*10)
				        {
				            if (AngularVelocity != 0)
				                AngularVelocity *= -Elasticity;
				        }
				        else
				        {
				            Velocity.Y = 0;
				            AngularVelocity = 0;
				        }
				    }
				}
				else
				{
                    Drag.X = 0;				    
				}
			}
		}

        /// <summary>
        /// Triggered whenever this object is launched by a <code>FlxEmitter</code>.
        /// You can override this to add custom behavior like a sound or AI or something.
        /// </summary>
		public void onEmit()
		{

		}
	}
}
