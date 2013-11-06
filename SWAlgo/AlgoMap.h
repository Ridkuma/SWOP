#pragma once
#include <vector>


#ifdef SWAlgo_EXPORTS
	#define SWAlgo_API __declspec(dllexport)
#else
	#define SWAlgo_API __declspec(dllimport)
#endif


// Type tile constants
#define TILE_FIELD		0
#define TILE_MOUNTAIN	1
#define TILE_DESERT		2
#define TILE_FOREST		3
#define TILE_WATER		4


class SWAlgo_API AlgoMap
{
private:
	int mapSize;
	int mapRange;
	std::vector<std::vector<int> > tiles;

	void BuildSeaCoasts();
	void BuildSpecialTiles(int tileType);

public:
	AlgoMap(void);
	~AlgoMap(void);

	void BuildMap(int size);
	int GetTileType(int x, int y);

};