using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using WebApi.BookOperations.CreateBook;
using WebApi.BookOperations.GetBooks;
using WebApi.BookOperations.GetBookDetail;
using WebApi.DBOperations;
using static WebApi.BookOperations.CreateBook.CreateBookCommand;
using WebApi.BookOperations.UpdateBook;
using WebApi.BookOperations.DeleteBook;

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
        public IActionResult GetById(int id)
        {
            BookDetailViewModel result; 
            try
            {
                GetBookDetailQuery query = new GetBookDetailQuery(_context); 
                query.BookId = id;
                result = query.Handle(); 
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            
            return Ok(result);
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
        public IActionResult UpdateBook(int id, [FromBody] UpdateBookModel updatedBook)
        {
            try
            {
                UpdateBookCommand command = new UpdateBookCommand(_context);
                command.BookId = id; 
                command.Model = updatedBook;
                command.Handle();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
           
            return Ok();
        } 

        [HttpDelete("{id}")]
        public IActionResult DeleteBook(int id) 
        {
           try
            {
                DeleteBookCommand command = new DeleteBookCommand(_context);
                command.BookId = id; 
                command.Handle();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
           
            return Ok();
        }
    } 
}