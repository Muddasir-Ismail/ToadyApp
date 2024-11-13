using eTickets.wwwroot.json.ViewModels;
using eTickets.wwwroot.json.Cart;
using eTickets.wwwroot.json.Services;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace eTickets.Controllers
{
    public class OrdersController : Controller
    {
        private readonly IMoviesService _moviesService;
        private readonly ShoppingCart _shoppingCart;
        private readonly IOrdersService _ordersService;

        public OrdersController(IMoviesService moviesService, ShoppingCart shoppingCart, IOrdersService ordersService)
        {
            _moviesService = moviesService;
            _shoppingCart = shoppingCart;
            _ordersService = ordersService;
        }

        public IActionResult Index()
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            string userRole = User.FindFirstValue(ClaimTypes.Role);

            var orders = _ordersService.GetOrdersByUserIdAndRole(userId, userRole);
            return View(orders);
        }

        public IActionResult ShoppingCart()
        {
            var items = _shoppingCart.GetShoppingCartItems();
            _shoppingCart.ShoppingCartItems = items;

            // Explicitly casting decimal to double, if needed
            var response = new ShoppingCartVM()
            {
                ShoppingCart = _shoppingCart,
                ShoppingCartTotal = (double)_shoppingCart.GetShoppingCartTotal() // Explicit cast
            };

            return View(response);
        }

        public IActionResult AddItemToShoppingCart(int id)
        {
            var item = _moviesService.GetMovieById(id);

            if (item != null)
            {
                // Ensure ShoppingCartId is set before adding
                if (string.IsNullOrEmpty(_shoppingCart.ShoppingCartId))
                {
                    throw new InvalidOperationException("ShoppingCartId must be set before adding items to the cart.");
                }

                _shoppingCart.AddItemToCart(item);
            }
            return RedirectToAction(nameof(ShoppingCart));
        }


        public IActionResult RemoveItemFromShoppingCart(int id)
        {
            var item = _moviesService.GetMovieById(id);

            if (item != null)
            {
                _shoppingCart.RemoveItemFromCart(item);
            }
            return RedirectToAction(nameof(ShoppingCart));
        }

        public IActionResult CompleteOrder()
        {
            var items = _shoppingCart.GetShoppingCartItems();
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            string userEmailAddress = User.FindFirstValue(ClaimTypes.Email);

            _ordersService.StoreOrder(items, userId, userEmailAddress);
            _shoppingCart.ClearShoppingCart();

            return View("OrderCompleted");
        }
    }
}
