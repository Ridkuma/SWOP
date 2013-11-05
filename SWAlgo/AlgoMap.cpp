#include "AlgoMap.h"

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
	mapSize = size;
	vector<vector<int> > t (mapSize, vector<int>(mapSize));
	tiles = t;

	for (int x = 0; x < mapSize; x++)
	{
		for (int y = 0; y < mapSize; y++)
		{
			tiles[x][y] = (x*y) % 5;
		}
	}

	BuildSeaCoasts();

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

void AlgoMap::BuildSeaCoasts()
{
	for (int i = 0; i < mapSize; i++)
	{
		tiles[i][0] = TILE_WATER;
		tiles[i][mapSize - 1] = TILE_WATER;
		tiles[0][i] = TILE_WATER;
		tiles[mapSize - 1][i] = TILE_WATER;
	}
}
