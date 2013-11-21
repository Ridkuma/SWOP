using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SmallWorld
{
    public interface IFaction
    {
        List<IUnit> Units { get; }
        void GenerateUnits();
    }

    public class VikingsFaction : IFaction
    {
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

        public void GenerateUnits()
        {
            throw new NotImplementedException();
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

        public void GenerateUnits()
        {
            throw new NotImplementedException();
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

        public void GenerateUnits()
        {
            throw new NotImplementedException();
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
