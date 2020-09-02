using Book_API_App.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Book_API_App.Services
{
    public interface ICategoryRepository
    {
        ICollection<Category> GetCategories();
        Category GetCategory(int categoryId);
        ICollection<Category> GetCategoriesOfBook(int bookId);
        ICollection<Book> GetBookForCategory(int categoryId);
        bool CategoryExist(int categoryId);
        bool isDuplicateCategoryName(int categoryId, string categoryName);
        bool isDuplicateCountryName(int countryId, string countryName);
        bool CreateCategory(Category category);
        bool UpdateCategory(Category category);
        bool DeleteCategory(Category category);
        bool Save();
    }
}
