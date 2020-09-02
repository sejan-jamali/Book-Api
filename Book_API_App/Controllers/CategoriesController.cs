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
    public class CategoriesController : Controller
    {
        private ICategoryRepository _categoryRepository;
        private IBookRepository _bookRepository;
        public CategoriesController(ICategoryRepository categoryRepository, IBookRepository bookRepository)
        {
            _categoryRepository = categoryRepository;
            _bookRepository = bookRepository;
        }


        //api/catregories
        [HttpGet]
        public IActionResult GetAllCategories()
        {
            List<Category> categories = _categoryRepository.GetCategories().ToList();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            List<CategoryDto> categoryDtos = new List<CategoryDto>();
            foreach (Category c in categories)
            {
                categoryDtos.Add(new CategoryDto
                {
                    Id = c.Id,
                    Name = c.Name
                });
            }
            return Ok(categoryDtos);
        }

        //api/categories/categoryid
        [HttpGet("{categoryId}", Name = "GetCategory")]
        public IActionResult GetCountry(int categoryId)
        {
            if (!_categoryRepository.CategoryExist(categoryId))
                return NotFound();

            Category category = _categoryRepository.GetCategory(categoryId);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            CategoryDto categoryDto = new CategoryDto()
            {
                Id = category.Id,
                Name = category.Name
            };

            return Ok(categoryDto);
        }

        //api/categories/book/bookid
        [HttpGet("book/{bookId}")]
        public IActionResult GetCategoriesForBook(int bookId)
        {
            if (!_bookRepository.BookExist(bookId))
                return NotFound();

            IEnumerable<Category> category = _categoryRepository.GetCategoriesOfBook(bookId);
            List<CategoryDto> categoryDto = new List<CategoryDto>();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            foreach(Category c in category)
            {
                categoryDto.Add(new CategoryDto
                {
                    Id = c.Id,
                    Name = c.Name
                });
                
            }

            return Ok(categoryDto);

        }

        //api/books/bookid/categories
        [HttpGet("{categoryId}/book")]
        public IActionResult GetAllBooksForCounrty(int categoryId)
        {
            if (!_categoryRepository.CategoryExist(categoryId))
                return NotFound();

            ICollection<Book> books = _categoryRepository.GetBookForCategory(categoryId);
            if (!ModelState.IsValid)
                return BadRequest();

            List<BookDto> bookDtos = new List<BookDto>();

            foreach (Book b in books)
            {
                bookDtos.Add(new BookDto
                {
                    Id = b.Id,
                    Title = b.Title,
                    ISBN = b.Isbn,
                    Datetime = b.PublishedDate
                });
            }

            return Ok(bookDtos);
        }

        [HttpPost]
        public IActionResult CreateCategory([FromBody] Category categorytoCreate)
        {
            if (categorytoCreate == null)
                return BadRequest();

            if (_categoryRepository.isDuplicateCountryName(categorytoCreate.Id, categorytoCreate.Name))
                return BadRequest(ModelState);

            if (!ModelState.IsValid)
                return BadRequest();

            if (!_categoryRepository.CreateCategory(categorytoCreate))
                return BadRequest(ModelState);

            return CreatedAtRoute("GetCategory", new { categoryId = categorytoCreate.Id }, categorytoCreate);
        }

        [HttpPut("{categoryId}")]
        public IActionResult UpdateCategory(int categoryId, [FromBody] Category updateCategory)
        {
            if (updateCategory == null)
                return BadRequest();

            if (categoryId != updateCategory.Id)
                return BadRequest();

            if (!_categoryRepository.CategoryExist(updateCategory.Id))
                return BadRequest();

            if (!ModelState.IsValid)
                return BadRequest();

            if (_categoryRepository.isDuplicateCountryName(categoryId, updateCategory.Name))
                return BadRequest();

            if (!_categoryRepository.UpdateCategory(updateCategory))
                return BadRequest(ModelState);
            else
            {
                return Ok();
            }

            //return NotFound();
        }

        [HttpDelete("{categoryId}")]
        public IActionResult DeleteCountry(int categoryId)
        {
            if (!_categoryRepository.CategoryExist(categoryId))
                return BadRequest();

            var categoryToDelete = _categoryRepository.GetCategory(categoryId);

            if (_categoryRepository.isDuplicateCategoryName(categoryId, categoryToDelete.Name))
                return BadRequest();

            if (!ModelState.IsValid)
                return BadRequest();

            if (!_categoryRepository.DeleteCategory(categoryToDelete))
                return BadRequest(ModelState);
            else
                return Ok();
        }
    }
}
