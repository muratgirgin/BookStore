using System;
using System.Linq;
using WebApi.DBOperations;
using WebApi.Entities;

namespace  WebApi.Application.GenreOperations.CreateGenre 
{
    public class CreateGenreCommand 
    { 
        public CreateGenreModel Model { get; set; }

        private readonly BookStoreDBContext _context;

        public CreateGenreCommand(BookStoreDBContext context)
        {
            _context = context;
        }

        public void Handle()
        {
            var genre = _context.Genres.SingleOrDefault(x => x.Name == Model.Name); 
            if (genre is not null)
                throw new InvalidOperationException("Genre already exists");
            
            genre = new Genre(); 
            genre.Name = Model.Name; 
            
            _context.Genres.Add(genre);
            _context.SaveChanges();
        }
    }

    public class CreateGenreModel 
    {
        public string Name { get; set; }
    }
}