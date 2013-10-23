#pragma once

#ifdef SWAlgo_EXPORTS
	#define SWAlgo_API __declspec(dllexport)
#else
	#define SWAlgo_API __declspec(dllimport)
#endif

class SWAlgo_API AlgoMap
{
public:
	int BuildMap();
};