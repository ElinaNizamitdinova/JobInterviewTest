using CommandLine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeliveryTest
{
    public class Options
    {
        [Option('l', "logPath", Required = true, HelpText = "Log path directory.")]
        public string logFilePath { get; set; }

        [Option('r', "resultPath", Required = true, HelpText = "Result file path directory.")]
        public string resultFilePath { get; set; }
        [Option('d', "district", Required = true, HelpText = "Order's dictrict name")]
        public string cityDistrict { get; set; }
        [Option('t', "dateTime", Required = true, HelpText = "Order date in format \"yyyy-MM-dd HH:mm:ss\"")]
        public string firstDeliveryTimeStr { get; set; }
    }
}
