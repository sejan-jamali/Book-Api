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
    public class AuthorsController : Controller
    {
        private IAuthorRepository _authorRepository;
        private IBookRepository _bookRepository;
        private ICountryRepository _countryRepository;
        public AuthorsController(IAuthorRepository authorRepository, IBookRepository bookRepository, ICountryRepository countryRepository)
        {
            _authorRepository = authorRepository;
            _bookRepository = bookRepository;
            _countryRepository = countryRepository;
        }

        [HttpGet]
        public IActionResult GetAuthors()
        {
            IEnumerable<Author> authors = _authorRepository.GetAllAuthors();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            List<AuthorDto> authorDtos = new List<AuthorDto>();

            foreach (Author a in authors)
            {
                authorDtos.Add(new AuthorDto
                {
                    Id = a.Id,
                    FirstName = a.First_Name,
                    LastName = a.Last_Name
                });
            }

            return Ok(authorDtos);
        }

        [HttpGet("{authorId}", Name = "GetAuthor")]
        public IActionResult GetAuthor(int authorId)
        {
            if (!_authorRepository.AuthorExist(authorId))
                return NotFound();

            Author authors = _authorRepository.GetAuthor(authorId);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            AuthorDto authorDto = new AuthorDto()
            {
                Id = authors.Id,
                FirstName = authors.First_Name,
                LastName = authors.Last_Name
            };

            return Ok(authorDto);
        }

        [HttpGet("{authorId}/book")]
        public IActionResult GetBookOfAuthor(int authorId)
        {
            if (!_authorRepository.AuthorExist(authorId))
                return NotFound();

            IEnumerable<Book> books = _authorRepository.GetBooksOfAuthor(authorId);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            List<BookDto> bookDtos = new List<BookDto>();

            foreach(Book b in books)
            {
                bookDtos.Add(new BookDto
                {
                    Id = b.Id,
                    Title = b.Title,
                    ISBN = b.Title,
                    Datetime = b.PublishedDate
                });
            }

            return Ok(bookDtos);
        }

        [HttpGet("books/{bookId}")]
        public IActionResult GetAuthorOfBook(int bookId)
        {
            if (!_bookRepository.BookExist(bookId))
                return NotFound();

            IEnumerable<Author> authors = _authorRepository.GetAuthorsOfBook(bookId);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            List<AuthorDto> authorDtos = new List<AuthorDto>();

            foreach (Author a in authors)
            {
                authorDtos.Add(new AuthorDto
                {
                    Id = a.Id,
                    FirstName = a.First_Name,
                    LastName = a.Last_Name
                });
            }

            return Ok(authorDtos);
        }

        [HttpPost]
        public IActionResult CreateAuthor([FromBody] Author authortoCreate)
        {
            if (authortoCreate == null)
                return BadRequest();

            if (!_countryRepository.CountryExists(authortoCreate.Country.Id))
                return BadRequest();

            authortoCreate.Country = _countryRepository.GetCountry(authortoCreate.Country.Id);

            if (!ModelState.IsValid)
                return BadRequest();

            if (!_authorRepository.CreateAuthor(authortoCreate))
                return BadRequest(ModelState);

            return CreatedAtRoute("GetAuthor", new { authorId = authortoCreate.Id }, authortoCreate);
        }

        [HttpPut("{authorId}")]
        public IActionResult UpdateAuthor(int authorId, [FromBody] Author authortoUpdate)
        {
            if (authortoUpdate == null)
                return BadRequest();

            if (authorId != authortoUpdate.Id)
                return BadRequest();

            if (_authorRepository.AuthorExist(authortoUpdate.Id))
                return BadRequest();

            if (!_countryRepository.CountryExists(authortoUpdate.Country.Id))
                return BadRequest();

            authortoUpdate.Country = _countryRepository.GetCountry(authortoUpdate.Country.Id);

            if (!ModelState.IsValid)
                return BadRequest();

            if (!_authorRepository.UpdateAuthor(authortoUpdate))
                return BadRequest(ModelState);
            else
                return Ok();
        }

        [HttpDelete("{authorId}")]
        public IActionResult DeleteAuthor(int authorId)
        {
            if (!_authorRepository.AuthorExist(authorId))
                return BadRequest();

            var authorToDelete = _authorRepository.GetAuthor(authorId);

            if (_authorRepository.GetBooksOfAuthor(authorId).Count() > 0)
                return BadRequest();

            if (!ModelState.IsValid)
                return BadRequest();

            if (!_authorRepository.DeleteAuthor(authorToDelete))
                return BadRequest(ModelState);
            else
                return Ok();
        }
    }
}
