using System;
using System.Collections.Generic;
using System.Text;

namespace CSV_Parser.Models
{
    public class Obligations
    {
        public int TransID { get; set; }
        public string Order { get; set; }
        public string TransactionType { get; set; }
        public decimal Amount { get; set; }
        public DateTime TransactionDate { get; set; }
        public string Name { get; set; }
    }
}
