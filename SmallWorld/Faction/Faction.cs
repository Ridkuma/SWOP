using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SmallWorld
{
    public interface IFaction
    {
        List<IUnit> GenerateUnits();
    }

    public class VikingsFaction : IFaction
    {
        public List<IUnit> GenerateUnits()
        {
            throw new NotImplementedException();
        }
    }

    public class DwarvesFaction : IFaction
    {
        public List<IUnit> GenerateUnits()
        {
            throw new NotImplementedException();
        }
    }

    public class GaulsFaction : IFaction
    {
        public List<IUnit> GenerateUnits()
        {
            throw new NotImplementedException();
        }
    }
}
