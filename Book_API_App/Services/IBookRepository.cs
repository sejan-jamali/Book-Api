using Book_API_App.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Book_API_App.Services
{
    public interface IBookRepository
    {
        ICollection<Book> GetAllBooks();
        Book GetBook(int bookId);
        bool BookExist(int bookId);
        bool ISBNExist(string bookISBN);
        bool IsDuplicateISBN(int bookId, string bookISBN);
        decimal GetBookRatings(int bookId);
        bool CreateBook(List<int> authorId, List<int> categorieId, Book book);
        bool UpdateBook(List<int> authorId, List<int> categorieId, Book book);
        bool DeleteBook(Book book);
        bool Save();
    }
}
