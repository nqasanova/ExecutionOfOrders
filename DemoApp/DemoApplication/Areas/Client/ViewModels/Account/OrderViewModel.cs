using System;
namespace DemoApplication.Areas.Client.ViewModels.Account
{
    public class OrderViewModel
    {
        public string Id { get; set; }
        public string Status { get; set; }
        public decimal Total { get; set; }
        public DateTime CreatedAt { get; set; }

        public OrderViewModel(string id, string status, decimal total, DateTime createdAt)
        {
            Id = id;
            Status = status;
            Total = total;
            CreatedAt = createdAt;
        }
    }
}