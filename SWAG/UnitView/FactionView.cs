using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SmallWorld;

namespace SWAG
{
    public class FactionView
    {
        public IFaction Faction { get; set; }
        public Dictionary<IUnit, UnitView> UnitViews { get; set; }

        public FactionView(IFaction faction)
        {
			Faction = faction;
			UnitViews = new Dictionary<IUnit, UnitView>();
            foreach (IUnit unit in faction.Units)
            {
                UnitView unitView = new UnitView(unit);
                MainWindow.INSTANCE.MapView.TilesView[unit.Position].grid.Children.Add(unitView);
				UnitViews.Add(unit, unitView);
            }
        }

        public void BuryOurDeads()
        {
            foreach (IUnit unit in this.Faction.Units)
            {
                if (unit.State == UnitState.Dead)
                {
                    UnitView uView = UnitViews[unit];
                    uView.ParentTile.grid.Children.Remove(uView);
                }
            }
        }
    }
}
