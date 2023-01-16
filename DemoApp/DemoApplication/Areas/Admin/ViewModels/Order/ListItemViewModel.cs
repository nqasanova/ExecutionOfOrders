using System;
namespace DemoApplication.Areas.Admin.ViewModels.Order
{
    public class ListItemViewModel
    {
        public string Id { get; set; }
        public string Client { get; set; }
        public string OrderStatus { get; set; }
        public decimal OrderSum { get; set; }
        public DateTime OrderTime { get; set; }
    }
}