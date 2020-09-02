using Book_API_App.Models;
using Book_API_App.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Book_API_App.Repository
{
    public class CategoryRepository : ICategoryRepository
    {
        private BookDBContext _bookDBContext;

        public CategoryRepository(BookDBContext bookDBContext)
        {
            _bookDBContext = bookDBContext;
        }

        public bool CategoryExist(int categoryId)
        {
            return _bookDBContext.Categories.Any(c => c.Id == categoryId);
        }

        public bool CreateCategory(Category category)
        {
            _bookDBContext.AddAsync(category);
            return Save();
        }

        public bool DeleteCategory(Category category)
        {
            _bookDBContext.Remove(category);
            return Save();
        }

        public ICollection<Book> GetBookForCategory(int categoryId)
        {
            return _bookDBContext.BookCategories.Where(c => c.CategoryId == categoryId).Select(b => b.Book).ToList();
        }

        public ICollection<Category> GetCategories()
        {
            return _bookDBContext.Categories.OrderBy(c => c.Name).ToList();
        }

        public ICollection<Category> GetCategoriesOfBook(int bookId)
        {
            return _bookDBContext.BookCategories.Where(b => b.BookId == bookId).Select(c => c.Category).ToList();
        }

        public Category GetCategory(int categoryId)
        {
            return _bookDBContext.Categories.Where(c => c.Id == categoryId).FirstOrDefault();
        }

        public bool isDuplicateCategoryName(int categoryId, string categoryName)
        {
            var category = _bookDBContext.Categories.Where(c => c.Name.Trim().ToUpper() == categoryName.Trim().ToUpper() && c.Id != categoryId).FirstOrDefault();
            return category == null ? false : true;
        }

        public bool isDuplicateCountryName(int categoryId, string categoyName)
        {
            var country = _bookDBContext.Categories.Where(c => c.Name.Trim().ToUpper() == categoyName.Trim().ToUpper() && c.Id != categoryId).FirstOrDefault();
            return country == null ? false : true;
        }

        public bool Save()
        {
            var saved = _bookDBContext.SaveChanges();
            return saved > 0 ? true : false;
        }

        public bool UpdateCategory(Category category)
        {
            _bookDBContext.Update(category);
            return Save();
        }
    }
}
