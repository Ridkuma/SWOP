using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SmallWorld
{
    public interface IMapBuilder
    {
		Map Build();
    }

    public class DemoMapBuilder : IMapBuilder
    {
		public Map Build()
		{
			return new Map(5);
		}
    }

	public class SmallMapBuilder : IMapBuilder
	{
		public Map Build()
		{
			return new Map(10);
		}
    }

	public class NormalMapBuilder : IMapBuilder
	{
		public Map Build()
		{
			return new Map(15);
		}
    }
}
