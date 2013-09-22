using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SmallWorld
{
    public interface MapBuilder
    {
        Map Build();
    }

    public interface DemoMapBuilder : MapBuilder
    {
    }

    public interface SmallMapBuilder : MapBuilder
    {
    }

    public interface NormalMapBuilder : MapBuilder
    {
    }
}
