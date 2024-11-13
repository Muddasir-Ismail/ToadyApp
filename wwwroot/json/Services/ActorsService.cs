using eTickets.Models;
using eTickets.wwwroot.json.Base;
using Microsoft.EntityFrameworkCore;

namespace eTickets.wwwroot.json.Services
{
    public class ActorsService : EntityBaseRepository<Actor>, IActorsService
    {
        public ActorsService(AppDbContext context) : base(context) { }

    }
}
