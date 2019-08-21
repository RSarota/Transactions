using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockTransactions
{
    public class Transaction : Offer
    {
        public string PlayerName { get; set; }
        public DateTime DateOfTransaction { get; set; }

        public Transaction() { }
        public Transaction(string playerName,DateTime date, int id, string name, int value) : base(id,name,value)
        {
            PlayerName = playerName;
            DateOfTransaction = date;
        }
        
        public override string ToString()
        {
            return $"{PlayerName} {DateOfTransaction} {Date} {Instrument} {Value}";
        }
    }
}
