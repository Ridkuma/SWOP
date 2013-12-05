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
        List<IUnit> Units { get; }
        void GenerateUnits(int nbUnits, ITile startPos);
        FactionName Name { get; }
    }



    public class VikingsFaction : IFaction
    {
        public List<IUnit> Units { get; protected set; }
        public FactionName Name { get; private set; }

        static Random random = new Random();

        public VikingsFaction()
        {
            this.Name = FactionName.Vikings;
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

        public void GenerateUnits(int nbUnits, ITile startPos)
        {
            Units = new List<IUnit>();
            for (int i = 0; i < nbUnits; i++)
            {
                Units.Add(new VikingsUnit(GetRandomName(), startPos));
            }
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
        }

        private List<string> availableNames = new List<string> 
            { 
                "Astérix",
                "Obélix",
                "Ordralphabétix",
                "Assurancetourix",
                "Abraracourcix",
                "Bonnemine",
                "Goudurix",
                "Falbala",
                "Cétautomatix",
                "Iélosubmarine"
            };

        public void GenerateUnits(int nbUnits, ITile startPos)
        {
            Units = new List<IUnit>();
            for (int i = 0; i < nbUnits; i++)
            {
                Units.Add(new GaulsUnit(GetRandomName(), startPos));
            }
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

        public void GenerateUnits(int nbUnits, ITile startPos)
        {
            Units = new List<IUnit>();
            for (int i = 0; i < nbUnits; i++)
            {
                Units.Add(new DwarvesUnit(GetRandomName(), startPos));
            }
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
