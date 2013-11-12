using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SmallWorld
{
    public interface IFaction
    {
        List<Unit> GenerateUnits();
        string GetRandomName();
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

        public List<Unit> GenerateUnits()
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

        public List<Unit> GenerateUnits()
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

        public List<Unit> GenerateUnits()
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
}
