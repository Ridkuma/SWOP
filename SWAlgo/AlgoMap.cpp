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
	mapRange = (int) (mapSize / 5);
	vector<vector<int> > t (mapSize, vector<int>(mapSize));
	tiles = t;

	BuildSeaCoasts();

	for (int i = 0; i < mapRange * 2; i++)
	{
		BuildSpecialTiles(TILE_MOUNTAIN);
		BuildSpecialTiles(TILE_DESERT);
		BuildSpecialTiles(TILE_FOREST);
	}
}

// Getter of tile type at position (x,y)
// ----------------------------------------------
int AlgoMap::GetTileType(int x, int y)
{
	if (x < 0 || x >= mapSize || y < 0 || y >= mapSize)
		return 0;

	return tiles[x][y];
}



// ***********************************************
// *               PRIVATE SECTION               *
// ***********************************************

// WARNING => Do NOT cheat on us, or a poussin will die in a atrocious death.
// BTW, if you're reading this, it means you're already cheating...
// ....too late for the poussin... *SPROOooOOUTCH!!*  X-/

void AlgoMap::BuildSeaCoasts()
{
	int nbCoasts = 6;
	enum Coast {None, North, East, South, West, Center};

	Coast seaCoast[3] = {None, None, None};
	
	// Random coasts selection
	do
	{
		seaCoast[0] = (Coast) (rand() % nbCoasts);
	} while (seaCoast[0] == None);

	do
	{
		seaCoast[1] = (Coast) (rand() % nbCoasts);
	} while (seaCoast[1] == None || seaCoast[1] == seaCoast[0]);

	if (rand() % 10 > 6)
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


void AlgoMap::BuildSpecialTiles(int tileType)
{
	int centerX, centerY;
	do
	{
		centerX = rand() % mapSize;
		centerY = rand() % mapSize;
	} while(tiles[centerX][centerY] != TILE_FIELD);
	
	tiles[centerX][centerY] = tileType;

	for (int x = 0; x < mapSize; x++)
	{
		for (int y = 0; y < mapSize; y++)
		{
			int dist = (int) floor(sqrt(pow(centerX - x, 2) + pow(centerY - y, 2)));
			if (dist < min(mapRange, 3) && tiles[x][y] == TILE_FIELD && (rand() % 10) < (10 - mapRange * 2 - dist))
				tiles[x][y] = tileType;
		}
	}
}