using Microsoft.VisualBasic.FileIO;
using System.Globalization;

namespace JobInterviewTest
{
    internal class Program
    {
        static void Main(string[] args)
        {
            if (ValidateArgs(args))
            {
                Console.WriteLine("Некорректный формат аргументов");
                return;
            }

            string cityDistrict = args[0];
            string firstDeliveryDateTime = args[1];
            string deliveryLog = args[2];
            string deliveryOrder = args[3];
            string inputFile = args[4];

            string[] lines = File.ReadAllLines(inputFile);
            Log(deliveryLog, "Чтение данных из файла " + inputFile);



            var fileStream = new FileStream(inputFile, FileMode.Open);
            var reader = new StreamReader(fileStream);
            var csvReader = new CsvReader(reader, new CsvConfiguration(CultureInfo.InvariantCulture) { Delimiter = ";" });

            var records = csvReader.GetRecords<Order>().ToList();

            //List<Order> orders = new List<Order>();
            //foreach (string line in lines)
            //{
            //    var order = ParseLine(line);
            //    if (order is not null)
            //    { 
            //        orders.Add(ParseLine(line)); 
            //    }
            //}


            var filteredOrders = FilterOrders(orders, cityDistrict, firstDeliveryDateTime);
            Log(deliveryLog, "Фильтрация заказов для района " + cityDistrict + " и времени " + firstDeliveryDateTime + " успешно завершена.");


            foreach (var order in filteredOrders)
            {
                Writer(deliveryOrder, order.ToString());
            }

            Log(deliveryLog, "Запись результатов фильтрации заказов для доставки в конкретный район города в ближайшие полчаса" + inputFile);




        }
        private static bool ValidateArgs(string[] args)
        {
            bool result;
            if (args.Length == 0)
            {
                Console.WriteLine("Аргументы не найдены");
                result = false;
            }
            else if (args.Length < 5)
            {
                Console.WriteLine("Недостаточно аргументов командной строки");
                result = false;
            }
            else
            {
                DateTime isTrue;
                result = ((!DateTime.TryParse(args[1], out isTrue)) || (PathCheck(args[2])) || (PathCheck(args[3])) || (PathCheck(args[4])));
            }
            return result;
        }
        private static bool PathCheck(string path)
        {
            return File.Exists(path);

        }

        private static void Log(string file, string message)
        {
            Writer(file, message);
        }

        private static Order ParseLine(string line)
        {
            try
            {
                var orderElements = line.Split(';');
                Order order = new Order
                {
                    OrderNumber = int.Parse(orderElements[0]),
                    Weight = int.Parse(orderElements[1]),
                    District = orderElements[2],
                    DeliveryTime = DateTime.Parse(orderElements[3]),

                };

                return order;
            }
            catch (Exception e)
            {
                Console.WriteLine("Фаил содрежит данные неверного типа");
                return null;
            }

        }

        private static List<Order> FilterOrders(List<Order> orders, string cityDistrict, string firstDeliveryDateTime)
        {
            var filterOrder = orders.Where(order =>
            order.DeliveryTime.Minute < 30 &&
            order.District.ToLower() == cityDistrict.ToLower() &&
            order.DeliveryTime >= DateTime.Parse(firstDeliveryDateTime)
            ).ToList();

            return filterOrder;
        }
        private static void Writer(string filePath, string text, bool needToCreateNewFile)
        {
            
            if (needToCreateNewFile)
            {
                using (StreamWriter writer = new StreamWriter(filePath, true))
                {
                    writer.WriteLine(text);
                }
            }
            else
            {

            }
        }
    }
}


