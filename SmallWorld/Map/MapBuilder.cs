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
        Map Build();
    }

    public interface SmallMapBuilder : MapBuilder
    {
        Map Build();
    }

    public interface NormalMapBuilder : MapBuilder
    {
        Map Build();
    }
}
