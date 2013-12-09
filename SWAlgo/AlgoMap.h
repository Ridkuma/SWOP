#pragma once
#include <vector>


#ifdef SWAlgo_EXPORTS
	#define SWAlgo_API __declspec(dllexport)
#else
	#define SWAlgo_API __declspec(dllimport)
#endif

#define MAX_NB_PLAYERS	4

// Type tile constants
#define TILE_FIELD		0
#define TILE_MOUNTAIN	1
#define TILE_DESERT		2
#define TILE_FOREST		3
#define TILE_WATER		4


class SWAlgo_API AlgoMap
{
private:
	int randomSeed;
	int mapSize;
	int mapRange;
	std::vector<std::vector<int> > tiles;
	int startPositions[MAX_NB_PLAYERS][2];

	void BuildSeaCoasts();
	void BuildSpecialTiles(int tileType);
	void ProcessStartPositions();

	int GetDistance(int x1, int y1, int x2, int y2);

public:
	AlgoMap(void);
	~AlgoMap(void);

	void BuildMap(int size, int rndSeed);
	int GetRandomSeed();
	int GetTileType(int x, int y);
	int GetStartTileX(int playerId);
	int GetStartTileY(int playerId);
	//int IsStartTile(int x, int y);
};