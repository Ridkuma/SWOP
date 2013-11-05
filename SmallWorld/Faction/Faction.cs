using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SmallWorld
{
    public interface IFaction
    {
        List<Unit> GenerateUnits();
    }

    public class VikingsFaction : IFaction
    {
        public List<Unit> GenerateUnits()
        {
            throw new NotImplementedException();
        }
    }

    public class DwarvesFaction : IFaction
    {
        public List<Unit> GenerateUnits()
        {
            throw new NotImplementedException();
        }
    }

    public class GaulsFaction : IFaction
    {
        public List<Unit> GenerateUnits()
        {
            throw new NotImplementedException();
        }
    }
}
