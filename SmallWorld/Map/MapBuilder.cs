using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SmallWorld
{
    public interface IMapBuilder
    {
		Map Build(int randomSeed);
    }

    public class DemoMapBuilder : IMapBuilder
    {
		public Map Build(int randomSeed)
		{
			return new Map(5, 5, 4, randomSeed);
		}
    }

	public class SmallMapBuilder : IMapBuilder
	{
		public Map Build(int randomSeed)
		{
			return new Map(10, 20, 6, randomSeed);
		}
    }

	public class NormalMapBuilder : IMapBuilder
	{
		public Map Build(int randomSeed)
		{
			return new Map(15, 30, 8, randomSeed);
		}
    }
}
