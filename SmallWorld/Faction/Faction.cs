using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SmallWorld
{
    public enum FactionName
    {
        Vikings,
        Gauls,
        Dwarves,
    }

    public interface IFaction
    {
		FactionName Name { get; }
		List<IUnit> Units { get; }
		void AddUnit(string name, ITile startPos);
        void GenerateUnits(int nbUnits, ITile startPos);
        bool IsDecimated();
        int GetTurnScore();
    }

    [Serializable]
    public abstract class Faction : IFaction
    {
        public FactionName Name { get; protected set; }
        public List<IUnit> Units { get; protected set; }
        public List<string> AvailableNames { get; protected set; }

        /// <summary>
        /// Create new unit
        /// </summary>
        public abstract void AddUnit(string name, ITile startPos);

        /// <summary>
        /// Get score of all units for this turn
        /// </summary>
        public abstract int GetTurnScore();

        static Random random = new Random();

        /// <summary>
        /// Generate all units (called by a LocalGame or as Server only, no Client)
        /// </summary>
        public void GenerateUnits(int nbUnits, ITile startPos)
        {
            for (int i = 0; i < nbUnits; i++)
            {
                AddUnit(GetRandomName(), startPos);
            }
        }

        /// <summary>
        /// Check if this faction has been wiped out
        /// </summary>
        /// <returns></returns>
        public bool IsDecimated()
        {
            foreach (IUnit unit in this.Units)
            {
                if (unit.State != UnitState.Dead)
                    return false;
            }

            return true;
        }

        /// <summary>
        /// Get a random name among all available for this faction
        /// </summary>
        /// <returns>Name for a Unit</returns>
        private string GetRandomName()
        {
            int rand = random.Next(this.AvailableNames.Count);
            string name = this.AvailableNames[rand];
            this.AvailableNames.RemoveAt(rand);
            return name;
        }
    }

    [Serializable]
    public class VikingsFaction : Faction
    {
        public VikingsFaction()
        {
            base.Name = FactionName.Vikings;
			base.Units = new List<IUnit>();
            base.AvailableNames = new List<string> 
            { 
                "Olaf",
                "Grossebaf",
                "Epitaf",
                "Cinematograf",
                "Paragraf",
                "Telegraf",
                "Caraf",
                "Pictograf",
                "Ortograf",
                "Cryptograf"
            };
		}

		
		public override void AddUnit(string name, ITile startPos)
		{
			Units.Add(new VikingsUnit(name, startPos));
		}

        public override int GetTurnScore()
        {
            int score = 0;
            List<ITile> visitedTiles = new List<ITile>();
            foreach (Unit unit in this.Units)
            {
                // Skip if Unit is dead, Tile is water or Tile points have already been visited
                if (unit.State == UnitState.Dead || unit.Position.Type == TileType.Water || visitedTiles.Contains(unit.Position))
                    continue;

                if (unit.Position.IsAdjacentToWater())
                    score += 2;
                else
                    score += 1;

                visitedTiles.Add(unit.Position);
            }

            return score;
        }

    }

    [Serializable]
    public class GaulsFaction : Faction
    {
        public GaulsFaction()
        {
            base.Name = FactionName.Gauls;
			base.Units = new List<IUnit>();
            base.AvailableNames = new List<string> // no special chr (like accents) in names => cause bug with network -_-'
            { 
                "Asterix",
                "Obelix",
                "Ordralphabetix",
                "Assurancetourix",
                "Abraracourcix",
                "Bonnemine",
                "Goudurix",
                "Falbala",
                "Cetautomatix",
                "Ielosubmarine"
            };
		}

		public override void AddUnit(string name, ITile startPos)
		{
			Units.Add(new GaulsUnit(name, startPos));
		}

        public override int GetTurnScore()
        {
            int score = 0;
            List<ITile> visitedTiles = new List<ITile>();

            // Skip if Unit is dead, Tile is water or Tile points have already been visited
            foreach (Unit unit in this.Units)
            {
                if (unit.State == UnitState.Dead || unit.Position.Type == TileType.Mountain || visitedTiles.Contains(unit.Position))
                    continue;

                if (unit.Position.Type == TileType.Field)
                    score += 2;
                else
                    score += 1;

                visitedTiles.Add(unit.Position);
            }

            return score;
        }
    }

    [Serializable]
    public class DwarvesFaction : Faction
    {
        public DwarvesFaction()
        {
            base.Name = FactionName.Dwarves;
			base.Units = new List<IUnit>();
            base.AvailableNames = new List<string> 
            { 
                "Thorin Oakenshield",
                "Dwalin",
                "Balin",
                "Gloin",
                "Kili",
                "Fili",
                "Dori",
                "Nori",
                "Ori",
                "Oin",
                "Bifur",
                "Bombur",
                "Bofur"
            };
		}

        public override void AddUnit(string name, ITile startPos)
		{
			Units.Add(new DwarvesUnit(name, startPos));
		}

        public override int GetTurnScore()
        {
            int score = 0;
            List<ITile> visitedTiles = new List<ITile>();

            // Skip if Unit is dead, Tile is water or Tile points have already been visited
            foreach (Unit unit in this.Units)
            {
                if (unit.State == UnitState.Dead || unit.Position.Type == TileType.Field || visitedTiles.Contains(unit.Position))
                    continue;

                if (unit.Position.Type == TileType.Forest)
                    score += 2;
                else
                    score += 1;

                visitedTiles.Add(unit.Position);
            }

            return score;
        }
    }


    
}
