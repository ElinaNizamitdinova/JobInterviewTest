using System.Text.RegularExpressions;

namespace JobInterviewTest
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string cityDistrict = args[0];
            string firstDeliveryDateTime = args[1];
            string deliveryLog = args[2];
            string deliveryOrder = args[3];

            string _inputFile = "input.txt";
            string[] lines = File.ReadAllLines(_inputFile);
            Log(deliveryLog, "Чтение данных из файла " + _inputFile);


            List<Order> orders = new List<Order>();
            foreach (string line in lines)
            {

                orders.Add(ParseLine(line));
            }


            var filteredOrders = FilterOrders(orders, cityDistrict, firstDeliveryDateTime);
            Log(deliveryLog, "Фильтрация заказов для района " + cityDistrict + " и времени " + firstDeliveryDateTime + " успешно завершена.");


            File.WriteAllLines(deliveryOrder, filteredOrders.Select(order => order.ToString()));
            Log(deliveryLog, "Запись результатов фильтрации заказов для доставки в конкретный район города в ближайшие полчаса" + _inputFile);




        }

        private static void Log(string file, string message)
        {
            File.WriteAllText(file, message);
        }

        private static Order ParseLine(string line)
        {
            Match match = Regex.Match(line, @"^\d+$");
            if (!match.Success)
            {
                Console.WriteLine("Некорректный номер заказа: " + line);
                return null;
            }

            Order order = new Order();
            order.OrderNumber = int.Parse(match.Value);
            order.Weight = double.Parse(line.Split(' ')[1]);
            order.District = line.Split(' ')[2];
            order.DeliveryTime = DateTime.Parse(line.Split(' ')[3]);
            return order;
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
    }
}


