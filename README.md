#SmallWorld of POO

Computer Engineering School Project - INSA Rennes - 2013

Welcome to the ruthless world of SWOP! SWOP is a turn-based strategy game, playable alone
or with another player, on a single computer (hot seat) or through network. Find your opponent (be it human or AI),
choose your faction and fight for victory!

Checkout our User Manual (in french) to have a glimpse of what the game looks like.
The code consists mainly in C# and WPF, with a touch of C++ for map generation and tile suggestions, and was written with Visual Studio 2012.

##Features overview

* Single or two players
* Local or Network play
* Tactics based gameplay
* Game personalisation : player names, faction selection, map selection
* Ambient music
* Flexible UI
* Microsoft PixelSense version (pre-Alpha)

##Quick start guide

Here are the main concepts before getting into the fray :
* It's a turn based game, only one player can play at a time and then pass to the other player by clicking the "End my turn !" button.
* The game stops when one of the players has no unit left, or if time is up.
* To select a unit, left click it. If several units are on the same tile, keep clicking to cycle through them.
* To move or attack with an idle unit during your turn, right-click on a green or red highlighted tile. Flashing tiles are automatically recommended by the game.
* Each tile rewards the occupying player with a varying number of points. Mind you, it's the number of occupied tiles which matters, not the number of occupants.

###Factions

<dl>
<dt>Vikings</dt>
<dd><ul>
<li>Able to move on water, but get no point doing so</li>
<li>Get 1 more point on tiles adjacent to Water</li>
<li>Get no point on Desert tiles</li>
</ul></dd>

<dt>Gauls</dt>
<dd><ul>
<li>Move twice as fast on Fields</li>
<li>Get 1 more point on Fields</li>
<li>Get no point on Mountain tiles</li>
</ul></dd>

<dt>Dwarves</dt>
<dd><ul>
<li>Can move from a mountain to any other free Mountain</li>
<li>Get 1 more point on Forests</li>
<li>Get no point on Field tiles</li>
</ul></dd>
</dl>

###Maps

<dl>
<dt>Demo</dt>
<dd>
5*5 tiles, 5 turns, 4 units per faction
Here to give you a very quick overview of the game.
</dd>
<dt>Small</dt>
<dd>
10*10 tiles, 20 turns, 6 units per faction
</dd>
<dt>Classic</dt>
<dd>
15*15 tiles, 30 turns, 8 units per faction
</dd></dl>

Maps are generated randomly for each game.

##Launching a new game
### Hot Seat
Once the app is launched, click Start Game.
A new menu then appears, asking you to pick a name and a faction for each player. On this menu, you can also pick the map size.
When both players are ready, click Start to get into the fray.

### Network
The first player needs to create the network game by clicking on Create Server. He then can fill his info just like on a Local game. Once he's ready, he can click on Launch server and wait.
The other player can now click Join Game, fill his info and click on Join.
Then the server can start by clicking on Start Game.

NB : Due to a certain lack of time to fully flesh out this feature, some parts are still missing (entering IP Adress, some conflicts when players have the same faction...). But it works as good as the Local game feature.

##Play on Microsoft PixelSense table
Just for fun, we started another version of SWOP, named SWAG, targeting Microsoft Pixelsense tactile tables.
As is, it is only a very early pre-Alpha version, but it is working on Input Simulator.
The game has been modified to have 4 players around the table, each having his own info panel, which he can move around the surface.
Unlike SWOP, right clicking is obviously not supported, so you only need to touch a unit to select it, to move or attack. To unselect, just touch any unreachable tile (in grey).

##Acknowledgments

This game was made as a project for INSA Rennes's OOP course, by Damien Le Guen (Dayko) and Gwenegan Hudin (Gweylorth).

Royalte-free musics from Incompetech : http://incompetech.com/

Title screen background image by Fel-X : http://fel-x.deviantart.com/

Icons by Game Icons : http://game-icons.net/

Sprites by Battle for Wesnoth team : https://wesnoth.org/

Artworks by Dayko

[![Bitdeli Badge](https://d2weczhvl823v0.cloudfront.net/Gweylorth/swop/trend.png)](https://bitdeli.com/free "Bitdeli Badge")
