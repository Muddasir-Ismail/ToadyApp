using eTickets.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace eTickets.wwwroot.json.Services
{
    public class OrdersService : IOrdersService
    {
        private readonly AppDbContext _context;

        public OrdersService(AppDbContext context)
        {
            _context = context;
        }

        public List<Order> GetOrdersByUserIdAndRole(string userId, string userRole)
        {
            var orders = _context.Orders
                .Include(n => n.OrderItems).ThenInclude(n => n.Movie)
                .Include(n => n.User)
                .ToList();

            if (userRole != "Admin")
            {
                orders = orders.Where(n => n.UserId == userId).ToList();
            }

            return orders;
        }

        public void StoreOrder(List<ShoppingCartItem> items, string userId, string userEmailAddress)
        {
            var order = new Order()
            {
                UserId = userId,
                Email = userEmailAddress
            };
            _context.Orders.Add(order);
            _context.SaveChanges();

            foreach (var item in items)
            {
                var orderItem = new OrderItem()
                {
                    Amount = item.Amount,
                    MovieId = item.Movie.Id,
                    OrderId = order.Id,
                    Price = item.Movie.Price
                };
                _context.OrderItems.Add(orderItem);
            }
            _context.SaveChanges();
        }
    }
}
