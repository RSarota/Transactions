using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace StockTransactions
{
    public class StockPlayer
    {
        public string Name { get; set; }
        public Dictionary<string, int> Watcher { get; set; }


        public StockPlayer(string name)
        {
            Name = name;
            Watcher = new Dictionary<string, int>();
        }


        public void Buy(Offer offer, int price)
        {
            FileManager.SerializeTransaction(this, offer);
            Console.WriteLine(this.Name + " bought offer: " + offer);
            offer.CanBuy.Reset();
        }

        public Task Run(Stock stock, Dictionary<string,int> watcher)
        {
            
            return Task.Run(() =>
            {
                foreach (var offer in stock.Offers)
                {
                    offer.CanBuy.WaitOne();
                    foreach (var item in watcher)
                    {
                        if (offer.TryBuy(item.Value) && offer.Instrument == item.Key && offer.Value < item.Value)
                        {
                            Buy(offer, item.Value);
                        }
                    }
                    offer.ProcessOffer();
                    offer.AllProcessed.WaitOne();
                }
            });
        }
    }
}
