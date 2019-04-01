using System;
using System.Collections.Generic;
using System.Linq;

namespace ODataFaq.DataModel
{
    public class OrderManagementInMemoryData
	{
        public List<Customer> Customers { get; } = new List<Customer>();
        public IQueryable<Customer> CustomersQueryable { get => Customers.AsQueryable(); }

        public List<Product> Products { get; } = new List<Product>();
        public IQueryable<Product> ProductsQueryable { get => Products.AsQueryable(); }

        public List<OrderHeader> Orders { get; } = new List<OrderHeader>();
        public IQueryable<OrderHeader> OrdersQueryable { get => Orders.AsQueryable(); }

        public List<OrderDetail> OrderDetails { get; } = new List<OrderDetail>();
        public IQueryable<OrderDetail> OrderDetailQueryable { get => OrderDetails.AsQueryable(); }

        OrderManagementInMemoryData()
		{
            Customers.Add(new Customer() { CompanyName = "Corina Air Conditioning", CountryIsoCode = "AT" });
            Customers.Add(new Customer() { CompanyName = "Fernando Engineering", CountryIsoCode = "AT" });
            Customers.Add(new Customer() { CompanyName = "Murakami Plumbing", CountryIsoCode = "CH" });
            Customers.Add(new Customer() { CompanyName = "Naval Metal Construction", CountryIsoCode = "DE" });

            Products.Add(new Product() { Description = "Mountain Bike", IsAvailable = true, CategoryCode = "BIKE", PricePerUom = 2500 });
            Products.Add(new Product() { Description = "Road Bike", IsAvailable = true, CategoryCode = "BIKE", PricePerUom = 2000 });
            Products.Add(new Product() { Description = "Skate Board", IsAvailable = true, CategoryCode = "BOARD", PricePerUom = 100 });
            Products.Add(new Product() { Description = "Long Board", IsAvailable = true, CategoryCode = "BOARD", PricePerUom = 250 });
            Products.Add(new Product() { Description = "Scooter", IsAvailable = false, CategoryCode = "OTHERS", PricePerUom = 150 });

			var rand = new Random();
			for (var i = 0; i < 100; i++)
			{
				var order = new OrderHeader()
				{
					OrderDate = new DateTimeOffset(new DateTime(2014, rand.Next(1, 12), rand.Next(1, 28))),
					Customer = Customers[rand.Next(Customers.Count - 1)]
				};
				Orders.Add(order);

				for (var j = 0; j < 3; j++)
				{
					OrderDetails.Add(new OrderDetail()
					{
						Order = order,
						Product = Products[rand.Next(Products.Count - 1)],
						Amount = rand.Next(1, 5)
					});
				}
			}
		}
	}
}
