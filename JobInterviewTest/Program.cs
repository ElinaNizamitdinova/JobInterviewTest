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
            }

            string cityDistrict = args[0];
            string firstDeliveryDateTime = args[1];
            string deliveryLog = args[2];
            string deliveryOrder = args[3];
            string inputFile = args[4];

            string[] lines = File.ReadAllLines(inputFile);
            Log(deliveryLog, "Чтение данных из файла " + inputFile);


            List<Order> orders = new List<Order>();
            foreach (string line in lines)
            {

                orders.Add(ParseLine(line));
            }


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
            else if (args.Length == 4)
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
            if (!File.Exists(path))
            {
                return false;
            }
            return true;
        }

        private static void Log(string file, string message)
        {
            Writer(file, message);
        }

        private static Order ParseLine(string line)
        {
            try
            {
                Order order = new Order();
                order.OrderNumber = int.Parse(line.Split(';')[0]);
                order.Weight = double.Parse(line.Split(';')[1]);
                order.District = line.Split(';')[2];
                order.DeliveryTime = DateTime.Parse(line.Split(';')[3]);
                return order;
            }
            catch
            {
                Console.WriteLine("Фаил содеежит данные неверного типа");
                return new Order();
            }

        }

        private static List<Order> FilterOrders(List<Order> orders, string cityDistrict, string firstDeliveryDateTime)
        {
            try
            {
                var tmp = orders.Where(order =>
                order.DeliveryTime.Minute < 30 &&
                order.District.ToLower() == cityDistrict.ToLower() &&
                order.DeliveryTime >= DateTime.Parse(firstDeliveryDateTime)
                ).ToList();

                return tmp;
            }
            catch (Exception e)
            {
                Console.WriteLine("Ошибка при фильтрации заказов: " + e.Message);
                return new List<Order>();
            }
        }
        private static void Writer(string filePath, string text )
        {
            using (StreamWriter writer = new StreamWriter(filePath, true))
            {
                writer.WriteLine(text);
            }
        }
    }
}


