using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using WebApi.BookOperations.CreateBook;
using WebApi.BookOperations.GetBooks;
using WebApi.DBOperations;
using static WebApi.BookOperations.CreateBook.CreateBookCommand;

namespace WebApi.AddControllers 
{
    [ApiController]
    [Route("[controller]s")]
    public class BookController : ControllerBase
    {
        // to prevent to be overwritten (readonly) and set in the constructor (private)
        private readonly BookStoreDBContext _context; 

        public BookController (BookStoreDBContext context)
        {
            _context = context;
        }

        // private static List<Book> BookList = new List<Book>()
        // {
        //     new Book{
        //         Id = 1, 
        //         Title = "Lean Startup", 
        //         GenreId = 1, 
        //         PageCount = 250, 
        //         PublishDate = new DateTime(2001, 06, 12)
        //     },
        //     new Book{
        //         Id = 2, 
        //         Title = "Herland", 
        //         GenreId = 2, 
        //         PageCount = 300, 
        //         PublishDate = new DateTime(2010, 05, 23)
        //     }, 
        //     new Book{
        //         Id = 3, 
        //         Title = "Dume", 
        //         GenreId = 2, 
        //         PageCount = 540, 
        //         PublishDate = new DateTime(2001, 12, 21)
        //     }
        // }; 

        [HttpGet]
        public IActionResult GetBooks()
        {
            GetBooksQuery query = new GetBooksQuery(_context);
            var result = query.Handle(); 
            return Ok(result);
        }
        
        [HttpGet("{id}")]
        public Book GetById(int id)
        {
            var book = _context.Books.Where(book=> book.Id == id).SingleOrDefault();
            return book;
        }

        [HttpPost]
        public IActionResult AddBook([FromBody] CreateBookModel newBook) 
        {
            CreateBookCommand command = new CreateBookCommand(_context);
            try
            {
                command.Model = newBook;
                command.Handle();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

            return Ok();
        }

        [HttpPut("{id}")]
        public IActionResult UpdateBook(int id, [FromBody] Book updatedBook)
        {
            var book = _context.Books.SingleOrDefault(x=> x.Id == id); 
            if (book is null)
                return BadRequest(); 

            book.GenreId = updatedBook.GenreId != default ?  updatedBook.GenreId : book.GenreId; 
            book.PageCount = updatedBook.PageCount != default ? updatedBook.PageCount : book.PageCount; 
            book.PublishDate = updatedBook.PublishDate != default ? updatedBook.PublishDate : book.PublishDate; 
            book.Title = updatedBook.Title != default ? updatedBook.Title : book.Title; 

            _context.SaveChanges();

            return Ok();
        } 

        [HttpDelete("{id}")]
        public IActionResult DeleteBook(int id) 
        {
            var book = _context.Books.SingleOrDefault(x=> x.Id == id); 
            if (book is null)
                return BadRequest();

            _context.Books.Remove(book);
            _context.SaveChanges();
            
            return Ok(); 
        }
    } 
}