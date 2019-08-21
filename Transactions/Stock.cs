using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace StockTransactions
{
    public class Stock
    {
        public List<string> Instruments { get;  }
        public List<Offer> Offers { get; set; }
        public List<StockPlayer> StockPlayers { get; set; }

        public Stock()
        {
            Instruments = GetInstruments();
            FileManager.SerializeRandomOffers(Instruments);
            Offers = GetOfferHistory();
            StockPlayers = new List<StockPlayer>();
            File.Delete("transactions.txt");
        }

        private List<string> GetInstruments()
        {
            return FileManager.LoadInstruments();
        }

        private List<Offer> GetOfferHistory()
        {
            try
            {
                return FileManager.DeserializeOffers();
            }
            catch (Exception e)
            {
                Console.WriteLine("Cant load offer history.\n " + e.Message);
                return new List<Offer>();
            }
        }


        public void PrintInstruments()
        {
            foreach (var instrument in Instruments)
            {
                Console.WriteLine(instrument);
            }
        }

        public void PrintSelectedOffers(int date1, int date2)
        {
            var selectedOffers = Offers.Where(x => x.Date >= date1 && x.Date <= date2)
                .OrderByDescending(x => x.Date).ToList();
            foreach (var offer in selectedOffers)
            {
                Console.WriteLine(offer);
            }
        }


        public void PrintTransactions()
        {
            try
            {
                List<Transaction> list = FileManager.DeserializeTransactions();
                foreach (var item in list)
                {
                    Console.WriteLine(item);
                }
            }
            catch (FileNotFoundException)
            {
                Console.WriteLine("0 transactions recorded.");
            }
            catch(Exception e)
            {
                Console.WriteLine("Something went wrong. Couldnt get list of transactions.\n " + e.Message);
            }
        }



        public void PrintSelectedTransactions(string intrument)
        {
            try
            {
                var transactionList=FileManager.DeserializeTransactions()
                    .Where(x=>x.Instrument==intrument).ToList();
                foreach (var transaction in transactionList)
                {
                    Console.WriteLine(transaction);
                }
            }
            catch (FileNotFoundException)
            {
                Console.WriteLine("0 transactions recorded.");
            }
            catch (Exception e)
            {
                Console.WriteLine("Something went wrong. Couldnt get list of transactions.\n "+e.Message);
            }
        }


        public async Task Run()
        {
            await Task.Run(async() => 
            { 
                foreach (var player in StockPlayers)
                {
                    player.Run(this, player.Watcher);
                }
                foreach (var offer in Offers)
                {
                    await Task.Delay(200);
                    Console.WriteLine(offer);
                    offer.SetNewOffer(offer);
                }
            });
        }
        

    }
}
