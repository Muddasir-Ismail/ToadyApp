using eTickets.Models;
using System.Collections.Generic;
using System.Linq;

namespace eTickets.wwwroot.json.Services
{
    public class CinemasService : ICinemaService
    {
        private readonly AppDbContext _context;

        public CinemasService(AppDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Cinema> GetAll()
        {
            return _context.Cinemas.ToList();
        }

        public Cinema GetById(int id)
        {
            return _context.Cinemas.FirstOrDefault(c => c.Id == id);
        }

        public void Add(Cinema cinema)
        {
            _context.Cinemas.Add(cinema);
            _context.SaveChanges();
        }

        public Cinema Update(int id, Cinema updatedCinema)
        {
            var cinema = _context.Cinemas.Find(id);
            if (cinema != null)
            {
                cinema.Name = updatedCinema.Name;
                cinema.Logo = updatedCinema.Logo;
                cinema.Description = updatedCinema.Description;
                _context.SaveChanges();
            }
            return cinema;
        }

        public void Delete(int id)
        {
            var cinema = _context.Cinemas.Find(id);
            if (cinema != null)
            {
                _context.Cinemas.Remove(cinema);
                _context.SaveChanges();
            }
        }
    }
}
