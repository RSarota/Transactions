using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace StockTransactions
{
    public class Offer
    {
        public int Date { get; set; }
        public string Instrument { get; set; }
        public int Value { get; set; }

        private List<StockPlayer> subscribers = new List<StockPlayer>(); 

        public ManualResetEvent CanBuy = new ManualResetEvent(false);
        public ManualResetEvent AllProcessed = new ManualResetEvent(false);

        private readonly object _lock = new object();
        public bool OfferIsActual { get; set; }

        private int _timesProcessed; 

        public Offer() { }
        public Offer(int date,string name, int price)
        {
            Date = date;
            Instrument = name;
            Value = price;
        }


        public void SetNewOffer(Offer offer)
        {
            OfferIsActual = true;
            _timesProcessed = 0;
            AllProcessed.Reset();
            CanBuy.Set();
        }

        public void Subscribe(StockPlayer stockPlayer)
        {
            subscribers.Add(stockPlayer);
        }

        public void ProcessOffer()
        {
            if (subscribers.Count == 0)
            {
                AllProcessed.Set();
            }
            else
            {
                _timesProcessed++;
                if (_timesProcessed == subscribers.Count)
                {
                    AllProcessed.Set();
                    CanBuy.Reset();
                }
            }
        }

        public bool TryBuy(int price)
        {
            lock (_lock)
            {
                if (OfferIsActual && Value < price)
                {
                    OfferIsActual = false;
                    CanBuy.Set();
                    return true;
                }
                return false;
            }
            
        }

        public override string ToString()
        {
            return $"{Date} {Instrument} {Value}";
        }

    }
}
