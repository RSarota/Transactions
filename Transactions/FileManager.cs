using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace StockTransactions
{
    static class FileManager
    {
        public static List<string> LoadInstruments()
        {
            var Instruments = new List<string>();
            using (FileStream fileStream = new FileStream("instruments.txt", FileMode.Open))
            {
                StreamReader streamReader = new StreamReader(fileStream);
                do
                {
                    Instruments.Add(streamReader.ReadLine());
                }
                while (streamReader.Peek() != -1);
            }
            return Instruments;
        }

        public static void SerializeRandomOffers(List<string> instruments)
        {
            Random rnd = new Random();
            var document = new XDocument(new XElement("Offers"));
            for (int i = 1; i <= 1000; i++)
            {
                string instrument = instruments.ElementAt(rnd.Next(0, instruments.Count));
                int value = rnd.Next(5, 100);
                document.Root.Add(new XElement("Offer",
                        new XElement("Date", i.ToString()),
                        new XElement("Instrument", instrument),
                        new XElement("Value", value.ToString())
                    ));
            }
            document.Save("offers.txt");
        }

        public static List<Offer> DeserializeOffers()
        {
            var document = XDocument.Load("offers.txt");

            var list = document.Descendants("Offer").Select(x =>
                new Offer
                {
                    //OfferIsActual = true,
                    Date = Int32.Parse(x.Element("Date").Value),
                    Instrument = x.Element("Instrument").Value,
                    Value = Int32.Parse(x.Element("Value").Value)
                }).ToList();

            return list;
        }

        public static void AddOfferToHistory(Offer offer)
        {
            var document = XDocument.Load("offers.txt");

            document.Root.Add(new XElement("Offer",
                        new XElement("Date", offer.Date),
                        new XElement("Instrument", offer.Instrument),
                        new XElement("Value", offer.Value)
                    ));
            document.Save("offers.txt");
        }
        

        public static List<Transaction> DeserializeTransactions()
        {
            var document = XDocument.Load("transactions.txt");
            var list = document.Descendants("Transaction").Select(x =>
                new Transaction
                {
                    PlayerName = x.Element("Player").Value,
                    DateOfTransaction = DateTime.Parse(x.Element("DateOfTransaction").Value),
                    Date = int.Parse(x.Element("Date").Value),
                    Instrument = x.Element("Instrument").Value,
                    Value = int.Parse(x.Element("Value").Value)
                }).ToList();
            return list;
        }

        public static void SerializeTransaction(StockPlayer player, Offer offer)
        {
            XDocument document;
            try
            {
                document = XDocument.Load("transactions.txt");
            }
            catch
            {
                document = new XDocument(new XElement("Transactions"));
            }

            document.Root.Add(new XElement("Transaction",
                        new XElement("Player", player.Name),
                        new XElement("DateOfTransaction", DateTime.UtcNow),
                        new XElement("Date", offer.Date.ToString()),
                        new XElement("Instrument", offer.Instrument),
                        new XElement("Value", offer.Value.ToString())
                    ));
            document.Save("transactions.txt");
        }
    }
}
