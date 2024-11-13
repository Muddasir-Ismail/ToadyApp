using eTickets.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace eTickets.wwwroot.json.Cart
{
    public class ShoppingCart
    {
        private readonly AppDbContext _context;
        public string ShoppingCartId { get; set; } // Ensure this is set correctly
        public List<ShoppingCartItem> ShoppingCartItems { get; set; } = new List<ShoppingCartItem>();

        public ShoppingCart(AppDbContext context)
        {
            _context = context;
            // Generate a unique ID for the ShoppingCart if not already set
            if (string.IsNullOrEmpty(ShoppingCartId))
            {
                ShoppingCartId = Guid.NewGuid().ToString();
            }
        }

        // Method to get all items in the shopping cart
        public List<ShoppingCartItem> GetShoppingCartItems()
        {
            return _context.ShoppingCartItems
                .Include(s => s.Movie)
                .Where(c => c.ShoppingCartId == ShoppingCartId)
                .ToList();
        }

        // Method to calculate the total cost of items in the cart
        public decimal GetShoppingCartTotal()
        {
            return _context.ShoppingCartItems
                .Where(c => c.ShoppingCartId == ShoppingCartId)
                .Select(item => (decimal)item.Movie.Price * item.Amount) // Explicitly cast 'Price' to decimal
                .Sum();
        }

        // Method to add an item to the cart
        public void AddItemToCart(Movie movie)
        {
            if (string.IsNullOrEmpty(ShoppingCartId))
            {
                throw new InvalidOperationException("ShoppingCartId must be set before adding items to the cart.");
            }

            var shoppingCartItem = _context.ShoppingCartItems
                .FirstOrDefault(s => s.Movie.Id == movie.Id && s.ShoppingCartId == ShoppingCartId);

            if (shoppingCartItem == null)
            {
                shoppingCartItem = new ShoppingCartItem
                {
                    ShoppingCartId = ShoppingCartId,
                    Movie = movie,
                    Amount = 1
                };
                _context.ShoppingCartItems.Add(shoppingCartItem);
            }
            else
            {
                shoppingCartItem.Amount++;
            }

            _context.SaveChanges();
        }

        // Method to remove an item from the cart
        public void RemoveItemFromCart(Movie movie)
        {
            var shoppingCartItem = _context.ShoppingCartItems
                .FirstOrDefault(s => s.Movie.Id == movie.Id && s.ShoppingCartId == ShoppingCartId);

            if (shoppingCartItem != null)
            {
                if (shoppingCartItem.Amount > 1)
                {
                    shoppingCartItem.Amount--;
                }
                else
                {
                    _context.ShoppingCartItems.Remove(shoppingCartItem);
                }
                _context.SaveChanges();
            }
        }

        // Method to clear all items from the shopping cart
        public void ClearShoppingCart()
        {
            var items = _context.ShoppingCartItems
                .Where(cart => cart.ShoppingCartId == ShoppingCartId)
                .ToList();
            _context.ShoppingCartItems.RemoveRange(items);
            _context.SaveChanges();
        }

        // Method to get a ShoppingCart instance
        // Method to get a ShoppingCart instance
        public static ShoppingCart GetShoppingCart(IServiceProvider services)
        {
            var context = services.GetService<AppDbContext>();
            var httpContextAccessor = services.GetService<IHttpContextAccessor>();
            var session = httpContextAccessor?.HttpContext?.Session;

            if (session == null)
            {
                throw new InvalidOperationException("Session cannot be null");
            }

            string cartId = session.GetString("CartId") ?? Guid.NewGuid().ToString();
            session.SetString("CartId", cartId);

            return new ShoppingCart(context) { ShoppingCartId = cartId };
        }

    }
}
