using System;
namespace DemoApplication.Contracts.Order
{
    public enum OrderStatus
    {
        Created = 0,
        Confirmed = 1,
        Rejected = 2,
        Sended = 4,
        Completed = 8
    }

    public static class StatusStatusCode
    {
        public static string GetStatusCode(this OrderStatus status)
        {
            switch (status)
            {
                case OrderStatus.Created:
                    return "Created";
                case OrderStatus.Confirmed:
                    return "Confirmed";
                case OrderStatus.Rejected:
                    return "Rejected";
                case OrderStatus.Sended:
                    return "Sent";
                case OrderStatus.Completed:
                    return "Completed";
                default:
                    throw new Exception("Status code not found");
            }
        }
    }
}