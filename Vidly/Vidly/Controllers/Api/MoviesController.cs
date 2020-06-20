using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Vidly.App_Start;
using Vidly.Dtos;
using Vidly.Models;

namespace Vidly.Controllers.Api
{
    public class MoviesController : ApiController
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public MoviesController()
        {
            _context = new ApplicationDbContext();
            _mapper = MappingProfile.Configure().CreateMapper();
        }

        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
        }

        [HttpGet]
        public IEnumerable<MovieDto> GetMovies()
        {
            var movies = _context.Movies.ToList().Select(_mapper.Map<Movie, MovieDto>);
            return movies;
        }

        [HttpGet]
        public MovieDto GetMovie(int id)
        {
            var movie = _context.Movies.FirstOrDefault(m => m.Id == id);

            if (movie == null)
                throw new HttpResponseException(HttpStatusCode.NotFound);

            var returnMovieDto = _mapper.Map<Movie, MovieDto>(movie);

            return returnMovieDto;
        }

        [HttpPost]
        public IHttpActionResult AddMovie(MovieDto movieDto)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            movieDto.DateAdded = DateTime.Now;

            var movie = _mapper.Map<MovieDto, Movie>(movieDto);

            _context.Movies.Add(movie);
            _context.SaveChanges();

            movieDto.Id = movie.Id;

            return Created(new Uri(Request.RequestUri +"/"+ movie.Id), movieDto);
        }

        [HttpPut]
        public void UpdateMovie(int id, MovieDto movieDto)
        {
            if (!ModelState.IsValid)
                throw new HttpResponseException(HttpStatusCode.BadRequest);

            var movieInDb = _context.Movies.FirstOrDefault(m => m.Id == id);

            if (movieInDb == null)
                throw new HttpResponseException(HttpStatusCode.NotFound);

            _mapper.Map<MovieDto, Movie>(movieDto, movieInDb);                        

            _context.SaveChanges();
        }

        [HttpDelete]
        public void DeleteMovie(int id)
        {
            var movie = _context.Movies.FirstOrDefault(m => m.Id == id);

            _context.Movies.Remove(movie);
            _context.SaveChanges();
        }
    }
}
