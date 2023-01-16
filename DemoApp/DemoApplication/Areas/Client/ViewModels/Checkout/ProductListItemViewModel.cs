using System;
namespace DemoApplication.Areas.Client.ViewModels.Checkout
{
    public class ProductListItemViewModel
    {
        public int BookId { get; set; }
        public List<ListItem>? Products { get; set; }

        public class ListItem
        {
            public int Id { get; set; }
            public int Quantity { get; set; }
            public string? Title { get; set; }
            public decimal Price { get; set; }
            public decimal Total { get; set; }

            public ListItem(int id, int quantity, string title, decimal price, decimal total)
            {
                Id = id;
                Quantity = quantity;
                Title = title;
                Price = price;
                Total = total;
            }
        }
    }
}