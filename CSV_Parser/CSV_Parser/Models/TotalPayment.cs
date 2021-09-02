using System;
using System.Collections.Generic;
using System.Text;

namespace CSV_Parser.Models
{
    public class TotalPayment
    {
        public string Order { get; set; }
        public string Name { get; set; }
        public decimal TotalOb { get; set; }
        public decimal Paid { get; set; }
        public decimal Owed { get; set; }
    }
}
