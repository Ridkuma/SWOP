********************************************************************************************************************************
********************************************************************************************************************************
********************************************************************************************************************************

	   ,-,--.         ___    ,---.                                          ,-.-.    _,.---._                                       
	 ,-.'-  _\ .-._ .'=.'\ .--.'  \       _.-.      _.-.           ,-..-.-./  \==\ ,-.' , -  `.   .-.,.---.    _.-.     _,..---._   
	/==/_ ,_.'/==/ \|==|  |\==\-/\ \    .-,.'|    .-,.'|           |, \=/\=|- |==|/==/_,  ,  - \ /==/  `   \ .-,.'|   /==/,   -  \  
	\==\  \   |==|,|  / - |/==/-|_\ |  |==|, |   |==|, |           |- |/ |/ , /==/==|   .=.     |==|-, .=., |==|, |   |==|   _   _\ 
	 \==\ -\  |==|  \/  , |\==\,   - \ |==|- |   |==|- |            \, ,     _|==|==|_ : ;=:  - |==|   '='  /==|- |   |==|  .=.   | 
	 _\==\ ,\ |==|- ,   _ |/==/ -   ,| |==|, |   |==|, |            | -  -  , |==|==| , '='     |==|- ,   .'|==|, |   |==|,|   | -| 
	/==/\/ _ ||==| _ /\   /==/-  /\ - \|==|- `-._|==|- `-._          \  ,  - /==/ \==\ -    ,_ /|==|_  . ,'.|==|- `-._|==|  '='   / 
	\==\ - , //==/  / / , |==\ _.\=\.-'/==/ - , ,/==/ - , ,/         |-  /\ /==/   '.='. -   .' /==/  /\ ,  )==/ - , ,/==|-,   _`/  
	 `--`---' `--`./  `--` `--`        `--`-----'`--`-----'          `--`  `--`      `--`--''   `--`-`--`--'`--`-----'`-.`.____.'   
						   _,.---._        _,---.                   _ __      _,.---._       _,.---._                               
						 ,-.' , -  `.   .-`.' ,  \               .-`.' ,`.  ,-.' , -  `.   ,-.' , -  `.                             
						/==/_,  ,  - \ /==/_  _.-'              /==/, -   \/==/_,  ,  - \ /==/_,  ,  - \                            
					   |==|   .=.     /==/-  '..-.             |==| _ .=. |==|   .=.     |==|   .=.     |                           
					   |==|_ : ;=:  - |==|_ ,    /             |==| , '=',|==|_ : ;=:  - |==|_ : ;=:  - |                           
					   |==| , '='     |==|   .--'              |==|-  '..'|==| , '='     |==| , '='     |                           
						\==\ -    ,_ /|==|-  |                 |==|,  |    \==\ -    ,_ / \==\ -    ,_ /                            
						 '.='. -   .' /==/   \                 /==/ - |     '.='. -   .'   '.='. -   .'                             
						   `--`--''   `--`---'                 `--`---'       `--`--''       `--`--''                               

********************************************************************************************************************************
********************************************************************************************************************************
********************************************************************************************************************************


SmallWorld of POO
==================

Computer Engineering School Project - INSA Rennes - 2013

Welcome to the ruthless world of SWOP! SWOP is a turn-based strategy game, playable alone
or with another player, on a single computer (hot seat) or through network. Find your opponent (be it human or AI),
choose your faction and fight for victory!

[![Bitdeli Badge](https://d2weczhvl823v0.cloudfront.net/Gweylorth/swop/trend.png)](https://bitdeli.com/free "Bitdeli Badge")

Features overview
-----------------

* Single or two players
* Local or Network play
* Tactics based gameplay
* Game personalisation : player names, faction selection, map selection
* Ambient music
* Flexible UI
* Microsoft PixelSense version (pre-Alpha)

Quick start guide
-----------------
Here are the main concepts before getting into the fray :
* It's a turn based game, only one player can play at a time and then pass to the other player by clicking the "End my turn !" button.
* The game stops when one of the players has no unit left, or if time is up.
* To select a unit, left click it. If several units are on the same tile, keep clicking to cycle through them.
* To move or attack with an idle unit during your turn, right-click on a green or red highlighted tile. Flashing tiles are automatically recommended by the game.
* Each tile rewards the occupying player with a varying number of points. Mind you, it's the number of occupied tiles which matters, not the number of occupants.

Factions
--------
<dl>

<dt>Vikings</dt>
<dd>
* Able to move on water, but get no point doing so.
* Get 1 more point on tiles adjacent to Water
* Get no point on Desert tiles
</dd>

<dt>Gauls</dt>
<dd>
* Move twice as fast on Fields
* Get 1 more point on Fields
* Get no point on Mountain tiles
</dd>

<dt>Dwarves</dt>
<dd>
* Can move from a mountain to any other free Mountain
* Get 1 more point on Forests
* Get no point on Field tiles
</dd>
</dl>

Maps
--------
<dl>
<dt>Demo</dt>
<dd>
5*5 tiles, 5 turns, 4 units per faction
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
