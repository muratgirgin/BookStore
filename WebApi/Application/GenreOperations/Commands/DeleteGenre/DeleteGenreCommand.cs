using System;
using System.Linq;
using WebApi.DBOperations;
using WebApi.Entities;

namespace  WebApi.Application.GenreOperations.DeleteGenre 
{
    public class DeleteGenreCommand 
    { 
        public int GenreId { get; set; }

        private readonly BookStoreDBContext _context;

        public DeleteGenreCommand(BookStoreDBContext context)
        {
            _context = context;
        }

        public void Handle()
        {
            var genre = _context.Genres.SingleOrDefault(x => x.Id == GenreId); 
            if (genre is null)
                throw new InvalidOperationException("GenreId not found!"); 
            
            _context.Genres.Remove(genre); 
            _context.SaveChanges();
        }
    }
}