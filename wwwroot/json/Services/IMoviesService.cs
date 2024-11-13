using eTickets.Models;
using eTickets.wwwroot.json.Base;
using eTickets.wwwroot.json.ViewModels;

namespace eTickets.wwwroot.json.Services
{
    public interface IMoviesService : IEntityBaseRepository<Movie>
    {
        Movie GetMovieById(int id);
        NewMovieDropdownsVM GetNewMovieDropdownsValues();
        void AddNewMovie(NewMovieVM data);
        void UpdateMovie(NewMovieVM data);
    }
}
