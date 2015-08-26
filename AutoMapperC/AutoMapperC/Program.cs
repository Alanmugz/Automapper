using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using NUnit.Framework;
using Should;

namespace AutoMapperSamples
{
    namespace Flattening
    {
        public class Order
        {
            private readonly IList<OrderLineItem> _orderLineItems = new List<OrderLineItem>();

            public Customer Customer { get; set; }

            public OrderLineItem[] GetOrderLineItems()
            {
                return _orderLineItems.ToArray();
            }

            public void AddOrderLineItem(Product product, int quantity)
            {
                _orderLineItems.Add(new OrderLineItem(product, quantity));
            }

            public decimal GetTotal()
            {
                return _orderLineItems.Sum(item => item.GetTotal());
            }
        }

        public class Product
        {
            public decimal Price { get; set; }
            public string Name { get; set; }
        }

        public class OrderLineItem
        {
            public OrderLineItem(Product product, int quantity)
            {
                Product = product;
                Quantity = quantity;
            }

            public Product Product { get; private set; }
            public int Quantity { get; private set; }

            public decimal GetTotal()
            {
                return Quantity * Product.Price;
            }
        }

        public class Customer
        {
            public string Name { get; set; }
        }

        public class OrderDto
        {
            public string CustomerName { get; set; }
            public decimal Total { get; set; }
        }

        [TestFixture]
        public class Flattening
        {
            [Test]
            public void Example()
            {
                // Complex model
                var customer = new Customer
                {
                    Name = "John Doe"
                };
                var order = new Order
                {
                    Customer = customer
                };
                var tv = new Product
                {
                    Name = "Sony",
                    Price = 4.99m
                };
                order.AddOrderLineItem(tv, 15);

                // Configure AutoMapper
                Mapper.CreateMap<Order, OrderDto>();

                // Perform mapping
                OrderDto dto = Mapper.Map<Order, OrderDto>(order);

                dto.CustomerName.ShouldEqual("John Doe");
                dto.Total.ShouldEqual(74.85m);
            }

            static void Main(string[] args)
            {

            }
        }
    }
}