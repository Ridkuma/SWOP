using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SmallWorld;

namespace SWOP
{
    class FactionView
    {
        public IFaction Faction { get; set; }
        public Dictionary<IUnit, UnitView> UnitViews { get; set; }

        public FactionView(IFaction faction)
        {
            foreach (IUnit unit in faction.Units)
            {
                UnitView unitView = new UnitView(unit);
                MainWindow.INSTANCE.MapView.TilesView[unit.Position].grid.Children.Add(unitView);
            }
        }
    }
}
