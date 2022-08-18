using System.Collections.Generic;

namespace MyCarAssistant.Models
{
    public class Auto
    {
        public int ID { get; set; }
        
        public string VIN { get; set; }
        public string Mark { get; set; }
        public string Model { get; set; }

        public int Year { get; set; }
        public float EngineCapacity { get; set; }
        public int Power { get; set; }
        public int Mileage { get; set; }
        
        public List<T_O> T_Os { get; set; }
        public List<Expens> Expenses { get; set; }
        public List<Record> Records { get; set; }

        public override string ToString()
        {
            return $"{Mark} {Model}";
        }

    }
}
