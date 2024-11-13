using eTickets.Models;
using eTickets.wwwroot.json.Base;

namespace eTickets.wwwroot.json.Services
{
    public interface IActorsService : IEntityBaseRepository<Actor>
    {
        // No additional methods to change if IEntityBaseRepository<Actor> is already synchronous
    }
}
