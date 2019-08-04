using System.Collections.Generic;

namespace Domain.Entities
{
    public class ParsedData
    {
        public Dictionary<string, double> Products { get; set; }

        public double TotalPrice { get; set; }
    }
}
