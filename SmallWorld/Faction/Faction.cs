using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SmallWorld
{
    public interface IFaction
    {
        List<IUnit> Units { get; }
        void GenerateUnits(int nbUnits);
    }



    public class VikingsFaction : IFaction
	{
		public List<IUnit> Units { get; protected set; }
        static Random random = new Random();

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

		public void GenerateUnits(int nbUnits)
		{
			Units = new List<IUnit>();
			for (int i = 0; i < nbUnits; i++)
			{
				Units.Add(new VikingsUnit(GetRandomName()));
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
        static Random random = new Random();

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

		public void GenerateUnits(int nbUnits)
		{
			Units = new List<IUnit>();
			for (int i = 0; i < nbUnits; i++)
			{
				Units.Add(new DwarvesUnit(GetRandomName()));
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
        static Random random = new Random();

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

		public void GenerateUnits(int nbUnits)
		{
			Units = new List<IUnit>();
			for (int i = 0; i < nbUnits; i++)
			{
				Units.Add(new GaulsUnit(GetRandomName()));
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


    public enum FactionName
    {
        Vikings,
        Gauls,
        Dwarves,
    }
}
