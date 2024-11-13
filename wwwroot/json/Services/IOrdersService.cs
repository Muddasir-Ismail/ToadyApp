using eTickets.Models;
using System.Collections.Generic;

namespace eTickets.wwwroot.json.Services
{
    public interface IOrdersService
    {
        void StoreOrder(List<ShoppingCartItem> items, string userId, string userEmailAddress);
        List<Order> GetOrdersByUserIdAndRole(string userId, string userRole);
    }
}
