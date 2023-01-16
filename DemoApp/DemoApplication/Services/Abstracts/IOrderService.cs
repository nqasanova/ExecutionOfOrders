using System;
using DemoApplication.Areas.Client.ViewModels.Account;
using DemoApplication.Database.Models;

namespace DemoApplication.Services.Abstracts
{
    public interface IOrderService
    {
        public string OrderCode { get; }
    }
}