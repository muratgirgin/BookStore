using System;
using System.Linq;
using AutoMapper;
using FluentAssertions;
using TestSetup;
using WebApi.Application.BookOperations.Commands.CreateBook;
using WebApi.DBOperations;
using WebApi.Entities;
using Xunit;
using static WebApi.Application.BookOperations.Commands.CreateBook.CreateBookCommand;

namespace Application.BookOperations.Commands.CreateBook
{
    public class CreateBookCommandTests : IClassFixture<CommonTestFixture>
    {
        private readonly BookStoreDBContext _context;
        private readonly IMapper _mapper;

        public CreateBookCommandTests(CommonTestFixture testFixture)
        {
            _context = testFixture.Context;
            _mapper = testFixture.Mapper;
        }

        [Fact]
        public void WhenAlreadyExistsBookTitleIsGiven_InvalidOperationException_ShouldReturn()
        {
            // arrange 
            var book = new Book() { Title = "WhenAlreadyExistsBookTitleIsGiven_InvalidOperationException_ShouldReturn", PageCount = 100, PublishDate = new System.DateTime(1983, 01, 10), GenreId = 1 };
            _context.Books.Add(book);
            _context.SaveChanges();

            CreateBookCommand command = new CreateBookCommand(_context, _mapper);
            command.Model = new CreateBookModel() { Title = book.Title };

            // act & assert
            FluentActions
                .Invoking(() => command.Handle())
                .Should().Throw<InvalidOperationException>().And.Message.Should().Be("Book already exists");
        }

        [Fact]
        public void WhenValidInputsAreGiven_BookShouldBeCreated()
        {
            // arrange 
            CreateBookCommand command = new CreateBookCommand(_context, _mapper);
            CreateBookModel model = new CreateBookModel() { Title = "Hobbit", PageCount = 250, PublishDate = DateTime.Now.Date.AddYears(-20), GenreId = 1 };
            command.Model = model;

            // act 
            FluentActions.Invoking(() => command.Handle()).Invoke();

            // assert 
            var book = _context.Books.SingleOrDefault(book => book.Title == model.Title);

            book.Should().NotBeNull();
            book.PageCount.Should().Be(model.PageCount);
            book.PublishDate.Should().Be(model.PublishDate);
            book.GenreId.Should().Be(model.GenreId);
        }
    }
}