using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using WebApi.BookOperations.GetBooks;
using WebApi.DBOperations;

namespace WebApi.BookOperations.DeleteBook 
{
    public class DeleteBookCommand 
    {
        private readonly BookStoreDBContext _dbContext;
        public int BookId { get; set; }

        public DeleteBookCommand(BookStoreDBContext dBContext)
        {
            _dbContext = dBContext; 
        }

        public void Handle()
        {
            var book = _dbContext.Books.SingleOrDefault(x=> x.Id == BookId); 
            if (book is null)
                 throw new InvalidOperationException("No such a book found!");

            _dbContext.Books.Remove(book);
            _dbContext.SaveChanges();
        }
    }
}