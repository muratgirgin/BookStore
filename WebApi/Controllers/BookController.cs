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
using AutoMapper;

namespace WebApi.AddControllers 
{
    [ApiController]
    [Route("[controller]s")]
    public class BookController : ControllerBase
    {
        // to prevent to be overwritten (readonly) and set in the constructor (private)
        private readonly BookStoreDBContext _context; 
        private readonly IMapper _mapper;
        public BookController(BookStoreDBContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet]
        public IActionResult GetBooks()
        {
            GetBooksQuery query = new GetBooksQuery(_context, _mapper);
            var result = query.Handle(); 
            return Ok(result);
        }
        
        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            BookDetailViewModel result; 
            try
            {
                GetBookDetailQuery query = new GetBookDetailQuery(_context, _mapper); 
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
            CreateBookCommand command = new CreateBookCommand(_context, _mapper);
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