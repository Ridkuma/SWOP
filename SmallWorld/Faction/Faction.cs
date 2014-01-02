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
    }



    public class VikingsFaction : IFaction
    {
		public FactionName Name { get; private set; }
		public List<IUnit> Units { get; protected set; }

        static Random random = new Random();

        public VikingsFaction()
        {
            this.Name = FactionName.Vikings;
			Units = new List<IUnit>();
		}

        private List<string> availableNames = new List<string> 
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
		/// Create new unit
		/// </summary>
		public void AddUnit(string name, ITile startPos)
		{
			Units.Add(new VikingsUnit(name, startPos));
		}

        private string GetRandomName()
        {
            int rand = random.Next(this.availableNames.Count);
            string name = this.availableNames[rand];
            this.availableNames.RemoveAt(rand);
            return name;
        }
    }

    public class GaulsFaction : IFaction
    {
        public List<IUnit> Units { get; protected set; }
        public FactionName Name { get; private set; }

        static Random random = new Random();

        public GaulsFaction()
        {
            this.Name = FactionName.Gauls;
			Units = new List<IUnit>();
		}

        private List<string> availableNames = new List<string> // no special chr (like accents) in names => cause bug with network -_-'
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
		/// Create new unit
		/// </summary>
		public void AddUnit(string name, ITile startPos)
		{
			Units.Add(new GaulsUnit(name, startPos));
		}

        private string GetRandomName()
        {
            int rand = random.Next(this.availableNames.Count);
            string name = this.availableNames[rand];
            this.availableNames.RemoveAt(rand);
            return name;
        }
    }

    public class DwarvesFaction : IFaction
    {
        public List<IUnit> Units { get; protected set; }
        public FactionName Name { get; private set; }

        static Random random = new Random();

        public DwarvesFaction()
        {
            this.Name = FactionName.Dwarves;
			Units = new List<IUnit>();
		}

        private List<string> availableNames = new List<string> 
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
		/// Create new unit
		/// </summary>
		public void AddUnit(string name, ITile startPos)
		{
			Units.Add(new DwarvesUnit(name, startPos));
		}

        private string GetRandomName()
        {
            int rand = random.Next(this.availableNames.Count);
            string name = this.availableNames[rand];
            this.availableNames.RemoveAt(rand);
            return name;
        }
    }


    
}
