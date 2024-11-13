using eTickets.Models;
using eTickets.wwwroot.json.Base;
using eTickets.wwwroot.json.ViewModels;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace eTickets.wwwroot.json.Services
{
    public class MoviesService : EntityBaseRepository<Movie>, IMoviesService
    {
        private readonly AppDbContext _context;

        public MoviesService(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public void AddNewMovie(NewMovieVM data)
        {
            var newMovie = new Movie()
            {
                Name = data.Name,
                Description = data.Description,
                Price = data.Price,
                ImageURL = data.ImageURL,
                CinemaId = data.CinemaId,
                StartDate = data.StartDate,
                EndDate = data.EndDate,
                MovieCategory = data.MovieCategory,
                ProducerId = data.ProducerId
            };
            _context.Movies.Add(newMovie);
            _context.SaveChanges();

            // Add Movie Actors
            foreach (var actorId in data.ActorIds)
            {
                var newActorMovie = new Actor_Movie()
                {
                    MovieId = newMovie.Id,
                    ActorId = actorId
                };
                _context.Actors_Movies.Add(newActorMovie);
            }
            _context.SaveChanges();
        }

        public Movie GetMovieById(int id)
        {
            var movieDetails = _context.Movies
                .Include(c => c.Cinema)
                .Include(p => p.Producer)
                .Include(am => am.Actors_Movies).ThenInclude(a => a.Actor)
                .FirstOrDefault(n => n.Id == id);
            return movieDetails;
        }

        public NewMovieDropdownsVM GetNewMovieDropdownsValues()
        {
            var response = new NewMovieDropdownsVM()
            {
                Actors = _context.Actors.OrderBy(n => n.FullName).ToList(),
                Cinemas = _context.Cinemas.OrderBy(n => n.Name).ToList(),
                Producers = _context.Producers.OrderBy(n => n.FullName).ToList()
            };

            return response;
        }

        public void UpdateMovie(NewMovieVM data)
        {
            var dbMovie = _context.Movies.FirstOrDefault(n => n.Id == data.Id);

            if (dbMovie != null)
            {
                dbMovie.Name = data.Name;
                dbMovie.Description = data.Description;
                dbMovie.Price = data.Price;
                dbMovie.ImageURL = data.ImageURL;
                dbMovie.CinemaId = data.CinemaId;
                dbMovie.StartDate = data.StartDate;
                dbMovie.EndDate = data.EndDate;
                dbMovie.MovieCategory = data.MovieCategory;
                dbMovie.ProducerId = data.ProducerId;
                _context.SaveChanges();
            }

            // Remove existing actors
            var existingActorsDb = _context.Actors_Movies.Where(n => n.MovieId == data.Id).ToList();
            _context.Actors_Movies.RemoveRange(existingActorsDb);
            _context.SaveChanges();

            // Add Movie Actors
            foreach (var actorId in data.ActorIds)
            {
                var newActorMovie = new Actor_Movie()
                {
                    MovieId = data.Id,
                    ActorId = actorId
                };
                _context.Actors_Movies.Add(newActorMovie);
            }
            _context.SaveChanges();
        }
    }
}
