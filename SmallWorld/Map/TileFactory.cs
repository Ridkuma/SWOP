using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SmallWorld
{
    public interface TileFactory
    {
        MountainTile CreateFieldTile();

        MountainTile CreateMoutainTile();

        FieldTile CreateDesertTile();

        DesertTile CreateForestTile();

        WaterTile CreateWaterTile();

        Tile CreateTile();
    }
}
