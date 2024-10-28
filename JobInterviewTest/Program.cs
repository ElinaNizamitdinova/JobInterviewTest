using CommandLine;
using JobInterviewTest;
using Microsoft.VisualBasic.FileIO;
using System.Diagnostics;
using System.Globalization;
using System.Configuration;

namespace DeliveryService
{
    class Program
    {

        static async Task<int> Main(string[] args)
        {

            var parserResult = Parser.Default.ParseArguments<Options>(args);

            return await parserResult.MapResult(
             async options =>
             {

                 try
                 {
                    
                     DateTime firstDeliveryTime;
                     if (!DateTime.TryParseExact(options.firstDeliveryTimeStr, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out firstDeliveryTime))
                     {
                         Log(options.logFilePath, "Некорректное время первой доставки");
                         return 0;
                     }

                  
                     string orderPath = ConfigurationManager.AppSettings["Orders"];

                     // Загрузка и валидация данных
                     List<Order> orders = LoadOrders(orderPath,options.logFilePath);


                     // Фильтрация заказов

                     List<Order> filteredOrders = FilterOrders(orders, options.cityDistrict, firstDeliveryTime);

                     // Запись результата
                     SaveFilteredOrders(filteredOrders, options.resultFilePath, options.logFilePath);

                     return 0;
                 }
                 catch (Exception ex)
                 {

                     return 1;
                 }
                 finally
                 {

                 }
             },
             errs => Task.FromResult(1));

        }

        static void Test(string str)
        {
            Debug.WriteLine(str);
        }

        static string GetArgument(string[] args, string name, string defaultValue)
        {
            foreach (string arg in args)
            {
                if (arg.StartsWith(name))
                {
                    return arg.Split('-')[1];
                }
            }
            return defaultValue;

        }

        static void Log(string logFilePath, string message)
        {
            // Проверка и создание файла логов, если его нет
            if (!File.Exists(logFilePath))
            {
                using (FileStream fs = File.Create(logFilePath))
                {
                    // Файл создан, можно закрыть FileStream
                }
            }

            // Запись сообщения в лог
            using (StreamWriter logFile = new StreamWriter(logFilePath, true))
            {
                logFile.WriteLine($"{DateTime.Now:yyyy-MM-dd HH:mm:ss} - {message}");
            }
        }

        static List<Order> LoadOrders(string filePath, string logFilePath)
        {
            List<Order> orders = new List<Order>();
            try
            {
                using (TextFieldParser parser = new TextFieldParser(filePath))
                {
                    parser.TextFieldType = FieldType.Delimited;
                    parser.SetDelimiters(";");

                    // Пропускаем заголовок
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
                Log(logFilePath, $"Ошибка загрузки заказов: {ex.Message}");
            }
            return orders;
        }

        static bool ValidateOrderData(string[] data, string logFilePath)
        {
            bool isValid = int.TryParse(data[0], out _) &&
                           double.TryParse(data[1], out _) &&
                           DateTime.TryParseExact(data[3], "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out _);

            if (!isValid)
            {
                Log(logFilePath, $"Ошибка валидации данных: {string.Join(", ", data)}");
            }
            return isValid;
        }

        static List<Order> FilterOrders(List<Order> orders, string district, DateTime firstDeliveryTime)
        {
            DateTime endTime = firstDeliveryTime.AddMinutes(30);
            List<Order> filteredOrders = orders.Where(order => order.District == district &&
                                         order.DeliveryTime >= firstDeliveryTime &&
                                         order.DeliveryTime <= endTime).ToList();
            return filteredOrders;
        }

        static void SaveFilteredOrders(List<Order> orders, string resultFilePath, string logFilePath)
        {
            try
            {
                using (StreamWriter writer = new StreamWriter(resultFilePath))
                {
                    foreach (var order in orders)
                    {
                        writer.WriteLine($"{order.OrderId}, {order.Weight}, {order.District}, {order.DeliveryTime:yyyy-MM-dd HH:mm:ss}");
                    }
                }
                Log(logFilePath, $"Сохранены отфильтрованные заказы: {orders.Count}");
            }
            catch (Exception ex)
            {
                Log(logFilePath, $"Ошибка записи результатов: {ex.Message}");
            }
        }
    }


}