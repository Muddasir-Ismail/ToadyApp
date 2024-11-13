using eTickets.Models;
using System.Collections.Generic;

namespace eTickets.wwwroot.json.Services
{
    public interface ICinemaService
    {
        IEnumerable<Cinema> GetAll();
        Cinema GetById(int id);
        void Add(Cinema cinema);
        Cinema Update(int id, Cinema newCinema);
        void Delete(int id);
    }
}
