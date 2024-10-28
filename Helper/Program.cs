namespace Helper
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var str = @"-_cityDistrict Central -_firstDeliveryDateTime 2021-01-01 10:01:00 -_deliveryLog E:\elina\JobInterviewTest\JobInterviewTest\delivery_log.txt -_deliveryOrder E:\elina\JobInterviewTest\JobInterviewTest\filtered_orders.txt";
            string logFilePath = GetArgument(str, "-_deliveryLog", "delivery_log.txt");
            string resultFilePath = GetArgument(str, "-_deliveryOrder", "filtered_orders.txt");
            string cityDistrict = GetArgument(str, "-_cityDistrict", "Central");
            string firstDeliveryTimeStr = GetArgument(str, "-_firstDeliveryDateTime", "");

            Console.WriteLine(logFilePath);
            Console.WriteLine( resultFilePath);
            Console.WriteLine(cityDistrict);
            Console.WriteLine(firstDeliveryTimeStr);
        }
        static string GetArgument(string args, string name, string defaultValue)
        {
            string[] strings = args.Split("-");
            foreach (string s in strings)
            {
                if (s.StartsWith(name))
                {
                    string result = s.Split(" ")[1];
                    return result;
                }
            }
            return defaultValue;
        }

    }
}
