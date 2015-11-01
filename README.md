# MonoFlixel

Cross-platform game engine written in C# that aims to seamlessly join the power of MonoGame and Flixel.


## Background

### MonoGame

MonoGame is an open source implementation of the Microsoft XNA 4.x Framework.

### Flixel

Flixel is an open source game-making library that is completely free for personal or commercial use. Written entirely in Actionscript 3, and designed to be used with free development tools, Flixel is easy to learn, extend and customize.

### Various

This project is based on multiple different Flixel projects: Flixel, flxSharp by jlorek, FliXNA by percevic, flixel-gdx (JAVA).


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
- Add MonoFlixel to your project's references
- Copy the defult asset files from MonoFlixel project Content directory to your project Content directory.
- All Done, Start creating your game.

## Licence

MIT
