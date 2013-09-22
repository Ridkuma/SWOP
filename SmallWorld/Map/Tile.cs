using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SmallWorld
{
    public interface Tile
    {
    }

    public interface ForestTile : Tile
    {
    }

    public interface DesertTile : Tile
    {
    }

    public interface WaterTile : Tile
    {
    }

    public interface MountainTile : Tile
    {
    }

    public interface FieldTile : Tile
    {
    }
}
