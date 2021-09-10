using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.ViewModels.Regions
{
    public class Dog
    {
        public string Name { set; get; }
        public string Breed { set; get; }

        public Dog()
        {
            Name = "BalKan";
            Breed = "Street excellent";
        }

        //Method
        public void Bark()
        {
            Console.WriteLine("Bark");
        }

        public override string ToString()
        {
            return base.ToString();
        }
    }
}