using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Vidly.Models;
using Vidly.ViewModels;

namespace Vidly.Controllers
{
    public class MoviesController : Controller
    {
        //GET: Movies/Index
        public ActionResult Index()
        {
            var movies = GetMovies().ToList();

            return View(movies);
        }

        //GET: Movies/Details/Id
        public ActionResult Details(int id)
        {
            var movie = GetMovies().FirstOrDefault(m => m.Id == id);

            if (movie == null)
                return HttpNotFound();

            return View(movie);
        }

        private IEnumerable<Movie> GetMovies()
        {
            var movies = new List<Movie>()
            {
                new Movie { Id=1, Name="Sairat" },
                new Movie { Id=2, Name="Baban" },
                new Movie { Id=3, Name="Babu Band Baja" }
            };

            return movies;
        }
    }
}