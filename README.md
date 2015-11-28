# MonoFlixel

Cross-platform game engine written in C# that aims to seamlessly join the power of MonoGame and Flixel.

## Project abandoned

This project was created because I wanted to use Flixel to build 2D games for consoles. As MonoGame had console export support it seemed to be a good way to go. Halfway through the project I found out that HaxeFlixel community is already working hard to support the consoles and the work seemed really promosing. I decided to end this project and choose HaxeFlixel as it has a good community and more supported platforms. Never the less this was a good learning experience to the depths of MonoGame, C# and Flixel.


## Alternative Flixel C# ports

If you are still looking for Flixel C# port then there are few projects that were also used as inpiration for this engine.

- [XNAFlixel](https://github.com/initials/XNAFlixel)
- [X-flixel](https://github.com/StAidan/X-flixel)
- [flxSharp](https://github.com/jlorek/flxSharp)


## Background

### MonoGame

MonoGame is an open source implementation of the Microsoft XNA 4.x Framework.

### Flixel

Flixel is an open source game-making library that is completely free for personal or commercial use. Written entirely in Actionscript 3, and designed to be used with free development tools, Flixel is easy to learn, extend and customize.

### Various

This project is based on multiple different Flixel projects: Flixel, flxSharp by jlorek, FliXNA by percevic, flixel-gdx (JAVA).

## Examples

There is a separate examples repo at https://github.com/IndrekV/MonoFlixel.Examples


## Setup (based on mac)

### Setting up MonoGame

- Install mono - [Download mono](http://www.mono-project.com/docs/getting-started/install/mac/)
- Clone MonoGame repo - `git clone git@github.com:mono/MonoGame.git`
- Clone MonoGame Dependencies repo to Dependencies directory - `cd ThirdParty && git clone git@github.com:MonoGame/MonoGame.Dependencies.git`
- Run `mono Protobuild.exe` in MonoGame root directory
- Install Xamarin IDE
- Install the MonoGame addon inside Xamarin
- Open MonoGame project in Xamarin and select `Build All`

### Setting up MonoFlixel

- Clone this repo
- Add MonoFlixel project to your Solution
- Add MonoFlixel project to your project's references
- Copy the defult asset files from MonoFlixel project Content directory to your project Content directory.
- Add using clauses where needed `using MonoFlixel;`
- All Done, Start creating your game.

## Licence

MIT
