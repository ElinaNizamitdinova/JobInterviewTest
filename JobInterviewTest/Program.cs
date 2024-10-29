using CommandLine;
using JobInterviewTest;
using Microsoft.VisualBasic.FileIO;
using System.Configuration;
using System.Globalization;

namespace DeliveryService
{
    public class Program
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
                         Logger.Log(options.logFilePath, "Некорректное время первой доставки");
                         return 0;
                     }

                     string orderPath = ConfigurationManager.AppSettings["Orders"];

                     List<Order> orders = Order.LoadOrders(orderPath, options.logFilePath);

                     List<Order> filteredOrders = Order.FilterOrders(orders, options.cityDistrict, firstDeliveryTime);

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
                    Logger.Log(logFilePath, $"Сохранены отфильтрованные заказы: {orders.Count}");
                    Console.WriteLine("Результаты фильтрации записаны в файл по пути: " + resultFilePath);
                }
                catch (Exception ex)
                {
                    Logger.Log(logFilePath, $"Ошибка записи результатов: {ex.Message}");
                }
            }
        }


    }