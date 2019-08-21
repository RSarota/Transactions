using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace StockTransactions
{
    class Program
    {
        public static void PrintMenu()
        {
            Console.Write("Choose one:\n" +
                "1. Get offer history\n" +
                "2. Set watcher\n" +
                "3. Get transactions\n" +
                "4. Run\n" +
                "Press any key to quit\n");
        }
        static async Task Main(string[] args)
        {
            var stock = new Stock();
            stock.StockPlayers.Add(new StockPlayer("player1"));
            stock.StockPlayers[0].Watcher.Add("LPP", 30);
            stock.StockPlayers[0].Watcher.Add("PZU", 30);
            stock.StockPlayers[0].Watcher.Add("MBANK", 30);
            stock.StockPlayers.Add(new StockPlayer("player2"));
            stock.StockPlayers[1].Watcher.Add("LPP", 30);
            stock.StockPlayers[1].Watcher.Add("PZU", 30);
            stock.StockPlayers[1].Watcher.Add("MBANK", 30);
            stock.StockPlayers.Add(new StockPlayer("player3"));
            stock.StockPlayers[2].Watcher.Add("LPP", 30);
            stock.StockPlayers[2].Watcher.Add("PZU", 30);
            stock.StockPlayers[2].Watcher.Add("MBANK", 30);
            stock.StockPlayers[2].Watcher.Add("GPW", 50);


            PrintMenu();
            int.TryParse(Console.ReadLine(), out int number);
            do
            {
                switch (number)
                {
                    case 1:
                        Console.WriteLine("Select from:");
                        int.TryParse(Console.ReadLine(),out int date1);
                        Console.WriteLine("to:");
                        int.TryParse(Console.ReadLine(), out int date2);
                        stock.PrintSelectedOffers(date1, date2);
                        break;
                    case 2:
                        Console.WriteLine("Available instruments:");
                        stock.PrintInstruments();
                        foreach (var player in stock.StockPlayers)
                        {
                            Console.WriteLine(player.Name);
                            Console.WriteLine("Instrument:");
                            string instrument = Console.ReadLine();
                            Console.WriteLine("Value:");
                            bool success=int.TryParse(Console.ReadLine(), out int value);
                            if (stock.Instruments.Contains(instrument.ToUpper()) && success)
                            {
                                player.Watcher.Add(instrument.ToUpper(), value);
                                var list=stock.Offers.Where(x => x.Instrument == instrument.ToUpper()&&x.Value<value).ToList();
                                foreach (var element in list)
                                {
                                    element.Subscribe(player);
                                }
                                Console.WriteLine("Watcher successfully added.");
                            }
                        }
                        break;
                    case 3:
                        Console.WriteLine("Would you like to see all transactions? (Y/N) (Type N to select instrument)");
                        string input = Console.ReadLine();
                        if (input == "Y" || input == "y")
                        {
                            stock.PrintTransactions();
                        }
                        else if(input == "N" || input == "n")
                        {
                            Console.WriteLine("Instrument:");
                            string input1 = Console.ReadLine();
                            stock.PrintSelectedTransactions(input1.ToUpper());
                        }
                        break;
                    case 4:
                        await stock.Run();
                        break;
                    default:
                        break;
                }
                PrintMenu();
                int.TryParse(Console.ReadLine(), out number);
            } while (number > 0 && number < 5);


            Console.ReadKey();
        }
    }
}
