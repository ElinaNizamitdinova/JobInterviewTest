using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobInterviewTest
{
    public class Order
    {
        public int OrderNumber { get; set; }

        public double Weight { get; set; }

     
        public string District { get; set; }

        public DateTime DeliveryTime { get; set; }

        public Order()
        {

        }
        public Order(int orderNumber, double weight, string district, DateTime deliveryTime)
        {
            OrderNumber = orderNumber;
            Weight = weight;
            District = district;
            DeliveryTime = deliveryTime;
        }
    }


}
