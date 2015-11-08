using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
//using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using MonoFlixel;

namespace MonoFlixel
{
    public class FlxTileblock : FlxSprite
    {
        /// <summary>
        /// Creates a new rectangular FlxSprite object with specified position and size.
        /// Great for walls and floors.
        /// </summary>
        /// <param name="X">X position of the block</param>
        /// <param name="Y">Y position of the block</param>
        /// <param name="Width">Width of the block</param>
        /// <param name="Height">Height of the block</param>
        public FlxTileblock(float X, float Y, float Width, float Height)
            : base(X, Y)
        {
            makeGraphic((uint)Width, (uint)Height, FlxColor.WHITE);
            Active = false;
            Immovable = true;
			Moves = false;
        }

		/**
		 * Fills the block with a randomly arranged selection of graphics from the image provided.
		 * 
		 * @param	TileGraphic 	The graphic class that contains the tiles that should fill this block.
		 * @param	TileWidth		The width of a single tile in the graphic.
		 * @param	TileHeight		The height of a single tile in the graphic.
		 * @param	Empties			The number of "empty" tiles to add to the auto-fill algorithm (e.g. 8 tiles + 4 empties = 1/3 of block will be open holes).
		 */
		public FlxTileblock loadTiles(string TileGraphic,uint TileWidth=0,uint TileHeight=0, uint Empties=0)
		{
			if(TileGraphic == null)
				return this;

			//First create a tile brush
			FlxSprite sprite = new FlxSprite();
			sprite.loadGraphic(FlxS.ContentManager.Load<Texture2D>(TileGraphic), true, false, TileWidth, TileHeight);
			uint spriteWidth = (uint)sprite.Width;
			uint spriteHeight = (uint)sprite.Height;
			uint total = sprite.Frames + Empties;

			//Then prep the "canvas" as it were (just doublechecking that the size is on tile boundaries)
			bool regen = false;
			if(Width % sprite.Width != 0)
			{
				Width = (uint)(Width/spriteWidth+1)*spriteWidth;
				regen = true;
			}
			if(Height % sprite.Height != 0)
			{
				Height = (uint)(Height/spriteHeight+1)*spriteHeight;
				regen = true;
			}
			if(regen)
				makeGraphic((uint)Width,(uint)Height,Color.Black,true);
			else
				this.fill(Color.Black);

			//Stamp random tiles onto the canvas
			uint row = 0;
			uint column;
			int destinationX;
			int destinationY = 0;
			uint widthInTiles = (uint)Width/spriteWidth;
			uint heightInTiles = (uint)Height/spriteHeight;
			while(row < heightInTiles)
			{
				destinationX = 0;
				column = 0;
				while(column < widthInTiles)
				{
					if(FlxG.random()*total > Empties)
					{
						sprite.randomFrame();
						sprite.drawFrame();
						stamp(sprite, destinationX, destinationY);
					}
					destinationX += (int)spriteWidth;
					column++;
				}
				destinationY += (int)spriteHeight;
				row++;
			}

			return this;
		}
    }
}
