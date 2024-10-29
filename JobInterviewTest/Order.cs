
using Microsoft.VisualBasic.FileIO;
using System.Globalization;

namespace JobInterviewTest
{
    public class Order
    {
        public string? OrderId { get; set; }
        public double Weight { get; set; }
        public string? District { get; set; }
        public DateTime DeliveryTime { get; set; }


       public static List<Order> LoadOrders(string filePath, string logFilePath)
        {
            List<Order> orders = new List<Order>();
            try
            {
                using (TextFieldParser parser = new TextFieldParser(filePath))
                {
                    parser.TextFieldType = FieldType.Delimited;
                    parser.SetDelimiters(";");


                    parser.ReadLine();

                    while (!parser.EndOfData)
                    {
                        string[]? fields = parser.ReadFields();
                        if (fields.Length == 4 && ValidateOrderData(fields, logFilePath))
                        {
                            orders.Add(new Order
                            {
                                OrderId = fields[0],
                                Weight = double.Parse(fields[1]),
                                District = fields[2],
                                DeliveryTime = DateTime.ParseExact(fields[3], "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Log(logFilePath, $"Ошибка загрузки заказов: {ex.Message}");
            }
            return orders;
        }
       public static bool ValidateOrderData(string[] data, string logFilePath)
        {
            bool isValid = int.TryParse(data[0], out _) &&
                           double.TryParse(data[1], out _) &&
                           DateTime.TryParseExact(data[3], "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out _);

            if (!isValid)
            {
                Logger.Log(logFilePath, $"Ошибка валидации данных: {string.Join(", ", data)}");
            }
            return isValid;
        }
       public static List<Order> FilterOrders(List<Order> orders, string district, DateTime firstDeliveryTime)
        {
            DateTime endTime = firstDeliveryTime.AddMinutes(30);
            List<Order> filteredOrders = orders.Where(order => order.District == district &&
                                         order.DeliveryTime >= firstDeliveryTime &&
                                         order.DeliveryTime <= endTime).ToList();
            return filteredOrders;
        }
    }


}
