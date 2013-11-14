#include "AlgoMap.h"
#include <ctime>
#include <cstdlib>

// Process to propagate any update of this DLL Project to the other projects :
//		- Once your modifications are over, generate manually this project
//		- If you have added or modified the structure of the methods or added a new file, you need to launch the "swig.bat" in the project's folder

using namespace std;

// Constructor
// ----------------------------------------------
AlgoMap::AlgoMap(void)
{
}

// Destructor
// ----------------------------------------------
AlgoMap::~AlgoMap(void)
{
}

// Generate each tiles on map
// ----------------------------------------------
void AlgoMap::BuildMap(int size)
{
    srand(time(0));

	mapSize = size;
	mapRange = (int) (mapSize / 5); // Scale index
	vector<vector<int> > t (mapSize, vector<int>(mapSize));
	tiles = t;

	// Start by seas
	BuildSeaCoasts();

	// Then put some random zones
	for (int i = 0; i < mapRange * 2; i++)
	{
		BuildSpecialTiles(TILE_MOUNTAIN);
		BuildSpecialTiles(TILE_DESERT);
		BuildSpecialTiles(TILE_FOREST);
	}

	ProcessStartPositions();
}

// Getter of tile type at position (x,y)
// ----------------------------------------------
int AlgoMap::GetTileType(int x, int y)
{
	if (x < 0 || x >= mapSize || y < 0 || y >= mapSize)
		return 0;

	return tiles[x][y];
}



// Getter of X position of starting tile
// ----------------------------------------------
int AlgoMap::GetStartTileX(int playerId)
{
	return startPositions[playerId][0];
}


// Getter of X position of starting tile
// ----------------------------------------------
int AlgoMap::GetStartTileY(int playerId)
{
	return startPositions[playerId][1];
}

/*
// Is a starting tile (return player id)
// ----------------------------------------------
int AlgoMap::IsStartTile(int x, int y)
{
	for(int i = 0; i < MAX_NB_PLAYERS; i++)
		if (x == startPositions[i][0] && y == startPositions[i][1])
			return i;
	return -1;
}
*/



// ***********************************************
// *               PRIVATE SECTION               *
// ***********************************************

// WARNING => Do NOT cheat on us, or a poussin will die in a atrocious death.
// BTW, if you're reading this, it means you're already cheating...
// ....too late for the poussin... *SPROOooOOUTCH!!*  X-/


// Build sea area on map
// ----------------------------------------------
void AlgoMap::BuildSeaCoasts()
{
	int nbCoasts = 6;
	enum Coast {None, North, East, South, West, Center};

	Coast seaCoast[3] = {None, None, None};
	
	// Random coasts selection (2 mandatory + 1 optional)
	do
	{
		seaCoast[0] = (Coast) (rand() % nbCoasts);
	} while (seaCoast[0] == None);

	do
	{
		seaCoast[1] = (Coast) (rand() % nbCoasts);
	} while (seaCoast[1] == None || seaCoast[1] == seaCoast[0]);

	if (rand() % 10 > (7 - mapRange))
	{
		do
		{
			seaCoast[2] = (Coast) (rand() % nbCoasts);
		} while (seaCoast[2] == seaCoast[0] || seaCoast[2] == seaCoast[1]);
	}

	// Tiles setting
	int x1, y1;
	int x2, y2;
		
	for (int i = 0; i < mapSize; i++)
	{
		for (int j = 0; j < 3; j++)
		{
			switch (seaCoast[j])
			{
			case North:
				x1 = i; y1 = 0;
				x2 = i; y2 = 1;
				break;
			
			case East:
				x1 = mapSize - 1; y1 = i;
				x2 = mapSize - 2; y2 = i;
				break;

			case South:
				x1 = i; y1 = mapSize - 1;
				x2 = i; y2 = mapSize - 2;
				break;
			
			case West:
				x1 = 0; y1 = i;
				x2 = 1; y2 = i;
				break;

			case Center:
				if (i == 0)
					BuildSpecialTiles(TILE_WATER);
				break;

			default:
				break;
			}

			if (seaCoast[j] != Center)
			{
				tiles[x1][y1] = TILE_WATER;
				if (mapRange > 1 && rand() % 10 >= 5)
					tiles[x2][y2] = TILE_WATER;
			}
		}
	}
}


// Build area of a specific tile type
// ----------------------------------------------
void AlgoMap::BuildSpecialTiles(int tileType)
{
	// Find a random (and free) center for the zone
	int centerX, centerY;
	do
	{
		centerX = rand() % mapSize;
		centerY = rand() % mapSize;
	} while(tiles[centerX][centerY] != TILE_FIELD);
	
	tiles[centerX][centerY] = tileType;

	// Expand the zone around this center
	for (int x = 0; x < mapSize; x++)
	{
		for (int y = 0; y < mapSize; y++)
		{
			int dist = GetDistance(centerX, centerY, x, y);
			if (dist < min(mapRange, 3) && tiles[x][y] == TILE_FIELD && (rand() % 10) < (10 - mapRange * 2 - dist))
				tiles[x][y] = tileType;
		}
	}
}


// Calculate best starting positions for players
// ----------------------------------------------
void AlgoMap::ProcessStartPositions()
{
	for(int i = 0; i <= 1; i++)
		for(int j = 0; j <= 1; j++)
			startPositions[i][j] = (int) mapSize / 2;

	for (int x = 0; x < mapSize; x++)
	{
		for (int y = 0; y < mapSize; y++)
		{
			// Top-left corner
			if (tiles[x][y] != TILE_WATER && (x + y) < (startPositions[0][0] + startPositions[0][1]))
			{
				startPositions[0][0] = x;
				startPositions[0][1] = y;
			}
			// Bottom-right corner
			if (tiles[x][y] != TILE_WATER && (x + y) > (startPositions[1][0] + startPositions[1][1]))
			{
				startPositions[1][0] = x;
				startPositions[1][1] = y;
			}
			// TODO : also check top-right & bottom-left distance to see the farther
		}
	}
}


// Return distance between two tiles
// ----------------------------------------------
int AlgoMap::GetDistance(int x1, int y1, int x2, int y2)
{
	return (int) floor(sqrt(pow(x1 - x2, 2) + pow(y1 - y2, 2)));
}