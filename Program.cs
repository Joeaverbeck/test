using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using CSV_Parser.Models;

namespace CSV_Parser

{
    class Program
    {
        static void Main(string[] args)
        {

            ParsedFile parsedFile = new ParsedFile("C:\\Users\\joeav\\Desktop\\accounting.csv");

            foreach (TotalPayment totalPayment in parsedFile.Payments1)
            {
                if (totalPayment.Owed > 0)
                {
                    Console.WriteLine($"{totalPayment.Name} is owed:${totalPayment.Owed}" +
                        $" for order:{totalPayment.Order}");
                }
                else if (totalPayment.Owed < 0)
                {
                   Console.WriteLine($"{totalPayment.Name} was overpaid by:${Math.Abs(totalPayment.Owed)}" +
                        $" for order:{totalPayment.Order}");
                }
            }
        }
    }

    /// <summary>
    /// Parses CSV file into string
    /// </summary>

    public class ParsedFile
    {
        public List<Obligations> Obligation { get; set; }
        public List<TotalPayment> Payments1 { get; set; }
        public ParsedFile(string filename)
        {
            this.Obligation = new List<Obligations>();
            this.Payments1 = new List<TotalPayment>();
            this.Accounting(filename);
            this.TotalPayment();
        }
        /// <summary>
        /// Loads the Obligatio table from the parsed CSV file.
        /// </summary>
        /// <param name="filename">The CSV file.</param>
        private void Accounting(string filename)
        {
            string[] csvLines = File.ReadAllLines(filename);
            for (int i = 1; i < csvLines.Length; i++)
            {
                Obligations obligations = new Obligations();
                string[] csvValues = csvLines[i].Split(',');
                obligations.TransID = Convert.ToInt32(csvValues[0]);
                obligations.Order = Convert.ToString(csvValues[1]);
                obligations.TransactionType = Convert.ToString(csvValues[2]);
                obligations.Amount = Convert.ToDecimal(csvValues[3]);
                obligations.TransactionDate = Convert.ToDateTime(csvValues[4]);
                obligations.Name = Convert.ToString(csvValues[5]);
                // adds to the obligation table.
                Obligation.Add(obligations);
            }
        }
        /// <summary>
        /// Calculates total obligations and detemines what is owed and what is overpaid.
        /// </summary>
        private void TotalPayment()
        {
            List<string> distinctorder = (from o in Obligation
                                          select o.Order).Distinct().ToList();

            foreach (string order in distinctorder)
            {
                TotalPayment report = new TotalPayment();

                report.TotalOb = (from t in Obligation
                                  where t.Order == order
                                  && t.TransactionType == "Obligation"
                                  select t.Amount).Sum();

                report.Paid = (from t in Obligation
                               where t.Order == order
                               && t.TransactionType == "Payment"
                               select t.Amount).Sum();

                report.Name = (from t in Obligation
                               where t.Order == order
                               select t.Name).FirstOrDefault();

                report.Owed = report.TotalOb - report.Paid;

                TotalPayment totalPayment = new TotalPayment();

                totalPayment.TotalOb = report.TotalOb;
                totalPayment.Paid = report.Paid;
                totalPayment.Order = order;
                totalPayment.Name = report.Name;
                totalPayment.Owed = report.Owed;
                // adds to the totalpayments table.
                Payments1.Add(totalPayment);
            }
        }
    }
}