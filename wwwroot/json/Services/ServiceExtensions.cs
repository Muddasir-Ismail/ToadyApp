using eTickets.wwwroot.json;
using eTickets.Models;
using eTickets.wwwroot.json.Cart;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace eTickets.wwwroot.json.Services
{
    public static class ServiceExtensions
    {
        public static IServiceCollection AddCustomServices(this IServiceCollection services, IConfiguration configuration)
        {
            // DbContext configuration
            services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnectionString")));

            // Configure Identity
            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<AppDbContext>();

            // Set Authentication Defaults
            services.AddAuthentication(options =>
            {
                options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            });

            // Register custom services
            services.AddScoped<IActorsService, ActorsService>();
            services.AddScoped<ICinemaService, CinemasService>();
            services.AddScoped<IProducersService, ProducersService>();
            services.AddScoped<IMoviesService, MoviesService>();
            services.AddScoped<IOrdersService, OrdersService>();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddScoped(sc => ShoppingCart.GetShoppingCart(sc));

            // Configure session and caching
            services.AddMemoryCache();
            services.AddSession();

            return services;
        }
    }
}
