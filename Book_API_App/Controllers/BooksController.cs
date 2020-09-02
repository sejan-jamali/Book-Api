using Book_API_App.Dtos;
using Book_API_App.Models;
using Book_API_App.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Book_API_App.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : Controller
    {
        private IBookRepository _bookRepository;
        private IAuthorRepository _authorRepository;
        private ICategoryRepository _categoryRepository;
        private IReviewRepository _reviewRepository;
        public BooksController(IBookRepository bookRepository)
        {
            _bookRepository = bookRepository;
        }

        [HttpGet]
        public IActionResult GetBooks()
        {
            IEnumerable<Book> books = _bookRepository.GetAllBooks();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            List<BookDto> bookDtos = new List<BookDto>();

            foreach (Book b in books)
            {
                bookDtos.Add(new BookDto
                {
                    Id = b.Id,
                    Title = b.Title,
                    Datetime = b.PublishedDate,
                    ISBN = b.Isbn
                });
            }

            return Ok(bookDtos);
        }

        [HttpGet("{bookId}", Name = "GetBook")]
        public IActionResult GetBook(int bookId)
        {
            if (!_bookRepository.BookExist(bookId))
                return NotFound();

            Book book = _bookRepository.GetBook(bookId);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            BookDto bookDto = new BookDto()
            {
                Id = book.Id,
                Title = book.Title,
                Datetime = book.PublishedDate,
                ISBN = book.Isbn
            };

            return Ok(bookDto);
        }

        //[HttpGet("ISBN/{bookISBN}")]
        //public IActionResult GetBookISBN(string bookISBN)
        //{
        //    //if (!_bookRepository.BookExist(bookId))
        //    //    return NotFound();

        //    Book book = _bookRepository.GetBook(bookId);

        //    if (!ModelState.IsValid)
        //        return BadRequest(ModelState);

        //    BookDto bookDto = new BookDto()
        //    {
        //        Id = b.Id,
        //        Title = b.Title,
        //        Datetime = b.PublishedDate,
        //        ISBN = b.Isbn
        //    };

        //    return Ok(bookDto);
        //}

        [HttpGet("{bookId}/rating")]
        public IActionResult GetBookRatings(int bookId)
        {
            if (!_bookRepository.BookExist(bookId))
                return NotFound();

            var ratings = _bookRepository.GetBookRatings(bookId);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);
                       
            return Ok(ratings);
        }

        private StatusCodeResult ValidateStatus(List<int> authorId, List<int> categorieId, Book book)
        {
            if (book != null || authorId.Count() <= 0 || categorieId.Count() <= 0)
                return BadRequest();

            if (_bookRepository.IsDuplicateISBN(book.Id, book.Isbn))
                return BadRequest();

            foreach(var id in authorId)
            {
                if (!_authorRepository.AuthorExist(id))
                {
                    return BadRequest();
                }
            }

            foreach (var id in categorieId)
            {
                if (!_categoryRepository.CategoryExist(id))
                {
                    return BadRequest();
                }
            }

            if (!ModelState.IsValid)
                return BadRequest();

            return NoContent();
                
        }

        [HttpPost]
        public IActionResult CreateBooks([FromQuery]List<int> authorId, [FromQuery]List<int> categorieId, [FromQuery]Book bookToCreate)
        {
            var statusCode = ValidateStatus(authorId, categorieId, bookToCreate);

            if (!ModelState.IsValid)
                return StatusCode(statusCode.StatusCode);

            if (!_bookRepository.CreateBook(authorId, categorieId, bookToCreate))
                return BadRequest();

            return CreatedAtRoute("GetBook", new { bookId = bookToCreate.Id }, bookToCreate);

        }

        [HttpPut("{bookId}")]
        public IActionResult UpdateBooks(int bookId, [FromQuery] List<int> authorId, [FromQuery] List<int> categorieId, [FromQuery] Book bookToUpdate)
        {
            var statusCode = ValidateStatus(authorId, categorieId, bookToUpdate);

            if (bookId != bookToUpdate.Id)
                return BadRequest();

            if (!_bookRepository.BookExist(bookId))
                return BadRequest();

            if (!ModelState.IsValid)
                return StatusCode(statusCode.StatusCode);

            if (_bookRepository.UpdateBook(authorId, categorieId, bookToUpdate))
                return Ok();
            else
                return BadRequest();
        }

        [HttpDelete("{bookId}")]
        public IActionResult DeleteBook(int bookId)
        {
            if (!_bookRepository.BookExist(bookId))
                return BadRequest();

            var reviewsToDelete = _reviewRepository.GetReviewsByBook(bookId);
            var booksToDelete = _bookRepository.GetBook(bookId);

            if (!ModelState.IsValid)
                return BadRequest();

            if (!_reviewRepository.DeleteReviews(reviewsToDelete.ToList()))
                return BadRequest(ModelState);

            if (!_bookRepository.DeleteBook(booksToDelete))
                return BadRequest(ModelState);
            else
                return Ok();
        }
    }
}
